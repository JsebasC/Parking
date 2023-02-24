import { Component, Inject, OnInit } from "@angular/core";
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material/dialog";
import { MatSnackBar } from "@angular/material/snack-bar";
import { Parking } from "app/Interface/parking";
import { ParkingService } from "app/Service/Parking.service";
import { response } from "express";

@Component({
  selector: "app-exit-vehicle",
  templateUrl: "./exit-vehicle.component.html",
  styleUrls: ["./exit-vehicle.component.css"],
})
export class ExitVehicleComponent implements OnInit {

  vehiclePay?: Parking ;

  constructor(
    private dialogReference: MatDialogRef<ExitVehicleComponent>,
    @Inject(MAT_DIALOG_DATA) public data: Parking,
    private ParkingService: ParkingService,
    private snackBar: MatSnackBar,
  ) {}

  ngOnInit(): void {
    this.GetValuePay(this.data.vehicle);
  }
  
  //Valor a pagar
  GetValuePay(plate: string) {
    this.ParkingService.GetValuePay(plate).subscribe(data => {            
      this.vehiclePay = data;
    });
  }

  //Dar salida a un vehiculo
  ExitVehicle() {
    this.ParkingService.ExitVehicle(this.data.id,this.data).subscribe(
      (reponse) => {
        this.openAlert('Se ha dado salida ', 'Listo');        
        this.CloseDialog(this.data.id);
      },
      (e) => {                
        this.openAlert(e.error.detail, 'Error');
      }
    );
  }

  //Btn close modal
  CloseDialog(id?: string) {    
    this.dialogReference.close(id);
  }

  //Abrir alerta
  openAlert(message: string, action: string) {
    this.snackBar.open(message, action, {
      horizontalPosition: "end",
      verticalPosition: "top",
      duration: 3000,
    });
  }
}
