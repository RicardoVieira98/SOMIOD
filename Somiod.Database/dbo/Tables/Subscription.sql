CREATE TABLE [dbo].[Subscriptions]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [Name] NVARCHAR(MAX) NULL, 
    [CreatedDate] DATETIME NULL, 
    [Parent] INT NULL, 
    [Event] NVARCHAR(MAX) NULL, 
    [Endpoint] NVARCHAR(MAX) NULL
)
