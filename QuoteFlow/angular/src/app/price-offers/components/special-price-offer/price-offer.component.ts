import { PageModule } from '@abp/ng.components/page';
import { CoreModule, ListService } from '@abp/ng.core';
import { Confirmation, DateAdapter, ThemeSharedModule, TimeAdapter } from '@abp/ng.theme.shared';
import { ChangeDetectionStrategy, Component, inject, OnInit, ViewChild } from '@angular/core';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { PriceOfferExtendedService } from '@app/proxy-custom/price-offers/price-offer-extended.service';
import { ApproversModalComponent } from '@app/shared/components/approvers-modal/approvers-modal.component';
import { AuditInfoColumnComponent } from '@app/shared/components/audit-info-column';
import { ErrorDisplayComponent } from '@app/shared/components/error-display/error-display.component';
import { HistoryModalComponent } from '@app/shared/components/history-modal/history-modal.component';
import { TableEdgeScrollerComponent } from '@app/shared/components/table-edge-scroller/table-edge-scroller.component';
import { UsernamePipe } from '@app/shared/pipes/username.pipe';
import { LoadingService } from '@app/shared/services/loading/loading.service';
import { StatusLabelComponent } from '@app/shared/status/components/status-label.component';
import {
  NgbCollapseModule,
  NgbDateAdapter,
  NgbDatepickerModule,
  NgbDropdownModule,
  NgbPaginationModule,
  NgbTimeAdapter,
  NgbTimepickerModule,
  NgbTooltip,
  NgbTooltipModule,
} from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
import { NgxValidateCoreModule } from '@ngx-validate/core';
import {
  ImportAddMoreItemsInput,
  PriceOfferAPImportInput,
  PriceOfferDSImportInput,
  PriceOfferImportDto,
  PriceOfferImportInput,
  PriceOfferNBImportInput,
} from '@proxy/price-offers';
import { PriceOfferDetailImportDto } from '@proxy/price-offers/price-offer-details';
import { ExcelValidationResult } from '@proxy/shared/excels';
import { CommercialUiModule } from '@volo/abp.commercial.ng.ui';
import { finalize } from 'rxjs';
import { PriceOfferDetailViewService } from '../../services/price-offer-detail.service';
import { PriceOfferFilterService } from '../../services/price-offer-filter.service';
import { PriceOfferViewService } from '../../services/price-offer.service';
import { SpecialPriceOfferFilterComponent } from './components/special-price-offer-filter/special-price-offer-filter.component';
import { ImportPriceOfferComponent } from './import-price-offer/import-price-offer.component';
import {
  AbstractPriceOfferComponent,
  ChildComponentDependencies,
  ChildTabDependencies,
} from './price-offer.abstract.component';
import { ImportPriceOfferType } from './price-offer.types';
import { ResultImportPriceOfferComponent } from './result-import-price-offer/result-import-price-offer.component';
import { DataTableComponent } from '@app/shared/components/data-table/data-table.component';
import { HeaderTableComponent } from '@app/shared/components/data-table/header/header.component';
import { ColumnComponent } from '@app/shared/components/data-table/column/column.component';

@Component({
  selector: 'app-price-offer',
  changeDetection: ChangeDetectionStrategy.Default,
  imports: [
    ...ChildTabDependencies,
    NgbCollapseModule,
    NgbDatepickerModule,
    NgbTimepickerModule,
    NgbDropdownModule,
    NgbPaginationModule,
    NgxValidateCoreModule,
    PageModule,
    CoreModule,
    ThemeSharedModule,
    CommercialUiModule,
    MatCheckboxModule,
    StatusLabelComponent,
    UsernamePipe,
    HistoryModalComponent,
    SpecialPriceOfferFilterComponent,
    ImportPriceOfferComponent,
    ResultImportPriceOfferComponent,
    ApproversModalComponent,
    ErrorDisplayComponent,
    NgbTooltipModule,
    TableEdgeScrollerComponent,
    AuditInfoColumnComponent,
    NgSelectModule,
    NgbTooltip,
    DataTableComponent,
    HeaderTableComponent,
    ColumnComponent,
    ...ChildComponentDependencies,
  ],
  providers: [
    LoadingService,
    ListService,
    PriceOfferViewService,
    PriceOfferDetailViewService,
    PriceOfferFilterService,
    { provide: NgbDateAdapter, useClass: DateAdapter },
    { provide: NgbTimeAdapter, useClass: TimeAdapter },
  ],
  templateUrl: './price-offer.component.html',
  styleUrls: ['./price-offer.component.scss'],
})
export class PriceOfferComponent extends AbstractPriceOfferComponent implements OnInit {
  extendedProxyService = inject(PriceOfferExtendedService);
  protected readonly filterService = inject(PriceOfferFilterService);
  @ViewChild('importPriceOffer') importPriceOffer: ImportPriceOfferComponent | undefined;
  @ViewChild('resultImportPriceOffer') resultImportPriceOffer: ResultImportPriceOfferComponent | undefined;
  @ViewChild('approversModalComponent', { static: false }) approversModalComponent: ApproversModalComponent;

  // Add enum reference for template
  importPriceOfferType = ImportPriceOfferType;

  // Add More Items pagination properties
  addMoreItemsPageSize = 20;
  addMoreItemsCurrentPage = 1;
  addMoreItemsTotalCount = 0;
  addMoreItemsSearchText: string = '';
  filteredAddMoreItemsCount: number = 0;
  Math = Math;

  // Computed property for paged data
  get pagedAddMoreItemHistories() {
    const searchLower = this.addMoreItemsSearchText?.toLowerCase().trim() || '';
    let filtered = this.serviceDetail.addMoreItemHistories || [];

    // Apply search filter if search text exists
    if (searchLower) {
      filtered = filtered.filter(
        item =>
          item.materialCode?.toLowerCase().includes(searchLower) || item.model?.toLowerCase().includes(searchLower),
      );
    }

    // Update filtered count
    this.filteredAddMoreItemsCount = filtered.length;

    // Apply pagination
    const startIndex = (this.addMoreItemsCurrentPage - 1) * this.addMoreItemsPageSize;
    const endIndex = startIndex + this.addMoreItemsPageSize;
    return filtered.slice(startIndex, endIndex);
  }

  onAddMoreItemsPageChange(page: number) {
    this.addMoreItemsCurrentPage = page;
  }

  onAddMoreItemsSearch(): void {
    this.addMoreItemsCurrentPage = 1;
  }

  clearAddMoreItemsSearch(): void {
    this.addMoreItemsSearchText = '';
    this.addMoreItemsCurrentPage = 1;
  }

  override ngOnInit() {
    super.ngOnInit();

    // Initialize the new filter service
    this.filterService.initialize({
      buildForm: true,
      syncFromQuery: true,
      autoSearch: false,
    });

    // Hook to query parameters
    this.filterService.hookToQuery();
  }

  // Override data access to use new filter service
  get data() {
    return this.filterService.data;
  }

  // Override search and clear methods
  onSearch() {
    this.filterService.search();
  }

  clearFilters() {
    this.filterService.clearFilters();
  }

  // Navigation method for Smart Back Button integration
  onViewDetails(id: string) {
    this.filterService.navigateToDetail(id, 'details');
  }

  // Busy states for modals
  importBusy = false;
  submitBusy = false;

  // Type-safe getters for the result
  get resultImportMain(): ExcelValidationResult<PriceOfferImportDto> | undefined {
    return this.importMode !== ImportPriceOfferType.AddMoreItems
      ? (this.resultImport as ExcelValidationResult<PriceOfferImportDto>)
      : undefined;
  }

  get resultImportItems(): ExcelValidationResult<PriceOfferDetailImportDto> | undefined {
    return this.importMode === ImportPriceOfferType.AddMoreItems
      ? (this.resultImport as ExcelValidationResult<PriceOfferDetailImportDto>)
      : undefined;
  }

  verifyData() {
    const file = this.importPriceOffer?.fileImport;
    const values = this.importPriceOffer?.importForm.getRawValue();
    const isValidForm = this.importPriceOffer?.importForm.valid;

    // For AddMoreItems, we only need the file
    if (this.importMode === ImportPriceOfferType.AddMoreItems) {
      if (file && file instanceof File) {
        const formData = new FormData();
        formData.append('file', file);
        this.loadingService.show();
        this.importBusy = true;
        this.serviceDetail.importMode = this.importMode;
        const getPriceAuto = this.importPriceOffer?.importForm?.get('autoGetOfferPrice')?.value ?? false;

        // Use the base proxy service for AddMoreItems validation
        this.serviceDetail.proxyService
          .validateAndParseAddMoreItemDetail(this.selectedRowForHistory.id, formData, getPriceAuto)
          .pipe(
            finalize(() => {
              this.importBusy = false;
              this.loadingService.hide();
              this.clearFile();
            }),
          )
          .subscribe({
            next: response => {
              this.resultImport = response;
              this.showResultImportPriceOffer = true;
            },
            error: () => {
              this.resultImport = undefined;
              this.toast.error('Failed to process the file. Please check the file format and try again.');
            },
          });
      } else {
        this.resultImport = undefined;
        this.toast.error('Please select a file before submitting.');
      }
      return;
    }

    // For other modes, we need full form validation
    if (file && file instanceof File && isValidForm) {
      const buyerId = values.buyer || '';
      const locationId = values.saleLocation || '';
      const closeDate = values.closeDate
        ? new Date(values.closeDate).toISOString().split('T')[0]
        : new Date().toISOString().split('T')[0];
      const keyAccountId = values.keyAccount || '';
      const keyAccountClassId = values.keyAccountClass || '';
      const keyAccountTypeId = values.keyAccountType || '';

      // set import information
      this.importPriceOffer.importInformation.closeDate = values.closeDate;
      this.importPriceOffer.importInformation.file = file;
      this.importPriceOffer.importInformation.note = values.note || '';
      this.importPriceOffer.importInformation.saleName = values.saleName || '';
      this.importPriceOffer.importInformation.buyerId = buyerId;
      this.importPriceOffer.importInformation.locationId = locationId;
      this.importPriceOffer.importInformation.keyAccountId = keyAccountId;
      this.importPriceOffer.importInformation.keyAccountTypeId = keyAccountTypeId;
      this.importPriceOffer.importInformation.keyAccountClassId = keyAccountClassId;
      this.importPriceOffer.importInformation.materialTypeId = values.materialType || '';
      this.importPriceOffer.importInformation.projectName = values.projectName || '';
      this.importPriceOffer.importInformation.buyerTypeId = values.buyerType || '';

      this.loadingService.show();
      this.importBusy = true;
      this.serviceDetail.importMode = this.importMode;
      switch (this.importMode) {
        case ImportPriceOfferType.ProjectPriceOffer: {
          const input: PriceOfferImportInput = {
            buyerId: buyerId,
            buyerTypeId: values.buyerType || '',
            locationId: locationId,
            salePIC: values.saleName,
            closeDate: closeDate,
            materialType: values.materialType || '',
            projectName: values.projectName || '',
            note: values.note || '',
          };

          this.extendedProxyService.validateAndParsePPManual(file, input).subscribe({
            next: this.handleImportSuccess.bind(this),
            error: this.handleImportError.bind(this),
          });
          break;
        }

        // For Key Account Price Offer mode
        case ImportPriceOfferType.KeyAccountPriceOffer: {
          const input: PriceOfferAPImportInput = {
            getPriceAutomatically: values.autoGetOfferPrice || false,
            buyerId: buyerId,
            buyerTypeId: values.buyerType || '',
            locationId: locationId,
            salePIC: null,
            keyAccountId: keyAccountId,
            keyAccountClassId: keyAccountClassId,
            keyAccountTypeId: keyAccountTypeId,
            materialType: values.materialType || '',
            projectName: values.projectName || '',
            note: values.note || '',
          };

          this.extendedProxyService.validateAndParseAPManual(file, input).subscribe({
            next: this.handleImportSuccess.bind(this),
            error: this.handleImportError.bind(this),
          });
          break;
        }

        // For Buyer Stock Price Offer mode
        case ImportPriceOfferType.BuyerStockPriceOffer: {
          const input: PriceOfferDSImportInput = {
            buyerId: buyerId,
            buyerTypeId: values.buyerType || '',
            locationId: locationId,
            salePIC: null,
            materialType: values.materialType || '',
            projectName: values.projectName || '',
            note: values.note || '',
          };
          this.extendedProxyService.validateAndParseDSManual(file, input).subscribe({
            next: this.handleImportSuccess.bind(this),
            error: this.handleImportError.bind(this),
          });
          break;
        }

        // For No Buyer Price Offer mode
        case ImportPriceOfferType.NoBuyerPriceOffer: {
          const input: PriceOfferNBImportInput = {
            locationId: locationId,
            salePIC: values.saleName,
            closeDate: closeDate,
            materialType: values.materialType || '',
            projectName: values.projectName || '',
            note: values.note || '',
          };

          this.extendedProxyService.validateAndParseNBManual(file, input).subscribe({
            next: this.handleImportSuccess.bind(this),
            error: this.handleImportError.bind(this),
          });
          break;
        }

        default:
          break;
      }
    } else {
      this.resultImport = undefined;
      this.importPriceOffer?.importForm.markAllAsTouched();
      this.toast.error('Please select a file and fill in all required fields before submitting.');
    }
  }

  onSubmitImport(withForceSubmit: boolean = false) {
    this.submitBusy = true;

    const onSuccess = () => {
      this.submitBusy = false;
      this.loadingService.hide();
      this.toast.success('Price offer imported successfully.');
      this.resultImport = undefined;
      this.showImportPriceOffer = false;
      this.showResultImportPriceOffer = false;
      this.filterService.search();
    };

    const onError = (err: any) => {
      this.submitBusy = false;
      this.loadingService.hide();
      const code = err?.error.error.code;
      const message = err?.error.error.message;
      if (code == 'QuoteFlow:10703001') {
        // show a confirmation dialog with the error message
        this.confirmation.warn(message, 'Please review', this.warningOptions).subscribe({
          next: result => {
            if (result === Confirmation.Status.confirm) {
              this.onSubmitImport(true);
            }
          },
          error: () => {
            this.toast.error('Failed to open import result modal.');
          },
        });
      } else {
        this.toast.error('Failed to import price offer. Please try again.');
      }
    };

    switch (this.importMode) {
      case ImportPriceOfferType.ProjectPriceOffer: {
        this.extendedProxyService
          .importPP(this.resultImport as ExcelValidationResult<PriceOfferImportDto>, withForceSubmit)
          .subscribe({
            next: onSuccess,
            error: onError,
          });
        break;
      }

      case ImportPriceOfferType.KeyAccountPriceOffer: {
        this.extendedProxyService
          .importAP(this.resultImport as ExcelValidationResult<PriceOfferImportDto>, withForceSubmit)
          .subscribe({
            next: onSuccess,
            error: onError,
          });
        break;
      }

      case ImportPriceOfferType.BuyerStockPriceOffer: {
        this.extendedProxyService
          .importDS(this.resultImport as ExcelValidationResult<PriceOfferImportDto>, withForceSubmit)
          .subscribe({
            next: onSuccess,
            error: onError,
          });
        break;
      }

      case ImportPriceOfferType.NoBuyerPriceOffer: {
        this.extendedProxyService
          .importNB(this.resultImport as ExcelValidationResult<PriceOfferImportDto>, withForceSubmit)
          .subscribe({
            next: onSuccess,
            error: onError,
          });
        break;
      }

      case ImportPriceOfferType.AddMoreItems: {
        // For AddMoreItems, we need the price offer ID from the selected row
        if (!this.selectedRowForHistory || !this.selectedRowForHistory.id) {
          this.submitBusy = false;
          this.loadingService.hide();
          this.toast.error('No price offer selected for adding more items.');
          return;
        }

        var payLoad = {
          validationResult: this.resultImport,
          comment: this.importPriceOffer?.importForm.get('commentAddMoreItems')?.value || '',
        } as ImportAddMoreItemsInput;
        this.extendedProxyService
          .importAddMoreItemDetailWithFormData(this.selectedRowForHistory.id, payLoad)
          .subscribe({
            next: () => {
              this.submitBusy = false;
              this.loadingService.hide();
              this.toast.success('Additional items imported successfully.');
              this.resultImport = undefined;
              this.showImportPriceOffer = false;
              this.showResultImportPriceOffer = false;
              this.service.hookToQuery();
            },
            error: () => {
              this.submitBusy = false;
              this.loadingService.hide();
              this.toast.error('Failed to import additional items. Please try again.');
            },
          });
        break;
      }

      default:
        this.submitBusy = false;
        this.loadingService.hide();
        break;
    }
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

  onCloseImportForm() {
    this.importPriceOffer?.resetPreSelection();
  }

  // Update the onSubmittedMoreItems method with proper pagination setup
  onSubmittedMoreItems(history: any) {
    this.showHistoryAddMoreItems = true;
    this.addMoreItemsCurrentPage = 1;
    this.addMoreItemsTotalCount = 0;
    this.addMoreItemsSearchText = ''; // Reset search
    this.serviceDetail.addMoreItemHistories = [];

    this.serviceDetail
      .loadAddMoreItemHistories(history.id)
      .pipe(
        finalize(() => {
          // Update total count after data is loaded
          setTimeout(() => {
            this.addMoreItemsTotalCount = this.serviceDetail.addMoreItemHistories?.length || 0;
            this.filteredAddMoreItemsCount = this.addMoreItemsTotalCount;
            console.log('✅ Total count set to:', this.addMoreItemsTotalCount);
          }, 0);
        }),
      )
      .subscribe({
        next: () => {
          console.log('📦 Loaded histories:', this.serviceDetail.addMoreItemHistories?.length);
        },
        error: error => {
          console.error('❌ Failed to load add more item histories:', error);
          this.toast.error('Failed to load add more item histories');
        },
      });
  }

  private handleImportSuccess(response: any) {
    this.loadingService.hide();
    this.importBusy = false;
    this.resultImport = response;
    this.showResultImportPriceOffer = true;
    this.clearFile();
  }

  private handleImportError(error: any) {
    console.error('Error during import:', error);
    this.loadingService.hide();
    this.importBusy = false;
    this.resultImport = undefined;
    this.toast.error('Failed to process the file. Please check the file format and try again.');
    this.clearFile();
  }

  private clearFile() {
    // Clear the file from the import form to prevent errors
    if (this.importPriceOffer) {
      this.importPriceOffer.clearSelectedFile();
    }
  }

  onBackFromResult() {
    // Clear the result import data
    this.resultImport = undefined;
    this.showResultImportPriceOffer = false;

    // Clear the file from the import form to prevent errors
    if (this.importPriceOffer) {
      this.importPriceOffer.clearSelectedFile();
    }
  }
}
