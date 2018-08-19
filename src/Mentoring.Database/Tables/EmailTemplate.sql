CREATE TABLE [dbo].[EmailTemplate] (
    [Id]      INT            IDENTITY (1, 1) NOT NULL,
    [Name]    NVARCHAR (MAX) NOT NULL,
    [Subject] NVARCHAR (MAX) NOT NULL,
    [Content] NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_dbo.EmailTemplate] PRIMARY KEY CLUSTERED ([Id] ASC)
);

