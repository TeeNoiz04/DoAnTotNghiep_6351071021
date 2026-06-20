USE [QuoteFlowV2]
GO
/****** Object:  StoredProcedure [dbo].[usp_TimerJob_Materials_UpdateStatus]    Script Date: 6/20/2026 10:58:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*==============================================================================================
   Author			Date			Description
   ---------------------------------------------
   Lam Dat			20.07.2025		Run after approve, not timer job
============================================================================================= */
ALTER PROCEDURE [dbo].[usp_TimerJob_Materials_UpdateStatus]
	@prRequestId uniqueidentifier = null
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @today DATE = CONVERT(DATE, GETDATE());
	--============================================================================
	declare  @requestId uniqueidentifier;
	set @requestId = @prRequestId
	---------------------------------------------
	if (@requestId is not null )
	begin
		select d.Id as RequestDetailId, d.CreatorUsername, d.CreatorName
				, m.GolfaCode, m.MaterialStatus
				, Status_Update =  case when d.[Action] = 'Active'
												and (m.MaterialStatus = 'Deactive' or m.MaterialStatus = 'Discontinue') then  'Active'
										when (d.[Action] = 'Discontinue' and m.MaterialStatus = 'Active') then 'Discontinue'
										when d.[Action] = 'Deactive'
													and (m.MaterialStatus = 'Active' )
													and (ISNULL(ms.Available_Qty, 0)  > 0
															or ISNULL(ms.Locked, 0) > 0
															or ISNULL(ms.LockStockKeeping, 0) = 0
															or ISNULL(ms.LockStockSO, 0) = 0 )
												then  'Discontinue'
										when d.[Action] = 'Deactive' or d.[Action] = 'Discontinue'
													and (m.MaterialStatus = 'Active' or m.MaterialStatus = 'Discontinue')
													and ISNULL(ms.Available_Qty, 0)  = 0
													and ISNULL(ms.Locked, 0) = 0
													and ISNULL(ms.LockStockKeeping, 0) = 0
													and ISNULL(ms.LockStockSO, 0) = 0
												then  'Deactive'
									end
			into #MaterialRequestList
		FROM Materials m
			inner join MaterialApprovalRequestDetail d ON m.GolfaCode = d.GolfaCode
			inner join MaterialApprovalRequest mr on d.MaterialApprovalId = mr.Id
				and mr.ImportType = 'MATERIAL.STATUS'
				and mr.Status = 'APPROVED'
				and d.MaterialApprovalId = @requestId
				and d.ActionDate <= @today
				and mr.CreationTime >= '2025-10-01' --just run timer job for request since October 2025
			left outer join (
				select GolfaCode
						, Available_Qty		= SUM(a.Available_Qty)
						, Locked			=  SUM(a.Locked)
						, LockStockKeeping	= SUM(a.LockStockKeeping)
						, LockStockSO		= SUM(a.LockStockSO)
				from MaterialStock a
						inner join StockCategory sc on a.StockCategoryId = sc.Id
								and isnull(sc.DamagedStock, 0) = 0
				group by GolfaCode
			) ms on ms.GolfaCode = m.GolfaCode;

		-----------------------------
		-- Update Material status
		UPDATE m SET MaterialStatus = t.Status_Update
		FROM Materials m
				inner join #MaterialRequestList t on m.GolfaCode = t.GolfaCode
					and t.Status_Update is not null
					and t.MaterialStatus <> t.Status_Update ;
		-----------------------------------------------------
		-- Update Material status Update Request detail
		UPDATE d SET ExtraProperties  = JSON_MODIFY(ExtraProperties  , '$.TimejobRun', '1')
		from MaterialApprovalRequestDetail d
			inner join #MaterialRequestList t on d.Id = t.RequestDetailId
		-----------------------------
		--Write history
		INSERT INTO [dbo].[HistoryTracking]
				([Id]
				,[TrackingType]
				,[Action]
				,[ObjectId]
				,[GolfaCode]
				,[BeforeChange]
				,[AfterChange]
				,[ExtraProperties]
				,[ConcurrencyStamp]
				,[CreatorId]
				,[CreatorUsername]
				,[CreatorName]
				,[CreationTime])
		select [Id] = NEWID ()
				,[TrackingType] = 'Material'
				,[Action] = 'UPDATE STATUS'
				,[ObjectId] = null
				,[GolfaCode] = t.GolfaCode
				,[BeforeChange] = t.MaterialStatus
				,[AfterChange] = t.Status_Update
				,[ExtraProperties] = '{}'
				,[ConcurrencyStamp] = NEWID ()
				,[CreatorId] = null
				,[CreatorUsername] = t.CreatorUsername
				,[CreatorName] = t.CreatorName
				,[CreationTime] = GETDATE ()
		from #MaterialRequestList t
		where t.Status_Update is not null
				and t.MaterialStatus <> t.Status_Update ;
		-----------------------------
		Return
	end
	--============================================================================================
	--Timer job
	if (@requestId is not null )
		return;

	select d.Id as RequestDetailId, d.CreatorUsername, d.CreatorName
				, m.GolfaCode, m.MaterialStatus
				, Status_Update =  case when d.[Action] = 'Active'
												and (m.MaterialStatus = 'Deactive' or m.MaterialStatus = 'Discontinue') then  'Active'
										when (d.[Action] = 'Discontinue' and m.MaterialStatus = 'Active') then 'Discontinue'
										when d.[Action] = 'Deactive'
													and (m.MaterialStatus = 'Active' )
													and (ISNULL(ms.Available_Qty, 0)  > 0
															or ISNULL(ms.Locked, 0) > 0
															or ISNULL(ms.LockStockKeeping, 0) = 0
															or ISNULL(ms.LockStockSO, 0) = 0 )
												then  'Discontinue'
										when d.[Action] = 'Deactive' or d.[Action] = 'Discontinue'
													and (m.MaterialStatus = 'Active' or m.MaterialStatus = 'Discontinue')
													and ISNULL(ms.Available_Qty, 0)  = 0
													and ISNULL(ms.Locked, 0) = 0
													and ISNULL(ms.LockStockKeeping, 0) = 0
													and ISNULL(ms.LockStockSO, 0) = 0
												then  'Deactive'
									end
			into #MaterialStatusTimer
		FROM Materials m
			inner join MaterialApprovalRequestDetail d ON m.GolfaCode = d.GolfaCode
			inner join MaterialApprovalRequest mr on d.MaterialApprovalId = mr.Id
				and mr.ImportType = 'MATERIAL.STATUS'
				and mr.Status = 'APPROVED'
				and d.ActionDate <= @today
				and JSON_VALUE(d.ExtraProperties, '$.TimejobRun') is null
			left outer join (
				select GolfaCode
						, Available_Qty		= SUM(a.Available_Qty)
						, Locked			=  SUM(a.Locked)
						, LockStockKeeping	= SUM(a.LockStockKeeping)
						, LockStockSO		= SUM(a.LockStockSO)
				from MaterialStock a
						inner join StockCategory sc on a.StockCategoryId = sc.Id
								and isnull(sc.DamagedStock, 0) = 0
				group by GolfaCode
			) ms on ms.GolfaCode = m.GolfaCode;

		-----------------------------
		-- Update Material status
		UPDATE m SET MaterialStatus = t.Status_Update
		FROM Materials m
				inner join #MaterialStatusTimer t on m.GolfaCode = t.GolfaCode
					and t.Status_Update is not null
					and t.MaterialStatus <> t.Status_Update ;
		-----------------------------------------------------
		-- Update Material status Update Request detail
		UPDATE d SET ExtraProperties  = JSON_MODIFY(ExtraProperties  , '$.TimejobRun', '1')
		from MaterialApprovalRequestDetail d
			inner join #MaterialStatusTimer t on d.Id = t.RequestDetailId
		-----------------------------
		--Write history
		INSERT INTO [dbo].[HistoryTracking]
				([Id]
				,[TrackingType]
				,[Action]
				,[ObjectId]
				,[GolfaCode]
				,[BeforeChange]
				,[AfterChange]
				,[ExtraProperties]
				,[ConcurrencyStamp]
				,[CreatorId]
				,[CreatorUsername]
				,[CreatorName]
				,[CreationTime])
		select [Id] = NEWID ()
				,[TrackingType] = 'Material'
				,[Action] = 'UPDATE STATUS'
				,[ObjectId] = null
				,[GolfaCode] = t.GolfaCode
				,[BeforeChange] = t.MaterialStatus
				,[AfterChange] = t.Status_Update
				,[ExtraProperties] = '{}'
				,[ConcurrencyStamp] = NEWID ()
				,[CreatorId] = null
				,[CreatorUsername] = t.CreatorUsername
				,[CreatorName] = t.CreatorName
				,[CreationTime] = GETDATE ()
		from #MaterialStatusTimer t
		where t.Status_Update is not null
				and t.MaterialStatus <> t.Status_Update ;
END
