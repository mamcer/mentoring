CREATE TABLE [dbo].[Mentor] (
    [Id]                INT            IDENTITY (1, 1) NOT NULL,
    [CurrentActivity]   NVARCHAR (MAX) NOT NULL,
    [OtherTopic]        NVARCHAR (MAX) NULL,
    [MentoringTarget]   NVARCHAR (MAX) NOT NULL,
    [MaxMentees]        INT            NOT NULL,
    [OtherAvailability] NVARCHAR (MAX) NULL,
    [MeetingsMode]      INT            NOT NULL,
    [Expectations]      NVARCHAR (MAX) NOT NULL,
    [Status]            INT            NOT NULL,
    [User_Id]           INT            NULL,
    CONSTRAINT [PK_dbo.Mentor] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.Mentor_dbo.User_User_Id] FOREIGN KEY ([User_Id]) REFERENCES [dbo].[User] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_User_Id]
    ON [dbo].[Mentor]([User_Id] ASC);

