import { Directive, OnInit, ViewChild, inject } from '@angular/core';

import { ListService, PermissionService, TrackByService } from '@abp/ng.core';

import { ToasterService } from '@abp/ng.theme.shared';
import { Router } from '@angular/router';
import { AppRoutes } from '@app/app.routes';
import { ApproversModalComponent } from '@app/shared/components/approvers-modal/approvers-modal.component';
import { DEFAULT_PAGE_SIZE } from '@app/shared/constants';
import { TitleService } from '@app/shared/services/title/title.service';
import { KeyAccountDto } from '@proxy/key-accounts';
import { MaterialApprovalRequestDto } from '@proxy/materials/material-approval-requests';
import { MaterialViewService } from '../../services/material.service';

export const ChildTabDependencies = [];

export const ChildComponentDependencies = [];

@Directive()
export abstract class AbstractMyApprovalsMaterialsComponent implements OnInit {
  protected readonly router = inject(Router);
  public readonly list = inject(ListService);
  public readonly track = inject(TrackByService);
  public readonly service = inject(MaterialViewService);
  public readonly permissionService = inject(PermissionService);
  public readonly titleService = inject(TitleService);
  public readonly toast = inject(ToasterService);

  protected title = 'Material Approval';

  showHistoryModal = false;
  approvalHistories = [];

  @ViewChild('approversModalComponent', { static: false }) approversModalComponent: ApproversModalComponent;

  ngOnInit() {
    this.list.maxResultCount = DEFAULT_PAGE_SIZE;
    this.titleService.setTitle('Material Approval | Material Management');
    this.service.hookToQueryApproval();
  }

  getDetailUrl(request: KeyAccountDto): string {
    if (request?.id) {
      return this.router.serializeUrl(
        this.router.createUrlTree([
          AppRoutes.MATERIAL_STOCK.BASE,
          AppRoutes.DETAILS_WITH_ID(request.id),
          AppRoutes.MATERIAL_STOCK.DETAILS.BASE,
          AppRoutes.MATERIAL_STOCK.MY_APPROVALS.BASE,
        ]),
      );
    } else {
      console.error('Unknown key account:', request.id);
      return '';
    }
  }

  loadApprovalList(rowData: MaterialApprovalRequestDto) {
    if (rowData?.id) {
      this.service.getListApprovers(rowData.id).subscribe({
        next: approvers => {
          this.approversModalComponent.openModal(approvers);
        },
        error: () => {
          this.toast.error('Failed to load approvers.');
        },
      });
    }
  }

  showHistory(record: MaterialApprovalRequestDto) {
    this.showHistoryModal = true;
    this.approvalHistories = record.materialHistories;
  }

  closeHistoryDialog(): void {
    this.showHistoryModal = false;
    this.approvalHistories = [];
  }

  clearFilters() {
    this.service.clearFiltersApproval();
  }

  onSearch() {
    this.list.get();
  }

  exportToExcel() {
    this.service.exportToExcel();
  }
}
