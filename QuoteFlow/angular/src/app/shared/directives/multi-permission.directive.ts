import { Directive, Input, TemplateRef, ViewContainerRef } from '@angular/core';
import { PermissionService } from '@abp/ng.core';

@Directive({
  selector: '[appMultiPermission]',
  standalone: true,
})
export class MultiPermissionDirective {
  @Input() set appMultiPermission(permissions: string[]) {
    this.updateView(permissions);
  }

  constructor(
    private templateRef: TemplateRef<any>,
    private viewContainer: ViewContainerRef,
    private permissionService: PermissionService,
  ) {}

  private async updateView(permissions: string[]) {
    this.viewContainer.clear();

    const hasAnyPermission = await Promise.all(
      permissions.map(permission => this.permissionService.getGrantedPolicy(permission)),
    ).then(results => results.some(result => result));

    if (hasAnyPermission) {
      this.viewContainer.createEmbeddedView(this.templateRef);
    }
  }
}
