import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ParkingSpace } from '../Model/parking-space';


@Injectable({
  providedIn: 'root'
})
export class ParkingSpaceService {

  private endpoint: string = environment.endPoint;
  private apiUrl: string = this.endpoint + 'ParkingSpace';

  constructor(private http: HttpClient) { }
  
  getList(): Observable<ParkingSpace[]>{
    return this.http.get<ParkingSpace[]>(`${this.apiUrl}`)
  }

}
