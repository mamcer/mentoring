CREATE TABLE [dbo].[User] (
    [Id]        INT             IDENTITY (1, 1) NOT NULL,
    [Name]      NVARCHAR (100)  NOT NULL,
    [Email]     NVARCHAR (100)  NOT NULL,
    [NickName]  NVARCHAR (100)  NULL,
    [AvatarUrl] NVARCHAR (2083) NULL,
    [JoinDate]  DATETIME        NOT NULL,
    [Location]  NVARCHAR (100)  NOT NULL,
    [Seniority] NVARCHAR (100)  NOT NULL,
    CONSTRAINT [PK_dbo.User] PRIMARY KEY CLUSTERED ([Id] ASC)
);

