import { CoreModule } from '@abp/ng.core';
import { CommonModule } from '@angular/common';
import { Component, inject, Input, OnChanges, SimpleChanges } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { PriceOfferDetailViewService } from '@app/price-offers/services/price-offer-detail.service';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
import { PriceOfferImportDto } from '@proxy/price-offers';
import { ExcelValidationResult } from '@proxy/shared/excels';
import { ImportPriceOfferInformation } from '../../price-offer.types';

@Component({
  selector: 'app-import-information-form',
  templateUrl: './import-information-form.component.html',
  standalone: true,
  styleUrls: ['./import-information-form.component.scss'],
  imports: [CommonModule, FormsModule, ReactiveFormsModule, NgbModule, NgSelectModule, MatCheckboxModule, CoreModule],
})
export class ImportInformationFormComponent implements OnChanges {
  @Input() resultImport: ExcelValidationResult<PriceOfferImportDto> | undefined;
  @Input() importInformation: ImportPriceOfferInformation;
  @Input() useServiceData: boolean = true;
  @Input() isReadonly: boolean = false;
  @Input() enableValidation: boolean = false;
  @Input() parentDataLoaded: boolean = false;

  public readonly service = inject(PriceOfferDetailViewService);
  private readonly fb = inject(FormBuilder);

  importInfoForm: FormGroup;
  validationErrors: { [key: string]: boolean } = {};
  isFormReadonly = false;

  constructor() {
    this.service.buildDetailForm();

    this.importInfoForm = this.fb.group({
      buyerId: [''],
      location: [''],
      materialType: [''],
      saleName: [''],
      keyAccountName: [''],
      keyAccountType: [''],
      keyAccountClass: [''],
      closeDate: [''],
      note: [''],
      projectName: [''],
      buyerType: [''],
      autoGetOfferPrice: [false],
    });
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (!this.useServiceData && this.importInformation) {
      this.importInfoForm.patchValue({
        buyerId: this.importInformation.buyerName,
        location: this.importInformation.locationName,
        saleName: this.importInformation.saleName,
        keyAccountName: this.importInformation.keyAccountName,
        keyAccountType: this.importInformation.keyAccountTypeName,
        keyAccountClass: this.importInformation.keyAccountClassName,
        closeDate: this.importInformation.closeDate,
        note: this.importInformation.note,
        materialType: '',
        projectName: this.importInformation.projectName || '',
        buyerType: this.importInformation.buyerTypeName || '',
        autoGetOfferPrice: this.importInformation.autoGetOfferPrice || false,
      });

      if (this.enableValidation) {
        Object.keys(this.importInfoForm.controls).forEach(key => {
          const control = this.importInfoForm.get(key);
          control?.markAsTouched();
          this.validationErrors[key] = control?.invalid || false;
        });
      }
    }

    if (!this.useServiceData && changes['resultImport'] && this.resultImport) {
      const materialType = this.resultImport?.listData[0]?.rowData?.materialType;

      this.importInfoForm.patchValue({
        materialType: materialType || '',
      });
    }

    if (changes['isReadonly'] && this.isReadonly !== changes['isReadonly'].previousValue) {
      this.setReadonlyState(this.isReadonly);
    }

    if (changes['parentDataLoaded'] && changes['parentDataLoaded'].currentValue === true) {
      // Ensure form is updated when parent data is loaded
      if (this.useServiceData) {
        this.service.buildImportForm();

        // Sync with service form if using service data
        if (this.service.importInfoDetailForm) {
          const serviceFormValue = this.service.importInfoDetailForm.getRawValue();

          this.importInfoForm.patchValue(serviceFormValue);

          // Subscribe to service form changes
          this.service.importInfoDetailForm.valueChanges.subscribe(values => {
            this.importInfoForm.patchValue(values, { emitEvent: false });
          });

          // Sync back to service
          this.importInfoForm.valueChanges.subscribe(values => {
            if (this.service.importInfoDetailForm && !this.isFormReadonly) {
              this.service.importInfoDetailForm.patchValue(values, { emitEvent: false });
            }
          });
        }
      }
    }
  }

  private setReadonlyState(readonly: boolean): void {
    this.isFormReadonly = readonly;

    if (readonly) {
      // Disable editing
      this.importInfoForm.disable();
    } else {
      // Enable editing
      this.importInfoForm.enable();
    }
  }

  getForm(): FormGroup {
    return this.useServiceData ? this.service.importInfoDetailForm : this.importInfoForm;
  }
}
