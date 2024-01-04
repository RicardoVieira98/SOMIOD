CREATE TABLE [dbo].[Application] (
    [Id]          INT            NOT NULL,
    [Name]        NVARCHAR (MAX) NOT NULL,
    [CreatedDate] DATETIME       NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

