import { CommonModule } from '@angular/common';
import { Component, Input, TemplateRef, ViewChild, forwardRef } from '@angular/core';
import { NG_VALUE_ACCESSOR, ControlValueAccessor, NgModel, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';

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
  selector: 'app-select-cell',
  templateUrl: './selectCell.component.html',
  styleUrls: ['./selectCell.component.scss'],
  standalone: true,
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => AppSelectCellComponent),
      multi: true,
    },
  ],
  imports: [CommonModule, FormsModule, ReactiveFormsModule, NgSelectModule],
})
export class AppSelectCellComponent extends ValueAccessorBase<any> {
  @ViewChild(NgModel) model: NgModel;

  @Input() items: any[] = [];
  @Input() bindValue: string;
  @Input() bindLabel: string;
  @Input() placeholder: string = '';
  @Input() clearable: boolean = true;
  @Input() appendTo: string = 'body';
  @Input() entry: any;
  @Input() disabled: boolean;

  @Input() customLabel: TemplateRef<any> | null = null;
  @Input() customOption: TemplateRef<any> | null = null;

  setDisabledState(isDisabled: boolean): void {
    this.disabled = isDisabled;
  }

  onModelChange(val: any) {
    this.value = val;
    this.onChange(val);
  }
}
