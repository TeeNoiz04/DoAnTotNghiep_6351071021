import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

/**
 * Validator for Tax Code following Vietnamese and Internal format rules
 *
 * Valid formats:
 * 1. Internal: INT-XXXXXX... (alphanumeric after INT-)
 * 2. Vietnamese: 10 alphanumeric characters, optionally followed by "-" and 3 alphanumeric characters
 *
 * @returns ValidatorFn
 */
export function TaxCodeValidator(): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    const taxCode = control.value;

    // Allow empty values (use Validators.required separately if needed)
    if (!taxCode || taxCode.trim() === '') {
      return null;
    }

    const trimmedTaxCode = taxCode.trim();

    // Check if it matches pattern with prefix (e.g., "ABC-", "XYZ-")
    if (/^[A-Za-z]+-/.test(trimmedTaxCode)) {
      // If it has a letter prefix with dash, it must be "INT-"
      if (!trimmedTaxCode.toUpperCase().startsWith('INT-')) {
        return {
          taxCode: {
            message: 'Internal Tax Code format is invalid.',
          },
        };
      }

      // Check if the part after "INT-" is alphanumeric
      const internalPattern = /^INT-[A-Za-z0-9]+$/i;
      if (!internalPattern.test(trimmedTaxCode)) {
        return {
          taxCode: {
            message: 'Internal Tax Code format is invalid.',
          },
        };
      }

      return null; // Valid internal tax code
    }

    // Vietnamese tax code format: 10 alphanumeric characters, optionally followed by "-" and 3 alphanumeric characters
    const vietnamesePattern = /^[A-Za-z0-9]{10}(-[A-Za-z0-9]{3})?$/;

    if (!vietnamesePattern.test(trimmedTaxCode)) {
      // Provide more specific error messages
      if (trimmedTaxCode.includes('-')) {
        const parts = trimmedTaxCode.split('-');

        if (parts.length > 2) {
          return {
            taxCode: { message: "Tax Code has too many '-' characters." },
          };
        }

        const prefix = parts[0];
        const suffix = parts[1];

        if (prefix.length !== 10) {
          return {
            taxCode: { message: "Tax Code prefix (before '-') must be exactly 10 characters." },
          };
        }

        if (suffix.length !== 3) {
          return {
            taxCode: {
              message: "Tax Code suffix (after '-') must be exactly 3 alphanumeric characters.",
            },
          };
        }

        if (!/^[A-Za-z0-9]{3}$/.test(suffix)) {
          return {
            taxCode: { message: 'Tax Code suffix must be alphanumeric.' },
          };
        }

        if (!/^[A-Za-z0-9]{10}$/.test(prefix)) {
          return {
            taxCode: { message: 'Tax Code prefix must be alphanumeric.' },
          };
        }
      } else {
        return {
          taxCode: {
            message: 'Internal Tax Code format is invalid.',
          },
        };
      }
    }

    // Valid tax code
    return null;
  };
}
