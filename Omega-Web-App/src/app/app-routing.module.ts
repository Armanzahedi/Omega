import { ServicesReportComponent } from './dashboard/services-report/services-report.component';
import { ServiceDetailsComponent } from './home/our-services/service-details/service-details.component';
import { UsersListComponent } from './dashboard/users/users-list/users-list.component';
import { LoginComponent } from './login/login.component';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { AuthGuard } from './_guards/auth.guard';
import { RoleGuard } from './_guards/role.guard';

const routes: Routes = [
  {
    path: 'dashboard',
    component: DashboardComponent,
    canActivate: [AuthGuard],
  },
  {
    path: 'dashboard/services-report',
    component: ServicesReportComponent,
    canActivate: [AuthGuard],
  },
  {
    path: 'dashboard/users',
    component: UsersListComponent,
    canActivate: [RoleGuard],
    data: { expectedRole: 'Admin' },
  },
  { path: 'services/:id', component: ServiceDetailsComponent },
  { path: 'login', component: LoginComponent },
  { path: '', component: HomeComponent, pathMatch: 'full' },
  { path: '**', redirectTo: '' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
