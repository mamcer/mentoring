CREATE TABLE [dbo].[Mentee] (
    [Id]                   INT            IDENTITY (1, 1) NOT NULL,
    [CurrentActivity]      NVARCHAR (MAX) NOT NULL,
    [Focus]                NVARCHAR (MAX) NOT NULL,
    [FirstOptionAccepted]  BIT            NULL,
    [SecondOptionAccepted] BIT            NULL,
    [Status]               INT            NOT NULL,
    [Mentor_Id]            INT            NULL,
    [FirstOption_Id]       INT            NOT NULL,
    [Mentor_Id1]           INT            NULL,
    [SecondOption_Id]      INT            NULL,
    [User_Id]              INT            NULL,
    [MentorOptionDate]     DATETIME       DEFAULT ('1900-01-01T00:00:00.000') NOT NULL,
    CONSTRAINT [PK_dbo.Mentee] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.Mentee_dbo.Mentor_FirstOption_Id] FOREIGN KEY ([FirstOption_Id]) REFERENCES [dbo].[Mentor] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_dbo.Mentee_dbo.Mentor_Mentor_Id] FOREIGN KEY ([Mentor_Id]) REFERENCES [dbo].[Mentor] ([Id]),
    CONSTRAINT [FK_dbo.Mentee_dbo.Mentor_Mentor_Id1] FOREIGN KEY ([Mentor_Id1]) REFERENCES [dbo].[Mentor] ([Id]),
    CONSTRAINT [FK_dbo.Mentee_dbo.Mentor_SecondOption_Id] FOREIGN KEY ([SecondOption_Id]) REFERENCES [dbo].[Mentor] ([Id]),
    CONSTRAINT [FK_dbo.Mentee_dbo.User_User_Id] FOREIGN KEY ([User_Id]) REFERENCES [dbo].[User] ([Id])
);




GO
CREATE NONCLUSTERED INDEX [IX_Mentor_Id]
    ON [dbo].[Mentee]([Mentor_Id] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_FirstOption_Id]
    ON [dbo].[Mentee]([FirstOption_Id] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Mentor_Id1]
    ON [dbo].[Mentee]([Mentor_Id1] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_SecondOption_Id]
    ON [dbo].[Mentee]([SecondOption_Id] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_User_Id]
    ON [dbo].[Mentee]([User_Id] ASC);

