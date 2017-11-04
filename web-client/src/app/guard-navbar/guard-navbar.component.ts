import { Component } from '@angular/core';

import { AccountService } from '../shared/account.service';
import { UserService } from '../shared/user.service';

@Component({
  selector: 'guard-navbar',
  styleUrls: ['./guard-navbar.component.scss'],
  templateUrl: './guard-navbar.component.html'
})
export class GuardNavbarComponent {
  constructor(
    private accountService: AccountService,
    private userService: UserService
  ) {

  }
}
