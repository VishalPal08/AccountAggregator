USE [DbAccountAggregator]
GO

/****** Object:  StoredProcedure [dbo].[SP_StoreTransactionDetails]    Script Date: 22-08-2022 12:34:24 ******/
DROP PROCEDURE [dbo].[Sp_PushJsonData]
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
CREATE PROCEDURE [dbo].[Sp_PushJsonData]
	@Account_Json NVARCHAR(MAX),
	@FinancialId INT,
	@Message Varchar(max) OUTPUT
AS
BEGIN
	
	DECLARE @HolderType Varchar(100)

	SET NOCOUNT ON;
	BEGIN TRANSACTION;
		BEGIN TRY
			
			IF EXISTS(SELECT Count(1) FROM OpenJson(@Account_Json) WITH(maskedAccNumber NVARCHAR(MAX) '$.maskedAccNumber',
							version NVARCHAR(MAX) '$.version',linkedAccRef NVARCHAR(MAX) '$.linkedAccRef',type VARCHAR(150) '$.type'))
			BEGIN
				INSERT INTO [dbo].[Txn_DetailsOfAccount](Fid,maskedAccNumber,version,linkedAccRef,type)
				SELECT @FinancialId , * FROM OpenJson(@Account_Json)
				WITH ( maskedAccNumber NVARCHAR(MAX) '$.maskedAccNumber',
						version NVARCHAR(MAX) '$.version',
						linkedAccRef NVARCHAR(MAX) '$.linkedAccRef',
						type VARCHAR(150) '$.type')
			END
			ELSE
			BEGIN
				INSERT INTO [dbo].[Txn_DetailsOfAccount](Fid,maskedAccNumber,version,linkedAccRef,type)
				Values(@FinancialId,null,null,null,null)

				SET @Message = @Message +'|' + 'maskedAccNumber,version,linkedAccRef and type  does not exist in Account section';
			END


			SELECT @HolderType = HolderType FROM OPENJSON(@Account_Json,'$.Profile.Holders')
			WITH(HolderType Varchar(100) '$.type')

			IF(@HolderType != '' OR @HolderType is not null)
			BEGIN
				IF EXISTS(SELECT count(1) FROM OPENJSON(@Account_Json,'$.Profile.Holders.Holder'))
				BEGIN						
					INSERT INTO [dbo].[Txn_DetailsOfProfileHolders](Fid,type,Holder_Address,Holder_name,Holder_dob,
																Holder_mobile,Holder_nominee,Holder_email,
																Holder_pan,Holder_ckycCompliance)
						SELECT @FinancialId,@HolderType,* FROM OPENJSON(@Account_Json,'$.Profile.Holders.Holder')
						WITH(   Address  NVARCHAR(MAX) '$.address',
								name  NVARCHAR(MAX) '$.name',
								dob  Datetime '$.dob',
								mobile  VARCHAR(20) '$.mobile',
								nominee  NVARCHAR(MAX) '$.nominee',
								email  VARCHAR(250) '$.email',
								pan  VARCHAR(20) '$.pan',
								ckycCompliance  bit '$.ckycCompliance')
				END
				ELSE
				BEGIN
					INSERT INTO [dbo].[Txn_DetailsOfProfileHolders](Fid,type,Holder_Address,Holder_name,Holder_dob,
																Holder_mobile,Holder_nominee,Holder_email,
																Holder_pan,Holder_ckycCompliance)
						Values(@FinancialId,@HolderType,null,null,null,null,null,null,null,null)

						SET @Message = @Message +'|' + 'Holder details does not exist in Account:Profile:Holders:Holder section';
				END
			END
			ELSE
			BEGIN
				SET @Message = @Message +'|' + 'Holder type does not exist in Account:Profile:Holders section';
			END



			IF EXISTS(SELECT Count(1) FROM OPENJSON(@Account_Json,'$.Summary'))
			BEGIN 
					
				INSERT INTO [dbo].[Txn_DetailsOfSummary](Fid,currentBalance,currency,exchgeRate,balanceDateTime,
															type,branch,facility,ifscCode,micrCode,drawingLimit,
															status,openingDate,currentODLimit)
				SELECT @FinancialId,* FROM OPENJSON(@Account_Json,'$.Summary')
				WITH(	currentBalance  decimal(18,2) '$.currentBalance',
						currency  VARCHAR(250) '$.currency',
						exchgeRate  int '$.exchgeRate',
						balanceDateTime  datetime2  '$.balanceDateTime',
						type  VARCHAR(150) '$.type',
						branch  NVARCHAR(MAX) '$.branch',
						facility  NVARCHAR(MAX) '$.facility',
						ifscCode  NVARCHAR(150) '$.ifscCode',
						micrCode  NVARCHAR(150) '$.micrCode',
						drawingLimit  NVARCHAR(250) '$.drawingLimit',
						status  VARCHAR(50) '$.status',
						openingDate  datetime2  '$.openingDate',
						currentODLimit  NVARCHAR(250) '$.currentODLimit')

			END
			ELSE
			BEGIN
				INSERT INTO [dbo].[Txn_DetailsOfSummary](Fid,currentBalance,currency,exchgeRate,balanceDateTime,
															type,branch,facility,ifscCode,micrCode,drawingLimit,
															status,openingDate,currentODLimit)
				Values(@FinancialId,null,null,null,null,null,null,null,null,null,null,null,null,null)

				SET @Message = @Message +'|' + 'Summary details does not exist in Account:Summary section';
			END

			SET @Message = COALESCE(CAST(@Message AS VARCHAR (10)), '')   +'|' +'Data successfully save in DB';

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


