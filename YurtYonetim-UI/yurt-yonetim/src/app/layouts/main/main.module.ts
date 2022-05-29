import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { DashboardLayoutModule } from 'src/app/shared/layouts/dashboard-layout/dashboard-layout.module';
import { MainRoutes } from './main.routing';
import { MainComponent } from './main.component';
import { MainHomeComponent } from './main-home/main-home.component';
@NgModule({
  declarations: [MainComponent,MainHomeComponent],
  imports: [
    CommonModule,
    RouterModule.forChild(MainRoutes),
    DashboardLayoutModule,
  ]
})
export class MainModule { }

