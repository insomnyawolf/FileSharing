////import { Injectable } from '@angular/core';
////import {
////  HttpRequest,
////  HttpHandler,
////  HttpEvent,
////  HttpErrorResponse,
////  HttpInterceptor,
////  HttpResponse
////} from '@angular/common/http';
////import {
////  Router
////} from '@angular/router';
////import { Observable } from 'rxjs';
////import 'rxjs/add/operator/do';
////import { UtilsUser } from './app/utils/user';
////import HttpStatusCode from './app/utils/HttpStatusCodes';
////import { MessageSuccess } from './app/Models/messageSuccess';
////@Injectable()
////export class TokenInterceptor implements HttpInterceptor {
////  constructor(
////    private router: Router,
////  ) { }
////  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
////    const token: string = UtilsUser.getTokenRaw();
////    if (token) {
////      request = request.clone({
////        setHeaders: {
////          Authorization: 'Bearer ' + token,
////          CacheControl: 'no-cache',
////          Pragma: 'no-cache'
////        }
////      });
////    }
////    return next.handle(request).do(
////      (response: HttpResponse<any>) => {
////        // Executed on sucess
////        const data: any = response.body;
////      },
////      (error: HttpErrorResponse) => {
////        // Executed on error
////        const data: any = error.error;
////        switch (error.status) {
////          case HttpStatusCode.UNAUTHORIZED:
////            this.router.navigate(['/']);
////            break;
////          case HttpStatusCode.FORBIDDEN:
////            this.router.navigate(['/']);
////            break;
////          default:
////            console.error(data);
////            break;
////        }
////      },
////      () => {
////        // Always Executed
////      });
////  }
////}
//# sourceMappingURL=interceptor.js.map