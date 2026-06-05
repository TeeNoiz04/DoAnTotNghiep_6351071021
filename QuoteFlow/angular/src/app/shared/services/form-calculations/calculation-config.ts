export interface CalculationConfig {
  fields: {
    quantity?: string;
    unitPrice?: string;
    amount?: string;
    amountInVND?: string;
    currency?: string;
    exchangeRate?: string;
    noVatAmount?: string;
    noVatAmountInVND?: string;
    vatAmount?: string;
    vatAmountInVND?: string;
    totalAmount?: string;
    totalAmountInVND?: string;

    deductedAmount?: string;
    advanceAmount?: string;
    invoiceAmount?: string;
    amountToBePaid?: string;

    requestAmount?: string;
    amountInBP?: string;
    variance?: string;
  };
  numberHelper: {
    convertToNumber: (value: any) => number;
    convertToFormattedNumber: (value: number, decimalPlaces?: number) => string;
  };
  currencies?: any[];
}
