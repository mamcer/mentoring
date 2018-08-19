CREATE TABLE [dbo].[EmailMessage] (
    [Id]           INT            IDENTITY (1, 1) NOT NULL,
    [CreationDate] DATETIME       NOT NULL,
    [To]           NVARCHAR (MAX) NOT NULL,
    [Subject]      NVARCHAR (150) NOT NULL,
    [Message]      NVARCHAR (MAX) NOT NULL,
    [Status]       INT            NOT NULL,
    CONSTRAINT [PK_dbo.EmailMessage] PRIMARY KEY CLUSTERED ([Id] ASC)
);

