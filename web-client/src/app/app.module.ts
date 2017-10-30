import { HashLocationStrategy, LocationStrategy } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';

import { AboutComponent } from './about/about.component';
import { AppComponent } from './app.component';
import { GuardFooterComponent } from './guard-footer/guard-footer.component';
import { GuardHeaderComponent } from './guard-header/guard-header.component';
import { GuardNavbarComponent } from './guard-navbar/guard-navbar.component';
import { HomeComponent } from './home/home.component';
import { NotFoundComponent } from './not-found/not-found.component';

import { ROUTING } from './app.routing';

@NgModule({
    imports: [
        FormsModule,
        BrowserModule,
        ROUTING
    ],
    declarations: [
        AppComponent,
        AboutComponent,
        HomeComponent,
        NotFoundComponent,
        GuardNavbarComponent,
        GuardFooterComponent,
        GuardHeaderComponent
    ],
    providers: [
        {
            provide: LocationStrategy,
            useClass: HashLocationStrategy
        }
    ],
    bootstrap: [AppComponent]
})

export class AppModule { }
