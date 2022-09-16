IF (Not EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES  WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'txn_ProposerDetails'))
	BEGIN
			CREATE TABLE [dbo].[txn_ProposerDetails](
			[ProposerId] [int] IDENTITY(1,1) NOT NULL,
			[SourceName] [varchar](250) NULL,
			[policynumber] [varchar](100) NULL,
			[ApplicationNumber] [varchar](100) NULL,
			[ProductName] [varchar](100) NULL,
			[FirstName] [varchar](100) NULL,
			[LastName] [varchar](100) NULL,
			[DateOfBirth] [date] NULL,
			[Gender] [varchar](20) NULL,
			[EmailId] [varchar](100) NULL,
			[MobileNo] [varchar](100) NULL,
			[MaritalStatus] [varchar](20) NULL,
			[PANCardNo] [varchar](20) NULL,
			[CreatedAt] [datetime] NULL,
			[LastModifiedAt] [datetime] NULL,
			[TxtGuid] [nvarchar](max) NULL,
		 CONSTRAINT [PK_txn_ProposerDetails] PRIMARY KEY CLUSTERED 
		(
			[ProposerId] ASC
		)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
		) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
		
	END

go
IF (Not EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES  WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'MstTypeOfRequest'))
	BEGIN
			CREATE TABLE [dbo].[MstTypeOfRequest](
			[TypeId] [int] IDENTITY(1,1) NOT NULL,
			[TypeName] [varchar](250) NOT NULL,
			[CreatedAt] [datetime] NOT NULL,
			[LastModifiedAt] [datetime] NOT NULL,
		 CONSTRAINT [PK_MstTypeOfRequest_TypeId] PRIMARY KEY CLUSTERED 
		(
			[TypeId] ASC
		)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
		) ON [PRIMARY]
		
	END

GO

IF (Not EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES  WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'txn_RequestNResponse'))
	BEGIN
			CREATE TABLE [dbo].[txn_RequestNResponse](
			[Id] [int] IDENTITY(1,1) NOT NULL,
			[TypeId] [int] NOT NULL,
			[TxtGuid] [nvarchar](max) NOT NULL,
			[RequestBody] [nvarchar](max) NULL,
			[ResponseBody] [nvarchar](max) NULL,
			[RequestTimeStamp] [datetime] NULL,
			[ResponseTimeStamp] [datetime] NULL,
		 CONSTRAINT [PK_txn_RequestNResponse_Id] PRIMARY KEY CLUSTERED 
		(
			[Id] ASC
		)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
		) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]


		ALTER TABLE [dbo].[txn_RequestNResponse]  WITH CHECK ADD  CONSTRAINT [FK_txn_RequestNResponse_MstTypeOfRequest_TypeId] FOREIGN KEY([TypeId])
		REFERENCES [dbo].[MstTypeOfRequest] ([TypeId])


		ALTER TABLE [dbo].[txn_RequestNResponse] CHECK CONSTRAINT [FK_txn_RequestNResponse_MstTypeOfRequest_TypeId]
	END

go

IF (Not EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES  WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'txn_BankDetails'))
	BEGIN
			CREATE TABLE [dbo].[txn_BankDetails](
			[BankTxnId] [int] IDENTITY(101,1) NOT NULL,
			[ProposerId] [int] NOT NULL,
			[SudUniqueId]  AS ((((CONVERT([varchar](50),[BankTxnId])+'_')+[BankName])+'_')+[AccountNumber]) PERSISTED,
			[AccountNumber] [varchar](100) NOT NULL,
			[AccountHolderName] [varchar](100) NOT NULL,
			[IFSCCode] [varchar](100) NOT NULL,
			[AccountType] [varchar](100) NOT NULL,
			[BankName] [varchar](100) NOT NULL,
			[BranchName] [varchar](100) NOT NULL,
		 CONSTRAINT [PK_txn_BankDetails_BankTxnId] PRIMARY KEY CLUSTERED 
		(
			[BankTxnId] ASC
		)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
		) ON [PRIMARY]


		ALTER TABLE [dbo].[txn_BankDetails]  WITH CHECK ADD  CONSTRAINT [FK_txn_BankDetails_txn_ProposerDetails] FOREIGN KEY([ProposerId])
		REFERENCES [dbo].[txn_ProposerDetails] ([ProposerId])


		ALTER TABLE [dbo].[txn_BankDetails] CHECK CONSTRAINT [FK_txn_BankDetails_txn_ProposerDetails]

	END

Go

IF (Not EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES  WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'MstCheckSumKey'))
	BEGIN
			CREATE TABLE [dbo].[MstCheckSumKey](
			[CheckSumId] [int] IDENTITY(1,1) NOT NULL,
			[SourceName] [varchar](150) NULL,
			[HashKey] [nvarchar](max) NULL,
		 CONSTRAINT [PK_MstCheckSumKey] PRIMARY KEY CLUSTERED 
		(
			[CheckSumId] ASC
		)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
		) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

	END

Go

IF (Not EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES  WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'Txn_FipDetails'))
	BEGIN
			CREATE TABLE [dbo].[Txn_FipDetails](
			[Fid] [int] IDENTITY(1,1) NOT NULL,
			[ProposerId] [int] NULL,
			[txnid] [nvarchar](max) NULL,
			[fipid] [varchar](max) NULL,
			[fipname] [varchar](max) NULL,
			[PanNumber] [varchar](50) NULL,
			[consentid] [nvarchar](max) NULL,
			[maskedAccountNumber] [nvarchar](max) NULL,
			[accRefNumber] [nvarchar](max) NULL,
			[ActualTimestamp] [datetime] NULL,
			[CreatedAt] [datetime] NULL,
			[LastModifiedAt] [datetime] NULL,
			[Purpose] [varchar](50) NULL,
		 CONSTRAINT [PK_Txn_FipDetails] PRIMARY KEY CLUSTERED 
		(
			[Fid] ASC
		)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
		) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

		ALTER TABLE [dbo].[Txn_FipDetails]  WITH CHECK ADD  CONSTRAINT [FK_Txn_FipDetails_txn_ProposerDetails_ProposerId] FOREIGN KEY([ProposerId])
		REFERENCES [dbo].[txn_ProposerDetails] ([ProposerId])

		ALTER TABLE [dbo].[Txn_FipDetails] CHECK CONSTRAINT [FK_Txn_FipDetails_txn_ProposerDetails_ProposerId]

	END
Go

IF (Not EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES  WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'Txn_DetailsOfAccount'))
	BEGIN
			CREATE TABLE [dbo].[Txn_DetailsOfAccount](
			[DetailAccountId] [int] IDENTITY(1,1) NOT NULL,
			[Fid] [int] NULL,
			[maskedAccNumber] [nvarchar](max) NULL,
			[version] [decimal](18, 0) NULL,
			[linkedAccRef] [nvarchar](max) NULL,
			[type] [varchar](50) NULL,
		 CONSTRAINT [PK_Txn_DetailsOfAccount] PRIMARY KEY CLUSTERED 
		(
			[DetailAccountId] ASC
		)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
		) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

		ALTER TABLE [dbo].[Txn_DetailsOfAccount]  WITH CHECK ADD  CONSTRAINT [FK_Txn_FipDetails_Txn_DetailsOfAccount_Fid] FOREIGN KEY([Fid])
		REFERENCES [dbo].[Txn_FipDetails] ([Fid])

		ALTER TABLE [dbo].[Txn_DetailsOfAccount] CHECK CONSTRAINT [FK_Txn_FipDetails_Txn_DetailsOfAccount_Fid]

	END
Go

IF (Not EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES  WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'Txn_DetailsOfProfileHolders'))
	BEGIN
			CREATE TABLE [dbo].[Txn_DetailsOfProfileHolders](
			[ProfileId] [int] IDENTITY(1,1) NOT NULL,
			[Fid] [int] NULL,
			[type] [varchar](250) NULL,
			[Holder_Address] [nvarchar](max) NULL,
			[Holder_name] [varchar](250) NULL,
			[Holder_dob] [datetime] NULL,
			[Holder_mobile] [varchar](20) NULL,
			[Holder_nominee] [varchar](100) NULL,
			[Holder_email] [varchar](100) NULL,
			[Holder_pan] [varchar](20) NULL,
			[Holder_ckycCompliance] [bit] NULL,
		 CONSTRAINT [PK_Txn_DetailsOfProfileHolders] PRIMARY KEY CLUSTERED 
		(
			[ProfileId] ASC
		)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
		) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

		ALTER TABLE [dbo].[Txn_DetailsOfProfileHolders]  WITH CHECK ADD  CONSTRAINT [FK_Txn_FipDetails_Txn_DetailsOfProfileHolders_Fid] FOREIGN KEY([Fid])
		REFERENCES [dbo].[Txn_FipDetails] ([Fid])

		ALTER TABLE [dbo].[Txn_DetailsOfProfileHolders] CHECK CONSTRAINT [FK_Txn_FipDetails_Txn_DetailsOfProfileHolders_Fid]

	END
Go

IF (Not EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES  WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'Txn_DetailsOfSummary'))
	BEGIN
			CREATE TABLE [dbo].[Txn_DetailsOfSummary](
			[SummaryId] [int] IDENTITY(1,1) NOT NULL,
			[Fid] [int] NULL,
			[currentBalance] [decimal](18, 0) NULL,
			[currency] [varchar](50) NULL,
			[exchgeRate] [int] NULL,
			[balanceDateTime] [datetime] NULL,
			[type] [varchar](50) NULL,
			[branch] [nvarchar](250) NULL,
			[facility] [nvarchar](250) NULL,
			[ifscCode] [varchar](50) NULL,
			[micrCode] [varchar](50) NULL,
			[drawingLimit] [nvarchar](250) NULL,
			[status] [varchar](50) NULL,
			[openingDate] [date] NULL,
			[currentODLimit] [nvarchar](250) NULL,
		 CONSTRAINT [PK_Txn_DetailsOfSummary] PRIMARY KEY CLUSTERED 
		(
			[SummaryId] ASC
		)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
		) ON [PRIMARY]


		ALTER TABLE [dbo].[Txn_DetailsOfSummary]  WITH CHECK ADD  CONSTRAINT [FK_Txn_FipDetails_Txn_DetailsOfSummary_Fid] FOREIGN KEY([Fid])
		REFERENCES [dbo].[Txn_FipDetails] ([Fid])

		ALTER TABLE [dbo].[Txn_DetailsOfSummary] CHECK CONSTRAINT [FK_Txn_FipDetails_Txn_DetailsOfSummary_Fid]

	END
Go

IF (Not EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES  WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'Txn_Summary_DetailsPending'))
	BEGIN
			CREATE TABLE [dbo].[Txn_Summary_DetailsPending](
			[PendingId] [int] IDENTITY(1,1) NOT NULL,
			[amount] [decimal](18, 0) NULL,
			[transactionType] [varchar](150) NULL,
			[SummaryId] [int] NULL,
		 CONSTRAINT [PK_Txn_Summary_DetailsPending] PRIMARY KEY CLUSTERED 
		(
			[PendingId] ASC
		)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
		) ON [PRIMARY]


		ALTER TABLE [dbo].[Txn_Summary_DetailsPending]  WITH CHECK ADD  CONSTRAINT [FK_Txn_Summary_DetailsPending_Txn_DetailsOfSummary_SummaryId] FOREIGN KEY([SummaryId])
		REFERENCES [dbo].[Txn_DetailsOfSummary] ([SummaryId])

		ALTER TABLE [dbo].[Txn_Summary_DetailsPending] CHECK CONSTRAINT [FK_Txn_Summary_DetailsPending_Txn_DetailsOfSummary_SummaryId]

	END
Go

IF (Not EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES  WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'Txn_TransactionDetails'))
	BEGIN
			CREATE TABLE [dbo].[Txn_TransactionDetails](
			[TransactionId] [int] IDENTITY(1,1) NOT NULL,
			[Fid] [int] NULL,
			[txnId] [nvarchar](max) NULL,
			[type] [varchar](50) NULL,
			[mode] [varchar](50) NULL,
			[amount] [decimal](18, 0) NULL,
			[currentBalance] [decimal](18, 0) NULL,
			[transactionTimestamp] [datetime] NULL,
			[valueDate] [datetime] NULL,
			[narration] [nvarchar](max) NULL,
			[reference] [varchar](250) NULL,
		 CONSTRAINT [PK_Txn_TransactionDetails] PRIMARY KEY CLUSTERED 
		(
			[TransactionId] ASC
		)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
		) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

		ALTER TABLE [dbo].[Txn_TransactionDetails]  WITH CHECK ADD  CONSTRAINT [FK_Txn_FipDetails_Txn_TransactionDetails_Fid] FOREIGN KEY([Fid])
		REFERENCES [dbo].[Txn_FipDetails] ([Fid])

		ALTER TABLE [dbo].[Txn_TransactionDetails] CHECK CONSTRAINT [FK_Txn_FipDetails_Txn_TransactionDetails_Fid]

	END
Go

IF (Not EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES  WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'Txn_PdfBinary'))
	BEGIN
			CREATE TABLE [dbo].[Txn_PdfBinary](
			[BinaryId] [int] IDENTITY(1,1) NOT NULL,
			[ProposerId] [int] NULL,
			[PdfBinaryData] [nvarchar](max) NULL,
			 CONSTRAINT [PK_Txn_PdfBinary] PRIMARY KEY CLUSTERED 
			(
				[BinaryId] ASC
			)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
			) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
	

			ALTER TABLE [dbo].[Txn_PdfBinary]  WITH CHECK ADD  CONSTRAINT [FK_Txn_PdfBinary_txn_ProposerDetails_ProposerId] FOREIGN KEY([ProposerId])
			REFERENCES [dbo].[txn_ProposerDetails] ([ProposerId])

			ALTER TABLE [dbo].[Txn_PdfBinary] CHECK CONSTRAINT [FK_Txn_PdfBinary_txn_ProposerDetails_ProposerId]

	END
Go

IF (Not EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES  WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'Txn_PdfBase64'))
	BEGIN
			CREATE TABLE [dbo].[Txn_PdfBase64](
			[Base64Id] [int] IDENTITY(1,1) NOT NULL,
			[ProposerId] [int] NULL,
			[PdfBase64Data] [nvarchar](max) NULL,
			 CONSTRAINT [PK_Txn_PdfBase64] PRIMARY KEY CLUSTERED 
			(
				[Base64Id] ASC
			)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
			) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]


			ALTER TABLE [dbo].[Txn_PdfBase64]  WITH CHECK ADD  CONSTRAINT [FK_Txn_PdfBase64_txn_ProposerDetails_ProposerId] FOREIGN KEY([ProposerId])
			REFERENCES [dbo].[txn_ProposerDetails] ([ProposerId])


			ALTER TABLE [dbo].[Txn_PdfBase64] CHECK CONSTRAINT [FK_Txn_PdfBase64_txn_ProposerDetails_ProposerId]

	END
Go

IF (Not EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES  WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'Txn_PdfJson'))
	BEGIN
			CREATE TABLE [dbo].[Txn_PdfJson](
			[JsonId] [int] IDENTITY(1,1) NOT NULL,
			[ProposerId] [int] NULL,
			[PdfJsonData] [nvarchar](max) NULL,
			 CONSTRAINT [PK_Txn_PdfJson] PRIMARY KEY CLUSTERED 
			(
				[JsonId] ASC
			)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
			) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]


			ALTER TABLE [dbo].[Txn_PdfJson]  WITH CHECK ADD  CONSTRAINT [FK_Txn_PdfJson_txn_ProposerDetails_ProposerId] FOREIGN KEY([ProposerId])
			REFERENCES [dbo].[txn_ProposerDetails] ([ProposerId])


			ALTER TABLE [dbo].[Txn_PdfJson] CHECK CONSTRAINT [FK_Txn_PdfJson_txn_ProposerDetails_ProposerId]

	END
Go

IF (Not EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES  WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'Txn_PdfXml'))
	BEGIN
			CREATE TABLE [dbo].[Txn_PdfXml](
			[XmlId] [int] IDENTITY(1,1) NOT NULL,
			[ProposerId] [int] NULL,
			[PdfXmlData] [nvarchar](max) NULL,
			 CONSTRAINT [PK_Txn_PdfXml] PRIMARY KEY CLUSTERED 
			(
				[XmlId] ASC
			)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
			) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]


			ALTER TABLE [dbo].[Txn_PdfXml]  WITH CHECK ADD  CONSTRAINT [FK_Txn_PdfXml_txn_ProposerDetails_ProposerId] FOREIGN KEY([ProposerId])
			REFERENCES [dbo].[txn_ProposerDetails] ([ProposerId])


			ALTER TABLE [dbo].[Txn_PdfXml] CHECK CONSTRAINT [FK_Txn_PdfXml_txn_ProposerDetails_ProposerId]

	END
Go