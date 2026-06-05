import { AbstractControl, FormArray, FormGroup } from '@angular/forms';

export class FormGroupHelper {
  public static validateDisabledForm(formGroup: FormGroup): boolean {
    if (formGroup.valid) {
      return true; // Already valid, no need for temporary enabling
    }

    // Create a map to store disabled states with control paths as keys
    const disabledControls: Map<string, boolean> = new Map();

    try {
      // Enable all controls without emitting events
      const enableFormWithoutEmit = (group: FormGroup | FormArray, path: string = '') => {
        if (group instanceof FormGroup) {
          Object.keys(group.controls).forEach(controlName => {
            const control = group.get(controlName);
            const controlPath = path ? `${path}.${controlName}` : controlName;

            if (!control) return;

            // Store disabled state with full path
            disabledControls.set(controlPath, control.disabled);

            if (control instanceof FormGroup || control instanceof FormArray) {
              enableFormWithoutEmit(control, controlPath);
            } else if (control.disabled) {
              control.enable({ emitEvent: false });
            }
          });
        } else if (group instanceof FormArray) {
          group.controls.forEach((control, index) => {
            const controlPath = `${path}[${index}]`;

            // Store disabled state with full path
            disabledControls.set(controlPath, control.disabled);

            if (control instanceof FormGroup || control instanceof FormArray) {
              enableFormWithoutEmit(control, controlPath);
            } else if (control.disabled) {
              control.enable({ emitEvent: false });
            }
          });
        }
      };

      // Restore all disabled states
      const restoreDisabledState = (group: FormGroup | FormArray, path: string = '') => {
        if (group instanceof FormGroup) {
          Object.keys(group.controls).forEach(controlName => {
            const control = group.get(controlName);
            const controlPath = path ? `${path}.${controlName}` : controlName;

            if (!control) return;

            if (control instanceof FormGroup || control instanceof FormArray) {
              restoreDisabledState(control, controlPath);
            } else if (disabledControls.get(controlPath)) {
              control.disable({ emitEvent: false });
            }
          });
        } else if (group instanceof FormArray) {
          group.controls.forEach((control, index) => {
            const controlPath = `${path}[${index}]`;

            if (control instanceof FormGroup || control instanceof FormArray) {
              restoreDisabledState(control, controlPath);
            } else if (disabledControls.get(controlPath)) {
              control.disable({ emitEvent: false });
            }
          });
        }
      };

      // Enable the form temporarily
      enableFormWithoutEmit(formGroup);

      // Check validity
      const isValid = formGroup.valid;

      // Restore original disabled state
      restoreDisabledState(formGroup);

      return isValid;
    } catch (error) {
      console.error('Error validating disabled form:', error);

      // Attempt to restore form state in case of error
      try {
        disabledControls.forEach((isDisabled, path) => {
          if (isDisabled) {
            const pathParts = path.split('.');
            const control = formGroup.get(path);
            if (control) control.disable({ emitEvent: false });
          }
        });
      } catch (restoreError) {
        console.error('Failed to restore form state:', restoreError);
      }

      return false;
    }
  }

  public static toggleFormControls(
    control: AbstractControl,
    isDisabled: boolean,
    exceptions: FieldException[] = [],
  ): void {
    if (control instanceof FormGroup || control instanceof FormArray) {
      Object.entries(control.controls).forEach(([name, childControl]) => {
        // Check if this control is in the exceptions list
        const exception = exceptions.find(e => e.field === name);

        if (exception) {
          // Skip enabling if excludeOnEnable is true and we're enabling
          if (isDisabled === false && exception.config?.excludeOnEnable === true) {
            return;
          }
          // Skip disabling if excludeOnDisable is true and we're disabling
          if (isDisabled === true && exception.config?.excludeOnDisable === true) {
            return;
          }
        }

        this.toggleFormControls(childControl, isDisabled, exceptions);
      });
    }

    // Apply enable/disable to the current control
    if (isDisabled) {
      control.disable({ emitEvent: false });
    } else {
      control.enable({ emitEvent: false });
    }
  }

  public static markAsTouched(controls: AbstractControl[], excludeNull: boolean = false): void {
    controls.forEach(control => {
      if (control instanceof FormGroup || control instanceof FormArray) {
        this.markAsTouched(Object.values(control.controls), excludeNull);
      } else if (control && (!excludeNull || control.value !== null)) {
        control.markAsTouched({ onlySelf: true });
      }
    });
  }

  public static markFormGroupTouched(formGroup: FormGroup | FormArray): void {
    if (formGroup instanceof FormGroup) {
      Object.keys(formGroup.controls).forEach(key => {
        const control = formGroup.get(key);
        if (control instanceof FormGroup || control instanceof FormArray) {
          this.markFormGroupTouched(control);
        } else {
          control?.markAsTouched();
        }
      });
    } else if (formGroup instanceof FormArray) {
      formGroup.controls.forEach(control => {
        if (control instanceof FormGroup || control instanceof FormArray) {
          this.markFormGroupTouched(control);
        } else {
          control.markAsTouched();
        }
      });
    }
  }
}

interface ExceptionConfig {
  excludeOnEnable?: boolean;
  excludeOnDisable?: boolean;
}

interface FieldException {
  field: string;
  config?: ExceptionConfig;
}
