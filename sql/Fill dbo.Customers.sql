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
	Insert Into dbo.Customers("Name","NameOfEnglish", "GroupName","Address","Region","RegionSalesDirector","PartnerType", "Sales","ProductId", "Created")
	values ('customer-'+CAST(@id as nvarchar),
			'customer-'+CAST(@id as nvarchar),
			'Group-'+ CAST((CAST(RAND(CHECKSUM(NEWID())) * 10 as INT) + 1) as nvarchar),
			'Address-'+CAST(( CAST(RAND(CHECKSUM(NEWID())) * 10 as INT) + 1) as nvarchar),
			'CNC','Ru','TP',CAST(( CAST(RAND(CHECKSUM(NEWID())) * 100 as INT) + 1) as nvarchar),
						   CAST(RAND(CHECKSUM(NEWID())) * 10000 as INT) + 10000
								   ,GETDATE())
								   
   Print @id
   Set @id=@id+1
End
GO

