CREATE TABLE [dbo].[Container] (
    [Id]          INT           IDENTITY (1, 1) NOT NULL,
    [Name]        VARCHAR (255) NOT NULL,
    [CreatedDate] DATETIME      NULL,
    [Parent]      INT           NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    UNIQUE NONCLUSTERED ([Name] ASC)
);