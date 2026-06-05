import { Injectable } from '@angular/core';

export interface ModalNavigationCondition<T> {
  modalType: string;
  condition: (detail: T) => boolean;
}

export interface ModalNavigationConfig<T> {
  modalType: string;
  conditions: ModalNavigationCondition<T>[];
}

@Injectable({
  providedIn: 'root',
})
export class ModalNavigationService {
  private configRegistry = new Map<string, ModalNavigationConfig<any>>();

  /**
   * Register a modal navigation configuration
   * @param moduleType - Module identifier (e.g., 'dpo', 'gic')
   * @param config - Modal navigation configuration
   */
  registerConfig<T>(moduleType: string, config: ModalNavigationConfig<T>): void {
    this.configRegistry.set(moduleType, config);
  }

  /**
   * Gets the next eligible detail index for a specific modal type
   * @param moduleType - Module identifier (e.g., 'dpo', 'gic')
   * @param allDetails - All details
   * @param currentIndex - Current detail index
   * @param modalType - Type of modal to find next eligible detail for
   * @returns Index of next eligible detail or -1 if none found
   */
  getNextEligibleDetailIndex<T>(
    moduleType: string,
    allDetails: T[],
    currentIndex: number,
    modalType: string,
  ): number {
    if (!allDetails || allDetails.length === 0) {
      return -1;
    }

    const condition = this.getCondition(moduleType, modalType);
    if (!condition) {
      return -1;
    }

    // Start searching from the next item after current
    for (let i = currentIndex + 1; i < allDetails.length; i++) {
      const detail = allDetails[i];
      if (condition.condition(detail)) {
        return i;
      }
    }

    // Wrap around and search from beginning to current index
    for (let i = 0; i < currentIndex; i++) {
      const detail = allDetails[i];
      if (condition.condition(detail)) {
        return i;
      }
    }

    return -1;
  }

  /**
   * Gets the previous eligible detail index for a specific modal type
   * @param moduleType - Module identifier (e.g., 'dpo', 'gic')
   * @param allDetails - All details
   * @param currentIndex - Current detail index
   * @param modalType - Type of modal to find previous eligible detail for
   * @returns Index of previous eligible detail or -1 if none found
   */
  getPreviousEligibleDetailIndex<T>(
    moduleType: string,
    allDetails: T[],
    currentIndex: number,
    modalType: string,
  ): number {
    if (!allDetails || allDetails.length === 0) {
      return -1;
    }

    const condition = this.getCondition(moduleType, modalType);
    if (!condition) {
      return -1;
    }

    // Start searching from the previous item before current (backwards)
    for (let i = currentIndex - 1; i >= 0; i--) {
      const detail = allDetails[i];
      if (condition.condition(detail)) {
        return i;
      }
    }

    // Wrap around and search from end to current index (backwards)
    for (let i = allDetails.length - 1; i > currentIndex; i--) {
      const detail = allDetails[i];
      if (condition.condition(detail)) {
        return i;
      }
    }

    return -1;
  }

  /**
   * Checks if there's a next eligible detail for a specific modal type
   * @param moduleType - Module identifier (e.g., 'dpo', 'gic')
   * @param allDetails - All details
   * @param currentIndex - Current detail index
   * @param modalType - Type of modal to check
   * @returns True if there's a next eligible detail
   */
  hasNextEligibleDetail<T>(
    moduleType: string,
    allDetails: T[],
    currentIndex: number,
    modalType: string,
  ): boolean {
    return this.getNextEligibleDetailIndex(moduleType, allDetails, currentIndex, modalType) !== -1;
  }

  /**
   * Checks if there's a previous eligible detail for a specific modal type
   * @param moduleType - Module identifier (e.g., 'dpo', 'gic')
   * @param allDetails - All details
   * @param currentIndex - Current detail index
   * @param modalType - Type of modal to check
   * @returns True if there's a previous eligible detail
   */
  hasPreviousEligibleDetail<T>(
    moduleType: string,
    allDetails: T[],
    currentIndex: number,
    modalType: string,
  ): boolean {
    return (
      this.getPreviousEligibleDetailIndex(moduleType, allDetails, currentIndex, modalType) !== -1
    );
  }

  /**
   * Checks if a detail is eligible for a specific modal type
   * @param moduleType - Module identifier (e.g., 'dpo', 'gic')
   * @param detail - Detail to check
   * @param modalType - Type of modal to check eligibility for
   * @returns True if detail is eligible for the modal
   */
  isEligibleForModal<T>(moduleType: string, detail: T, modalType: string): boolean {
    const condition = this.getCondition(moduleType, modalType);
    return condition ? condition.condition(detail) : false;
  }

  private getCondition<T>(
    moduleType: string,
    modalType: string,
  ): ModalNavigationCondition<T> | undefined {
    const config = this.configRegistry.get(moduleType);
    if (!config) {
      return undefined;
    }
    return config.conditions.find(c => c.modalType === modalType);
  }
}
