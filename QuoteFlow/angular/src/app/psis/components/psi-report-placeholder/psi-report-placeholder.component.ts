import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-psi-report-placeholder',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="container-fluid">
      <div class="row justify-content-center">
        <div class="col-md-6">
          <div class="card">
            <div class="card-body text-center">
              <div class="spinner-border" role="status">
                <span class="visually-hidden">Loading...</span>
              </div>
              <p class="mt-3">Opening PSI Report...</p>
            </div>
          </div>
        </div>
      </div>
    </div>
  `,
})
export class PSIReportPlaceholderComponent {
  // This component should never actually be rendered as the guard intercepts the route
}
