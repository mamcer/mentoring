CREATE TABLE [dbo].[MenteeSeniority] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [Description] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_dbo.MenteeSeniority] PRIMARY KEY CLUSTERED ([Id] ASC)
);

