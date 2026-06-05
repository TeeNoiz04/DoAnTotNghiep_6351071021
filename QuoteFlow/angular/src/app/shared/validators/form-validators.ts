import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';
import { DateHelper } from '../helpers/date-helper';

export function toGreaterThanFromValidator(fromField: string, toField: string): ValidatorFn {
  return (formGroup: AbstractControl): ValidationErrors | null => {
    const fromDate = new Date(formGroup.get(fromField)?.value);
    const toDate = new Date(formGroup.get(toField)?.value);

    // Check if "To" is greater than "From"
    if (fromDate && toDate && toDate < fromDate) {
      return { fromDateNotGreater: true }; // Validation error
    }
    return null; // Valid
  };
}

export function toGreaterThanFromDateTimeValidator(
  fromField: string,
  toField: string,
  fromTimeField?: string,
  toTimeField?: string,
): ValidatorFn {
  return (formGroup: AbstractControl): ValidationErrors | null => {
    const fromDate = formGroup.get(fromField)?.value
      ? new Date(formGroup.get(fromField)?.value)
      : null;
    const toDate = formGroup.get(toField)?.value ? new Date(formGroup.get(toField)?.value) : null;
    const fromTimeValue = fromTimeField ? formGroup.get(fromTimeField)?.value : null;
    const toTimeValue = toTimeField ? formGroup.get(toTimeField)?.value : null;

    let combinedFromDateTime: Date | null = null;
    let combinedToDateTime: Date | null = null;

    if (fromDate) {
      combinedFromDateTime = new Date(fromDate);
      if (fromTimeValue) {
        combinedFromDateTime.setHours(
          fromTimeValue.hour,
          fromTimeValue.minute,
          fromTimeValue.second,
          0,
        );
      }
    }

    if (toDate) {
      combinedToDateTime = new Date(toDate);
      if (toTimeValue) {
        combinedToDateTime.setHours(toTimeValue.hour, toTimeValue.minute, toTimeValue.second, 0);
      }
    }
    // Check if "To" is greater than "From"
    if (combinedFromDateTime && combinedToDateTime && combinedToDateTime < combinedFromDateTime) {
      return { fromDateNotGreater: true }; // Validation error
    }
    return null; // Valid
  };
}

export function takeoutDateRangeValidator(
  fromDateField: string,
  toDateField: string,
  serviceLaptopField: string,
  paperDocumentField: string,
  usbField: string,
  officeLaptopField: string,
  diskField: string,
  otherField: string,
): ValidatorFn {
  return (formGroup: AbstractControl): ValidationErrors | null => {
    const fromDate = formGroup.get(fromDateField)?.value
      ? new Date(formGroup.get(fromDateField)?.value)
      : null;
    const toDate = formGroup.get(toDateField)?.value
      ? new Date(formGroup.get(toDateField)?.value)
      : null;

    // If either date is missing, we can't validate
    if (!fromDate || !toDate) {
      return null;
    }

    // Check selected takeout items
    const isServiceLaptopOnly =
      formGroup.get(serviceLaptopField)?.value === true &&
      formGroup.get(paperDocumentField)?.value !== true &&
      formGroup.get(usbField)?.value !== true &&
      formGroup.get(officeLaptopField)?.value !== true &&
      formGroup.get(diskField)?.value !== true &&
      formGroup.get(otherField)?.value !== true;

    // Calculate working days between dates (excluding weekends)
    const workingDays = DateHelper.getDaysBetweenDates(fromDate, toDate);

    // Determine max allowed days based on takeout items
    const maxAllowedDays = isServiceLaptopOnly ? 90 : 15;

    const result = {
      exceededMaxDays: true,
      currentDays: workingDays,
      maxAllowedDays: maxAllowedDays,
    };

    if (workingDays > maxAllowedDays) {
      return result;
    }

    return null; // Valid
  };
}

export function atLeastOneCheckboxCheckedValidator(): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    const formGroup = control as any;
    const controls = formGroup.controls;

    if (controls) {
      const isAnyCheckboxChecked = Object.keys(controls).some(key => controls[key].value === true);
      return isAnyCheckboxChecked ? null : { atLeastOneCheckboxChecked: true };
    }

    return null;
  };
}

export function conditionalRequiredValidatorForCheckbox(
  checkboxControlName: string,
  textboxControlName: string,
): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    const formGroup = control as any;
    const checkboxControl = formGroup.get(checkboxControlName);
    const textboxControl = formGroup.get(textboxControlName);

    if (checkboxControl && checkboxControl.value && (!textboxControl || !textboxControl.value)) {
      return { conditionalRequired: true };
    }

    return null;
  };
}

// for radio
export function conditionalRequiredValidatorForRadio(
  radioControlName: string,
  textboxControlName: string,
): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    const formGroup = control as any;
    const radioControl = formGroup.get(radioControlName);
    const textboxControl = formGroup.get(textboxControlName);

    if (radioControl && radioControl.value && (!textboxControl || !textboxControl.value)) {
      return { conditionalRequired: true };
    }

    return null;
  };
}

export function phoneNumberValidator(): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    const phone = control.value?.trim(); // Ensure it's a trimmed string
    if (!phone) {
      return { phoneInvalid: 'Phone number is required' };
    }

    // Extract only digits for length check
    const digitsOnly = phone.replace(/\D/g, '');

    // Improved regex for international phone number formats
    const phoneRegex = /^(\+\d{1,3}[\s]?)?(\(\d{1,5}\)|\d{1,5})([\s-]?\d{1,4}){1,3}$/;

    // Check format validity
    const validFormat = phoneRegex.test(phone);

    // Ensure valid digit length (between 7 and 15 digits)
    const validLength = digitsOnly.length >= 7 && digitsOnly.length <= 15;

    // Return error object if invalid
    return validFormat && validLength ? null : { phoneInvalid: 'Invalid phone number format' };
  };
}
