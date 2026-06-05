export function formatCurrency(value: number): string {
  if (!value && value !== 0) return '';
  return new Intl.NumberFormat('en-US', {
    style: 'decimal',
    minimumFractionDigits: 0,
    maximumFractionDigits: 2,
    useGrouping: true,
  }).format(value);
}
export function formatCurrencyDelete(value: number): string {
  if (!value && value !== 0) return '';
  if (value == -1) return 'DELETE VALUE';
  return new Intl.NumberFormat('en-US', {
    style: 'decimal',
    minimumFractionDigits: 0,
    maximumFractionDigits: 2,
    useGrouping: true,
  }).format(value);
}
export function formatStringDelete(value: any): string {
  if (!value) {
    return '';
  }
  if (value === '-1') {
    return 'DELETE VALUE';
  }
  return value;
}

export function formatNegative(value: number): string {
  if (value == null) return ''; // null hoặc undefined

  if (value < 0) {
    return 'KCT';
  }

  return new Intl.NumberFormat('en-US', {
    style: 'decimal',
    minimumFractionDigits: 0,
    maximumFractionDigits: 2,
    useGrouping: true,
  }).format(value);
}

export function formatNumberDelete(value: number): string {
  if (value == null) return '-'; // null hoặc undefined

  if (value < 0) {
    return 'DELETE VALUE';
  }

  return new Intl.NumberFormat('en-US', {
    style: 'decimal',
    minimumFractionDigits: 0,
    maximumFractionDigits: 2,
    useGrouping: true,
  }).format(value);
}

export function formatDateTime(value: number, time?: boolean): string {
  if (value === null || value === undefined) return '';

  return time
    ? new Date(value).toLocaleDateString('en-GB') +
        ' ' +
        new Date(value).toLocaleTimeString('en-GB', { hour: '2-digit', minute: '2-digit' })
    : new Date(value).toLocaleDateString('en-GB');
}

export function formatDateTimeDelete(value: number, time?: boolean): string {
  if (value === null || value === undefined) return '';

  // check -1 flag
  if (value === -1) return 'DELETE VALUE';

  const date = new Date(value);

  // check DateTime.MinValue (0001-01-01)
  if (
    date.getFullYear() === 1 &&
    date.getMonth() === 0 && // tháng 0 = January
    date.getDate() === 1
  ) {
    return 'DELETE VALUE';
  }

  if (isNaN(date.getTime())) return ''; // fallback nếu invalid

  return time
    ? date.toLocaleDateString('en-GB') +
        ' ' +
        date.toLocaleTimeString('en-GB', { hour: '2-digit', minute: '2-digit' })
    : date.toLocaleDateString('en-GB');
}

/**
 * Transforms a newline-separated string into a comma-separated string.
 * Trims whitespace and removes empty lines.
 *
 * @param input - The original string (e.g., from a textarea or form control)
 * @returns A comma-separated string
 */
export function formatCommaSeparatedStr(input: string | null | undefined): string {
  if (!input) {
    return '';
  }

  return input
    .split('\n') // Split by new lines
    .map(code => code.trim()) // Trim each line
    .filter(code => code !== '') // Remove empty lines
    .join(','); // Join with commas
}
