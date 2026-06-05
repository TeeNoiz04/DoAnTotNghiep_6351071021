import { Injectable } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { CalculationConfig } from './calculation-config';

@Injectable({
  providedIn: 'root',
})
export class FormCalculationService {
  /**
   * Set up value change listeners for a form based on the provided configuration
   * @param form The form group to listen to
   * @param config Calculation configuration
   * @returns A function to unsubscribe from all listeners
   */
  setupFormCalculations(form: FormGroup, config: CalculationConfig): () => void {
    const subscriptions: any[] = [];

    // Add quantity and unit price listeners if both are present
    if (config.fields.quantity && config.fields.unitPrice && form) {
      const qtySubscription = form
        .get(config.fields.quantity)
        ?.valueChanges.subscribe((val: string | number) => {
          const isNumber = !isNaN(Number(val));
          if (!isNumber && typeof val === 'string') {
            val = val.replace(/,/g, '');
            if (isNaN(Number(val))) return;
          }
          this.updateAmount(form, config, 'quantity');
        });
      const unitPriceSubscription = form
        .get(config.fields.unitPrice)
        ?.valueChanges.subscribe((val: string | number) => {
          const isNumber = !isNaN(Number(val));
          if (!isNumber && typeof val === 'string') {
            val = val.replace(/,/g, '');
            if (isNaN(Number(val))) return;
          }
          this.updateAmount(form, config, 'unitPrice');
        });
      subscriptions.push(qtySubscription, unitPriceSubscription);
    }
    // Add noVatAmount and vatAmount listeners if both are present
    else if (config.fields.noVatAmount && config.fields.vatAmount) {
      const noVatAmountSubscription = form
        .get(config.fields.noVatAmount)
        ?.valueChanges.subscribe(() => this.updateAmount(form, config, 'noVatAmount'));
      const vatAmountSubscription = form
        .get(config.fields.vatAmount)
        ?.valueChanges.subscribe(() => this.updateAmount(form, config, 'vatAmount'));
      subscriptions.push(noVatAmountSubscription, vatAmountSubscription);
    }
    // Add deductedAmount, advanceAmount, and invoiceAmount listeners if all are present
    else if (
      config.fields.deductedAmount &&
      config.fields.advanceAmount &&
      config.fields.invoiceAmount &&
      form
    ) {
      const deductedAmountSubscription = form
        .get(config.fields.deductedAmount)
        ?.valueChanges.subscribe(() => this.updateAmount(form, config, 'deductedAmount'));
      const advanceAmountSubscription = form
        .get(config.fields.advanceAmount)
        ?.valueChanges.subscribe(() => this.updateAmount(form, config, 'advanceAmount'));
      subscriptions.push(deductedAmountSubscription, advanceAmountSubscription);
    } else if (
      config.fields.requestAmount &&
      config.fields.amountInBP &&
      config.fields.variance &&
      form
    ) {
      const requestAmountSubscription = form
        .get(config.fields.requestAmount)
        ?.valueChanges.subscribe(() => this.updateAmount(form, config, 'requestAmount'));
      const amountInBPSubscription = form
        .get(config.fields.amountInBP)
        ?.valueChanges.subscribe(() => this.updateAmount(form, config, 'amountInBP'));
      subscriptions.push(requestAmountSubscription, amountInBPSubscription);
    } else if (config.fields.amountToBePaid && config.fields.amountInVND && form) {
      const amountToBePaidSubscription = form
        .get(config.fields.amountToBePaid)
        ?.valueChanges.subscribe(() => this.updateAmount(form, config, 'amountToBePaid'));
      subscriptions.push(amountToBePaidSubscription);
    }
    // Add requestAmount listener if present
    else if (config.fields.requestAmount && form) {
      const requestAmountSubscription = form
        .get(config.fields.requestAmount)
        ?.valueChanges.subscribe(() => this.updateAmount(form, config, 'requestAmount'));
      subscriptions.push(requestAmountSubscription);
    }

    const exchangeRateSubscription = form
      ?.get(config.fields.exchangeRate)
      ?.valueChanges.subscribe(() => this.updateAmount(form, config, 'exchangeRate'));

    // Currency-related listener
    const currencySubscription = form
      ?.get(config.fields.currency)
      ?.valueChanges.subscribe(() => this.updateCurrencyLogic(form, config));

    subscriptions.push(exchangeRateSubscription, currencySubscription);

    // Return a function to unsubscribe from all listeners
    return () => {
      subscriptions.forEach(sub => sub?.unsubscribe());
    };
  }

  /**
   * Update amount calculations based on amount, vat amount, and exchange rate
   * @param form Form group
   * @param config Calculation configuration
   */
  private updateAmount(form: FormGroup, config: CalculationConfig, triggeredField: string): void {
    // Safely get form values with default fallbacks
    const { convertToNumber } = config.numberHelper;
    const getValue = (field: string, defaultValue = 0) =>
      convertToNumber(form.get(field)?.value ?? defaultValue);

    const {
      exchangeRate: exchangeRateField,

      quantity: quantityField,
      unitPrice: unitPriceField,
      amount: amountField,
      amountInVND: amountInVNDField,

      noVatAmount: noVatAmountField,
      vatAmount: vatAmountField,
      noVatAmountInVND: noVatAmountInVNDField,
      vatAmountInVND: vatAmountInVNDField,
      totalAmount: totalAmountField,
      totalAmountInVND: totalAmountInVNDField,

      deductedAmount: deductedAmountField,
      advanceAmount: advanceAmountField,
      invoiceAmount: invoiceAmountField,
      amountToBePaid: amountToBePaidField,

      requestAmount: requestAmountField,
      amountInBP: amountInBPField,
      variance: varianceField,
    } = config.fields;

    const exchangeRate = getValue(exchangeRateField, 1);

    if (quantityField && unitPriceField) {
      const quantity = getValue(quantityField);
      const unitPrice = getValue(unitPriceField);
      const amount = +(quantity * unitPrice);
      const amountInVND = +(amount * exchangeRate);

      const updateFields: { [key: string]: any } = {
        [amountField]: amount,
        [amountInVNDField]: amountInVND,
      };
      form.patchValue(updateFields, { emitEvent: false });
    } else if (noVatAmountField && vatAmountField) {
      const noVatAmount = getValue(noVatAmountField);
      const vatAmount = getValue(vatAmountField);
      const noVatAmountInVND = +(noVatAmount * exchangeRate).toFixed(0);
      const vatAmountInVND = +(vatAmount * exchangeRate).toFixed(0);
      const totalAmount = +(noVatAmount + vatAmount).toFixed(2);
      const totalAmountInVND = +(noVatAmountInVND + vatAmountInVND).toFixed(0);

      const updateFields: { [key: string]: any } = {
        [noVatAmountInVNDField]: noVatAmountInVND,
        [vatAmountInVNDField]: vatAmountInVND,
        [totalAmountField]: totalAmount,
        [totalAmountInVNDField]: totalAmountInVND,
      };

      form.patchValue(updateFields, { emitEvent: false });
    } else if (deductedAmountField && advanceAmountField && invoiceAmountField) {
      const deductedAmount = getValue(deductedAmountField);
      const advanceAmount = getValue(advanceAmountField);
      const invoiceAmount = getValue(invoiceAmountField);
      const amountToBePaid = +(invoiceAmount - deductedAmount - advanceAmount).toFixed(2);

      const updateFields: { [key: string]: any } = {
        [amountToBePaidField]: amountToBePaid,
      };

      form.patchValue(updateFields, { emitEvent: false });
    } else if (requestAmountField && amountInBPField) {
      const requestAmount = getValue(requestAmountField);
      const amountInBP = getValue(amountInBPField);
      const variance = +(requestAmount - amountInBP).toFixed(2);

      const updateFields: { [key: string]: any } = {
        [varianceField]: variance,
      };

      form.patchValue(updateFields, { emitEvent: false });
    } else if (amountToBePaidField && amountInVNDField) {
      const amountToBePaid = getValue(amountToBePaidField);
      const amountInVND = +(amountToBePaid * exchangeRate).toFixed(0);

      const updateFields: { [key: string]: any } = {
        [amountInVNDField]: amountInVND,
      };

      form.patchValue(updateFields, { emitEvent: false });
    } else if (requestAmountField) {
      const requestAmount = getValue(requestAmountField);
      const amountInVND = +(requestAmount * exchangeRate).toFixed(0);

      const updateFields: { [key: string]: any } = {
        [amountInVNDField]: amountInVND,
      };

      form.patchValue(updateFields, { emitEvent: false });
    }
  }

  /**
   * Update currency-related logic and formatting
   * @param form Form group
   * @param config Calculation configuration
   */
  private updateCurrencyLogic(form: FormGroup, config: CalculationConfig): void {
    if (!config.fields || !config.numberHelper || !config.currencies) return;

    const {
      currency,
      unitPrice,
      exchangeRate,
      noVatAmount,
      vatAmount,
      deductedAmount,
      advanceAmount,
    } = config.fields;
    const { convertToNumber, convertToFormattedNumber } = config.numberHelper;
    const vndCurrencyCode = 'VND';

    const selectedCurrency = form.get(currency)?.value;
    const currencySelected = config.currencies.find(c => c.code === selectedCurrency);

    const isVND = selectedCurrency === vndCurrencyCode;
    const decimalPlaces = isVND ? 0 : 2;

    // Handle exchange rate control
    const exchangeRateControl = form.get(exchangeRate);
    if (exchangeRateControl) {
      if (isVND) {
        exchangeRateControl.disable();
      } else {
        exchangeRateControl.enable();
      }
    }

    // Format unit price
    const unitPriceControl = form.get(unitPrice);
    if (unitPriceControl && unitPriceControl.value) {
      const unitPriceVal = convertToNumber(unitPriceControl.value);
      const formattedUnitPrice = convertToFormattedNumber(unitPriceVal, decimalPlaces);
      unitPriceControl.setValue(unitPriceVal, { emitEvent: true });
    }

    // Format exchange rate
    if (currencySelected?.value) {
      const exchangeRateControl = form.get(exchangeRate);
      if (exchangeRateControl) {
        const formattedExchangeRate = convertToFormattedNumber(
          currencySelected.value,
          decimalPlaces,
        );
        exchangeRateControl.setValue(currencySelected.value, { emitEvent: true });
      }
    }

    // Format amount
    const noVatAmountControl = form.get(noVatAmount);
    if (noVatAmountControl && noVatAmountControl.value) {
      const noVatAmountVal = convertToNumber(noVatAmountControl.value);
      const formattedNoVatAmount = convertToFormattedNumber(noVatAmountVal, decimalPlaces);
      noVatAmountControl.setValue(noVatAmountVal, { emitEvent: true });
    }

    // Format vat amount
    const vatAmountControl = form.get(vatAmount);
    if (vatAmountControl && vatAmountControl.value) {
      const vatAmountVal = convertToNumber(vatAmountControl.value);
      const formattedVatAmount = convertToFormattedNumber(vatAmountVal, decimalPlaces);
      vatAmountControl.setValue(vatAmountVal, { emitEvent: true });
    }

    // Format deducted amount
    const deductedAmountControl = form.get(deductedAmount);
    if (deductedAmountControl && deductedAmountControl.value) {
      const deductedAmountVal = convertToNumber(deductedAmountControl.value);
      const formattedDeductedAmount = convertToFormattedNumber(deductedAmountVal, decimalPlaces);
      deductedAmountControl.setValue(deductedAmountVal, { emitEvent: true });
    }

    // Format advance amount
    const advanceAmountControl = form.get(advanceAmount);
    if (advanceAmountControl && advanceAmountControl.value) {
      const advanceAmountVal = convertToNumber(advanceAmountControl.value);
      const formattedAdvanceAmount = convertToFormattedNumber(advanceAmountVal, decimalPlaces);
      advanceAmountControl.setValue(advanceAmountVal, { emitEvent: true });
    }

    // Format request amount
    const requestAmountControl = form.get(config.fields.requestAmount);
    if (requestAmountControl && requestAmountControl.value) {
      const requestAmountVal = convertToNumber(requestAmountControl.value);
      const formattedRequestAmount = convertToFormattedNumber(requestAmountVal, decimalPlaces);
      requestAmountControl.setValue(requestAmountVal, { emitEvent: true });
    }
  }
}
