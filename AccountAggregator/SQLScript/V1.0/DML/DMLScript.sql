IF NOT EXISTS (select * from [dbo].[MstTypeOfRequest] WHERE TypeName = 'BOL')
BEGIN
	INSERT INTO [dbo].[MstTypeOfRequest](TypeName,CreatedAt,LastModifiedAt)
	values('BOL',GETDATE(),GETDATE())
END
Go
IF NOT EXISTS (select * from [dbo].[MstTypeOfRequest] WHERE TypeName = 'Authencation')
BEGIN
	INSERT INTO [dbo].[MstTypeOfRequest](TypeName,CreatedAt,LastModifiedAt)
	values('Authencation',GETDATE(),GETDATE())
END
Go
IF NOT EXISTS (select * from [dbo].[MstTypeOfRequest] WHERE TypeName = 'RedirectionUrl')
BEGIN
	INSERT INTO [dbo].[MstTypeOfRequest](TypeName,CreatedAt,LastModifiedAt)
	values('RedirectionUrl',GETDATE(),GETDATE())
END
Go
IF NOT EXISTS (select * from [dbo].[MstTypeOfRequest] WHERE TypeName = 'ConsentStatusNotification')
BEGIN
	INSERT INTO [dbo].[MstTypeOfRequest](TypeName,CreatedAt,LastModifiedAt)
	values('ConsentStatusNotification',GETDATE(),GETDATE())
END
Go
IF NOT EXISTS (select * from [dbo].[MstTypeOfRequest] WHERE TypeName = 'DownloadStatement')
BEGIN
	INSERT INTO [dbo].[MstTypeOfRequest](TypeName,CreatedAt,LastModifiedAt)
	values('DownloadStatement',GETDATE(),GETDATE())
END
Go

IF NOT EXISTS (select * from [dbo].[MstCheckSumKey] where SourceName = 'Bol' and HashKey = 'BC46ECC0A1024F4889ADA48A0FA8A9EE')
BEGIN
	INSERT INTO [dbo].[MstCheckSumKey](SourceName,HashKey)
	values('Bol','BC46ECC0A1024F4889ADA48A0FA8A9EE')
END