USE [QuoteFlowV2]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[usp_Dashboard_Approval]
	@username nvarchar(100)
	, @hasPermissionConfirmedDPO bit = 0
AS
BEGIN
	SET NOCOUNT ON;

	declare @DashboardItems table
	(
		Code nvarchar(50)
		, Title nvarchar(100)
		, SortOrder int
	)
	insert into @DashboardItems (Code, Title, SortOrder)
		values	('SPO', 'SPO', 1)
				, ('DPO', 'DPO', 2)
				, ('Material', 'Material Data Import', 3)

	---------------------
	;with tblSPO as (
		select Code, In_Approval = sum(In_Approval), ProjectResult = sum(ProjectResult)
		from (
			select Code = 'SPO', In_Approval = count(*), ProjectResult = 0
			from ApprovalRoute r with(nolock)
				inner join PriceOffer req with(nolock) on req.Id = r.PriceOfferId
					and r.EntityType = 'PriceOffer'
					and req.CurrentApprovalStepSequence = r.StepSequence
					and req.CurrentApproverRoleCode is not null
					and r.Approver = @username
			group by r.Approver
			union
			select Code = 'SPO', In_Approval = 0, ProjectResult = count(*)
			from SaleTeam s with(nolock)
					inner join PriceOffer req with(nolock) on req.MaterialType = s.MaterialType
						and req.BuyerId = s.BuyerId
						and req.LocationId = s.LocationId
						and req.ApprovalStatus = 'APPROVED'
						and req.ProjectResultStatus = 'PRE_ORDER'
						and s.SaleUserName = @username
			group by s.SaleUserName
		) spo
		group by code
	)
	------DPO
	, tblDPO as (
		select Code = 'DPO', In_Approval = count(*)
		from DPO dpo with(nolock)
		where status = 'SUBMITTED'
				and dpo.DPOType = 'DPO'
				and isnull(dpo.IsDeleted,0) = 0
				and @hasPermissionConfirmedDPO = 1
	)
	------Material
	, tblMaterial as (
		select Code = 'Material', In_Approval = count(*)
		from ApprovalRoute r with(nolock)
			inner join MaterialApprovalRequest req with(nolock) on req.Id = r.MaterialApprovalRequestId
				and req.CurrentApprovalStepSequence = r.StepSequence
				and req.CurrentApproverRoleCode is not null
				and r.Approver = @username
		group by r.Approver
	)

	select d.Title
		, In_Approval = case when d.Code = 'SPO'      then spo.In_Approval
							 when d.Code = 'DPO'      then dpo.In_Approval
							 when d.Code = 'Material' then m.In_Approval
						end
		, SetProjectResult = case when d.Code = 'SPO' then spo.ProjectResult
								  else 0 end
	from @DashboardItems d
		left outer join tblSPO      spo on d.Code = spo.Code
		left outer join tblDPO      dpo on d.Code = dpo.Code
		left outer join tblMaterial m   on d.Code = m.Code
	order by d.SortOrder
END
