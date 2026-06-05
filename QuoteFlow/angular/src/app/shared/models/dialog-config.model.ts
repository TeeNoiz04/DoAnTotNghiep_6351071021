export interface DialogConfig {
  title?: string;
  showClose?: boolean;
  data?: any;
  size?: 'sm' | 'lg' | 'xl';
  centered?: boolean;
  backdrop?: boolean | 'static';
  windowClass?: string;
  customClass?: string;
  keyboard?: boolean;
  closeBtnLabel?: string;
  confirmBtnLabel?: string;
}
