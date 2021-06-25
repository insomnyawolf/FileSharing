"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.FormHelper = void 0;
var FormHelper = /** @class */ (function () {
    function FormHelper() {
    }
    // Parsear los datos a editar a datos de formulario
    FormHelper.objectToURLEncoded = function (object) {
        if (object === undefined || object === null) {
            return '';
        }
        var encodedString = '';
        for (var prop in object) {
            if (object.hasOwnProperty(prop)) {
                if (object[prop] !== undefined || object[prop] !== null) {
                    if (encodedString.length > 0) {
                        encodedString += '&';
                    }
                    encodedString += encodeURI(prop + '=' + object[prop]);
                }
            }
        }
        return '?' + encodedString;
    };
    return FormHelper;
}());
exports.FormHelper = FormHelper;
//# sourceMappingURL=FormHelper.js.map