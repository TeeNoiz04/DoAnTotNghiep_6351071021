import { PageModule } from '@abp/ng.components/page';
import { CoreModule, ListService, TrackByService } from '@abp/ng.core';
import { ThemeSharedModule, ToasterService } from '@abp/ng.theme.shared';
import { CommonModule } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AppRoutes } from '@app/app.routes';
import { SpecialInputPriceService } from '@app/proxy/special-input-prices';
import { SpecialInputPriceDetailDto } from '@app/proxy/special-input-prices/special-input-price-details/models';
import {
  AppAdvancedDataTableComponent,
  AppTableColumnDirective,
  AppTableColumnGroupDirective,
} from '@app/shared/components/advanced-data-table';
import { ExpandablePanelV2Component } from '@app/shared/components/expandable-panel-v2/expandable-panel-v2.component';
import { SmartBackButtonComponent } from '@app/shared/components/smart-back-button';
import { NumberHelper } from '@app/shared/helpers/number-helper';
import { TableFilterPipe } from '@app/shared/pipes/table-filter.pipe';
import { UsernamePipe } from '@app/shared/pipes/username.pipe';
import { NavigationHistoryService } from '@app/shared/services/navigation-history/navigation-history.service';
import { TitleService } from '@app/shared/services/title/title.service';
import { NgbModule, NgbTooltip } from '@ng-bootstrap/ng-bootstrap';
import { CommercialUiModule } from '@volo/abp.commercial.ng.ui';

@Component({
  selector: 'app-special-input-price-detail',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    CoreModule,
    ThemeSharedModule,
    PageModule,
    CommercialUiModule,
    NgbModule,
    AppAdvancedDataTableComponent,
    AppTableColumnDirective,
    AppTableColumnGroupDirective,
    SmartBackButtonComponent,
    ExpandablePanelV2Component,
    UsernamePipe,
    NgbTooltip,
  ],
  providers: [ListService],
  templateUrl: './special-input-price-detail-page.component.html',
  styleUrls: ['./special-input-price-detail-page.component.scss'],
})
export class SpecialInputPriceDetailComponent implements OnInit {
  protected readonly route = inject(ActivatedRoute);
  protected readonly router = inject(Router);
  protected readonly specialInputPriceService = inject(SpecialInputPriceService);
  protected readonly list = inject(ListService);
  protected readonly track = inject(TrackByService);
  protected readonly titleService = inject(TitleService);
  protected readonly navigationHistoryService = inject(NavigationHistoryService);
  protected readonly toasterService = inject(ToasterService);

  private readonly tableFilterPipe = new TableFilterPipe();

  searchText = '';
  specialInputPrice: any = null;
  specialInputPriceDetails: SpecialInputPriceDetailDto[] = [];
  isLoading = false;

  // Search configuration
  searchColumns = ['materialCode', 'model', 'standard', 'inputPrice'];

  // Filtered data getter
  get filteredDetails(): SpecialInputPriceDetailDto[] {
    if (!this.searchText || this.searchText.trim() === '') {
      return this.specialInputPriceDetails;
    }
    return this.tableFilterPipe.transform(this.specialInputPriceDetails, this.searchText, this.searchColumns);
  }

  // Card collapse state
  isCardCollapsed: { [key: string]: boolean } = {
    basicInformation: false,
    details: false,
  };

  ngOnInit(): void {
    this.titleService.setTitle('Special Input Price Detail | Special Input Price Management');

    const specialInputPriceId = this.route.snapshot.paramMap.get('id');
    if (specialInputPriceId) {
      this.loadSpecialInputPrice(specialInputPriceId);
      this.loadSpecialInputPriceDetails(specialInputPriceId);
    }
  }

  navigateBack(): void {
    this.navigationHistoryService.smartBack(this.fallbackUrl);
  }

  // Formatter functions for advanced data table
  formatNumber = (value: any): string => {
    return value ? NumberHelper.convertToFormattedNumber(value, 0) : '0';
  };

  formatCurrency = (value: any): string => {
    return value
      ? `${new Intl.NumberFormat('en-US', {
          minimumFractionDigits: 2,
          maximumFractionDigits: 2,
        }).format(value)}`
      : '';
  };

  formatDate = (value: any): string => {
    return value
      ? new Date(value).toLocaleDateString('en-US', { month: '2-digit', day: '2-digit', year: 'numeric' })
      : '';
  };

  formatSpecialPrice = (value: any): string => {
    return value
      ? `${new Intl.NumberFormat('en-US', {
          minimumFractionDigits: 2,
          maximumFractionDigits: 2,
        }).format(value)}`
      : '0';
  };

  formatUsername = (value: any): string => {
    return value ? new UsernamePipe().transform(value) : '';
  };

  toggleCollapse(section: string): void {
    this.isCardCollapsed[section] = !this.isCardCollapsed[section];
  }

  clearSearch(): void {
    this.searchText = '';
  }

  // Fallback URL for smart back button
  get fallbackUrl(): string[] {
    return [AppRoutes.SPECIAL_INPUT_PRICE.BASE];
  }

  private loadSpecialInputPrice(id: string): void {
    this.isLoading = true;
    this.specialInputPriceService.get(id).subscribe({
      next: data => {
        this.specialInputPrice = data;
        this.isLoading = false;
      },
      error: error => {
        console.error('Error loading special input price:', error);
        this.toasterService.error('Failed to load special input price details');
        this.isLoading = false;
      },
    });
  }

  private loadSpecialInputPriceDetails(id: string): void {
    this.specialInputPriceService.getDetails(id).subscribe({
      next: result => {
        this.specialInputPriceDetails = result.items || [];
      },
      error: error => {
        console.error('Error loading special input price details:', error);
        this.toasterService.error('Failed to load special input price details');
      },
    });
  }
}
