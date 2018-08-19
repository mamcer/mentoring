CREATE TABLE [dbo].[UserLog] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [Date]        DATETIME       NOT NULL,
    [Action]      NVARCHAR (100) NOT NULL,
    [Description] NVARCHAR (MAX) NULL,
    [User_Id]     INT            NULL,
    CONSTRAINT [PK_dbo.UserLog] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.UserLog_dbo.User_User_Id] FOREIGN KEY ([User_Id]) REFERENCES [dbo].[User] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_User_Id]
    ON [dbo].[UserLog]([User_Id] ASC);

