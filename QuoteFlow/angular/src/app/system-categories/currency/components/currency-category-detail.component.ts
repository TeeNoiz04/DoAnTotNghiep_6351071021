import { CoreModule } from '@abp/ng.core';
import { ThemeSharedModule, DateAdapter, TimeAdapter } from '@abp/ng.theme.shared';
import { CommercialUiModule } from '@volo/abp.commercial.ng.ui';
import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { EscCloseModalDirective } from '@app/shared/esc-close-modal/esc-close-modal.directive';
import {
  NgbNavModule,
  NgbDatepickerModule,
  NgbTimepickerModule,
  NgbDateAdapter,
  NgbTimeAdapter,
} from '@ng-bootstrap/ng-bootstrap';
import { SystemCategoryDetailViewService } from '@app/system-categories/system-category/services/system-category-detail.service';
import { CategoryTypes } from '@app/system-categories/system-category.model';
import { InputNumberComponent } from '@app/shared/components/input-number/input-number.component';

@Component({
  selector: 'app-currency-category-detail-modal',
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
    InputNumberComponent,
    EscCloseModalDirective,
  ],
  providers: [
    { provide: NgbDateAdapter, useClass: DateAdapter },
    { provide: NgbTimeAdapter, useClass: TimeAdapter },
  ],
  templateUrl: './currency-category-detail.component.html',
  styleUrls: ['./currency-category-detail.component.scss'],
})
export class CurrencyCategoryDetailModalComponent {
  public readonly service = inject(SystemCategoryDetailViewService);

  submitForm() {
    this.service.submitForm(CategoryTypes.Currency);
  }
  get f() {
    return this.service.form.controls;
  }
}
