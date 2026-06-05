export class NumberHelper {
  private static readonly ones = [
    '',
    'One',
    'Two',
    'Three',
    'Four',
    'Five',
    'Six',
    'Seven',
    'Eight',
    'Nine',
    'Ten',
    'Eleven',
    'Twelve',
    'Thirteen',
    'Fourteen',
    'Fifteen',
    'Sixteen',
    'Seventeen',
    'Eighteen',
    'Nineteen',
  ];

  private static readonly tens = [
    '',
    '',
    'Twenty',
    'Thirty',
    'Forty',
    'Fifty',
    'Sixty',
    'Seventy',
    'Eighty',
    'Ninety',
  ];

  private static readonly scales = ['', 'Thousand', 'Million', 'Billion'];

  /**
   * Converts a given value to a number with optional constraints.
   *
   * @param value - The value to be converted, which can be a number, string, or undefined.
   * @param options - An optional object to specify conversion constraints.
   * @param options.defaultValue - The default value to return if the conversion fails or the value is undefined. Defaults to 0.
   * @param options.allowNegative - A boolean indicating whether negative numbers are allowed. Defaults to true.
   * @param options.decimalPlaces - The number of decimal places to round the result to. If set to -1, no rounding is applied. Defaults to -1.
   * @returns The converted number, adhering to the specified constraints.
   */
  static convertToNumber(
    value: number | string | undefined,
    options: {
      defaultValue?: number;
      allowNegative?: boolean;
      decimalPlaces?: number;
    } = {},
  ): number {
    const { defaultValue = 0, allowNegative = true, decimalPlaces = -1 } = options;

    if (!value) {
      return defaultValue;
    }

    if (typeof value === 'number') {
      return decimalPlaces >= 0 ? Number(value.toFixed(decimalPlaces)) : value;
    }

    try {
      // Remove commas, handle negative sign
      const cleanedValue = value.replace(/,/g, '').replace(/^(-)?0*/, '$1');

      // Parse value, handling scientific notation
      const parsedNumber = Number(cleanedValue);

      if (isNaN(parsedNumber)) {
        return defaultValue;
      }

      // Check negative number constraint
      if (!allowNegative && parsedNumber < 0) {
        return defaultValue;
      }

      // Handle decimal places
      return decimalPlaces >= 0 ? Number(parsedNumber.toFixed(decimalPlaces)) : parsedNumber;
    } catch {
      return defaultValue;
    }
  }

  static convertToFormattedNumber(
    value: number | undefined | null,
    decimalPlaces = 2,
  ): string | undefined {
    if (value == null) {
      return undefined;
    }

    const numberValue = Number(value);
    if (isNaN(numberValue)) {
      return '0';
    }

    return numberValue.toLocaleString('en-US', {
      minimumFractionDigits: decimalPlaces,
      maximumFractionDigits: decimalPlaces,
    });
  }

  static formatNumberExchangeRate(value: string): string {
    const [integerPart, decimalPart] = value.split('.');
    const formattedIntegerPart = Number(integerPart).toLocaleString('en-US');
    return decimalPart
      ? `${formattedIntegerPart}.${decimalPart.slice(0, 2)}`
      : formattedIntegerPart;
  }

  /**
   * Converts a number to its word representation.
   * @param num The number to convert.
   * @returns The word representation of the number.
   */
  static convertToWords(num: number): string {
    if (num === 0) {
      return 'Zero';
    }

    let words = '';
    let scaleIndex = 0;

    while (num > 0) {
      const segment = num % 1000;

      if (segment > 0) {
        words =
          this.convertSegmentToWords(segment) +
          (this.scales[scaleIndex] ? ` ${this.scales[scaleIndex]} ` : '') +
          words;
      }

      num = Math.floor(num / 1000);
      scaleIndex++;
    }

    return words.trim();
  }

  static isPercentage(
    value: string | number,
    options?: {
      allowNegative?: boolean;
      requirePercentSymbol?: boolean;
      maxValue?: number;
    },
  ): boolean {
    const { allowNegative = false, requirePercentSymbol = false, maxValue = 100 } = options || {};

    // Convert to string for consistent processing
    const stringValue =
      typeof value === 'number' ? `${value}${requirePercentSymbol ? '%' : ''}` : value;

    const percentageRegex = new RegExp(
      `^${allowNegative ? '-?' : ''}(\\d{1,3})(\\.\\d+)?${requirePercentSymbol ? '%' : ''}$`,
    );

    const match = percentageRegex.test(stringValue);
    if (!match) return false;

    // Additional numeric validation
    const numericValue = parseFloat(stringValue.replace('%', ''));
    return numericValue >= 0 && numericValue <= maxValue;
  }

  /**
   * Validates that a number does not exceed a maximum amount.
   *
   * @param value - The value to validate, which can be a number, string, or undefined.
   * @param maxAmount - The maximum allowed amount, which can be a number, string, or undefined.
   * @param options - An optional object to specify conversion constraints for both values.
   * @returns True if the value does not exceed the maximum amount, false otherwise.
   */
  static validateMaxAmount(
    value: number | string | undefined,
    maxAmount: number | string | undefined,
    options: {
      defaultValue?: number;
      allowNegative?: boolean;
      decimalPlaces?: number;
    } = {},
  ): boolean {
    if (maxAmount === undefined || maxAmount === null) {
      return true; // No maximum constraint
    }

    const convertedValue = this.convertToNumber(value, options);
    const convertedMaxAmount = this.convertToNumber(maxAmount, options);

    return convertedValue <= convertedMaxAmount;
  }

  /**
   * Abbreviates a number with K, M, B, T suffixes.
   *
   * @param value - The number to abbreviate.
   * @param options - Configuration options for abbreviation.
   * @param options.decimalPlaces - Number of decimal places to show. Defaults to 1.
   * @param options.threshold - Minimum value to abbreviate. Defaults to 1000.
   * @param options.useFullWords - Use full words (thousand, million) instead of letters. Defaults to false.
   * @returns The abbreviated number as a string.
   */
  static abbreviateNumber(
    value: number | string | undefined,
    options: {
      decimalPlaces?: number;
      threshold?: number;
      useFullWords?: boolean;
    } = {},
  ): string {
    const { decimalPlaces = 1, threshold = 1000, useFullWords = false } = options;

    const numericValue = this.convertToNumber(value, { defaultValue: 0 });

    if (Math.abs(numericValue) < threshold) {
      return this.convertToFormattedNumber(numericValue, decimalPlaces) || '0';
    }

    const suffixes = useFullWords
      ? ['', 'thousand', 'million', 'billion', 'trillion']
      : ['', 'K', 'M', 'B', 'T'];

    const isNegative = numericValue < 0;
    const absoluteValue = Math.abs(numericValue);

    let suffixIndex = 0;
    let abbreviatedValue = absoluteValue;

    while (abbreviatedValue >= 1000 && suffixIndex < suffixes.length - 1) {
      abbreviatedValue /= 1000;
      suffixIndex++;
    }

    // Use toLocaleString for consistent formatting with convertToFormattedNumber
    const formattedValue = abbreviatedValue.toLocaleString('en-US', {
      minimumFractionDigits: decimalPlaces,
      maximumFractionDigits: decimalPlaces,
    });

    return `${isNegative ? '-' : ''}${formattedValue}${suffixes[suffixIndex]}`;
  }

  /**
   * Converts a number segment (less than 1000) to words.
   * @param segment The number segment to convert.
   * @returns The word representation of the segment.
   */
  private static convertSegmentToWords(segment: number): string {
    let words = '';

    if (segment >= 100) {
      words += `${this.ones[Math.floor(segment / 100)]} Hundred `;
      segment %= 100;
    }

    if (segment >= 20) {
      words += `${this.tens[Math.floor(segment / 10)]} `;
      segment %= 10;
    }

    if (segment > 0) {
      words += `${this.ones[segment]} `;
    }

    return words.trim();
  }
}
