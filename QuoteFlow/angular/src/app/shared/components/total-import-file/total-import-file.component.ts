import { Component, Input } from '@angular/core';
import { NumberFormatPipe } from '../../pipes/number-format.pipe';

@Component({
  selector: 'app-total-import-file',
  standalone: true,
  imports: [NumberFormatPipe],
  templateUrl: './total-import-file.component.html',
})
export class TotalImportFileComponent {
  @Input() value: number;
  @Input() success: number | string | undefined;
  @Input() failed: number | string | undefined;
}
