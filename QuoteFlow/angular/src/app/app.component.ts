import { AuthService, BaseCoreModule, ReplaceableComponentsService, RoutesService } from '@abp/ng.core';
import { BaseThemeSharedModule, UserMenuService } from '@abp/ng.theme.shared';
import { AsyncPipe } from '@angular/common';
import { ChangeDetectorRef, Component, inject, OnDestroy, OnInit } from '@angular/core';
import { NgbModalOptions } from '@ng-bootstrap/ng-bootstrap';
import { eUserMenuItems } from '@volosoft/abp.ng.theme.lepton-x';
import { lightTheme, StyleNames, ThemeService } from '@volosoft/ngx-lepton-x';
import { environment } from '../environments/environment';
import { LoadingService } from './shared/services/loading/loading.service';
import { NavigationHistoryService } from './shared/services/navigation-history/navigation-history.service';
import { TitleService } from './shared/services/title/title.service';
import { TokenClaimsService } from './shared/services/token-claims/token-claims.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  imports: [BaseThemeSharedModule, BaseCoreModule, AsyncPipe],
})
export class AppComponent implements OnInit, OnDestroy {
  loadingOptions: NgbModalOptions = {
    animation: true,
    centered: true,
    modalDialogClass: 'loading-dialog',
  };
  loading = false;

  public loadingService = inject(LoadingService);
  private subscriptions = [];

  constructor(
    private themeService: ThemeService,
    private titleService: TitleService,
    userMenu: UserMenuService,
    private replaceableComponents: ReplaceableComponentsService,
    private cdr: ChangeDetectorRef,
    private authService: AuthService,
    private routes: RoutesService,
    private tokenClaims: TokenClaimsService,
    private navigationHistory: NavigationHistoryService,
    private router: Router,
  ) {
    if (this.themeService.selectedStyle.styleName != StyleNames.Light) {
      this.themeService.setTheme(lightTheme);
    }
    // userMenu.removeItem('Gdpr.GdprNavigation');
    userMenu.removeItem(eUserMenuItems.LinkedAccounts);

    userMenu.removeItem(eUserMenuItems.Sessions);
    userMenu.removeItem(eUserMenuItems.ExternalLogins);

    userMenu.patchItem(eUserMenuItems.MyAccount, {
      action: () => {
        this.router.navigate(['/account/manage']);
      },
    });

    userMenu.patchItem(eUserMenuItems.SecurityLogs, {
      action: () => {
        this.router.navigate(['/account/security-logs']);
      },
    });

    // Hide menu items based on environment configuration
    this.hideMenuItemsFromEnvironment();

    // Initialize the TitleService
    this.titleService.init();
  }

  ngOnInit(): void {
    if (!this.hasLoggedIn) {
      this.authService.navigateToLogin();
    }
    this.subscriptions.push(
      this.loadingService.loading$.subscribe(res => {
        this.loading = res.isLoading;
        this.cdr.detectChanges();
      }),
    );
  }

  get hasLoggedIn(): boolean {
    return this.authService.isAuthenticated;
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(sub => sub.unsubscribe());
  }

  private hideMenuItemsFromEnvironment(): void {
    if (environment?.features?.hideMenuItemNames) {
      environment?.features?.hideMenuItemNames.forEach((itemName: string) => {
        this.routes.flat.forEach(element => {
          if (element.name === itemName) {
            this.routes.patch(element.name, { invisible: true });
          }
        });
      });
    }
  }
}
