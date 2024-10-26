USE [SolarModule]
GO
/****** Object:  Table [dbo].[Students]    Script Date: 07-Aug-24 02:54:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Students](
	[StudentId] [bigint] IDENTITY(1,1) NOT NULL,
	[FirstName] [varchar](50) NULL,
	[LastName] [varchar](50) NULL,
	[Email] [varchar](50) NULL,
	[Password] [varchar](50) NULL,
	[Phone] [varchar](50) NULL,
	[InstituteName] [varchar](50) NULL,
	[Standard] [varchar](50) NULL,
	[IsActive] [bit] NULL,
	[CreatedBy] [nvarchar](50) NULL,
	[CreatedOn] [datetime] NULL,
	[ModifiedBy] [varchar](50) NULL,
	[ModifiedOn] [datetime] NULL,
 CONSTRAINT [PK_Students] PRIMARY KEY CLUSTERED 
(
	[StudentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Students] ON 

INSERT [dbo].[Students] ([StudentId], [FirstName], [LastName], [Email], [Password], [Phone], [InstituteName], [Standard], [IsActive], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn]) VALUES (1, N'Viraj', N'Takke', N'virajtakke24@gmail.com', N'123', N'8692914030', N'Mumbai University', N'12th', 1, NULL, CAST(N'2024-08-07T13:12:04.157' AS DateTime), NULL, NULL)
SET IDENTITY_INSERT [dbo].[Students] OFF
GO
