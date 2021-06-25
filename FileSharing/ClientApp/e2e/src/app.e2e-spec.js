"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var app_po_1 = require("./app.po");
describe('App', function () {
    var page;
    beforeEach(function () {
        page = new app_po_1.AppPage();
    });
    it('should display welcome message', function () {
        page.navigateTo();
        expect(page.getMainHeading()).toEqual('Hello, world!');
    });
});
//# sourceMappingURL=app.e2e-spec.js.map