USE [PhucThanh_Ecomerce]
GO
/****** Object:  StoredProcedure [dbo].[Sp_Product_Get_By_Filter]    Script Date: 4/20/2017 12:16:44 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER procedure [dbo].[Sp_Product_Get_By_Filter]
	@CateId				int				=	0,
	@MakerId			int				=	0,
	@Keyword			nvarchar(500)	=	'',
	@PageIndex			int				=	1,
	@PageSize			int				=	12,
	@Sort				varchar(50)		=	'CreatedDate desc',
	@MinPrice			decimal(18,2)	=	0,
	@MaxPrice			decimal(18,2)	=	0
as	
begin
		 
	
	
	;with ProductList as (
		select
			p.[Id],
			[MakerId],
			[ProductName],
			[ProductCode],
			[Description],
			[Images],
			[SellPrice],
			[ListedPrice],
			[Views],
			[CreatedDate],
			p.[IsHot],
			pc.CateID As CateId,
			dbo.[func_GetPicture](p.PictureId,0) as PicturePath
			
			
		from  Products p (nolock)
		left join ProductCate pc on p.Id = pc.ProductID
		left join Picture pt on p.PictureId = pt.Id
		where
		Published = 1
		And p.IsDel		=0
		And (@CateId	= 0 or (@CateId > 0 and p.CateId = @CateId Or pc.CateID = @CateId))
		And (@MakerId	= 0 or (@MakerId > 0 and MakerId = @MakerId))
		And (@Keyword	= '' or (@Keyword <> '' and (ProductName like '%'+ @Keyword +'%' Or ProductCode = @Keyword)))
		And (@MinPrice	= 0 or (@MinPrice > 0 and SellPrice >= @MinPrice))
		And (@MaxPrice	= 0 or (@MaxPrice > 0 and SellPrice <= @MaxPrice))
	)

	,ProductCount as (
		select  count(*) As TotalRow from ProductList
	)
	 
	select * 	 
	from ProductList p, ProductCount
	order by CreatedDate desc offset ((1 - 1)* 12) row fetch next 12 row only
end
