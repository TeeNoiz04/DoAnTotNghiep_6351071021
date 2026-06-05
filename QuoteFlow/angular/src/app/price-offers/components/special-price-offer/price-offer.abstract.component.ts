import { Directive, inject, OnDestroy, OnInit } from '@angular/core';

import { ListService, PermissionService, TrackByService } from '@abp/ng.core';

import { Confirmation, ConfirmationService, ToasterService } from '@abp/ng.theme.shared';
import { ActivatedRoute, NavigationEnd, Router } from '@angular/router';
import { AppPermissions } from '@app/app.permissions';
import { AppRoutes } from '@app/app.routes';
import { DEFAULT_PAGE_SIZE } from '@app/shared/constants';
import { LoadingService } from '@app/shared/services/loading/loading.service';
import { TitleService } from '@app/shared/services/title/title.service';
import { TemplateService } from '@proxy/general-templates';
import { PriceOfferDetailImportDto } from '@proxy/price-offers/price-offer-details';
import { ExcelValidationResult } from '@proxy/shared/excels';
import { filter, Subject, takeUntil } from 'rxjs';
import type { PriceOfferDto, PriceOfferImportDto } from '../../../proxy/price-offers/models';
import { PriceOfferDetailViewService } from '../../services/price-offer-detail.service';
import { PriceOfferViewService } from '../../services/price-offer.service';
import {
  ImportPriceOfferType,
  ImportPriceOfferTypeMap,
  ImportPriceOfferTypeOption,
  PriceOfferTypes,
} from './price-offer.types';

export const ChildTabDependencies = [];

export const ChildComponentDependencies = [];

@Directive()
export abstract class AbstractPriceOfferComponent implements OnInit, OnDestroy {
  public readonly list = inject(ListService);
  public readonly track = inject(TrackByService);
  public readonly service = inject(PriceOfferViewService);
  public readonly serviceDetail = inject(PriceOfferDetailViewService);
  public readonly permissionService = inject(PermissionService);
  public readonly toast = inject(ToasterService);
  public readonly confirmation = inject(ConfirmationService);
  protected readonly router = inject(Router);
  protected readonly route = inject(ActivatedRoute);
  protected readonly loadingService = inject(LoadingService);
  protected readonly titleService = inject(TitleService);
  protected readonly templateService = inject(TemplateService);

  private destroy$ = new Subject<void>();

  protected title = 'Special Price Offer (SPO)';
  showImportPriceOffer = false;
  titleImportKeyAccount = 'Key-Account Price Offer';
  showResultImportPriceOffer = false;
  resultImport: ExcelValidationResult<PriceOfferImportDto | PriceOfferDetailImportDto> | undefined;

  showHistory = false;
  selectedRowForHistory: any = null;

  importPriceOfferOptions: ImportPriceOfferTypeOption[] = [
    {
      label: 'Key Account Price Offer',
      value: ImportPriceOfferType.KeyAccountPriceOffer,
      requiredPolicy: `${AppPermissions.PriceOffers.Uploads.PriceOfferAP}`,
    },
    {
      label: 'Buyer Stock Price Offer',
      value: ImportPriceOfferType.BuyerStockPriceOffer,
      requiredPolicy: `${AppPermissions.PriceOffers.Uploads.PriceOfferDS}`,
    },
    {
      label: 'Project Price Offer',
      value: ImportPriceOfferType.ProjectPriceOffer,
      requiredPolicy: `${AppPermissions.PriceOffers.Uploads.PriceOfferPP}`,
    },
    {
      label: 'No Buyer Price Offer',
      value: ImportPriceOfferType.NoBuyerPriceOffer,
      requiredPolicy: `${AppPermissions.PriceOffers.Uploads.PriceOfferNB}`,
    },
  ];
  importMode: ImportPriceOfferType | undefined;
  warningOptions: Partial<Confirmation.Options> = {
    yesText: 'Yes',
    cancelText: 'No',
    dismissible: false,
    hideCancelBtn: false,
    hideYesBtn: false,
  };
  ngOnInit() {
    this.titleService.setTitle(this.title);
    this.list.maxResultCount = DEFAULT_PAGE_SIZE;

    this.router.events
      .pipe(
        filter(e => e instanceof NavigationEnd),
        takeUntil(this.destroy$),
      )
      .subscribe(() => {
        // Preserve title when navigation occurs due to filter changes
        this.titleService.setTitle(this.title);
      });

    this.checkImportOptionPermission();
  }

  ngOnDestroy() {
    this.destroy$.next();
    this.destroy$.complete();
  }

  openDetails(val: PriceOfferDto) {
    this.router.navigate([
      `/${AppRoutes.SPECIAL_PRICE_OFFERS.BASE}/${val.id}/${AppRoutes.SPECIAL_PRICE_OFFERS.DETAILS.BASE}`,
    ]);
  }

  delete(record: PriceOfferDto) {
    this.service.delete(record);
  }

  exportToExcel() {
    this.service.exportToExcel();
  }

  onOpenImportPriceOffer(val: ImportPriceOfferTypeOption) {
    this.importMode = val.value;
    this.titleImportKeyAccount = ImportPriceOfferTypeMap[val.value];
    this.showImportPriceOffer = !this.showImportPriceOffer;
  }

  onDownloadTemplate(val: ImportPriceOfferTypeOption) {
    let templateType: string;
    switch (val.value) {
      case ImportPriceOfferType.KeyAccountPriceOffer:
        templateType = PriceOfferTypes.KeyAccount;
        break;
      case ImportPriceOfferType.BuyerStockPriceOffer:
        templateType = PriceOfferTypes.Distributor;
        break;
      case ImportPriceOfferType.ProjectPriceOffer:
        templateType = PriceOfferTypes.Project;
        break;
      case ImportPriceOfferType.NoBuyerPriceOffer:
        templateType = PriceOfferTypes.NoBuyer;
        break;
      default:
        this.toast.error('Invalid template type');
        return;
    }

    this.templateService.getPriceOfferTemplate(templateType).subscribe({
      next: (response: Blob) => {
        const url = window.URL.createObjectURL(response);
        const a = document.createElement('a');
        a.href = url;
        a.download = `Template_${val.value}.xlsx`;
        a.click();
        window.URL.revokeObjectURL(url);
      },
      error: error => {
        this.toast.error(error.message || 'Failed to download template');
      },
    });
  }

  checkImportOptionPermission() {
    this.importPriceOfferOptions = this.importPriceOfferOptions.filter(option =>
      this.permissionService.getGrantedPolicy(option.requiredPolicy),
    );
  }
  onViewHistory(row: any): void {
    this.selectedRowForHistory = row;
    // Set approval histories from the row data
    this.serviceDetail.approvalHistories = row?.approvalHistories || [];
    this.showHistory = true;
  }

  closeHistoryDialog(): void {
    this.showHistory = false;
    this.selectedRowForHistory = null;
  }
  onAddMoreItems(row: any): void {
    // Set the selected row for context
    this.selectedRowForHistory = row;
    // Set import mode to AddMoreItems
    this.importMode = ImportPriceOfferType.AddMoreItems;
    this.serviceDetail.importMode = ImportPriceOfferType.AddMoreItems;
    // Show the import modal
    this.showImportPriceOffer = true;
    this.titleImportKeyAccount = 'Add More Items';
  }

  onExport(row: any): void {}

  showHistoryAddMoreItems = false;
  // In component
  onSubmittedMoreItems(history: any) {
    this.service.addMoreItemHistories = [];
    this.showHistoryAddMoreItems = true;
    this.serviceDetail.loadAddMoreItemHistories(history.id).subscribe(result => {});
  }
}
