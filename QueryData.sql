select po.PriceOffer_Code,po.ProjectName,po.LocationDescription ,po.ApprovalStatus
	,po.BuyerCode
	,pod.GolfaCode, pod.ModelName, pod.Qty, pod.StandardPrice,pod.LandingCost, pod.MEVNOfferPrice, po.CreationTime
from PriceOfferDetail pod
	inner join PriceOffer po on po.Id= pod.PriceOfferId
where po.ApprovalStatus in ('APPROVED')
	and CONVERT(date, po.CreationTime) >= '2025-04-01'
	and Convert(date, po.CreationTime) <= '2026-03-31'
order by po.CreationTime desc

select m.Id as IDSanPham,sod.GolfaCode,m.Standard_Price,m.LandedCost,sum(sod.Qty) as SoLuongBanTheo1Nam
from SaleOrderDetail sod 
	inner join SaleOrder so on so.Id = sod.SaleOrderId 
	inner join Materials m on m.GolfaCode = sod.GolfaCode
where sod.GolfaCode = '2DD005A000006'
	and sod.IsDeleted = 0
	and so.IsDeleted = 0
	and so.StatusCode = 'CLOSED'
	and CONVERT(date, so.SAPInvoiceDate) >= '2025-04-01'
	and Convert(date, so.SAPInvoiceDate) <= '2026-03-31'
group by m.Id, sod.GolfaCode,m.Standard_Price,m.LandedCost


declare @fromDate date = '2026-04-01',
		@toDate date = '2026-05-20'

select m.Id as IDSanPham,sod.GolfaCode,m.MaterialType,m.Standard_Price,m.LandedCost,sum(sod.Qty) as SoLuongBanTheo1Nam
from SaleOrderDetail sod 
	inner join SaleOrder so on so.Id = sod.SaleOrderId 
	inner join Materials m on m.GolfaCode = sod.GolfaCode
where 1=1
	and sod.IsDeleted = 0
	and so.IsDeleted = 0
	and so.StatusCode = 'CLOSED'
	and CONVERT(date, so.SAPInvoiceDate) >= @fromDate
	and Convert(date, so.SAPInvoiceDate) <= @toDate
group by m.Id, sod.GolfaCode,m.Standard_Price,m.LandedCost,m.MaterialType
order by SoLuongBanTheo1Nam desc



select m.Id as IDSanPham,sod.GolfaCode,m.Standard_Price,m.LandedCost,sum(sod.Qty) as SoLuongBanTuDauQuyDenHienTai
from SaleOrderDetail sod 
	inner join SaleOrder so on so.Id = sod.SaleOrderId 
	inner join Materials m on m.GolfaCode = sod.GolfaCode
where sod.GolfaCode = '2DD005A000006'
	and sod.IsDeleted = 0
	and so.IsDeleted = 0
	and so.StatusCode = 'CLOSED'
	and CONVERT(date, so.SAPInvoiceDate) >= '2025-04-01'
	and Convert(date, so.SAPInvoiceDate) <= '2026-03-31'
group by m.Id, sod.GolfaCode,m.Standard_Price,m.LandedCost

select po.PriceOffer_Code,po.ProjectName, po.ApprovalStatus
	,pod.GolfaCode, pod.ModelName, pod.Qty, pod.StandardPrice,pod.LandingCost, pod.MEVNOfferPrice, po.CreationTime
	, history.Note
from PriceOfferDetail pod
	inner join PriceOffer po on po.Id= pod.PriceOfferId
	inner join ApprovalHistories history on history.PriceOfferId = po.Id 
		and history.action	 = 'Rejected'
where po.ApprovalStatus in ('REJECTED')
	--and po.BuyerCode = 'SaGiang'
	and po.CreationTime >= '2025-04-01'
	and po.CreationTime <= '2026-03-31'
order by po.CreationTime desc, po.PriceOffer_Code


