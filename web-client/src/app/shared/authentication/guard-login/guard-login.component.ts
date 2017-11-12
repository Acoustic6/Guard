import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';

import { AuthenticationService } from '../authentication.service';

@Component({
    selector: 'guard-login',
    styleUrls: ['./guard-login.component.scss'],
    templateUrl: './guard-login.component.html'
})
export class GuardLoginComponent implements OnInit {
    private model: any = {};
    private loading: boolean = false;
    private returnUrl: string;

    constructor(
        private route: ActivatedRoute,
        private router: Router,
        private authenticationService: AuthenticationService
    ) { }

    ngOnInit() {
        this.authenticationService.logout();
        this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';
    }

    loginUser() {
        this.loading = true;
        this.authenticationService.login(this.model.login, this.model.password)
            .subscribe(
            data => {
                this.router.navigate([this.returnUrl]);
            },
            error => {
                this.loading = false;
            });
    }
}
