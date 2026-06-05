import { CoreModule } from '@abp/ng.core';
import { Component, inject, Input } from '@angular/core';
import { FormBuilder, FormsModule } from '@angular/forms';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
import { ImportDPODto } from '@app/proxy/dpos/models';
import { ImportDPODetailDto } from '@app/proxy/dpos/dpodetails/models';
import { ExcelValidationResult } from '@app/proxy/shared/excels/models';
import { DpoItemsComponent } from './components/dpo-items/dpo-items.component';

@Component({
  selector: 'app-result-import-dpo',
  templateUrl: './result-import-dpo.component.html',
  styleUrls: ['./result-import-dpo.component.scss'],
  standalone: true,
  providers: [],
  imports: [FormsModule, NgbModule, NgSelectModule, MatCheckboxModule, CoreModule, DpoItemsComponent],
})
export class ResultImportDpoComponent {
  @Input() resultImport: ExcelValidationResult<ImportDPODto> | undefined;

  readonly fb = inject(FormBuilder);

  public fileImport: File | null = null;

  isCardCollapsed: { [key: string]: boolean } = {
    importInformation: false,
    dpoItems: false,
  };
  isLoading = false;
  validationErrors: { [key: string]: boolean } = {};

  get dpoData(): ImportDPODto | undefined {
    if (!this.resultImport?.listData?.length) return undefined;
    return this.resultImport.listData[0]?.rowData;
  }

  get dpoDetailsValidation(): ExcelValidationResult<ImportDPODetailDto> | undefined {
    return this.dpoData?.details;
  }

  get totalItems(): number {
    return this.dpoDetailsValidation?.listData?.length || 0;
  }

  get validItemsCount(): number {
    return this.dpoDetailsValidation?.listData?.filter(item => !item.hasErrors)?.length || 0;
  }

  get invalidItemsCount(): number {
    return this.dpoDetailsValidation?.listData?.filter(item => item.hasErrors)?.length || 0;
  }

  get errorsCount(): number {
    return this.resultImport?.errors?.length || 0;
  }

  get dpoItemsCount(): number {
    return this.totalItems;
  }

  toggleCardCollapse(cardId: string): void {
    this.isCardCollapsed[cardId] = !this.isCardCollapsed[cardId];
  }

  percentFormatter = (value: number): string => {
    return value.toFixed(2);
  };
}
