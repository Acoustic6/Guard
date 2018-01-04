import { RouterModule, Routes } from '@angular/router';
import { AboutComponent } from './about/about.component';
import { HomeComponent } from './home/home.component';
import { NotFoundComponent } from './not-found/not-found.component';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { AuthGuard } from './shared/guards/auth.guard';
import { UserPersonalPageComponent } from './user-personal-page/user-personal-page.component';

const APP_ROUTES: Routes = [
    { path: '', component: HomeComponent },
    { path: 'login', component: LoginComponent },
    { path: 'register', component: RegisterComponent },
    { path: 'about', component: AboutComponent },
    { path: 'user/:login', component: UserPersonalPageComponent, canActivate: [AuthGuard] },
    { path: '**', component: NotFoundComponent }
];

export const ROUTING = RouterModule.forRoot(APP_ROUTES);
