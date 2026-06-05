import { Directive, OnInit, ViewChild, inject } from '@angular/core';

import { ListService, PermissionService, TrackByService } from '@abp/ng.core';
import { ToasterService } from '@abp/ng.theme.shared';
import { Router } from '@angular/router';
import { AppRoutes } from '@app/app.routes';
import { PSIViewService } from '@app/psis/services/psi.service';
import { ApproversModalComponent } from '@app/shared/components/approvers-modal/approvers-modal.component';
import { DEFAULT_PAGE_SIZE } from '@app/shared/constants';
import { TitleService } from '@app/shared/services/title/title.service';
import { PSIDto } from '@proxy/psis';

export const ChildTabDependencies = [];

export const ChildComponentDependencies = [];

@Directive()
export abstract class AbstractMyApprovalsComponent implements OnInit {
  protected readonly router = inject(Router);
  public readonly list = inject(ListService);
  public readonly track = inject(TrackByService);
  public readonly service = inject(PSIViewService);
  public readonly toast = inject(ToasterService);
  public readonly titleService = inject(TitleService);
  public readonly permissionService = inject(PermissionService);

  protected title = 'PSI Approval';
  protected isActionButtonVisible: boolean | null = null;

  showImportPSI = false;
  showHistoryModal = false;
  approvalHistories = [];

  @ViewChild('approversModalComponent', { static: false }) approversModalComponent: ApproversModalComponent;

  ngOnInit() {
    this.list.maxResultCount = DEFAULT_PAGE_SIZE;
    this.titleService.setTitle('PSI Approval');
    this.service.hookToQueryApproval();
  }

  getDetailUrl(request: PSIDto): string {
    if (request?.id) {
      return this.router.serializeUrl(
        this.router.createUrlTree([
          AppRoutes.PSI.BASE,
          AppRoutes.DETAILS_WITH_ID(request.id),
          AppRoutes.PSI.DETAILS.BASE,
          AppRoutes.MATERIAL_STOCK.MY_APPROVALS.BASE,
        ]),
      );
    } else {
      console.error('Unknown key account:', request.id);
      return '';
    }
  }

  showHistory(record: PSIDto) {
    this.showHistoryModal = true;
    this.approvalHistories = record?.approvalHistories;
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

  loadApprovalList(rowData: PSIDto) {
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
}
