import { CoreModule } from '@abp/ng.core';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { EscCloseModalDirective } from '@app/shared/esc-close-modal/esc-close-modal.directive';
import { CategoryTypes } from '@app/system-categories/system-category.model';
import { SystemCategoryDetailViewService } from '@app/system-categories/system-category/services/system-category-detail.service';
import { CommercialUiModule } from '@volo/abp.commercial.ng.ui';

@Component({
  selector: 'app-buyer-type-category-detail-modal',
  changeDetection: ChangeDetectionStrategy.Default,
  imports: [
    CoreModule,
    ThemeSharedModule,
    CommercialUiModule,
    ReactiveFormsModule,
    MatSlideToggleModule,
    EscCloseModalDirective,
  ],
  providers: [],
  templateUrl: './buyer-type-category-detail.component.html',
  styles: [],
})
export class BuyerTypeCategoryDetailModalComponent {
  public readonly service = inject(SystemCategoryDetailViewService);

  submitForm() {
    this.service.submitForm(CategoryTypes.BuyerType);
  }

  get f() {
    return this.service.form.controls;
  }
}
