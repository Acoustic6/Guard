import { Component } from '@angular/core';
import { Router } from '@angular/router';

import { Account } from '../../account/account.model';
import { AccountService } from '../../account/account.service';

@Component({
    selector: 'guard-register',
    styleUrls: ['./guard-register.component.scss'],
    templateUrl: './guard-register.component.html'
})
export class GuardRegisterComponent {
    model: any = {};
    loading = false;

    constructor(
        private router: Router,
        private accountService: AccountService
    ) { }

    register(model, event) {
        this.loading = true;
        this.accountService.create(this.mapModelToAccount(this.model))
            .subscribe(
            data => {
                this.router.navigate(['/login']);
            },
            error => {
                this.loading = false;
            });
    }

    private mapModelToAccount(model: any): Account {
        let result = new Account();
        result.login = model.Login;
        result.password = model.Password;
        result.confirmationPassword = model.ConfirmationPassword;
        if (result.user) {
            result.user.firstName = model.UserFirstName;
            result.user.lastName = model.UserLastName;
            result.user.givenName = model.UserGivenName;
            result.user.email = model.UserEmail;
            result.user.birthday = model.UserBirthday;
        }

        return result;
    }
}
