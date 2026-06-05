import { NgModule } from '@angular/core';
import { CookiePolicyComponent } from './cookie-policy.component';
import { PrivacyPolicyComponent } from './privacy-policy.component';
import { GdprCookieConsentRoutingModule } from './gdpr-cookie-consent-routing.module';

@NgModule({
  imports: [GdprCookieConsentRoutingModule, CookiePolicyComponent, PrivacyPolicyComponent],
})
export class GdprCookieConsentModule {}
