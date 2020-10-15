import { IService } from './../_models/IService';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class ServicesService {
baseUrl = environment.apiUrl;

constructor(private http: HttpClient) {}

getServices(): Observable<IService[]> {
  var services: IService[] = new Array<IService>();

  let params = new HttpParams();

  return this.http.get<IService[]>(this.baseUrl + 'services/getAllItems', { observe: 'response', params})
    .pipe(
      map(response => {
        services = response.body;
        return services;
      })
    );
}
getServiceImages(serviceId: number): Observable<string[]> {
  var images: string[] = new Array<string>();
  return this.http.get<string[]>(this.baseUrl + 'services/item/'+serviceId+'/images', { observe: 'response'})
    .pipe(
      map(response => {
        images = response.body;
        return images;
      })
    );
}
}
