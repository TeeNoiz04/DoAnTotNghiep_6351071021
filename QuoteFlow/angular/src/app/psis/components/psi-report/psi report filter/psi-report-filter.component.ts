import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { NgbDatepickerModule } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-psi-report-filter',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, NgbDatepickerModule],
  templateUrl: './psi-report-filter.component.html',
})
export class PsiReportFilterComponent implements OnInit {
  @Output() filterChanged = new EventEmitter<any>();
  form: FormGroup;

  constructor(private fb: FormBuilder) {}

  ngOnInit() {
    this.form = this.fb.group({
      fiscalYear: ['2025'],
      materialType: ['FA'],
      productCategory: [null],
    });

    this.filterChanged.emit(this.form.value);

    this.form.get('materialType')?.valueChanges.subscribe(value => {
      this.filterChanged.emit(this.form.value);
    });
  }

  onClear() {
    this.form.reset({
      fiscalYear: null,
      materialType: null,
      productCategory: null,
    });
    this.filterChanged.emit(this.form.value);
  }
}
