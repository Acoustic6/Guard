import { Component, Input } from '@angular/core';
import { Router } from '@angular/router';
import { TokenService } from '../shared/authentication/token.service';

@Component({
  selector: 'guard-navbar',
  styleUrls: ['./guard-navbar.component.scss'],
  templateUrl: './guard-navbar.component.html'
})
export class GuardNavbarComponent {
  @Input() authenticationEnabled: boolean = true;

  private login: string = null;
  private isLogged: boolean = false;

  constructor(
    private tokenService: TokenService,
    private router: Router
  ) {
    let login = this.tokenService.userLogin();
    if (login) {
      this.login = login;
      this.isLogged = true;
    }
  }

  private logout() {
    this.login = '';
    this.isLogged = false;
    this.tokenService.removeToken();
    this.router.navigate(['/']);
  }
}
