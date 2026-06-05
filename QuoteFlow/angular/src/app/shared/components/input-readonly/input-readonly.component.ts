import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { NumberFormatPipe } from 'src/app/shared/pipes/number-format.pipe';

@Component({
  selector: 'app-input-readonly',
  standalone: true,
  imports: [ReactiveFormsModule, MatInputModule, NumberFormatPipe, CommonModule],
  templateUrl: './input-readonly.component.html',
  styleUrls: ['./input-readonly.component.scss'],
})
export class InputReadOnlyComponent {
  @Input() value: string | number | null;
  @Input() allowDecimal = true;
  @Input() inputStyle: { [key: string]: any } = {};
  @Input() blankValue = '-';
}
