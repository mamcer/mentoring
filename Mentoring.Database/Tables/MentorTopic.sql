CREATE TABLE [dbo].[MentorTopic] (
    [MentorId] INT NOT NULL,
    [TopicId]  INT NOT NULL,
    CONSTRAINT [PK_dbo.MentorTopic] PRIMARY KEY CLUSTERED ([MentorId] ASC, [TopicId] ASC),
    CONSTRAINT [FK_dbo.MentorTopic_dbo.Mentor_MentorId] FOREIGN KEY ([MentorId]) REFERENCES [dbo].[Mentor] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_dbo.MentorTopic_dbo.Topic_TopicId] FOREIGN KEY ([TopicId]) REFERENCES [dbo].[Topic] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_MentorId]
    ON [dbo].[MentorTopic]([MentorId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_TopicId]
    ON [dbo].[MentorTopic]([TopicId] ASC);

