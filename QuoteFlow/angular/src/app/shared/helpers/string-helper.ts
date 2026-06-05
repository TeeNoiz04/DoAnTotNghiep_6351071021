export class StringHelper {
  static isGuid(value: string): boolean {
    const guidRegex = new RegExp(
      '^[{]?[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}[}]?$',
    );
    return guidRegex.test(value);
  }
}
