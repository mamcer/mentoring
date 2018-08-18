CREATE TABLE [dbo].[TimeSlot] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [Description] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_dbo.TimeSlot] PRIMARY KEY CLUSTERED ([Id] ASC)
);

