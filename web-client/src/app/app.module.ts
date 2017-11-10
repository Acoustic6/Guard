import { HashLocationStrategy, LocationStrategy } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';

import { HttpModule } from '@angular/http';
import { AboutComponent } from './about/about.component';
import { AppComponent } from './app.component';
import { GuardFooterComponent } from './guard-footer/guard-footer.component';
import { GuardHeaderComponent } from './guard-header/guard-header.component';
import { HomeComponent } from './home/home.component';
import { NotFoundComponent } from './not-found/not-found.component';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { GuardNavbarComponent } from './guard-navbar/guard-navbar.component';

import { APP_CONFIG, CONFIG } from './app.config';
import { ROUTING } from './app.routing';
import { SharedModule } from './shared/shared.module';

@NgModule({
    imports: [
        FormsModule,
        BrowserModule,
        HttpModule,
        SharedModule.forRoot(),
        ROUTING
    ],
    declarations: [
        AppComponent,
        AboutComponent,
        HomeComponent,
        NotFoundComponent,
        GuardFooterComponent,
        GuardHeaderComponent,
        LoginComponent,
        RegisterComponent,
        GuardNavbarComponent
    ],
    providers: [
        {
            provide: LocationStrategy,
            useClass: HashLocationStrategy
        },
        {
            provide: APP_CONFIG,
            useValue: CONFIG
        }
    ],
    bootstrap: [AppComponent]
})

export class AppModule { }
