USE PhrasePlanter
GO

DROP SECURITY POLICY IF EXISTS RowLevelSecurityFilter
DROP FUNCTION IF EXISTS pp_security.rowlevelsecuritypredicate
DROP FUNCTION IF EXISTS pp_security.useraccountrowlevelsecuritypredicate
GO

CREATE FUNCTION pp_security.useraccountrowlevelsecuritypredicate(@UserAccount AS nvarchar(50))
    RETURNS TABLE
    WITH SCHEMABINDING
    AS
    RETURN (
        SELECT 1 AS useraccountrowlevelsecuritypredicate_result
        WHERE
            USER_NAME() = 'dbo' OR
            @UserAccount = USER_NAME()
    )
GO

CREATE FUNCTION pp_security.rowlevelsecuritypredicate(@UserId AS int)
    RETURNS TABLE
    WITH SCHEMABINDING
    AS
    RETURN (
        SELECT 1 AS rowlevelsecuritypredicate_result
        WHERE
            USER_NAME() = 'dbo' OR
            @UserId = (SELECT UserId From dbo.Users WHERE UserAccount = USER_NAME())
    )
GO

CREATE SECURITY POLICY RowLevelSecurityFilter
    ADD FILTER PREDICATE pp_security.useraccountrowlevelsecuritypredicate(UserAccount) ON dbo.Users,
    ADD FILTER PREDICATE pp_security.rowlevelsecuritypredicate(UserId) ON pp.Phrases,
    ADD BLOCK PREDICATE pp_security.rowlevelsecuritypredicate(UserId) ON pp.Phrases AFTER INSERT,
    ADD BLOCK PREDICATE pp_security.rowlevelsecuritypredicate(UserId) ON pp.Phrases AFTER UPDATE,
    ADD BLOCK PREDICATE pp_security.rowlevelsecuritypredicate(UserId) ON pp.Phrases BEFORE UPDATE,
    ADD BLOCK PREDICATE pp_security.rowlevelsecuritypredicate(UserId) ON pp.Phrases BEFORE DELETE
WITH (STATE = ON);
GO
