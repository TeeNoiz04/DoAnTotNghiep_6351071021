import { Directive, OnInit, ViewChild, inject } from '@angular/core';

import { ListService, PermissionService, TrackByService } from '@abp/ng.core';

import { ToasterService } from '@abp/ng.theme.shared';
import { Router } from '@angular/router';
import { AppRoutes } from '@app/app.routes';
import { PriceOfferDetailViewService } from '@app/price-offers/services/price-offer-detail.service';
import { PriceOfferViewService } from '@app/price-offers/services/price-offer.service';
import { ApproversModalComponent } from '@app/shared/components/approvers-modal/approvers-modal.component';
import { DEFAULT_PAGE_SIZE } from '@app/shared/constants';
import { TitleService } from '@app/shared/services/title/title.service';
import { KeyAccountDto } from '@proxy/key-accounts';
import { PriceOfferDto } from '@proxy/price-offers';

export const ChildTabDependencies = [];

export const ChildComponentDependencies = [];

@Directive()
export abstract class AbstractMyApprovalsPriceOffersComponent implements OnInit {
  protected readonly router = inject(Router);
  public readonly list = inject(ListService);
  public readonly track = inject(TrackByService);
  public readonly service = inject(PriceOfferViewService);
  public readonly serviceDetail = inject(PriceOfferDetailViewService);
  public readonly permissionService = inject(PermissionService);
  public readonly titleService = inject(TitleService);
  public readonly toast = inject(ToasterService);

  @ViewChild('approversModalComponent', { static: false }) approversModalComponent: ApproversModalComponent;

  protected title = 'SPO Approval';

  showHistory = false;
  selectedRowForHistory: any = null;

  ngOnInit() {
    this.list.maxResultCount = DEFAULT_PAGE_SIZE;
    this.titleService.setTitle('SPO Approval');
  }

  getDetailUrl(request: KeyAccountDto): string {
    if (request?.id) {
      return this.router.serializeUrl(
        this.router.createUrlTree([
          AppRoutes.SPECIAL_PRICE_OFFERS.BASE,
          AppRoutes.DETAILS_WITH_ID(request.id),
          AppRoutes.SPECIAL_PRICE_OFFERS.DETAILS.BASE,
        ]),
      );
    } else {
      console.error('Unknown key account:', request.id);
      return '';
    }
  }

  delete(record: PriceOfferDto) {
    this.service.delete(record);
  }

  exportToExcel() {
    this.service.exportToExcel();
  }

  onViewHistory(row: any): void {
    this.selectedRowForHistory = row;
    // Set approval histories from the row data
    this.serviceDetail.approvalHistories = row?.approvalHistories || [];
    this.showHistory = true;
  }
  showHistoryAddMoreItems = false;
  importBusy = false;
  // In component
  onSubmittedMoreItems(history: any) {
    this.service.addMoreItemHistories = [];
    this.showHistoryAddMoreItems = true;
    this.service.loadAddMoreItemHistories(history.id).subscribe(result => {});
  }

  closeHistoryDialog(): void {
    this.showHistory = false;
    this.selectedRowForHistory = null;
  }

  onShowApprovers(row: any) {
    if (row && row.id) {
      this.service.getListApprovers(row.id).subscribe({
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
