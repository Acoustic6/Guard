import { ModuleWithProviders, NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { RouterModule } from '@angular/router';

import { TabComponent } from './tabs/tab/tab.component';
import { TabsComponent } from './tabs/tabs.component';

import { Account, AccountService } from './account/index';
import { AuthenticationService, GuardLoginComponent, GuardRegisterComponent, TokenService } from './authentication/index';
import { } from './directives/index';
import { AuthGuard } from './guards/index';
import { User, UserService } from './user/index';

@NgModule({
    imports: [
        FormsModule,
        BrowserModule,
        RouterModule
    ],
    declarations: [
        TabComponent,
        TabsComponent,
        GuardLoginComponent,
        GuardRegisterComponent
    ],
    exports: [
        TabComponent,
        TabsComponent,
        GuardLoginComponent,
        GuardRegisterComponent
    ],
    providers: [
    ]
})
export class SharedModule {
    public static forRoot(): ModuleWithProviders {
        return {
            ngModule: SharedModule,
            providers: [
                AuthenticationService,
                UserService,
                AuthGuard,
                TokenService,
                AccountService
            ]
        };
    }
}
