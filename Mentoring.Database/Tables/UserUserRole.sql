CREATE TABLE [dbo].[UserUserRole] (
    [UserId]     INT NOT NULL,
    [UserRoleId] INT NOT NULL,
    CONSTRAINT [PK_dbo.UserUserRole] PRIMARY KEY CLUSTERED ([UserId] ASC, [UserRoleId] ASC),
    CONSTRAINT [FK_dbo.UserUserRole_dbo.User_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_dbo.UserUserRole_dbo.UserRole_UserRoleId] FOREIGN KEY ([UserRoleId]) REFERENCES [dbo].[UserRole] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_UserId]
    ON [dbo].[UserUserRole]([UserId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_UserRoleId]
    ON [dbo].[UserUserRole]([UserRoleId] ASC);

