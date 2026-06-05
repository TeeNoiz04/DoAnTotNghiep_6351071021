import { CoreModule } from '@abp/ng.core';
import { DateAdapter, ThemeSharedModule, TimeAdapter } from '@abp/ng.theme.shared';
import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import {
  NgbDateAdapter,
  NgbDatepickerModule,
  NgbNavModule,
  NgbTimeAdapter,
  NgbTimepickerModule,
} from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
import { CommercialUiModule } from '@volo/abp.commercial.ng.ui';
import { BuyerCategoryDetailViewService } from '../services/buyer-category-detail.service';

@Component({
  selector: 'app-buyer-category-detail-modal',
  changeDetection: ChangeDetectionStrategy.Default,
  imports: [
    CoreModule,
    ThemeSharedModule,
    CommercialUiModule,
    ReactiveFormsModule,
    NgbDatepickerModule,
    NgbTimepickerModule,
    NgbNavModule,
    MatSlideToggleModule,
    NgSelectModule,
  ],
  providers: [
    { provide: NgbDateAdapter, useClass: DateAdapter },
    { provide: NgbTimeAdapter, useClass: TimeAdapter },
  ],
  templateUrl: './buyer-category-detail.component.html',
  styles: [],
})
export class BuyerCategoryDetailModalComponent {
  public readonly service = inject(BuyerCategoryDetailViewService);
}
