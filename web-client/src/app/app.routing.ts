import { RouterModule, Routes } from '@angular/router';
import { AboutComponent } from './about/about.component';
import { HomeComponent } from './home/home.component';
import { NotFoundComponent } from './not-found/not-found.component';

const APP_ROUTES: Routes = [
    { path: '', component: HomeComponent },
    { path: 'about', component: AboutComponent },
    { path: '**', component: NotFoundComponent }
];

export const ROUTING = RouterModule.forRoot(APP_ROUTES);
