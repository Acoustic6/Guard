import { Component } from '@angular/core';
import { User } from '../shared/user/user.model';
import { UserService } from '../shared/user/user.service';
import { ActivatedRoute } from '@angular/router';

@Component({
    selector: 'userpersonalpage',
    styleUrls: ['./user-personal-page.component.scss'],
    templateUrl: './user-personal-page.component.html'
})
export class UserPersonalPageComponent {
    private user: User = new User();
    private paramsLogin: string;

    constructor(private userService: UserService, private route: ActivatedRoute) {
        this.userService.getUserByLogin().subscribe(user => this.user = user);
        this.paramsLogin = this.route.snapshot.params['login'];
    }
}