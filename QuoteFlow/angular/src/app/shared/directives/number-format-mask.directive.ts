import {
  AfterViewInit,
  Directive,
  ElementRef,
  Input,
  OnChanges,
  SimpleChanges,
} from '@angular/core';
import { NgControl } from '@angular/forms';

@Directive({
  selector: '[appNumberFormatMask]',
  standalone: true,
})
export class NumberFormatMaskDirective implements AfterViewInit, OnChanges {
  @Input() allowDecimal = true;
  @Input() allowNegative = false;
  @Input() decimalPlaces = 2;
  @Input() mask: string | null = null;
  @Input() thousandSeparator = ',';
  @Input() decimalMarker = '.';
  @Input() maxValue = 999999999;

  constructor(
    private el: ElementRef<HTMLInputElement>,
    private control: NgControl,
  ) {}

  ngAfterViewInit(): void {
    setTimeout(() => this.applyMask(), 0);
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (
      changes['allowDecimal'] ||
      changes['decimalPlaces'] ||
      changes['allowNegative'] ||
      changes['mask'] ||
      changes['thousandSeparator'] ||
      changes['decimalMarker'] ||
      changes['maxValue']
    ) {
      setTimeout(() => this.applyMask(), 0);
    }
  }

  private applyMask(): void {
    const input = this.el.nativeElement;

    // Check for existing direct mask attribute
    const existingMask = input.getAttribute('mask');

    // If direct mask attribute is present and we haven't set our own input property,
    // parse it to extract settings
    if (existingMask && !this.mask) {
      // Parse existing mask like "separator.2"
      const parts = existingMask.split('.');
      if (parts[0] === 'separator' && parts.length > 1) {
        this.decimalPlaces = parseInt(parts[1], 10);
        this.allowDecimal = true;
        // Don't override the mask, as it's already set
      } else if (parts[0] === 'separator' && parts.length === 1) {
        this.allowDecimal = false;
        // Don't override the mask, as it's already set
      }
    } else {
      // Apply our mask based on directive inputs
      if (this.allowDecimal && this.decimalPlaces > 0) {
        input.setAttribute('mask', `separator.${this.decimalPlaces}`);
      } else {
        input.setAttribute('mask', 'separator');
      }
    }

    // Apply common settings regardless of mask source
    input.setAttribute('thousandSeparator', this.thousandSeparator);
    input.setAttribute('decimalMarker', this.decimalMarker);
    input.setAttribute('allowNegativeNumbers', String(this.allowNegative));
    input.setAttribute('dropSpecialCharacters', 'false');
    input.setAttribute('separatorLimit', String(this.maxValue));
  }
}
