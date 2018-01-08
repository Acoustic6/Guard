import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { User, UserService } from '../shared/user/index';
import { TokenService } from '../shared/authentication/index';

@Component({
    selector: 'userpersonalpage',
    styleUrls: ['./user-personal-page.component.scss'],
    templateUrl: './user-personal-page.component.html'
})
export class UserPersonalPageComponent {
    private monitoredUserLogin: string;
    private user: User = new User();

    constructor(
        private userService: UserService,
        private route: ActivatedRoute,
        private tokenService: TokenService) {
        this.monitoredUserLogin = this.route.snapshot.params['login'];
        this.userService.getUserByLogin(this.monitoredUserLogin).subscribe(user => this.user = user, error => console.log(error));
    }
}