import { Injectable, inject } from '@angular/core';
import { Router, NavigationEnd } from '@angular/router';
import { Location } from '@angular/common';
import { DialogService } from './dialog/dialog.service';
import { filter } from 'rxjs/operators';

@Injectable({
  providedIn: 'root',
})
export class MenuClickHandlerService {
  private readonly router = inject(Router);
  private readonly location = inject(Location);
  private readonly dialogService = inject(DialogService);
  private dialogRoutes: Record<string, (path: string) => void> = {};
  private previousUrl: string = '/';

  constructor() {
    this.router.events
      .pipe(filter(event => event instanceof NavigationEnd))
      .subscribe((event: any) => {
        const url = event.urlAfterRedirects || event.url;

        // Check if this is a dialog route before processing
        if (!this.isDialogRoute(url)) {
          // Update previous URL only for non-dialog routes
          this.previousUrl = url;
        } else {
          // Handle dialog route
          this.handleDialogRoute(url);
        }
      });
  }

  registerDialogRoute(path: string, handler: (path: string) => void): void {
    if (path.startsWith('/')) {
      path = path.substring(1);
    }
    this.dialogRoutes[path] = handler;
  }

  private isDialogRoute(url: string): boolean {
    if (url.startsWith('/')) {
      url = url.substring(1);
    }
    return !!this.dialogRoutes[url];
  }

  private handleDialogRoute(url: string): boolean {
    if (url.startsWith('/')) {
      url = url.substring(1);
    }

    const handler = this.dialogRoutes[url];
    if (handler) {
      // Use location service to navigate back without page refresh
      setTimeout(() => {
        // Replace current state with previous URL to avoid adding to history
        this.location.replaceState(this.previousUrl);
        handler(url);
      }, 0);
      return true;
    }

    return false;
  }
}
