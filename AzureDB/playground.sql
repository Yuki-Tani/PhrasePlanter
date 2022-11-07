-- Select * FROM fn_my_permissions('tentative', 'User');
-- GO

SELECT USER_NAME();
GO

SELECT * FROM dbo.Users;
GO

SELECT userid FROM dbo.Users Where useraccount = 'yuki';
GO

-- INSERT INTO pp.Phrases (UserId, Phrase, PhraseMeaning) VALUES (
--     SELECT UserId FROM dbo.Users Where UserAccount = 'yuki',
--     'Test Phrase in ID',
--     'テスト'
-- );
INSERT INTO pp.Phrases (UserId, Phrase, PhraseMeaning) VALUES (1, 'Test Phrase', N'テストフレーズ');
-- INSERT INTO pp.Phrases (UserId, Phrase, PhraseMeaning) VALUES (2, 'Test Phrase', 'テストフレーズ');
GO

SELECT * FROM pp.Phrases;
SELECT Phrase, PhraseMeaning FROM pp.Phrases;
-- SELECT Phrase, PhraseMeaning FROM pp.Phrases;
GO

-- SELECT * FROM sys.database_principals AS principals
-- JOIN sys.database_permissions AS permissions
--     ON principals.principal_id = permissions.grantee_principal_id
-- GO
