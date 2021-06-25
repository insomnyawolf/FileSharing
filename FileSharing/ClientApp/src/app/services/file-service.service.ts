import { HttpClient, HttpParams } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { PagedResult } from '../models/PagedResult';
import { SharedFile } from '../models/sharedfile';

@Injectable({
  providedIn: 'root'
})
export class FileServiceService {
  private apiUrl: string;
  private http: HttpClient;
  constructor(http: HttpClient, @Inject('API_URL') baseUrl: string)
  {
    this.apiUrl = baseUrl + '/Files';
  }

  getFileInfo(filename: string, pageNumber: number): Observable<PagedResult<SharedFile>> {

    const params = new HttpParams();
    params.set('filename', filename);
    params.set('page', pageNumber.toString());

    return this.http.get<PagedResult<SharedFile>>(this.apiUrl + '/GetFileInfo', { params });
  }
}
