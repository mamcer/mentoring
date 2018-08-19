CREATE TABLE [dbo].[MentorMenteeSeniority] (
    [MentorId]          INT NOT NULL,
    [MenteeSeniorityId] INT NOT NULL,
    CONSTRAINT [PK_dbo.MentorMenteeSeniority] PRIMARY KEY CLUSTERED ([MentorId] ASC, [MenteeSeniorityId] ASC),
    CONSTRAINT [FK_dbo.MentorMenteeSeniority_dbo.MenteeSeniority_MenteeSeniorityId] FOREIGN KEY ([MenteeSeniorityId]) REFERENCES [dbo].[MenteeSeniority] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_MenteeSeniorityId]
    ON [dbo].[MentorMenteeSeniority]([MenteeSeniorityId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_MentorId]
    ON [dbo].[MentorMenteeSeniority]([MentorId] ASC);

