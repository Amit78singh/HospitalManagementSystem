import { Routes } from '@angular/router';
import { AuthGuard } from './guards/auth-guard';
import { adminGuard } from './guards/admin.gaurd';
import { RegisterComponent } from './components/Register/register.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';

export const routes: Routes = [
  {
    path: '',
    loadComponent: () =>
      import('./components/redirect.component').then(m => m.RedirectComponent)
  },
  { path: 'dashboard', component: DashboardComponent 

  },
  
  {
    path: 'patients',
    canActivate: [AuthGuard],
    loadComponent: () =>
      import('./components/patient/patient.component').then(m => m.PatientComponent)
  },
  {
    path: 'doctors',
    canActivate: [AuthGuard,adminGuard],
    loadComponent: () =>
      import('./components/doctor/doctor.component').then(m => m.DoctorComponent),
  },

  {
  path: 'register',
  loadComponent: () =>
    import('./components/Register/register.component').then(m => m.RegisterComponent)
},

  {
    path: 'login',
    loadComponent: () =>
      import('./components/login.component').then(m => m.LoginComponent)
  }
];
