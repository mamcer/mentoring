CREATE TABLE [dbo].[ProgramStatus] (
    [Id]                INT            IDENTITY (1, 1) NOT NULL,
    [CreationDate]      DATETIME       NOT NULL,
    [StatusCode]        INT            NOT NULL,
    [StatusDescription] NVARCHAR (100) NOT NULL,
    CONSTRAINT [PK_dbo.ProgramStatus] PRIMARY KEY CLUSTERED ([Id] ASC)
);

