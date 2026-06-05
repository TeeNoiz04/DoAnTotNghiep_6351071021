import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CoreModule } from '@abp/ng.core';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { PageModule } from '@abp/ng.components/page';

@Component({
  selector: 'app-drawer-modal',
  templateUrl: './drawer-modal.component.html',
  styleUrls: ['./drawer-modal.component.scss'],
  standalone: true,
  imports: [CoreModule, ThemeSharedModule, PageModule],
})
export class DrawerModalComponent {
  @Input() isOpen = false;
  @Input() title = 'Drawer Modal';

  @Output() closeDrawer = new EventEmitter<void>();

  close(): void {
    this.isOpen = false;
    this.closeDrawer.emit();
  }
}
