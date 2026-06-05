import { CommonModule } from '@angular/common'; // Always include CommonModule for feature modules
import { NgModule } from '@angular/core';
import { WorkflowConfigurationRoutingModule } from './workflow-configuration-routing.module';

@NgModule({
  declarations: [
    // Add any other components declared in this module
  ],
  imports: [CommonModule, WorkflowConfigurationRoutingModule],
})
export class WorkflowConfigurationModule {}
