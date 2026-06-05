import { CoreModule } from '@abp/ng.core';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { CommonModule } from '@angular/common';
import { Component, inject, Input, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { AppPermissions } from '@app/app.permissions';
import { ExpandablePanelComponent } from '@app/shared/components/expandable-panel/expandable-panel.component';
import { InputNumberComponent } from '@app/shared/components/input-number/input-number.component';
import { TokenClaimsService } from '@app/shared/services/token-claims/token-claims.service';
import { NgbModalModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
import { LookupService } from '@proxy/general-lookups';
import { MaterialDto } from '@proxy/materials';
import { StockManagementDetailViewService } from '../../services/stock-management-detail.service';
import { EscCloseModalDirective } from '@app/shared/esc-close-modal/esc-close-modal.directive';

@Component({
  selector: 'app-stock-details-modal',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    CoreModule,
    ThemeSharedModule,
    NgbModalModule,
    ExpandablePanelComponent,
    NgSelectModule,
    InputNumberComponent,
    EscCloseModalDirective,
  ],
  templateUrl: './stock-details-modal.component.html',
  styleUrls: ['./stock-details-modal.component.scss'],
})
export class StockDetailsModalComponent implements OnInit, OnChanges {
  private fb = inject(FormBuilder);
  public service = inject(StockManagementDetailViewService);
  protected proxyLookupService = inject(LookupService);
  protected tokenClaims = inject(TokenClaimsService);
  @Input() selected?: MaterialDto;

  basicInfoForm: FormGroup;
  informationForm: FormGroup;
  specificationsForm: FormGroup;
  priceInfosForm: FormGroup;
  adminAreaForm: FormGroup;

  currencyTypeOptions: { value: string; label: string }[] = [];
  AppPermissions = AppPermissions;

  ngOnInit(): void {
    this.buildForms();

    if (this.service.selected) {
      this.patchFormValues();
    }
    this.getCurrencyLookup();
    this.disableAllForms();
  }
  ngOnChanges(changes: SimpleChanges): void {
    if (changes['selected'] && this.selected) {
      this.buildForms();
      this.disableAllForms();
    }
  }

  buildForms(): void {
    const percentSaleDiscount =
      this.selected?.standard_Price && this.selected?.sellingPrice1
        ? ((this.selected.standard_Price - this.selected.sellingPrice1) / this.selected.standard_Price) * 100
        : null;

    const percentManagerDiscount =
      this.selected?.standard_Price && this.selected?.maxMangerOfferPrice
        ? ((this.selected.standard_Price - this.selected.maxMangerOfferPrice) / this.selected.standard_Price) * 100
        : null;
    this.basicInfoForm = this.fb.group({
      golfaCode: [this.selected?.golfaCode, [Validators.required]],
      sapCode: [this.selected?.sap_Code],
      model: [this.selected?.model, [Validators.required]],
      materialGroup: [this.selected?.material_Group],
    });

    // Information section form
    this.informationForm = this.fb.group({
      materialType: [this.selected?.materialType, Validators.required],
      factory: [this.selected?.factory_Text],
      supplierCode: [this.selected?.supplierCode],
      unit: [this.selected?.unit],
      productHierarchy: [this.selected?.productHierarchyDescription],
      stockWarning: [this.selected?.stockWarning],
      vat: [this.selected?.vat],
      descriptionEN: [this.selected?.description_EN],
      descriptionVN: [this.selected?.description_VN],
      supplierBUCode: [this.selected?.supplierBUCode],
      note: [this.selected?.note],
      epa: [this.selected?.epa ?? false],
    });

    // Specifications section form
    this.specificationsForm = this.fb.group({
      spec1: [this.selected?.spec1],
      spec2: [this.selected?.spec2],
      spec3: [this.selected?.spec3],
      spec4: [this.selected?.spec4],
    });

    // Price Infos section form
    this.priceInfosForm = this.fb.group({
      standardPrice: [this.selected?.standard_Price],
      buyerPrice1: [this.selected?.sellingPrice1],
      buyerPrice2: [this.selected?.sellingPrice2],
      buyerPrice3: [this.selected?.sellingPrice3],
      buyerPrice4: [this.selected?.sellingPrice4],
      buyerPrice5: [this.selected?.sellingPrice5],
    });

    // Admin Area section form
    this.adminAreaForm = this.fb.group({
      inputPrice: [this.selected?.input_Price],
      inputCurrency: [this.selected?.inputCurrency],
      landedCost: [this.selected?.landedCost],
      percentSaleDiscount: [percentSaleDiscount],
      percentManagerDiscount: [percentManagerDiscount],
    });
  }

  patchFormValues(): void {
    const material = this.service.selected;

    if (material) {
      this.informationForm.patchValue({
        materialType: material.materialType,
        vendor: material.supplierCode,
        unit: material.unit,
        productHierarchy: '', //material.productHierarchy,
        stockWarning: '', // material.stockValueWarning,
        vat: material.vat,
        descriptionEN: '', // material.descriptionEN,
        descriptionVN: '', // material.descriptionVN,
        supplierBUCode: '',
        note: material.note,
        epa: material.epa,
      });

      this.specificationsForm.patchValue({
        spec1: material.spec1,
        spec2: material.spec2,
        spec3: material.spec3,
        spec4: material.spec4,
      });

      this.priceInfosForm.patchValue({
        standardPrice: 0, // material.standardPrice,
      });

      this.adminAreaForm.patchValue({
        inputPrice: 0, // material.inputPrice,
        inputCurrency: material.inputCurrency,
        landedCost: material.landedCost,
      });
    }
  }

  save(): void {
    const formValues = {
      ...this.informationForm.value,
      ...this.specificationsForm.value,
      ...this.priceInfosForm.value,
      ...this.adminAreaForm.value,
    };

    this.service.form.patchValue(formValues);

    if (this.service.selected?.id) {
      this.service.update();
    } else {
      this.service.create();
    }
  }

  disableAllForms(disable: boolean = true): void {
    const disableFormGroup = (form: FormGroup) => {
      if (!form) return;

      Object.keys(form.controls).forEach(controlName => {
        const control = form.get(controlName);
        if (disable) {
          control.disable();
        } else {
          control.enable();
        }
      });
    };

    disableFormGroup(this.basicInfoForm);
    disableFormGroup(this.informationForm);
    disableFormGroup(this.specificationsForm);
    disableFormGroup(this.priceInfosForm);
    disableFormGroup(this.adminAreaForm);
  }

  getCurrencyLookup() {
    this.proxyLookupService.getCurrencyCategoryLookup().subscribe(result => {
      this.currencyTypeOptions = result.items.map(item => ({
        value: item.id,
        label: item.displayName,
      }));
    });
  }
}
