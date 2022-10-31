USE PhrasePlanter
GO

CREATE TABLE dbo.Users (
    UserId int NOT NULL IDENTITY(1,1) PRIMARY KEY,
    UserAccount varchar(50) NOT NULL UNIQUE,
)
GO