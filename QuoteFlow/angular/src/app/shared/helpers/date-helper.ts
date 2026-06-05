export class DateHelper {
  static FISCAL_YEAR_START_MONTH = 4;

  static convertDate(inputFormat: Date): string {
    function pad(s: number) {
      return s < 10 ? '0' + s : s.toString();
    }
    const d = new Date(inputFormat);
    return [d.getFullYear(), pad(d.getMonth() + 1), pad(d.getDate())].join('-');
  }

  static convertToDate(year: number, month: number, day: number): Date {
    const newMonth = month.toString().length === 1 ? `0${month}` : month;
    const newDay = day.toString().length === 1 ? `0${day}` : day;
    return new Date(`${year}-${newMonth}-${newDay}`);
  }

  static convertDateTime(inputFormat: string | null): string {
    if (!inputFormat) {
      return '';
    }
    const d = new Date(inputFormat);
    return [d.getDate(), d.getMonth() + 1, d.getFullYear()].join('-');
  }

  static isDateInFiscalYear(dateCompare: Date, year: number): boolean {
    const fiscalYearStart = new Date(year, DateHelper.FISCAL_YEAR_START_MONTH - 1, 1);
    const fiscalYearEnd = new Date(year + 1, DateHelper.FISCAL_YEAR_START_MONTH - 1, 0);
    return dateCompare >= fiscalYearStart && dateCompare <= fiscalYearEnd;
  }

  static calculateTotalDays(fromDate: Date, toDate: Date): number | null {
    // convert to utc to avoid timezone issues
    const fromDateUtc = new Date(fromDate.toUTCString());
    const toDateUtc = new Date(toDate.toUTCString());

    fromDateUtc.setHours(0, 0, 0, 0);
    toDateUtc.setHours(0, 0, 0, 0);

    if (fromDateUtc > toDateUtc) {
      return null;
    }

    // Correct calculation: Difference in days + 1 for inclusivity
    const timeDiff = toDateUtc.getTime() - fromDateUtc.getTime();
    const totalDays = Math.floor(timeDiff / (1000 * 3600 * 24)) + 1;

    return totalDays;
  }

  static getFiscalYearFromDate(date: Date): number {
    const month = date.getMonth() + 1;
    return month >= DateHelper.FISCAL_YEAR_START_MONTH
      ? date.getFullYear()
      : date.getFullYear() - 1;
  }

  // Helper function to calculate working days between two dates (excluding weekends)
  static getWorkingDaysBetweenDates(startDate: Date, endDate: Date): number {
    // Clone dates to avoid modifying the originals
    const start = new Date(startDate);
    const end = new Date(endDate);

    // Reset hours to ensure fair date comparison
    start.setHours(0, 0, 0, 0);
    end.setHours(0, 0, 0, 0);

    // Initialize counter
    let workingDays = 0;

    // Loop through each day
    const current = new Date(start);
    while (current <= end) {
      // Check if current day is a weekday (0 = Sunday, 6 = Saturday)
      const dayOfWeek = current.getDay();
      if (dayOfWeek !== 0 && dayOfWeek !== 6) {
        workingDays++;
      }

      // Move to next day
      current.setDate(current.getDate() + 1);
    }

    return workingDays;
  }

  static getDaysBetweenDates(startDate: Date, endDate: Date): number {
    const start = new Date(startDate);
    const end = new Date(endDate);

    start.setHours(0, 0, 0, 0);
    end.setHours(0, 0, 0, 0);

    const timeDiff = end.getTime() - start.getTime();
    const MS_PER_DAY = 1000 * 60 * 60 * 24;

    return Math.abs(timeDiff / MS_PER_DAY) + 1; // or use Math.abs(...) if needed
  }

  static formatDateGmt7(d: Date): string {
    return d.toLocaleDateString('en-CA', { timeZone: 'Asia/Bangkok' });
  }
}
