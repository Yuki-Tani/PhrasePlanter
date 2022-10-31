-- CREATE USER tentative WITH PASSWORD = 'pZ*KKmV^LVW9+c8U.Gky';
-- GO

-- Select * FROM fn_my_permissions('tentative', 'User');
-- GO

-- EXEC sp_helprole 'public'
-- GO

SELECT * FROM sys.database_principals AS principals
JOIN sys.database_permissions AS permissions
    ON principals.principal_id = permissions.grantee_principal_id