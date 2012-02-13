USE [habanero_test_branch_2_6_1]
GO

/****** Object:  Table [dbo].[MyBO]    Script Date: 05/20/2011 12:03:23 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MyBO]') AND type in (N'U'))
DROP TABLE [dbo].[MyBO]
GO

USE [habanero_test_branch_2_6_1]
GO

/****** Object:  Table [dbo].[MyBO]    Script Date: 05/20/2011 12:03:24 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MyBO](
	[MyBOID] [uniqueidentifier] NOT NULL,
	[ByteArrayProp] [image] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO


