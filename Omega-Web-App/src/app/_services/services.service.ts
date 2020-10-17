import { Service } from '../_models/Service';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { map } from 'rxjs/operators';
import { DatePipe, formatDate } from '@angular/common';

@Injectable({
  providedIn: 'root',
})
export class ServicesService {
  baseUrl = environment.apiUrl;
  token = localStorage.getItem('token');
  headers = new HttpHeaders().set('Authorization', 'Bearer ' + this.token);
  constructor(private http: HttpClient) {}

  getAllServices(): Observable<Service[]> {
    var services: Service[] = new Array<Service>();
    return this.http
      .get<Service[]>(this.baseUrl + 'services', { observe: 'response' })
      .pipe(
        map((response) => {
          services = response.body;
          return services;
        })
      );
  }
  getServiceImages(serviceId: number): Observable<string[]> {
    var images: string[] = new Array<string>();
    return this.http
      .get<string[]>(this.baseUrl + 'services/' + serviceId + '/images', {
        observe: 'response',
      })
      .pipe(
        map((response) => {
          images = response.body;
          return images;
        })
      );
  }
  getService(serviceId: number): Observable<Service> {
    var service: Service = new Service();
    return this.http
      .get<Service>(this.baseUrl + 'services/' + serviceId, {
        observe: 'response',
      })
      .pipe(
        map((response) => {
          service = response.body;
          return service;
        })
      );
  }
  getReport(reportParams?): Observable<Service[]> {
    var services: Service[] = new Array<Service>();
    let params = new HttpParams();
    const format = 'dd/MM/yyyy';
    const locale = 'en-US';
    if (reportParams.searchString != null) {
      params = params.append('searchString', reportParams.searchString);
    }
    if (reportParams.from != null) {
      params = params.append(
        'from',
        formatDate(reportParams.from, format, locale)
      );
    }
    if (reportParams.to != null) {
      params = params.append('to', formatDate(reportParams.to, format, locale));
    }
    return this.http
      .get<Service[]>(this.baseUrl + 'services/getReport', {
        observe: 'response',
        params: params,
        headers: this.headers,
      })
      .pipe(
        map((response) => {
          services = response.body;
          return services;
        })
      );
  }
  updateServicesTable(): Observable<any> {
    return this.http.get<any>(this.baseUrl + 'services/UpdateServicesTable', {
      headers: this.headers,
    });
  }
}
