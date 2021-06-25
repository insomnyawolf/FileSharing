"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.JwtHelper = exports.JwtTokenModel = void 0;
var angular_jwt_1 = require("@auth0/angular-jwt");
var JwtTokenModel = /** @class */ (function () {
    function JwtTokenModel() {
    }
    return JwtTokenModel;
}());
exports.JwtTokenModel = JwtTokenModel;
var JwtHelper = /** @class */ (function () {
    function JwtHelper() {
    }
    JwtHelper.setToken = function (data) {
        return sessionStorage.setItem('identity', data);
    };
    JwtHelper.getTokenValue = function () {
        return this.jwtHelper.decodeToken(JwtHelper.getTokenRaw());
    };
    JwtHelper.getTokenRaw = function () {
        return sessionStorage.getItem('identity');
    };
    JwtHelper.clearToken = function () {
        return sessionStorage.removeItem('identity');
    };
    JwtHelper.setRemember = function (data) {
        return sessionStorage.setItem('remmember', data);
    };
    JwtHelper.getRemember = function () {
        return sessionStorage.getItem('remmember');
    };
    JwtHelper.clearRemember = function () {
        return sessionStorage.removeItem('remmember');
    };
    JwtHelper.jwtHelper = new angular_jwt_1.JwtHelperService();
    return JwtHelper;
}());
exports.JwtHelper = JwtHelper;
//# sourceMappingURL=JwtHelper.js.map