import { InjectionToken } from '@angular/core';

export const IS_404_HANDLED = new InjectionToken<boolean>('Is404Handled', {
  providedIn: 'root',
  factory: () => false, // Default value
});
