USE [DbAccountAggregator]
GO

/****** Object:  StoredProcedure [dbo].[SP_StoreTransactionDetails]    Script Date: 22-08-2022 12:34:24 ******/
DROP PROCEDURE [dbo].[SP_StoreTransactionDetails]
GO

/****** Object:  StoredProcedure [dbo].[SP_StoreTransactionDetails]    Script Date: 22-08-2022 12:34:24 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[SP_StoreTransactionDetails]
	@purpose VARCHAR(100),
	@ver DECIMAL(18,2),
	@timestamp DateTime,
	@txnid NVARCHAR(MAX),
	@clienttxnid NVARCHAR(MAX),
	@customerId VARCHAR(250),
	@fipid VARCHAR(250),
	@fipname VARCHAR(250),
	@pan VARCHAR(20),
	@JsonData NVARCHAR(MAX),
	@PdfBase64 NVARCHAR(MAX),
	@PdfBinary NVARCHAR(MAX),
	@XMLData NVARCHAR(MAX),
	@consentid NVARCHAR(MAX),
	@maskedAccountNumber VARCHAR(250),
	@PdfDetailsFlg INT,
	@Message Varchar(max) OUTPUT
AS
BEGIN
	DECLARE @ProposerId INT,
			@FinancialId INT,
			@HolderType Varchar(100)

	SET NOCOUNT ON;

	BEGIN TRANSACTION;
		BEGIN TRY
			
			IF(@PdfDetailsFlg = 3)
			BEGIN
				SELECT @ProposerId = ProposerId FROM [dbo].[txn_ProposerDetails] with(nolock) WHERE TxtGuid = @clienttxnid
			
				IF(@ProposerId is not NULL OR @ProposerId != '')
				BEGIN
					INSERT INTO [dbo].[Txn_FipDetails](ProposerId,txnid,fipid,fipname,PanNumber,consentid,maskedAccountNumber
												,ActualTimestamp,CreatedAt,LastModifiedAt,Purpose)
										   VALUES(@ProposerId,@txnid,@fipid,@fipname,@pan,@consentid,@maskedAccountNumber
												,@timestamp,GETDATE(),GETDATE(),@purpose)

					SET @FinancialId = @@IDENTITY;

					IF(@JsonData is not null Or @JsonData != '')
					BEGIN
						EXEC [dbo].[Sp_PushJsonData] @JsonData,@FinancialId,@Message OUTPUT
					
						INSERT INTO [dbo].[Txn_PdfJson](ProposerId,PdfJsonData)
											 VALUES(@ProposerId,@JsonData)
					END

					
				END
				ELSE
				BEGIN 
					SET @Message =  @Message +'|' +'Proposer id does not exit for particular client transaction Id. kindly check your client transaction Id';
				END
			END

			ELSE IF(@PdfDetailsFlg = 1)
			BEGIN
				IF(@PdfBase64 is not null Or @PdfBase64 != '')
				BEGIN
					INSERT INTO [dbo].[Txn_PdfBase64](ProposerId,PdfBase64Data)
											 VALUES(@ProposerId,@PdfBase64)
				END
			END

			ELSE IF(@PdfDetailsFlg = 2)
			BEGIN
				IF(@PdfBinary is not null Or @PdfBinary != '')
				BEGIN
					INSERT INTO [dbo].[Txn_PdfBinary](ProposerId,PdfBinaryData)
											 VALUES(@ProposerId,@PdfBinary)
				END
			END

			ELSE IF(@PdfDetailsFlg = 4)
			BEGIN
				IF(@XMLData is not null Or @XMLData != '')
				BEGIN
					INSERT INTO [dbo].[Txn_PdfXml](ProposerId,PdfXmlData)
											 VALUES(@ProposerId,@XMLData)
				END
				
			END

			COMMIT TRANSACTION;

		END TRY
		BEGIN CATCH
			SELECT 
				ERROR_NUMBER() As ErrorNumber,
				ERROR_STATE() As ErrorState,
				ERROR_SEVERITY() As ErrorSeverity,
				ERROR_PROCEDURE() As ErrorProcedure,
				ERROR_LINE() As ErrorLine,
				ERROR_MESSAGE() ErrorMessage;

			IF @@TRANCOUNT > 0
			BEGIN
				ROLLBACK TRANSACTION;
			END
		END CATCH

END

GO


