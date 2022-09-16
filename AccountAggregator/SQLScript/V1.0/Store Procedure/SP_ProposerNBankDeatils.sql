USE [DbAccountAggregator]
GO

/****** Object:  StoredProcedure [dbo].[SP_ProposerNBankDeatils]    Script Date: 22-08-2022 12:30:06 ******/
DROP PROCEDURE [dbo].[SP_ProposerNBankDeatils]
GO

/****** Object:  StoredProcedure [dbo].[SP_ProposerNBankDeatils]    Script Date: 22-08-2022 12:30:06 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[SP_ProposerNBankDeatils]
	@ProposerDetails dbo.UT_ProposerDetails READONLY,
	@BankDetails dbo.UT_BankDetails READONLY,
	@BankTransactionId INT OUTPUT,
	@ProposerId INT OUTPUT,
	@GuidForProposer NVARCHAR(max) OUTPUT
AS
BEGIN

	SET NOCOUNT ON;

	BEGIN TRANSACTION;

	BEGIN TRY
		
		INSERT INTO [dbo].[txn_ProposerDetails](ApplicationNumber,ProductName,FirstName,LastName,DateOfBirth,Gender,
												EmailId,MobileNo,MaritalStatus,PANCardNo,policynumber,SourceName,CreatedAt,LastModifiedAt,TxtGuid)
										 SELECT ApplicationNumber,ProductName,FirstName,LastName,DateOfBirth,Gender,
												EmailId,MobileNo,MaritalStatus,PANCardNo,policynumber,SourceName,GETDATE(),GETDATE(),NEWID() 
										 FROM @ProposerDetails
		SET @ProposerId = @@IDENTITY;

		SELECT @GuidForProposer = TxtGuid FROM [dbo].[txn_ProposerDetails] where ProposerId = @ProposerId

		INSERT INTO [dbo].[txn_BankDetails](ProposerId,AccountNumber,AccountHolderName,IFSCCode,AccountType,BankName,BranchName)
									SELECT @ProposerId,AccountNumber,AccountHolderName,IFSCCode,AccountType,BankName,BranchName 
									FROM @BankDetails
		
		SET @BankTransactionId = @@IDENTITY;

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


