import { CoreModule } from '@abp/ng.core';
import { ThemeSharedModule, DateAdapter, TimeAdapter } from '@abp/ng.theme.shared';
import { CommercialUiModule } from '@volo/abp.commercial.ng.ui';
import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import {
  NgbNavModule,
  NgbDatepickerModule,
  NgbTimepickerModule,
  NgbDateAdapter,
  NgbTimeAdapter,
} from '@ng-bootstrap/ng-bootstrap';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { VendorCategoryDetailViewService } from '../service/vendor-category-detail.service';
import { EscCloseModalDirective } from '@app/shared/esc-close-modal/esc-close-modal.directive';
@Component({
  selector: 'app-vendor-category-detail-modal',
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
    EscCloseModalDirective,
  ],
  providers: [
    { provide: NgbDateAdapter, useClass: DateAdapter },
    { provide: NgbTimeAdapter, useClass: TimeAdapter },
  ],
  templateUrl: './vendor-category-detail.component.html',
  styles: [],
})
export class VendorCategoryDetailModalComponent {
  public readonly service = inject(VendorCategoryDetailViewService);
  submitForm() {
    this.service.submitForm();
  }
}
