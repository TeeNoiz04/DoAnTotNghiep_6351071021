import { inject } from '@angular/core';
import { ConfirmationService } from '@abp/ng.theme.shared';
import { ABP, AbpWindowService, ListService, PagedResultDto } from '@abp/ng.core';
import { ActivatedRoute, Router } from '@angular/router';
import { DEFAULT_PAGE_SIZE } from '@app/shared/constants';
import { Subscription } from 'rxjs';
import { LoadingService } from '@app/shared/services/loading/loading.service';
import { CfgDiscountRatioService } from '@proxy/cfg-discount-ratios';
import type {
  GetCfgDiscountRatiosInput,
  CfgDiscountRatioDto,
} from '@proxy/cfg-discount-ratios/models';
import { LookupDto } from '@proxy/shared';
import { LookupService } from '@proxy/general-lookups';

export abstract class AbstractSpoDiscountViewService {
  protected readonly proxyService = inject(CfgDiscountRatioService);
  protected readonly confirmationService = inject(ConfirmationService);
  protected readonly list = inject(ListService);
  protected readonly abpWindowService = inject(AbpWindowService);
  protected readonly router = inject(Router);
  protected readonly route = inject(ActivatedRoute);
  protected readonly loadingService = inject(LoadingService);
  protected readonly lookupService = inject(LookupService);

  private currentSubscription?: Subscription;

  isLoadingFilters = false;
  isLoadingSpoTypes = false;
  isLoadingKATypes = false;
  isLoadingKAClasses = false;

  filters = {
    approval_Type: undefined,
    product_Type: undefined,
    kaType: undefined,
    accountClassify: undefined,
    maxResultCount: DEFAULT_PAGE_SIZE,
  } as GetCfgDiscountRatiosInput;

  data: PagedResultDto<CfgDiscountRatioDto> = {
    items: [],
    totalCount: 0,
  };

  productTypeOptions: LookupDto<string>[] = [];
  approvalTypeOptions: LookupDto<string>[] = [];
  kaTypeOptions: LookupDto<string>[] = [];
  accountClassifyOptions: LookupDto<string>[] = [];
  hasKAType = false;

  loadFilterOptions(): void {
    this.isLoadingFilters = true;

    this.lookupService.getMaterialTypeLookup().subscribe({
      next: materialTypes => {
        this.productTypeOptions = materialTypes.items || [];
        this.isLoadingFilters = false;

        // --- Set Default Value Here ---
        if (this.productTypeOptions.length > 0) {
          const defaultItem = this.productTypeOptions[0];
          this.filters.product_Type = defaultItem.displayCode;
        }
      },
      error: error => {
        this.productTypeOptions = [];
        this.isLoadingFilters = false;
      },
    });

    this.lookupService.getSpoTypeLookup().subscribe({
      next: spoTypes => {
        // Convert string array to LookupDto array
        this.approvalTypeOptions = (spoTypes || []).map(
          spoType =>
            ({
              id: spoType,
              displayName: spoType,
              displayCode: spoType,
            }) as LookupDto<string>,
        );

        this.isLoadingSpoTypes = false;
        if (this.approvalTypeOptions.length > 0) {
          const defaultItem = this.approvalTypeOptions[0];
          this.filters.approval_Type = defaultItem.displayCode;
          this.hasKAType = this.filters.approval_Type === 'AP';

          this.loadKAType(defaultItem.displayCode);
        }
      },
      error: error => {
        this.approvalTypeOptions = [];
        this.isLoadingSpoTypes = false;
      },
    });
  }

  loadKAType(spoType: string): void {
    this.lookupService.getKAbySPO(spoType).subscribe({
      next: kaTypes => {
        this.kaTypeOptions = (kaTypes || []).map(
          kaType =>
            ({
              id: kaType,
              displayName: kaType,
              displayCode: kaType,
            }) as LookupDto<string>,
        );
        if (this.kaTypeOptions.length > 0) {
          const defaultItem = this.kaTypeOptions[0];
          this.filters.kaType = defaultItem.displayCode;
          this.loadKAClassesByKAType(defaultItem.displayCode);
        }

        this.isLoadingKATypes = false;
      },
      error: error => {
        console.error('Failed to load KA types:', error);

        this.kaTypeOptions = [];
        this.isLoadingKATypes = false;
      },
    });
  }

  loadSpoTypesByMaterialType(materialType: string | undefined): void {
    // Clear dependent filters
    this.approvalTypeOptions = [];
    this.kaTypeOptions = [];
    this.accountClassifyOptions = [];
    this.filters.approval_Type = undefined;
    this.filters.kaType = undefined;
    this.filters.accountClassify = undefined;

    if (!materialType) {
      return;
    }

    this.isLoadingSpoTypes = true;

    this.lookupService.getSpoTypeLookup().subscribe({
      next: spoTypes => {
        // Convert string array to LookupDto array
        this.approvalTypeOptions = (spoTypes || []).map(
          spoType =>
            ({
              id: spoType,
              displayName: spoType,
              displayCode: spoType,
            }) as LookupDto<string>,
        );
        if (this.approvalTypeOptions.length > 0) {
          const defaultItem = this.approvalTypeOptions[0];
          this.filters.approval_Type = defaultItem.displayCode;

          this.loadKATypesBySpoType(defaultItem.displayCode);
        }

        this.isLoadingSpoTypes = false;
      },
      error: error => {
        this.approvalTypeOptions = [];
        this.isLoadingSpoTypes = false;
      },
    });
  }

  loadKATypesBySpoType(spoType: string | undefined): void {
    // Clear dependent filters
    this.kaTypeOptions = [];
    this.accountClassifyOptions = [];
    this.filters.kaType = undefined;
    this.filters.accountClassify = undefined;

    if (!spoType) {
      return;
    }

    this.isLoadingKATypes = true;

    this.lookupService.getKAbySPO(spoType).subscribe({
      next: kaTypes => {
        // Convert string array to LookupDto array
        this.kaTypeOptions = (kaTypes || []).map(
          kaType =>
            ({
              id: kaType,
              displayName: kaType,
              displayCode: kaType,
            }) as LookupDto<string>,
        );

        if (this.kaTypeOptions.length > 0) {
          const defaultItem = this.kaTypeOptions[0];
          this.filters.kaType = defaultItem.displayCode;
          this.loadKAClassesByKAType(defaultItem.displayCode);
        }

        this.isLoadingKATypes = false;
      },
      error: error => {
        this.kaTypeOptions = [];
        this.isLoadingKATypes = false;
      },
    });
  }

  loadKAClassesByKAType(kaTypeCode: string | undefined): void {
    // Clear dependent filter
    this.accountClassifyOptions = [];
    this.filters.accountClassify = undefined;

    if (!kaTypeCode) {
      return;
    }

    this.isLoadingKAClasses = true;

    const selectedKAType = this.kaTypeOptions.find(opt => opt.displayCode === kaTypeCode);

    if (!selectedKAType) {
      this.isLoadingKAClasses = false;
      return;
    }

    this.lookupService.getKeyAccountClassChildLookup(selectedKAType.displayCode, true).subscribe({
      next: result => {
        this.accountClassifyOptions = (result.items || []).filter(item => {
          const code = item.displayCode?.toUpperCase() || '';
          const name = item.displayName?.toUpperCase() || '';
          return code !== 'N/A' && code !== 'NA' && name !== 'N/A' && name !== 'NA';
        });

        this.isLoadingKAClasses = false;
      },
      error: error => {
        this.accountClassifyOptions = [];
        this.isLoadingKAClasses = false;
      },
    });
  }

  onMaterialTypeChange(materialType: string | undefined): void {
    this.loadSpoTypesByMaterialType(materialType);
  }

  onSpoTypeChange(spoType: string | undefined): void {
    this.loadKATypesBySpoType(spoType);
  }

  onKATypeChange(kaTypeCode: string | undefined): void {
    this.loadKAClassesByKAType(kaTypeCode);
  }

  hookToQuery() {
    this.hasKAType = this.filters.approval_Type === 'AP';
    const getData = (query: ABP.PageQueryParams) => {
      const params = {
        ...query,
        ...this.filters,
        filterText: this.filters.filterText,
        Approval_Type: this.filters.approval_Type,
        Product_Type: this.filters.product_Type,
        KAType: this.filters.kaType,
        AccountClassify: this.filters.accountClassify,
      };

      return this.proxyService.getList(params);
    };

    const setData = (list: PagedResultDto<CfgDiscountRatioDto>) => {
      this.data = list;
      this.loadingService.hide();
    };

    this.currentSubscription?.unsubscribe();
    this.currentSubscription = this.list.hookToQuery(getData).subscribe({
      next: setData,
      error: err => {
        this.loadingService.hide();
      },
    });
  }

  clearFilters() {
    this.filters = {
      approval_Type: undefined,
      product_Type: undefined,
      kaType: undefined,
      accountClassify: undefined,
      maxResultCount: DEFAULT_PAGE_SIZE,
    } as GetCfgDiscountRatiosInput;
    if (this.productTypeOptions.length > 0) {
      const defaultItem = this.productTypeOptions[0];
      this.filters.product_Type = defaultItem.displayCode;
    }
    if (this.approvalTypeOptions.length > 0) {
      const defaultItem = this.approvalTypeOptions[0];
      this.filters.approval_Type = defaultItem.displayCode;
    }
    if (this.kaTypeOptions.length > 0) {
      const defaultItem = this.kaTypeOptions[0];
      this.filters.kaType = defaultItem.displayCode;
    }
    this.hasKAType = this.filters.approval_Type === 'AP';

    // Clear all dependent options
    this.accountClassifyOptions = [];

    this.list.get();
  }
}
