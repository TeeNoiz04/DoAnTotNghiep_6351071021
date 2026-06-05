import { Directive, OnInit, inject } from '@angular/core';

import { ListService, PermissionService, TrackByService } from '@abp/ng.core';

import { DEFAULT_PAGE_SIZE } from '@app/shared/constants';
import { TitleService } from '@app/shared/services/title/title.service';
import { StockTracingDetailViewService } from '../../services/stock-tracing-detail.service';

export const ChildTabDependencies = [];

export const ChildComponentDependencies = [];

@Directive()
export abstract class AbstractSearchStockTracingComponent implements OnInit {
  public readonly list = inject(ListService);
  public readonly track = inject(TrackByService);
  public readonly service = inject(StockTracingDetailViewService);
  public readonly permissionService = inject(PermissionService);
  public readonly titleService = inject(TitleService);

  protected title = 'Tracing';

  ngOnInit() {
    this.list.maxResultCount = DEFAULT_PAGE_SIZE;
    this.service.buildSearchForm();
    this.titleService.setTitle('Tracing');
    // this.service.hookToQuery().subscribe();
  }

  private isQueryHooked = false;
  clearFilters() {
    this.service.searchForm.reset();
    this.list.page = 0; //reset page index
    this.service.filters = {
      stockTracingId: undefined,
      reportType: undefined,
      skipCount: 0,
      maxResultCount: DEFAULT_PAGE_SIZE,
    };
    if (!this.isQueryHooked) {
      this.service.hookToQuery();
      this.isQueryHooked = true;
    } else {
      this.list.get();
    }
  }

  onSearch() {
    const values = this.service.searchForm.getRawValue();
    this.list.page = 0; //reset page index

    this.service.filters = {
      ...this.service.filters,
      ...values,
      model: values.model,
      dateEnteredMin: values.fromDate,
      dateEnteredMax: values.toDate,
      reportType: values.reportType,
      materialType: values.materialType,
      customer: values.customer,
      skuName: values.skuName,
      packingListCode: values.packingListCode,
      skipCount: 0,
    };
    if (!this.isQueryHooked) {
      this.service.hookToQuery();
      this.isQueryHooked = true;
    } else {
      this.list.get();
    }
  }
  onExport() {
    const values = this.service.searchForm.getRawValue();
    this.service.filters = {
      ...this.service.filters,
      ...values,
      skuCode: values.model,
      dateEnteredMin: values.fromDate,
      dateEnteredMax: values.toDate,
      reportType: values.reportType,
      customer: values.customer,
      skuName: values.skuName,
      packingListCode: values.packingListCode,
    };
    this.service.exportStockTracingDetail();
  }
}
