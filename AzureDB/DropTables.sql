USE PhrasePlanter
GO

DROP SECURITY POLICY IF EXISTS RowLevelSecurityFilter
DROP FUNCTION IF EXISTS pp_security.rowlevelsecuritypredicate
DROP FUNCTION IF EXISTS pp_security.useraccountrowlevelsecuritypredicate

DROP TABLE IF EXISTS pp.Phrases;
-- DROP TABLE IF EXISTS dbo.Users;
GO
