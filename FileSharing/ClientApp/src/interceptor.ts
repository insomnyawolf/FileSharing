import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpErrorResponse,
  HttpInterceptor,
  HttpResponse
} from '@angular/common/http';
import {
  Router
} from '@angular/router';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';
import 'rxjs/add/operator/do';
import { JwtHelper } from './helpers/JwtHelper';
import HttpStatusCode from './helpers/HttpStatusCodes';

@Injectable()
export class TokenInterceptor implements HttpInterceptor {
  constructor(
    private router: Router,
  ) { }
  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const token: string = JwtHelper.getTokenRaw();
    if (token) {
      request = request.clone({
        setHeaders: {
          Authorization: 'Bearer ' + token,
          CacheControl: 'no-cache',
          Pragma: 'no-cache'
        }
      });
    }
    return next.handle(request).pipe(
      tap(
        (response: HttpResponse<any>) => {
          // Executed on sucess
          const data: any = response.body;
        },
        (error: HttpErrorResponse) => {
          // Executed on error
          const data: any = error.error;
          switch (error.status) {
            case HttpStatusCode.UNAUTHORIZED:
              this.router.navigate(['/']);
              break;
            case HttpStatusCode.FORBIDDEN:
              this.router.navigate(['/']);
              break;
            default:
              console.error(data);
              break;
          }
        },
        () => {
          // Always Executed
        }
      )
    );
  }
}
