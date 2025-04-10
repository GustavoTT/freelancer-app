import { Routes } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { ProjectsComponent } from './components/projects/projects.component';
import { AuthGuard } from './guards/auth.guard';
import { RegisterComponent } from './components/register/register.component';
import { ProjectFormComponent } from './components/project-form/project-form.component';

export const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'projects', component: ProjectsComponent, canActivate: [AuthGuard]},
  { path: 'projects/edit/:id', component: ProjectFormComponent, canActivate: [AuthGuard] },
  { path: 'projects/create', component: ProjectFormComponent, canActivate: [AuthGuard] },
  { path: '', redirectTo: 'login', pathMatch: 'full' }
];
