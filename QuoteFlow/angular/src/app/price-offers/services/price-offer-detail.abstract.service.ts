import {
  AbpWindowService,
  ListService,
  PagedResultDto,
  PermissionService,
  TrackByService,
} from '@abp/ng.core';
import { inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

import { finalize, tap } from 'rxjs/operators';

import { ConfirmationService, ToasterService } from '@abp/ng.theme.shared';
import { ActivatedRoute } from '@angular/router';
import { SystemConfigurationKeys } from '@app/shared/constants/system-configuration-keys';
import { NumberHelper } from '@app/shared/helpers/number-helper';
import { LoadingService } from '@app/shared/services/loading/loading.service';
import { AttachmentSource, UploadProxyService } from '@app/shared/services/upload/upload.service';
import { ApprovalHistoryDto } from '@proxy/approval-histories';
import {
  GetPriceOfferCustomersInput,
  PriceOfferCustomerDto,
} from '@proxy/price-offers/price-offer-customers';
import {
  GetPriceOfferDetailsInput,
  PriceOfferDetailDto,
  PriceOfferDetailImportDto,
  PriceOfferUpdateLandingCostImportDto,
} from '@proxy/price-offers/price-offer-details';
import { ActionDto } from '@proxy/shared';
import { ExcelValidationResult } from '@proxy/shared/excels';
import { SystemConfigurationService } from '@proxy/system-configurations';
import { Observable } from 'rxjs';
import type { PriceOfferDto } from '../../proxy/price-offers/models';
import { PriceOfferService } from '../../proxy/price-offers/price-offer.service';
import { ImportPriceOfferType } from '../components/special-price-offer/price-offer.types';
import { AddMoreItemHistoryDto } from '@proxy/add-more-item-histories';
import { LookupService } from '@proxy/general-lookups';

export abstract class AbstractPriceOfferDetailViewService {
  protected readonly fb = inject(FormBuilder);
  protected readonly track = inject(TrackByService);
  public readonly confirmationService = inject(ConfirmationService);
  public readonly uploadProxyService = inject(UploadProxyService);
  public readonly proxyService = inject(PriceOfferService);
  public readonly systemConfigurationService = inject(SystemConfigurationService);
  public readonly list = inject(ListService);
  public readonly loadingService = inject(LoadingService);
  public readonly toast = inject(ToasterService);
  public readonly abpWindowService = inject(AbpWindowService);
  protected readonly permissionSerivce = inject(PermissionService);
  protected readonly lookupService = inject(LookupService);
  protected route = inject(ActivatedRoute);

  // SPO Adjustment Allowance Percentage (loaded from system configuration)
  public spoAdjustmentAllowancePercent = 20; // Default value of 20%

  showImportPriceOffer = false;
  showResultImportPriceOffer = false;
  resultImport:
    | ExcelValidationResult<PriceOfferDetailImportDto | PriceOfferUpdateLandingCostImportDto>
    | undefined;
  importMode: ImportPriceOfferType | undefined;
  selectedFiles: File[] = [];
  isBusy = false;
  selected = {} as PriceOfferDto;
  projectInfoDetailForm: FormGroup | undefined;
  importInfoDetailForm: FormGroup | undefined;
  priceOfferDetails: PagedResultDto<PriceOfferDetailDto> = {
    items: [],
    totalCount: 0,
  };
  showHistory = false;
  approvalHistories: ApprovalHistoryDto[] = [];
  addMoreItemHistories: AddMoreItemHistoryDto[] = [];

  openCommentRequestModal = false;
  requiredComment = true;

  customers: PagedResultDto<PriceOfferCustomerDto> = {
    items: [],
    totalCount: 0,
  };

  showProjectInfo = false;
  projectInfo: PagedResultDto<any> = {
    items: [
      {
        materialGroup: 'FA_HMI_Large',
        offerAmount: 27478880,
        standardAmount: 31226000,
      },
      {
        materialGroup: 'FA_HMI_MEAMC',
        offerAmount: 8392560,
        standardAmount: 9537000,
      },
      {
        materialGroup: 'FA_INV',
        offerAmount: 231121280,
        standardAmount: 267966000,
      },
      {
        materialGroup: 'FA_PLC_Compact',
        offerAmount: 104746400,
        standardAmount: 119030000,
      },
    ],
    totalCount: 0,
  };

  isDiscussionBoxOpen = false;

  protected createRequest() {
    const formValues = {
      ...this.projectInfoDetailForm?.getRawValue(),
    };

    if (this.selected) {
      return this.proxyService.update(this.selected.id, {
        ...formValues,
        concurrencyStamp: this.selected.concurrencyStamp,
      });
    }

    return this.proxyService.create(formValues);
  }

  buildImportForm() {
    const { materialType, closeDate, note } = this.selected || {};

    this.importInfoDetailForm = this.fb.group({
      buyerId: [this.selected?.buyer?.id ?? null, [Validators.required]],
      buyerName: [
        this.selected?.buyer?.shortName ?? null,
        [Validators.required, Validators.maxLength(4000)],
      ],
      buyerType: [this.selected?.buyerTypeDescription ?? null, [Validators.required]],
      location: [
        this.selected?.locationDescription ?? null,
        [Validators.required, Validators.maxLength(4000)],
      ],
      materialType: [materialType ?? null, [Validators.required, Validators.maxLength(50)]],
      saleName: [this.selected?.creatorName, [Validators.required, Validators.maxLength(4000)]], // This might need to be mapped from a different field
      keyAccountName: [
        this.selected?.keyAccount?.keyAccountName ?? null,
        [Validators.required, Validators.maxLength(4000)],
      ],
      keyAccountType: [this.selected?.keyAccountTypeDescription ?? null, [Validators.required]],
      keyAccountClass: [this.selected?.keyAccountClassDescription ?? null, [Validators.required]],
      closeDate: [closeDate ?? null, [Validators.required]],
      note: [note ?? null, [Validators.maxLength(4000)]],
      autoGetOfferPrice: [true],
      projectName: [this.selected?.projectName ?? null, [Validators.maxLength(4000)]],
      submittedDate: [this.selected?.creationTime ?? null],
    });
    this.importInfoDetailForm.disable();
  }

  buildDetailForm() {
    const {
      priceOfferCode,
      buyerId,
      materialType,
      locationId,
      locationOld,
      projectName,
      projectTypeId,
      euIndustryId,
      application,
      country,
      province,
      detailedAddress,
      competitorBrand,
      priceGapWithCompetitor,
      decisionRight,
      poPlannedDate,
      deliveryDate,
      upcomingPotentialProjects,
      otherPJInformation,
      fileName,
      note,
      closeDate,
      totalMEVNOfferAmount,
      approvalStatus,
      accountNo,
      keyAccountId,
      keyAccountTypeId,
      keyAccountClassId,
      creatorName,
    } = this.selected || {};

    this.projectInfoDetailForm = this.fb.group({
      projectCode: [priceOfferCode ?? null],
      projectType: [this.selected?.projectTypeDescription ?? null],
      euIndustry: [this.selected?.euIndustryDescription ?? null],
      description: [projectName ?? null],
      buyerId: [buyerId ?? null, [Validators.required]],
      euPrice: [''],
      euHomePage: [''],
      euLocation: [this.selected?.locationDescription],
      euTypeOfBusiness: [''],
      replaceFromCompetitor: [''],
      taxCode: [''],
      saleChanel: [''],
      projectName: [projectName ?? null, [Validators.maxLength(4000)]],
      application: [application ?? null, [Validators.maxLength(4000)]],
      country: [country ?? null, [Validators.maxLength(255)]],
      province: [province ?? null, [Validators.maxLength(4000)]],
      detailedAddress: [detailedAddress ?? null, [Validators.maxLength(4000)]],
      priceGap: [priceGapWithCompetitor ?? null, [Validators.maxLength(4000)]],
      otherInformation: [otherPJInformation ?? null, [Validators.maxLength(4000)]],
      competitorBrand: [competitorBrand ?? null, [Validators.maxLength(4000)]],
      decisionRight: [decisionRight ?? null, [Validators.maxLength(4000)]],
      poPlannedDate: [poPlannedDate ?? null, []],
      upcomingPotentialProjects: [upcomingPotentialProjects ?? null, []],

      // Additional fields for Price Offer
      priceOfferCode: [priceOfferCode ?? null, [Validators.required, Validators.maxLength(50)]],
      materialType: [materialType ?? null, [Validators.required, Validators.maxLength(50)]],
      locationId: [locationId ?? null, []],
      locationOld: [locationOld ?? null, [Validators.maxLength(4000)]],
      projectTypeId: [projectTypeId ?? null, []],
      euIndustryId: [euIndustryId ?? null, []],
      priceGapWithCompetitor: [priceGapWithCompetitor ?? null, [Validators.maxLength(4000)]],
      deliveryDate: [deliveryDate ?? null, []],
      otherPJInformation: [otherPJInformation ?? null, []],
      fileName: [fileName ?? null, []],
      note: [note ?? null, []],
      closeDate: [closeDate ?? null, []],
      totalAmount: [totalMEVNOfferAmount ?? null, []],
      status: [approvalStatus ?? null, [Validators.maxLength(50)]],
      accountNo: [accountNo ?? null, [Validators.maxLength(50)]],
      keyAccountId: [keyAccountId ?? null, []],
      keyAccountTypeId: [keyAccountTypeId ?? null, []],
      keyAccountClassId: [keyAccountClassId ?? null, []],
    });
    this.projectInfoDetailForm.disable();
  }

  toggleDiscussionBox() {
    this.isDiscussionBoxOpen = !this.isDiscussionBoxOpen;
  }

  approveHistory() {
    this.showHistory = !this.showHistory;
  }

  closeHistoryDialog() {
    this.showHistory = false;
  }

  showSummaryInformation() {
    this.showProjectInfo = true;
  }

  closeProjectInfoDialog() {
    this.showProjectInfo = false;
  }

  onCloseCommentRequestModal() {
    this.openCommentRequestModal = false;
    this.requiredComment = true;
  }

  private loadSpoAdjustmentAllowance(): void {
    this.systemConfigurationService
      .getList({
        cfgKey: SystemConfigurationKeys.SpoAddItemAllowancePercent,
        maxResultCount: 1,
      })
      .subscribe({
        next: result => {
          if (result.items && result.items.length > 0) {
            const configValue = result.items[0].cfgValue;
            // Convert to number and ensure it's a valid percentage
            const numericValue = parseFloat(configValue);
            if (!isNaN(numericValue) && numericValue > 0) {
              this.spoAdjustmentAllowancePercent = numericValue;
            }
          }
        },
        error: error => {
          console.warn('Failed to load SPO adjustment allowance configuration:', error);
          // Keep default value of 110%
        },
      });
  }

  loadPriceOfferDetails(id: string): Observable<PriceOfferDto> {
    this.isBusy = true;
    this.selected = {} as PriceOfferDto;

    // Load system configuration for SPO adjustment allowance
    this.loadSpoAdjustmentAllowance();

    return this.proxyService.get(id).pipe(
      finalize(() => (this.isBusy = false)),
      tap(result => {
        this.selected = result;
        this.loadCustomers(id);
        this.loadOfferItems(id).subscribe();
        this.buildImportForm();
        this.buildDetailForm();
      }),
    );
  }

  loadCustomers(id: string) {
    this.isBusy = true;
    const payload = {
      priceOfferId: id,
    } as GetPriceOfferCustomersInput;
    this.proxyService
      .getListCustomers(id, payload)
      .pipe(
        finalize(() => (this.isBusy = false)),
        tap(result => {
          this.customers = result;
        }),
      )
      .subscribe();
  }

  loadOfferItems(id: string): Observable<PagedResultDto<PriceOfferDetailDto>> {
    this.isBusy = true;

    const payload = {
      maxResultCount: 1000, // Adjust as needed
      skipCount: 0,
    } as GetPriceOfferDetailsInput;

    return this.proxyService.getListDetails(id, payload).pipe(
      finalize(() => (this.isBusy = false)),
      tap(result => {
        this.priceOfferDetails = result;
      }),
    );
  }

  formatCurrency(value: number): string {
    return NumberHelper.convertToFormattedNumber(value, 0);
  }

  submitForm() {
    if (this.projectInfoDetailForm.invalid) return;

    this.isBusy = true;
  }

  performAction(action: string, comment?: string): Observable<PriceOfferDto> {
    if (!this.selected || !this.selected.id) {
      this.toast.error('No price offer available to process.');
      return;
    }

    this.isBusy = true;
    const concurrencyStamp = this.selected.concurrencyStamp || '';

    return this.proxyService
      .performAction(this.selected.id, { action, comment, concurrencyStamp } as ActionDto)
      .pipe(finalize(() => (this.isBusy = false)));
  }
  uploadFiles() {
    if (this.selectedFiles.length === 0) {
      this.toast.warn('Please select files to upload.');
      return;
    }

    const attachmentCode = AttachmentSource.PriceOffer;
    this.uploadProxyService
      .uploadFilewithData(
        this.selectedFiles,
        this.route.snapshot.paramMap.get('id') || '',
        attachmentCode,
      )
      .subscribe({
        next: response => {
          this.toast.success('Files uploaded successfully.');
          this.selectedFiles = [];
          // Subscribe to the observable to actually reload the data
          this.loadPriceOfferDetails(this.route.snapshot.paramMap.get('id')).subscribe();
        },
        error: error => {
          this.toast.error('Failed to upload files.', 'Error');
        },
      });
  }

  // In service
  loadAddMoreItemHistories(id: string): Observable<any> {
    this.isBusy = true;

    return this.lookupService.addMoreItemHistory(id).pipe(
      finalize(() => (this.isBusy = false)),
      tap(result => {
        this.addMoreItemHistories = result;
      }),
    );
  }
}
