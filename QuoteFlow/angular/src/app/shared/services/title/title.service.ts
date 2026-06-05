import { Injectable } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { ActivatedRouteSnapshot, NavigationEnd, Router } from '@angular/router';
import { filter } from 'rxjs/operators';

@Injectable({
  providedIn: 'root',
})
export class TitleService {
  private baseTitle = 'MEVN.SalesWF';

  constructor(
    private titleService: Title,
    private router: Router,
  ) {}

  /**
   * Initialize the title service to listen for route changes
   */
  init() {
    this.router.events.pipe(filter(event => event instanceof NavigationEnd)).subscribe(() => {
      const routeSnapshot = this.getDeepestChildSnapshot(this.router.routerState.snapshot.root);
      const title = routeSnapshot.data?.['title'] || this.baseTitle;

      this.setTitle(title);
    });
  }

  /**
   * Manually set the page title
   * @param title Page title (optional - if not provided, only the base title will be shown)
   */
  setTitle(title?: string): void {
    if (title) {
      this.titleService.setTitle(`${title} | ${this.baseTitle}`);
    } else {
      this.titleService.setTitle(this.baseTitle);
    }
  }

  /**
   * Get the current page title
   */
  getTitle(): string {
    return this.titleService.getTitle();
  }

  /**
   * Get the deepest child route snapshot to extract title data
   */
  private getDeepestChildSnapshot(snapshot: ActivatedRouteSnapshot): ActivatedRouteSnapshot {
    let deepestChild = snapshot;
    while (deepestChild.firstChild !== null) {
      deepestChild = deepestChild.firstChild;
    }
    return deepestChild;
  }
}
