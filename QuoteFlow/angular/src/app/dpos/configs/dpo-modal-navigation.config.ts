import { DPODetailDto } from '@app/proxy/dpos/dpodetails/models';
import {
  ModalNavigationConfig,
  ModalNavigationCondition,
} from '@app/shared/services/modal-navigation.service';
import { RequestStatusEnum } from '@app/shared/status/components/status-label.component';

export enum DpoModalType {
  LockOnOrderStockAvailable = 'lockOnOrderStockAvailable',
  LockStock = 'lockStock',
  LockOnOrderStock = 'lockOnOrderStock',
  Delivered = 'delivered',
  SaleOrder = 'saleOrder',
  ExtraFee = 'extraFee',
}

export const DPO_MODAL_NAVIGATION_CONDITIONS: ModalNavigationCondition<DPODetailDto>[] = [
  {
    modalType: DpoModalType.LockOnOrderStockAvailable,
    condition: (detail: DPODetailDto) =>
      (detail.needDelivery || 0) > 0 &&
      (detail.onOrderStockAvailable || 0) > 0 &&
      detail.status !== RequestStatusEnum.CANCELLED &&
      detail.status !== RequestStatusEnum.CLOSED,
  },
  {
    modalType: DpoModalType.LockStock,
    condition: (detail: DPODetailDto) =>
      // TODO: Define condition for lock stock modal
      // Placeholder condition - customize based on requirements
      (detail.needDelivery || 0) > 0 &&
      detail.status !== RequestStatusEnum.CANCELLED &&
      detail.status !== RequestStatusEnum.CLOSED,
  },
  {
    modalType: DpoModalType.LockOnOrderStock,
    condition: (detail: DPODetailDto) =>
      // TODO: Define condition for lock on order stock modal
      // Placeholder condition - customize based on requirements
      (detail.needDelivery || 0) > 0 &&
      detail.status !== RequestStatusEnum.CANCELLED &&
      detail.status !== RequestStatusEnum.CLOSED,
  },
  {
    modalType: DpoModalType.Delivered,
    condition: (detail: DPODetailDto) =>
      // TODO: Define condition for delivered modal
      // Placeholder condition - customize based on requirements
      (detail.delivered || 0) >= 0 &&
      detail.status !== RequestStatusEnum.CANCELLED &&
      detail.status !== RequestStatusEnum.CLOSED,
  },
  {
    modalType: DpoModalType.SaleOrder,
    condition: (detail: DPODetailDto) =>
      // TODO: Define condition for sale order modal
      // Placeholder condition - customize based on requirements
      !!detail.spoCode &&
      detail.status !== RequestStatusEnum.CANCELLED &&
      detail.status !== RequestStatusEnum.CLOSED,
  },
  {
    modalType: DpoModalType.ExtraFee,
    condition: (detail: DPODetailDto) =>
      // TODO: Define condition for extra fee modal
      // Placeholder condition - customize based on requirements
      (detail.extrafee || 0) >= 0 &&
      detail.status !== RequestStatusEnum.CANCELLED &&
      detail.status !== RequestStatusEnum.CLOSED,
  },
];

export const DPO_MODAL_NAVIGATION_CONFIG: ModalNavigationConfig<DPODetailDto> = {
  modalType: 'dpo',
  conditions: DPO_MODAL_NAVIGATION_CONDITIONS,
};
