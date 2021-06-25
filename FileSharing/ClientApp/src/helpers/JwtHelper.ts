import { JwtHelperService } from "@auth0/angular-jwt";

export class JwtTokenModel {

}

export class JwtHelper {
  static jwtHelper: JwtHelperService = new JwtHelperService();

  static setToken(data: string) {
    return sessionStorage.setItem('identity', data);
  }

  static getTokenValue(): JwtTokenModel {
    return this.jwtHelper.decodeToken(JwtHelper.getTokenRaw());
  }

  static getTokenRaw(): string {
    return sessionStorage.getItem('identity');
  }

  static clearToken() {
    return sessionStorage.removeItem('identity');
  }

  static setRemember(data: string) {
    return sessionStorage.setItem('remmember', data);
  }

  static getRemember(): string {
    return sessionStorage.getItem('remmember');
  }

  static clearRemember() {
    return sessionStorage.removeItem('remmember');
  }
}
