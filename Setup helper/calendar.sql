USE [project_management]
GO

/****** Object:  Table [dbo].[Calendar]    Script Date: 10/15/2025 10:20:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Calendar](
	[EventID] [varchar](200) IDENTITY(1,1) NOT NULL,
	[Subject] [varchar](200) NULL,
	[StartDateTime] [datetime] NULL,
	[EndDateTime] [datetime] NULL,
	[Description] [varchar](max) NULL,
	[Location] [varchar](200) NULL,
	[IsFullDay] [bit] NULL,
	[CreatedBy] [varchar](200) NULL,
	[CreatedDate] [datetime] NULL,
	[ThemeColor] [varchar](200) NULL,
 CONSTRAINT [PK__Calendar__7944C870BF0B40B4] PRIMARY KEY CLUSTERED 
(
	[EventID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[Calendar] ADD  CONSTRAINT [DF__Calendar__IsAllD__3F115E1A]  DEFAULT ((0)) FOR [IsFullDay]
GO

ALTER TABLE [dbo].[Calendar] ADD  CONSTRAINT [DF__Calendar__Create__40058253]  DEFAULT (getdate()) FOR [CreatedDate]
GO


