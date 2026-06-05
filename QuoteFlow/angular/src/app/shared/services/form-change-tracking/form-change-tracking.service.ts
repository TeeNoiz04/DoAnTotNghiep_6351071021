import { Injectable } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { BehaviorSubject, Observable } from 'rxjs';
import isEqual from 'lodash/isEqual';
import cloneDeep from 'lodash/cloneDeep';

@Injectable({
  providedIn: 'root',
})
export class FormChangeTrackingService {
  private originalDataMap = new Map<string, any>();
  private formDirtyState = new BehaviorSubject<boolean>(false);
  private changedFieldsMap = new BehaviorSubject<Map<string, string[]>>(new Map());

  /**
   * Initialize tracking for a form with original data
   */
  initializeTracking(formId: string, form: FormGroup, originalData: any): void {
    // Store original data
    this.originalDataMap.set(formId, cloneDeep(originalData));

    // Setup form value change subscription
    form.valueChanges.subscribe(() => {
      this.checkFormChanges(formId, form);
    });
  }

  /**
   * Compare current form values with original data
   */
  private checkFormChanges(formId: string, form: FormGroup): void {
    const originalData = this.originalDataMap.get(formId);
    if (!originalData) return;

    const currentValues = form.getRawValue();
    const changedFields = this.findChangedFields(originalData, currentValues);
    const currentMap = this.changedFieldsMap.getValue();
    currentMap.set(formId, changedFields);
    this.changedFieldsMap.next(currentMap);

    // Update overall dirty state
    this.updateDirtyState();
  }

  updateOriginalData(formId: string, newOriginalData: any, preserveChanges = false): void {
    // Keep track of which fields were changed before
    const currentChangedFields = preserveChanges
      ? this.changedFieldsMap.getValue().get(formId) || []
      : [];

    // Update the original data
    this.originalDataMap.set(formId, cloneDeep(newOriginalData));

    if (preserveChanges && currentChangedFields.length > 0) {
      // Keep the changed fields as they were
      const changedFieldsMap = this.changedFieldsMap.getValue();
      changedFieldsMap.set(formId, currentChangedFields);
      this.changedFieldsMap.next(changedFieldsMap);
    } else {
      // Clear changed fields
      const changedFieldsMap = this.changedFieldsMap.getValue();
      changedFieldsMap.set(formId, []);
      this.changedFieldsMap.next(changedFieldsMap);
    }

    // Update the dirty state
    this.updateDirtyState();
  }

  /**
   * Get a list of field paths that have changed values
   */
  private findChangedFields(originalData: any, currentValues: any, prefix = ''): string[] {
    const changedFields: string[] = [];

    Object.keys(currentValues).forEach(key => {
      const currentPath = prefix ? `${prefix}.${key}` : key;

      // Skip if the key doesn't exist in original data
      if (originalData[key] === undefined) return;

      if (
        typeof currentValues[key] === 'object' &&
        currentValues[key] !== null &&
        typeof originalData[key] === 'object' &&
        originalData[key] !== null
      ) {
        // Recursively check nested objects
        const nestedChanges = this.findChangedFields(
          originalData[key],
          currentValues[key],
          currentPath,
        );
        changedFields.push(...nestedChanges);
      } else if (!this.areValuesEqual(currentValues[key], originalData[key])) {
        changedFields.push(currentPath);
      }
    });

    return changedFields;
  }

  resetAllTracking(): void {
    // Clear all changed fields
    this.changedFieldsMap.next(new Map<string, string[]>());

    // Reset dirty state
    this.formDirtyState.next(false);
  }

  resetFormStructure(formId: string, form: FormGroup, preserveDirtyState = false): void {
    const wasDirty = this.formDirtyState.getValue();
    const currentValues = form.getRawValue();
    this.originalDataMap.set(formId, cloneDeep(currentValues));

    // Reset changed fields
    const currentMap = this.changedFieldsMap.getValue();
    currentMap.set(formId, []);
    this.changedFieldsMap.next(currentMap);

    // Update dirty state
    if (preserveDirtyState && wasDirty) {
      this.formDirtyState.next(true);
    } else {
      this.updateDirtyState();
    }
  }

  private areValuesEqual(value1: any, value2: any): boolean {
    // If both values are null/undefined, they're equal
    if (value1 == null && value2 == null) return true;

    // Handle Date objects
    if (value1 instanceof Date || value2 instanceof Date) {
      return this.areDatesEqual(value1, value2);
    }

    // Handle date strings (ISO format or date-only strings)
    if (this.isDateString(value1) && this.isDateString(value2)) {
      return this.areDatesEqual(value1, value2);
    }

    // Default to lodash isEqual for other types
    return isEqual(value1, value2);
  }

  private isDateString(value: any): boolean {
    if (typeof value !== 'string') return false;

    const datePattern = /^\d{4}-\d{2}-\d{2}(T\d{2}:\d{2}:\d{2}(.\d+)?(Z|[+-]\d{2}:\d{2})?)?$/;
    if (!datePattern.test(value)) return false;

    // Make sure it's a valid date
    const date = new Date(value);
    return !isNaN(date.getTime());
  }

  private areDatesEqual(date1: any, date2: any): boolean {
    try {
      // Convert to Date objects if they're strings
      const d1 = date1 instanceof Date ? date1 : new Date(date1);
      const d2 = date2 instanceof Date ? date2 : new Date(date2);

      // Check if both are valid dates
      if (isNaN(d1.getTime()) || isNaN(d2.getTime())) {
        return isEqual(date1, date2);
      }

      // Compare year, month, day only (ignore time)
      return (
        d1.getFullYear() === d2.getFullYear() &&
        d1.getMonth() === d2.getMonth() &&
        d1.getDate() === d2.getDate()
      );
    } catch (error) {
      console.error('Error comparing dates:', error);
      return isEqual(date1, date2);
    }
  }

  /**
   * Update the overall form dirty state based on all tracked forms
   */
  private updateDirtyState(): void {
    const changedFieldsMap = this.changedFieldsMap.getValue();
    let isDirty = false;

    changedFieldsMap.forEach(fields => {
      if (fields.length > 0) {
        isDirty = true;
      }
    });

    this.formDirtyState.next(isDirty);
  }

  /**
   * Get observable of form dirty state
   */
  getDirtyState(): Observable<boolean> {
    return this.formDirtyState.asObservable();
  }

  /**
   * Get observable of changed fields by form
   */
  getChangedFields(): Observable<Map<string, string[]>> {
    return this.changedFieldsMap.asObservable();
  }

  /**
   * Get changed fields for a specific form
   */
  getChangedFieldsForForm(formId: string): string[] {
    const map = this.changedFieldsMap.getValue();
    return map.get(formId) || [];
  }

  /**
   * Reset tracking - mark as pristine
   */
  resetTracking(formId: string, form: FormGroup): void {
    this.originalDataMap.set(formId, cloneDeep(form.getRawValue()));

    const currentMap = this.changedFieldsMap.getValue();
    currentMap.set(formId, []);
    this.changedFieldsMap.next(currentMap);

    this.updateDirtyState();
  }

  /**
   * Helper to flatten nested objects for easier comparison
   */
  private flattenObject(obj: any, prefix = ''): { [key: string]: any } {
    const flattened: { [key: string]: any } = {};

    Object.keys(obj).forEach(key => {
      const currentPath = prefix ? `${prefix}.${key}` : key;

      if (typeof obj[key] === 'object' && obj[key] !== null) {
        Object.assign(flattened, this.flattenObject(obj[key], currentPath));
      } else {
        flattened[currentPath] = obj[key];
      }
    });

    return flattened;
  }
}
