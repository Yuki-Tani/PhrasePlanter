-- Select * FROM fn_my_permissions('tentative', 'User');
-- GO

SELECT USER_NAME();
GO

SELECT * FROM dbo.Users;
GO

-- INSERT INTO pp.Phrases (UserId, Phrase, PhraseMeaning) VALUES (1, 'Test Phrase', 'テストフレーズ');
-- INSERT INTO pp.Phrases (UserId, Phrase, PhraseMeaning) VALUES (2, 'Test Phrase', 'テストフレーズ');
GO

SELECT * FROM pp.Phrases;
GO

-- SELECT * FROM sys.database_principals AS principals
-- JOIN sys.database_permissions AS permissions
--     ON principals.principal_id = permissions.grantee_principal_id
-- GO
