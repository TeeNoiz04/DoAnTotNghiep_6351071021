import { CoreModule } from '@abp/ng.core';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { CommercialUiModule } from '@volo/abp.commercial.ng.ui';
import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MaterialGroupDetailViewService } from '../services/material-group-category-detail.service';
import { NgSelectModule } from '@ng-select/ng-select';
import { LookupService } from '@proxy/general-lookups';
import { LookupDto } from '@proxy/shared';
import { EscCloseModalDirective } from '@app/shared/esc-close-modal/esc-close-modal.directive';

@Component({
  selector: 'app-material-group-category-detail-modal',
  changeDetection: ChangeDetectionStrategy.Default,
  imports: [
    CoreModule,
    ThemeSharedModule,
    CommercialUiModule,
    ReactiveFormsModule,
    MatSlideToggleModule,
    NgSelectModule,
    EscCloseModalDirective,
  ],
  providers: [],
  templateUrl: './material-group-category-detail.component.html',
  styles: [],
})
export class MaterialGroupCategoryDetailModalComponent {
  public readonly service = inject(MaterialGroupDetailViewService);
  // protected readonly lookupService = inject(LookupService);

  submitForm() {
    this.service.submitForm();
  }
  get f() {
    return this.service.form.controls;
  }

  // materialTypeOptions = [
  //   { label: 'FA', value: 'FA' },
  //   { label: 'LVS', value: 'LVS' },
  // ];
  // materialGroupPSIOptions: LookupDto<string>[] = [];
  // onMaterialTypeChange(selectedValue: any) {
  //   this.f['materialGroupPSI']?.reset();
  //   if (!selectedValue || !selectedValue.value) {
  //     this.materialGroupPSIOptions = [];
  //     return;
  //   }
  //   this.lookupService.getMaterialGroupPSILookup(selectedValue.value).subscribe({
  //     next: result => {
  //       this.materialGroupPSIOptions = result.items || [];
  //     },
  //     error: error => {
  //       console.error('Error loading suppliers:', error);
  //     },
  //   });
  // }
}
