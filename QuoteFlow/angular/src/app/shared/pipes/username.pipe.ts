import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'username',
  standalone: true,
})
export class UsernamePipe implements PipeTransform {
  transform(value: string | null | undefined): string {
    if (!value) {
      return 'System';
    }

    // Split by '@' and return only the part before '@'
    const atIndex = value.indexOf('@');
    if (atIndex !== -1) {
      return value.substring(0, atIndex);
    }

    // If no '@' found, return the original value
    return value;
  }
}
