import { CommonModule } from '@angular/common';
import { Component, Input, ViewChild } from '@angular/core';
import { ControlValueAccessor, FormsModule, NG_VALUE_ACCESSOR, NgModel, ReactiveFormsModule } from '@angular/forms';
import { InputNumberComponent } from '../../input-number/input-number.component';

export abstract class ValueAccessorBase<T> implements ControlValueAccessor {
  private innerValue: T;

  get value(): T {
    return this.innerValue;
  }

  set value(value: T) {
    if (value !== this.innerValue) {
      this.innerValue = value;
      this.onChange(value);
    }
  }

  onChange = (value: T) => {};
  onTouched = () => {};

  writeValue(value: T): void {
    this.innerValue = value;
  }

  registerOnChange(fn: (value: T) => void): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: () => void): void {
    this.onTouched = fn;
  }
}

@Component({
  selector: 'app-textbox-cell',
  templateUrl: './textboxCell.component.html',
  styleUrls: ['./textboxCell.component.scss'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: AppTextboxCellComponent,
      multi: true,
    },
  ],
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule, InputNumberComponent],
})
export class AppTextboxCellComponent extends ValueAccessorBase<any> {
  @ViewChild(NgModel) model: NgModel;

  @Input() name: any;
  @Input() typeControl: string = 'text';
  @Input() entry: any;
  @Input() disabled: boolean = false;
  @Input() cssClass: string;
  setDisabledState(isDisabled: boolean): void {
    this.disabled = isDisabled;
  }
}
