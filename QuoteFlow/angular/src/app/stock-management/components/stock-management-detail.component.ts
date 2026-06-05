import { PageModule } from '@abp/ng.components/page';
import { CoreModule } from '@abp/ng.core';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { Component, inject } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgbModalModule, NgbTooltipModule } from '@ng-bootstrap/ng-bootstrap';
import { NgxValidateCoreModule } from '@ngx-validate/core';
import { StockManagementDetailViewService } from '../services/stock-management-detail.service';

@Component({
  selector: 'app-stock-detail-modal',
  template: `
    <form [formGroup]="service.form" (ngSubmit)="save()" [validateOnSubmit]="true">
      <div class="row">
        <div class="col-md-6">
          <div class="form-group">
            <label for="stock-golfaCode">{{ '::Material Code' | abpLocalization }}</label>
            <input autocomplete="off" id="stock-golfaCode" formControlName="golfaCode" class="form-control" autofocus />
          </div>
        </div>
        <div class="col-md-6">
          <div class="form-group">
            <label for="stock-model">{{ '::Model' | abpLocalization }}</label>
            <input autocomplete="off" id="stock-model" formControlName="model" class="form-control" />
          </div>
        </div>
        <div class="col-md-6">
          <div class="form-group">
            <label for="stock-sapCode">{{ '::SapCode' | abpLocalization }}</label>
            <input autocomplete="off" id="stock-sapCode" formControlName="sapCode" class="form-control" />
          </div>
        </div>
        <div class="col-md-6">
          <div class="form-group">
            <label for="stock-Status">{{ '::Status' | abpLocalization }}</label>
            <input autocomplete="off" id="stock-Status" formControlName="materialStatus" class="form-control" />
          </div>
        </div>
        <div class="col-md-6">
          <div class="form-group">
            <label for="stock-descriptionEN">{{ '::DescriptionEN' | abpLocalization }}</label>
            <input autocomplete="off" id="stock-descriptionEN" formControlName="descriptionEN" class="form-control" />
          </div>
        </div>
        <div class="col-md-6">
          <div class="form-group">
            <label for="stock-descriptionVN">{{ '::DescriptionVN' | abpLocalization }}</label>
            <input autocomplete="off" id="stock-descriptionVN" formControlName="descriptionVN" class="form-control" />
          </div>
        </div>
        <div class="col-md-6">
          <div class="form-group">
            <label for="stock-unit">{{ '::Unit' | abpLocalization }}</label>
            <input autocomplete="off" id="stock-unit" formControlName="unit" class="form-control" />
          </div>
        </div>
        <div class="col-md-6">
          <div class="form-group">
            <label for="stock-origin">{{ '::Origin' | abpLocalization }}</label>
            <input autocomplete="off" id="stock-origin" formControlName="origin" class="form-control" />
          </div>
        </div>
        <div class="col-md-6">
          <div class="form-group">
            <label for="stock-standardPrice">{{ '::StandardPrice' | abpLocalization }}</label>
            <input
              autocomplete="off"
              type="number"
              id="stock-standardPrice"
              formControlName="standardPrice"
              class="form-control" />
          </div>
        </div>
        <div class="col-md-6">
          <div class="form-check mt-4">
            <input autocomplete="off" type="checkbox" id="stock-epa" formControlName="epa" class="form-check-input" />
            <label for="stock-epa" class="form-check-label">
              {{ '::EPA' | abpLocalization }}
            </label>
          </div>
        </div>
        <div class="col-md-12">
          <div class="form-group">
            <label for="stock-note">{{ '::Note' | abpLocalization }}</label>
            <textarea id="stock-note" formControlName="note" class="form-control"></textarea>
          </div>
        </div>
      </div>
      <div class="modal-footer">
        <!-- <button type="button" class="btn btn-secondary" (click)="activeModal.dismiss()">
          {{ '::Cancel' | abpLocalization }}
        </button> -->
        <button type="submit" class="btn btn-primary">
          {{ '::Save' | abpLocalization }}
        </button>
      </div>
    </form>
  `,
  imports: [
    CoreModule,
    ThemeSharedModule,
    FormsModule,
    ReactiveFormsModule,
    NgbTooltipModule,
    NgxValidateCoreModule,
    PageModule,
    NgbModalModule,
  ],
  standalone: true,
})
export class MaterialDetailModalComponent {
  service = inject(StockManagementDetailViewService);

  save(): void {
    if (this.service.form.invalid) {
      return;
    }

    if (this.service.selected?.id) {
      this.service.update();
    } else {
      this.service.create();
    }
  }
}
