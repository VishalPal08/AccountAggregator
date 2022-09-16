USE [DbAccountAggregator]
GO

/****** Object:  UserDefinedTableType [dbo].[UT_BankDetails]    Script Date: 22-08-2022 12:36:13 ******/
DROP TYPE [dbo].[UT_BankDetails]
GO

/****** Object:  UserDefinedTableType [dbo].[UT_BankDetails]    Script Date: 22-08-2022 12:36:13 ******/
CREATE TYPE [dbo].[UT_BankDetails] AS TABLE(
	[AccountNumber] [varchar](100) NOT NULL,
	[AccountHolderName] [varchar](100) NOT NULL,
	[IFSCCode] [varchar](100) NOT NULL,
	[AccountType] [varchar](100) NOT NULL,
	[BankName] [varchar](100) NOT NULL,
	[BranchName] [varchar](100) NOT NULL
)
GO


