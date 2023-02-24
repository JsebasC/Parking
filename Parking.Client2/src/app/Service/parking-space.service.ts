import { Injectable } from "@angular/core";
import { environment } from "environments/environment";

import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { ParkingSpace } from "app/Interface/parking-space";

@Injectable({
  providedIn: "root",
})
export class ParkingSpaceService {
  private endpoint: string = environment.endPoint;
  private apiUrl: string = this.endpoint + "ParkingSpace";

  constructor(private Http: HttpClient) {}

  //Obtener todos los espacios
  Get(): Observable<ParkingSpace[]> {
    return this.Http.get<ParkingSpace[]>(`${this.apiUrl}`);
  }

  

}
