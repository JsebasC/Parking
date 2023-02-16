import { Injectable } from '@angular/core';

import { environment } from 'src/environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

import { Parking } from '../Model/parking';
import { ParkingRequest } from '../Model/parking-request';


@Injectable({
  providedIn: 'root'
})
  
export class ParkingService {

  private endpoint: string = environment.endPoint;
  private apiUrl: string = this.endpoint;

  
  constructor(private http: HttpClient) { }
  
  getListNoExitVehicle(): Observable<Parking[]>{   
    return this.http.get<Parking[]>(`${this.apiUrl}Parking/GetAllNotExitVehicle`);
  }

  getValueExitVehicle(plate: string): Observable<Parking>{
    return this.http.get<Parking>(`${this.apiUrl}Parking/GetValuePayExitVehicle/${plate}`);
  }

  add(model: ParkingRequest): Observable<ParkingRequest>{
    return this.http.post<ParkingRequest>(`${this.apiUrl}ParkingVehicle`,model);
  } 

  exitVehicle(id: string, model: Parking): Observable<Parking>{    
    return this.http.put<Parking>(`${this.apiUrl}Parking/ExitVehicle/${id}`,model);
  }

  // update(idParking:string,model: Parking): Observable<Parking>{
  //   return this.http.put<Parking>(`${this.apiUrl}/ ${idParking}`,model);
  // }

  // delete(idParking:string): Observable<void>{
  //   return this.http.delete<void>(`${this.apiUrl}/ ${idParking}`);
  // }
}
