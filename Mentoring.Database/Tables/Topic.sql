CREATE TABLE [dbo].[Topic] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [Description] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_dbo.Topic] PRIMARY KEY CLUSTERED ([Id] ASC)
);

