import { ModuleWithProviders, NgModule } from '@angular/core';

import { AccountService } from './account.service';
import { UserService } from './user.service';

@NgModule({
    imports:      [ ],
    declarations: [ ],
    exports:      [ ],
    providers:    [
    ]
})
export class SharedModule {
    public static forRoot(): ModuleWithProviders {
        return {
            ngModule: SharedModule,
            providers: [
                AccountService,
                UserService
            ]
        };
    }
}
