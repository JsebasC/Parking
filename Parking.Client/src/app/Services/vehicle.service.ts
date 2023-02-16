import { Injectable } from '@angular/core';

import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { VehicleDTO } from '../Model/vehicle';

@Injectable({
  providedIn: 'root'
})
export class VehicleService {

  private endpoint: string = environment.endPoint;
  private apiUrl: string = this.endpoint + 'Vehicle';
  
  constructor(private http: HttpClient) { }
  
  getList(): Observable<VehicleDTO>{
    return this.http.get<VehicleDTO>(`${this.apiUrl}`)
  }

  add(model: VehicleDTO): Observable<VehicleDTO>{ 
    return this.http.post<VehicleDTO>(`${this.apiUrl}`,model);
  }

  update(idVehicle:string,model: VehicleDTO): Observable<VehicleDTO>{
    return this.http.put<VehicleDTO>(`${this.apiUrl}/ ${idVehicle}`,model);
  }

  delete(idVehicle:string): Observable<void>{
    return this.http.delete<void>(`${this.apiUrl}/ ${idVehicle}`);
  }

}
