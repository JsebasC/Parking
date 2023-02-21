import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Parking } from 'src/app/Model/parking';
import { ParkingService } from 'src/app/Services/parking.service';
import { MatSnackBar } from '@angular/material/snack-bar';


@Component({
  selector: 'app-exit-parking',
  templateUrl: './exit-parking.component.html',
  styleUrls: ['./exit-parking.component.css']
})
export class ExitParkingComponent implements OnInit {
  
  vehiclePay?: Parking ;

  ngOnInit(): void {
    console.log(this.data);
    this.valuePay(this.data.vehicle);
  }
  
  constructor(
    private dialogReference: MatDialogRef<ExitParkingComponent>,
    @Inject(MAT_DIALOG_DATA) public data: Parking,
    private _snackBar: MatSnackBar,
    private _parkingService: ParkingService
  ) { }

  valuePay(plate: string) {
    this._parkingService.getValueExitVehicle(plate).subscribe(data => {            
      this.vehiclePay = data;
    });
  }

  confirmarSalida() {
    this._parkingService.exitVehicle(this.data.id,this.data).subscribe(
      (data) => {
        this.openAlert('Se ha dado salida ', 'Listo');
        this.dialogReference.close();
      },
      (e) => {                
        this.openAlert(e.error.detail, 'Error');
      }
    );
  }

  openAlert(message: string, action: string) {
    this._snackBar.open(message, action, {
      horizontalPosition: 'end',
      verticalPosition: 'top',
      duration: 3000,
    });
  }

}
