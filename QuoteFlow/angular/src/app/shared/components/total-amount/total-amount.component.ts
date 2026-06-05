import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';
import { NumberFormatPipe } from '../../pipes/number-format.pipe';

@Component({
  selector: 'app-total-amount',
  standalone: true,
  imports: [NumberFormatPipe, CommonModule],
  templateUrl: './total-amount.component.html',
})
export class TotalAmountComponent {
  @Input() label = 'Total In VND:';
  @Input() subLabel = '';
  @Input() value: number;
  @Input() textColor = 'text-danger';
  @Input() decimalPlaces = 0;
}
