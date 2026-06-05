import { SaleOrderDto } from '@proxy/sale-orders';

export type SaleOrderEntity = SaleOrderDto & {
  id: string;
  total: number;
  status?: string;
  lastModifierUserName?: string;
  creationTime?: string;
  creatorUserName?: string;
  orderDate?: string;
  factory?: string;
  vendor?: string;
  currency?: string;
  epa?: boolean;
};

export const enum SaleOrderTypes {
  DPO = 'SO-DPO',
  GIC = 'SO-GIC',
}
