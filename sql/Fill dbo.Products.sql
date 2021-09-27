USE [CleanArchitecture.RazorDb]
GO

/****** Object: Table [dbo].[Products] Script Date: 06.09.2021 16:54:29 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

Declare @UserId nvarchar(max)
Set @UserId=(select Id from dbo.AspNetUsers where Email='new163@163.com')
Print @UserId


declare @id int
set @id=1

While @id<10000
Begin
	Insert Into dbo.Products("Name","Price", "Description","Created")
	values ('Product-'+CAST(@id as nvarchar),
	   	CONVERT( DECIMAL(13, 4), 10 + (30-10)*RAND(CHECKSUM(NEWID()))),
								   'Auto fill data: Product-'+CAST(@id as nvarchar)
								   ,GETDATE())
								   
   Print @id
   Set @id=@id+1
End
GO

