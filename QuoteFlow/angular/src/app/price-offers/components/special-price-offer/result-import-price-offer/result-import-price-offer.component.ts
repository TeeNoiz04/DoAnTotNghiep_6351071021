import { CoreModule } from '@abp/ng.core';
import { Component, inject, Input } from '@angular/core';
import { FormBuilder, FormsModule } from '@angular/forms';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MetricProgressComponent, MetricsPanelComponent } from '@app/shared/components';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
import { PriceOfferImportDto } from '@proxy/price-offers';
import {
  PriceOfferDetailImportDto,
  PriceOfferUpdateLandingCostImportDto,
} from '@proxy/price-offers/price-offer-details';
import { ExcelValidationResult } from '@proxy/shared/excels';
import { CustomerInformationTableComponent } from '../components/customer-information-table/customer-information-table.component';
import { ImportInformationFormComponent } from '../components/import-information-form/import-information-form.component';
import { ProjectInformationFormComponent } from '../components/project-information-form/project-information-form.component';
import { ImportPriceOfferInformation, ImportPriceOfferType } from '../price-offer.types';
import { PriceOfferItemsComponent } from './components/price-offer-items/price-offer-items.component';

@Component({
  selector: 'app-result-import-price-offer',
  templateUrl: './result-import-price-offer.component.html',
  styleUrls: ['./result-import-price-offer.component.scss'],
  standalone: true,
  providers: [],
  imports: [
    FormsModule,
    NgbModule,
    NgSelectModule,
    MatCheckboxModule,
    CoreModule,
    ProjectInformationFormComponent,
    PriceOfferItemsComponent,
    CustomerInformationTableComponent,
    ImportInformationFormComponent,
    MetricProgressComponent,
    MetricsPanelComponent,
  ],
})
export class ResultImportPriceOfferComponent {
  @Input() importMode: ImportPriceOfferType | undefined;
  @Input() importInformation: ImportPriceOfferInformation;
  @Input() resultImport: ExcelValidationResult<PriceOfferImportDto> | undefined;
  @Input() resultImportItems:
    | ExcelValidationResult<PriceOfferDetailImportDto | PriceOfferUpdateLandingCostImportDto>
    | undefined;

  readonly fb = inject(FormBuilder);

  public fileImport: File | null = null;
  importPriceOfferType = ImportPriceOfferType;

  isCardCollapsed: { [key: string]: boolean } = {
    importInformation: false,
    projectInformation: false,
    customerInformation: false,
  };
  isLoading = false;
  validationErrors: { [key: string]: boolean } = {};

  toggleCardCollapse(cardId: string): void {
    this.isCardCollapsed[cardId] = !this.isCardCollapsed[cardId];
  }

  percentFormatter = (value: number): string => {
    return value.toFixed(2);
  };

  getDiscountTooltipPositive(): string {
    return 'Available discount: {remaining}%';
  }

  getDiscountTooltipNegative(): string {
    return 'Discount over limit by {excess}%';
  }
}
