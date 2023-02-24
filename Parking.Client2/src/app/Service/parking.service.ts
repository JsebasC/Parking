import { Injectable } from "@angular/core";
import { environment } from "environments/environment";

import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { ParkingRequest } from "app/Interface/parkingRequest";
import { Parking } from "app/Interface/parking";

@Injectable({
  providedIn: "root",
})
export class ParkingService {
  private endpoint: string = environment.endPoint;
  private apiUrl: string = this.endpoint + "Parking";

  constructor(private Http: HttpClient) { }

  //Get: Historial de todos los vehiculos
  GetAllVehicle(): Observable<Parking[]>{
    return this.Http.get<Parking[]>(`${this.apiUrl}`);
  }

  //GET: Obtener los vehiculos que no han salido
  GetNoExitVehicle(): Observable<Parking[]> {    
    return this.Http.get<Parking[]>(`${this.apiUrl}/GetAllNotExitVehicle`);
  }

  //GET: Total a pagar
  GetValuePay(plate: string): Observable<Parking> {
    return this.Http.get<Parking>(
      `${this.apiUrl}/GetValuePayExitVehicle/${plate}`
    );
  }

  //POST: Agregar vehiculo parqueadero
  ParkingVehicle(model: ParkingRequest): Observable<ParkingRequest> {
    return this.Http.post<ParkingRequest>(`${this.apiUrl}/ParkingVehicle`,model );
  }

  //PUT: Salida al vehiculo del parqueadero
  ExitVehicle(id: string, model: Parking): Observable<Parking> {
    return this.Http.put<Parking>(`${this.apiUrl}/ExitVehicle/${id}`, model);
  }
}
