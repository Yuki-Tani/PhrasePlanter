USE PhrasePlanter
GO

CREATE TABLE pp.Phrases (
    Id int NOT NULL IDENTITY(1,1) PRIMARY KEY,
    UserId int NOT NULL FOREIGN KEY REFERENCES dbo.Users(UserId),
    Phrase varchar(100),
    PhraseMeaning nvarchar(100),
)
GO