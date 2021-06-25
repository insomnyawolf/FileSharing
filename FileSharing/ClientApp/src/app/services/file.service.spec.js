"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var testing_1 = require("@angular/core/testing");
var file_service_1 = require("./file.service");
describe('FileServiceService', function () {
    beforeEach(function () { return testing_1.TestBed.configureTestingModule({}); });
    it('should be created', function () {
        var service = testing_1.TestBed.get(file_service_1.FileService);
        expect(service).toBeTruthy();
    });
});
//# sourceMappingURL=file.service.spec.js.map