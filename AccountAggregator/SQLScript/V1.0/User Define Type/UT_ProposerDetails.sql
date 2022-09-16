USE [DbAccountAggregator]
GO

/****** Object:  UserDefinedTableType [dbo].[UT_ProposerDetails]    Script Date: 22-08-2022 12:36:58 ******/
DROP TYPE [dbo].[UT_ProposerDetails]
GO

/****** Object:  UserDefinedTableType [dbo].[UT_ProposerDetails]    Script Date: 22-08-2022 12:36:58 ******/
CREATE TYPE [dbo].[UT_ProposerDetails] AS TABLE(
	[ApplicationNumber] [varchar](100) NOT NULL,
	[SourceName] [varchar](250) NOT NULL,
	[policynumber] [varchar](100) NOT NULL,
	[ProductName] [varchar](100) NOT NULL,
	[FirstName] [varchar](100) NOT NULL,
	[LastName] [varchar](100) NOT NULL,
	[DateOfBirth] [datetime] NOT NULL,
	[Gender] [varchar](20) NOT NULL,
	[EmailId] [varchar](100) NOT NULL,
	[MobileNo] [varchar](100) NOT NULL,
	[MaritalStatus] [varchar](20) NOT NULL,
	[PANCardNo] [varchar](20) NOT NULL
)
GO


