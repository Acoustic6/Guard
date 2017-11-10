import { Headers, Response, RequestOptions } from '@angular/http';

export class TokenService {
    static localStorageTokenName: string = 'cjwtt';

    public requestOptionsWithToken(): RequestOptions {
        let currentToken = this.jwt();

        if (currentToken) {
            let headers = new Headers({ 'Authorization': 'Bearer ' + currentToken });
            headers.append('Content-Type', 'application/x-www-form-urlencoded');
            return new RequestOptions({ headers: headers });
        }
    }

    public requestOptions(): RequestOptions {
        let headers = new Headers({});
        headers.append('Content-Type', 'application/x-www-form-urlencoded');
        return new RequestOptions({ headers: headers });
    }

    public updateToken(token) {
        if (token) {
            localStorage.setItem(TokenService.localStorageTokenName, JSON.stringify(token));
        }
    }

    public removeToken() {
        localStorage.removeItem(TokenService.localStorageTokenName);
    }

    public getToken() {
        return this.jwt();
    }

    public userLogin() {
        let e = this.storageTokenData();
        return e && e.login;
    }

    private jwt() {
        let e = this.storageTokenData();
        return e && e.token;
    }

    private storageTokenData() {
        return JSON.parse(localStorage.getItem(TokenService.localStorageTokenName));
    }
}
