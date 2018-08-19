/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

USE [Mentoring]
GO

-- TIME SLOT

CREATE TABLE #TimeSlot (
	[Id] [int] NOT NULL,
	[Description] [nvarchar](max) NULL
)
GO

INSERT #TimeSlot ([Id], [Description]) VALUES (1, N'8 to 9')
GO
INSERT #TimeSlot ([Id], [Description]) VALUES (2, N'9 to 10')
GO
INSERT #TimeSlot ([Id], [Description]) VALUES (3, N'10 to 11')
GO
INSERT #TimeSlot ([Id], [Description]) VALUES (4, N'11 to 12')
GO
INSERT #TimeSlot ([Id], [Description]) VALUES (5, N'12 to 13')
GO
INSERT #TimeSlot ([Id], [Description]) VALUES (6, N'13 to 14')
GO
INSERT #TimeSlot ([Id], [Description]) VALUES (7, N'14 to 15')
GO
INSERT #TimeSlot ([Id], [Description]) VALUES (8, N'15 to 16')
GO
INSERT #TimeSlot ([Id], [Description]) VALUES (9, N'16 to 17')
GO
INSERT #TimeSlot ([Id], [Description]) VALUES (10, N'17 to 18')
GO
INSERT #TimeSlot ([Id], [Description]) VALUES (11, N'18 to 19')
GO

SET IDENTITY_INSERT [dbo].[TimeSlot] ON 
GO

MERGE [dbo].[TimeSlot] dst
USING #TimeSlot src
ON (src.Id = dst.Id)
WHEN MATCHED THEN
UPDATE SET 
dst.[Description] = src.[Description]
WHEN NOT MATCHED THEN
INSERT ([Id], [Description]) VALUES (src.Id, src.[Description])
WHEN NOT MATCHED BY SOURCE THEN
DELETE;
GO

SET IDENTITY_INSERT [dbo].[TimeSlot] OFF
GO

DROP TABLE #TimeSlot
GO

-- TOPIC

CREATE TABLE #Topic (
	[Id] [int] NOT NULL,
	[Description] [nvarchar](max) NULL
)
GO

INSERT #Topic ([Id], [Description]) VALUES (1, N'Soft skills development (E.g. Leadership, Team Work, Time Management, Effective Communication, Management, etc.)')
GO
INSERT #Topic ([Id], [Description]) VALUES (2, N'Technical skills development (E.g. Java, Web UI, Big data, .NET, Frameworks)')
GO
INSERT #Topic ([Id], [Description]) VALUES (3, N'Changes of area/ profile/ position/ site/ country')
GO
INSERT #Topic ([Id], [Description]) VALUES (4, N'Preparation for a certification (Scrum, PMI, ISO, Agile, English)')
GO
INSERT #Topic ([Id], [Description]) VALUES (5, N'Other')
GO

SET IDENTITY_INSERT [dbo].[Topic] ON 
GO

MERGE [dbo].[Topic] dst
USING #Topic src
ON (src.Id = dst.Id)
WHEN MATCHED THEN
UPDATE SET 
dst.[Description] = src.[Description]
WHEN NOT MATCHED THEN
INSERT ([Id], [Description]) VALUES (src.Id, src.[Description])
WHEN NOT MATCHED BY SOURCE THEN
DELETE;
GO

SET IDENTITY_INSERT [dbo].[Topic] OFF
GO

DROP TABLE #Topic
GO

-- EMAIL TEMPLATE

CREATE TABLE #EmailTemplate (
	[Id] [int] NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Subject] [nvarchar](max) NOT NULL,
	[Content] [nvarchar](max) NOT NULL
)
GO

INSERT #EmailTemplate ([Id], [Name], [Subject], [Content]) VALUES (2, N'RejectMentor', N'Mentoring Program', N'REJECT MENTOR HTML EMAIL TEMPLATE BODY')
GO
INSERT #EmailTemplate ([Id], [Name], [Subject], [Content]) VALUES (1003, N'PreApprovedMentor', N'Pre-Approved Mentor', N'PRE APPROVED MENTOR HTML EMAIL TEMPLATE BODY')
GO
INSERT #EmailTemplate ([Id], [Name], [Subject], [Content]) VALUES (1004, N'PreApprovedMentee', N'New Mentee', N'PRE APPROVE MENTEE HTML EMAIL TEMPLATE BODY')
GO
INSERT #EmailTemplate ([Id], [Name], [Subject], [Content]) VALUES (2003, N'ApprovedMentor', N'Welcome!', N'APPROVED MENTOR HTML EMAIL TEMPLATE BODY')
GO
INSERT #EmailTemplate ([Id], [Name], [Subject], [Content]) VALUES (2004, N'RejectMentee', N'Mentoring Program', N'REJECT MENTEE HTML EMAIL TEMPLATE BODY')
GO

SET IDENTITY_INSERT [dbo].[EmailTemplate] ON 
GO

MERGE [dbo].[EmailTemplate] dst
USING #EmailTemplate src
ON (src.Id = dst.Id)
WHEN MATCHED THEN
UPDATE SET 
dst.[Name] = src.[Name],
dst.[Subject] = src.[Subject],
dst.[Content] = src.[Content]
WHEN NOT MATCHED THEN
INSERT ([Id], [Name], [Subject], [Content]) VALUES (src.Id, src.[Name], src.[Subject], src.[Content])
WHEN NOT MATCHED BY SOURCE THEN
DELETE;
GO

SET IDENTITY_INSERT [dbo].[EmailTemplate] OFF
GO

DROP TABLE #EmailTemplate
GO

-- PROGRAM STATUS

CREATE TABLE #ProgramStatus (
	[Id] [int] NOT NULL,
	[CreationDate] [datetime] NOT NULL,
	[StatusCode] [int] NOT NULL,
	[StatusDescription] [nvarchar](100) NOT NULL
)
GO

INSERT #ProgramStatus ([Id], [CreationDate], [StatusCode], [StatusDescription]) VALUES (1, CAST(N'2015-01-17 18:21:19.277' AS DateTime), 4, N'Program In Progress')
GO

SET IDENTITY_INSERT [dbo].[ProgramStatus] ON 
GO

MERGE [dbo].[ProgramStatus] dst
USING #ProgramStatus src
ON (src.Id = dst.Id)
WHEN MATCHED THEN
UPDATE SET 
dst.[CreationDate] = src.[CreationDate],
dst.[StatusCode] = src.[StatusCode],
dst.[StatusDescription] = src.[StatusDescription]
WHEN NOT MATCHED THEN
INSERT ([Id], [CreationDate], [StatusCode], [StatusDescription]) VALUES (src.Id, src.[CreationDate], src.[StatusCode], src.[StatusDescription])
WHEN NOT MATCHED BY SOURCE THEN
DELETE;
GO

SET IDENTITY_INSERT [dbo].[ProgramStatus] OFF
GO

DROP TABLE #ProgramStatus
GO

-- USER

CREATE TABLE #User (
	[Id] [int] NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Email] [nvarchar](100) NOT NULL,
	[NickName] [nvarchar](100) NULL,
	[AvatarUrl] [nvarchar](2083) NULL,
	[JoinDate] [datetime] NOT NULL,
	[Location] [nvarchar](100) NOT NULL,
	[Seniority] [nvarchar](100) NOT NULL
)
GO

INSERT #User ([Id], [Name], [Email], [NickName], [AvatarUrl], [JoinDate], [Location], [Seniority]) VALUES (1, N'mario', N'mario@company.com', N'Mario', N'https://blzmedia-a.akamaihd.net/d3/icons/portraits/42/barbarian_male.png', CAST(N'2015-01-17 00:00:00.000' AS DateTime), N'', N'Senior')
GO

SET IDENTITY_INSERT [dbo].[User] ON 
GO

MERGE [dbo].[User] dst
USING #User src
ON (src.Id = dst.Id)
WHEN MATCHED THEN
UPDATE SET 
dst.[Name] = src.[Name],
dst.[Email] = src.[Email],
dst.[NickName] = src.[NickName],
dst.[AvatarUrl] = src.[AvatarUrl],
dst.[JoinDate] = src.[JoinDate],
dst.[Location] = src.[Location],
dst.[Seniority] = src.[Seniority]
WHEN NOT MATCHED THEN
INSERT ([Id], [Name], [Email], [NickName], [AvatarUrl], [JoinDate], [Location], [Seniority]) VALUES (src.Id, src.[Name], src.[Email], src.[NickName], src.[AvatarUrl], src.[JoinDate], src.[Location], src.[Seniority])
WHEN NOT MATCHED BY SOURCE THEN
DELETE;
GO

SET IDENTITY_INSERT [dbo].[User] OFF
GO

DROP TABLE #User
GO

-- UserRole

CREATE TABLE #UserRole (
	[Id] [int] NOT NULL,
	[Description] [nvarchar](150) NOT NULL
)
GO

INSERT #UserRole ([Id], [Description]) VALUES (1, N'Mentee')
GO
INSERT #UserRole ([Id], [Description]) VALUES (2, N'Mentor')
GO
INSERT #UserRole ([Id], [Description]) VALUES (4, N'Career')
GO

MERGE [dbo].[UserRole] dst
USING #UserRole src
ON (src.Id = dst.Id)
WHEN MATCHED THEN
UPDATE SET 
dst.[Description] = src.[Description]
WHEN NOT MATCHED THEN
INSERT ([Id], [Description]) VALUES (src.Id, src.[Description])
WHEN NOT MATCHED BY SOURCE THEN
DELETE;
GO


DROP TABLE #UserRole
GO

-- UserUserRole

DECLARE @ExitsUserUserRole INT

    SELECT @ExitsUserUserRole =
        (SELECT TOP (1) 
        [EU].[UserId]
        FROM ( SELECT 
        UserId
        FROM [dbo].[UserUserRole] AS [UUR]
        WHERE [UUR].[UserId] = 1 AND [UUR].[UserRoleId] = 4
        ) as EU) 
		

IF @ExitsUserUserRole IS NULL
    INSERT [dbo].[UserUserRole] ([UserId], [UserRoleId]) VALUES (1, 4)

-- MenteeSeniority

CREATE TABLE #MenteeSeniority (
	[Id] [int] NOT NULL,
	[Description] [nvarchar](max) NULL
)
GO

SET IDENTITY_INSERT [dbo].[MenteeSeniority] ON 
GO

INSERT #MenteeSeniority ([Id], [Description]) VALUES (1, N'Junior')
GO
INSERT #MenteeSeniority ([Id], [Description]) VALUES (2, N'Semi Senior')
GO
INSERT #MenteeSeniority ([Id], [Description]) VALUES (4, N'Senior')
GO
INSERT #MenteeSeniority ([Id], [Description]) VALUES (8, N'Higher than Senior')
GO

MERGE [dbo].[MenteeSeniority] dst
USING #MenteeSeniority src
ON (src.Id = dst.Id)
WHEN MATCHED THEN
UPDATE SET 
dst.[Description] = src.[Description]
WHEN NOT MATCHED THEN
INSERT ([Id], [Description]) VALUES (src.Id, src.[Description])
WHEN NOT MATCHED BY SOURCE THEN
DELETE;
GO

SET IDENTITY_INSERT [dbo].[MenteeSeniority] OFF 
GO

DROP TABLE #MenteeSeniority
GO