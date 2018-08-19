CREATE TABLE [dbo].[MentorAvailability] (
    [MentorId]   INT NOT NULL,
    [TimeSlotId] INT NOT NULL,
    CONSTRAINT [PK_dbo.MentorAvailability] PRIMARY KEY CLUSTERED ([MentorId] ASC, [TimeSlotId] ASC),
    CONSTRAINT [FK_dbo.MentorAvailability_dbo.Mentor_MentorId] FOREIGN KEY ([MentorId]) REFERENCES [dbo].[Mentor] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_dbo.MentorAvailability_dbo.TimeSlot_TimeSlotId] FOREIGN KEY ([TimeSlotId]) REFERENCES [dbo].[TimeSlot] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_MentorId]
    ON [dbo].[MentorAvailability]([MentorId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_TimeSlotId]
    ON [dbo].[MentorAvailability]([TimeSlotId] ASC);