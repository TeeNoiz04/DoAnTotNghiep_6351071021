import { PageModule } from '@abp/ng.components/page';
import { CoreModule } from '@abp/ng.core';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { Component, inject } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgbModalModule, NgbTooltipModule } from '@ng-bootstrap/ng-bootstrap';
import { NgxValidateCoreModule } from '@ngx-validate/core';
import { MaterialDetailViewService } from '../services/material-detail.service';

@Component({
  selector: 'app-material-detail-modal',
  template: `
    <form [formGroup]="service.form" (ngSubmit)="save()" [validateOnSubmit]="true">
      <div class="row">
        <div class="col-md-6">
          <div class="form-group">
            <label for="material-golfaCode">{{ '::Material Code' | abpLocalization }}</label>
            <input
              autocomplete="off"
              id="material-golfaCode"
              formControlName="golfaCode"
              class="form-control"
              autofocus />
          </div>
        </div>
        <div class="col-md-6">
          <div class="form-group">
            <label for="material-model">{{ '::Model' | abpLocalization }}</label>
            <input autocomplete="off" id="material-model" formControlName="model" class="form-control" />
          </div>
        </div>
        <div class="col-md-6">
          <div class="form-group">
            <label for="material-sapCode">{{ '::SapCode' | abpLocalization }}</label>
            <input autocomplete="off" id="material-sapCode" formControlName="sapCode" class="form-control" />
          </div>
        </div>
        <div class="col-md-6">
          <div class="form-group">
            <label for="material-materialStatus">{{ '::MaterialStatus' | abpLocalization }}</label>
            <input
              autocomplete="off"
              id="material-materialStatus"
              formControlName="materialStatus"
              class="form-control" />
          </div>
        </div>
        <div class="col-md-6">
          <div class="form-group">
            <label for="material-descriptionEN">{{ '::DescriptionEN' | abpLocalization }}</label>
            <input
              autocomplete="off"
              id="material-descriptionEN"
              formControlName="descriptionEN"
              class="form-control" />
          </div>
        </div>
        <div class="col-md-6">
          <div class="form-group">
            <label for="material-descriptionVN">{{ '::DescriptionVN' | abpLocalization }}</label>
            <input
              autocomplete="off"
              id="material-descriptionVN"
              formControlName="descriptionVN"
              class="form-control" />
          </div>
        </div>
        <div class="col-md-6">
          <div class="form-group">
            <label for="material-unit">{{ '::Unit' | abpLocalization }}</label>
            <input autocomplete="off" id="material-unit" formControlName="unit" class="form-control" />
          </div>
        </div>
        <div class="col-md-6">
          <div class="form-group">
            <label for="material-origin">{{ '::Origin' | abpLocalization }}</label>
            <input autocomplete="off" id="material-origin" formControlName="origin" class="form-control" />
          </div>
        </div>
        <div class="col-md-6">
          <div class="form-group">
            <label for="material-standardPrice">{{ '::StandardPrice' | abpLocalization }}</label>
            <input
              autocomplete="off"
              type="number"
              id="material-standardPrice"
              formControlName="standardPrice"
              class="form-control" />
          </div>
        </div>
        <div class="col-md-6">
          <div class="form-check mt-4">
            <input type="checkbox" id="material-epa" formControlName="epa" class="form-check-input" />
            <label for="material-epa" class="form-check-label">
              {{ '::EPA' | abpLocalization }}
            </label>
          </div>
        </div>
        <div class="col-md-12">
          <div class="form-group">
            <label for="material-note">{{ '::Note' | abpLocalization }}</label>
            <textarea id="material-note" formControlName="note" class="form-control"></textarea>
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
  service = inject(MaterialDetailViewService);

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
