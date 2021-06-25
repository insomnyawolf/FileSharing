export class FormHelper {
  // Parsear los datos a editar a datos de formulario
  static objectToURLEncoded(object: any): string {
    if (object === undefined || object === null) {
      return '';
    }

    let encodedString = '';
    for (const prop in object) {
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
  }
}
