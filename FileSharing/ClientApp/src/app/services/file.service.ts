
import { HttpClient, HttpParams } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { PagedResult } from '../models/PagedResult';
import { SharedFile } from '../models/sharedfile';

@Injectable({
  providedIn: 'root'
})
export class FileService {
  private apiUrl: string;
  private httpClient: HttpClient;
  constructor(httpClient: HttpClient, @Inject('API_URL') baseUrl: string)
  {
    this.httpClient = httpClient;
    this.apiUrl = baseUrl + '/File';
  }

  getFileInfo(filename: string, pageNumber: number): Observable<PagedResult<SharedFile>> {

    const params = new HttpParams()
    .set('filename', filename)
    .set('page', pageNumber.toString());

    return this.httpClient.get<PagedResult<SharedFile>>(this.apiUrl + '/Info', { params });
  }

  uploadFiles(formData: FormData): Observable<any> {
    return this.httpClient.post(this.apiUrl + '/Upload', formData, { headers: undefined });
  }
}
