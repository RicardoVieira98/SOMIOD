BEGIN TRANSACTION
CREATE TABLE [dbo].[Application] (
    [Id]          INT           IDENTITY (1, 1) NOT NULL,
    [Name]        VARCHAR (255) NOT NULL,
    [CreatedDate] DATETIME      NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    UNIQUE NONCLUSTERED ([Name] ASC)
);

CREATE TABLE [dbo].[Container] (
    [Id]          INT           IDENTITY (1, 1) NOT NULL,
    [Name]        VARCHAR (255) NOT NULL,
    [CreatedDate] DATETIME      NULL,
    [Parent]      INT           NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    UNIQUE NONCLUSTERED ([Name] ASC)
);

CREATE TABLE [dbo].[Subscription] (
    [Id]          INT           IDENTITY (1, 1) NOT NULL,
    [Name]        VARCHAR (255) NOT NULL,
    [CreatedDate] DATETIME      NULL,
    [Parent]      INT           NOT NULL,
    [Event]       VARCHAR (55)  NOT NULL,
    [Endpoint]    VARCHAR (MAX) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    UNIQUE NONCLUSTERED ([Name] ASC)
);

INSERT INTO Appllication VALUES ('App1','10-10-2010')
INSERT INTO Appllication VALUES ('App2','10-10-2010')
INSERT INTO Appllication VALUES ('App3','10-10-2010')

INSERT INTO Container VALUES ('Container1','10-10-10-2010',1)
INSERT INTO Container VALUES ('Container1','10-10-10-2010',1)
INSERT INTO Container VALUES ('Container1','10-10-10-2010',1)
INSERT INTO Container VALUES ('Container1','10-10-10-2010',2)
INSERT INTO Container VALUES ('Container1','10-10-10-2010',2)
INSERT INTO Container VALUES ('Container1','10-10-10-2010',3)

INSERT INTO Subscription VALUES ('Subscription1','10-10-10-2010',1,'creation','http:192.168.1.15')
INSERT INTO Subscription VALUES ('Subscription2','10-10-10-2010',2,'creation','http:192.168.1.15')
INSERT INTO Subscription VALUES ('Subscription3','10-10-10-2010',3,'deletion','http:192.168.1.15')
COMMIT;
