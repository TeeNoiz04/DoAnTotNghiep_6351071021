import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'tableFilter',
  standalone: true,
  pure: false,
})
export class TableFilterPipe implements PipeTransform {
  transform(items: any[] | null | undefined, searchText: string, columns?: string[]): any[] {
    if (!items || !Array.isArray(items)) return [];
    if (!searchText || searchText.trim() === '') return items;

    const normalizedSearchText = searchText.toLowerCase().trim();

    return items.filter(item => {
      if (columns && columns.length > 0) {
        return columns.some(column => this.searchInProperty(item, column, normalizedSearchText));
      } else {
        return Object.keys(item).some(key =>
          this.searchInProperty(item, key, normalizedSearchText),
        );
      }
    });
  }

  private searchInProperty(item: any, property: string, searchText: string): boolean {
    if (!item) return false;

    if (property === 'amountToBePaid') {
      const value =
        (item.invoiceAmount || 0) - (item.advanceAmount || 0) - (item.deductedAmount || 0);
      return this.checkValue(value, searchText);
    }

    const properties = property.split('.');
    let value = item;

    for (const prop of properties) {
      if (value === null || value === undefined) {
        return false;
      }
      const actualProp = this.findPropertyCaseInsensitive(value, prop);
      if (actualProp === null) {
        return false;
      }

      value = value[actualProp];
    }

    return this.checkValue(value, searchText);
  }

  private findPropertyCaseInsensitive(obj: any, prop: string): string | null {
    if (!obj || typeof obj !== 'object') return null;

    const keys = Object.keys(obj);
    const lowerProp = prop.toLowerCase();

    for (const key of keys) {
      if (key.toLowerCase() === lowerProp) {
        return key;
      }
    }

    return null;
  }

  private checkValue(value: any, searchText: string): boolean {
    if (value === null || value === undefined) {
      return false;
    }

    switch (typeof value) {
      case 'string': {
        const date = new Date(value);

        if (!isNaN(date.getTime()) && value.trim() !== '') {
          const day = String(date.getDate()).padStart(2, '0');
          const month = String(date.getMonth() + 1).padStart(2, '0'); // Months are 0-indexed
          const year = date.getFullYear();

          const formattedDate = `${day}/${month}/${year}`;

          return (
            formattedDate.includes(searchText) ||
            value.toLowerCase().includes(searchText.toLowerCase())
          );
        }

        // Fall back to simple string search
        return value.toLowerCase().includes(searchText.toLowerCase());
      }
      case 'number': {
        const numString = value.toString();
        const localizedNum = value.toLocaleString();
        return numString.includes(searchText) || localizedNum.toLowerCase().includes(searchText);
      }

      case 'boolean':
        return value.toString().toLowerCase().includes(searchText);

      case 'object':
        if (value instanceof Date) {
          const dateFormats = [
            value.toLocaleDateString(),
            value.toLocaleString(),
            value.toISOString().split('T')[0],
          ];
          return dateFormats.some(format => format.toLowerCase().includes(searchText));
        }

        if (Array.isArray(value)) {
          if (value.length === 0) return false;
          return value.some(val => this.checkValue(val, searchText));
        }

        try {
          return Object.values(value).some(val => this.checkValue(val, searchText));
        } catch {
          return String(value).toLowerCase().includes(searchText);
        }

      default:
        return String(value).toLowerCase().includes(searchText);
    }
  }
}
