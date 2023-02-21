import { Component, DebugElement, OnInit} from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';

import { ParkingRequest } from 'src/app/Model/parking-request';
import { ParkingService } from 'src/app/Services/parking.service';
import { ParkingSpace } from 'src/app/Model/parking-space';
import { ParkingSpaceService } from 'src/app/Services/parking-space.service';

@Component({
  selector: 'app-add-parking',
  templateUrl: './add-parking.component.html',
  styleUrls: ['./add-parking.component.css'],
})
  
export class AddParkingComponent implements OnInit {
  formParking: FormGroup;
  tituloAction: string = 'Agregar';
  botonAction: string = 'Guardar';
  listSpace: ParkingSpace[] = [];
  filteredSpaces: any[] = [];

  constructor(
    private dialogReference: MatDialogRef<AddParkingComponent>,
    private fb: FormBuilder,
    private _snackBar: MatSnackBar,
    private _parkingSpace: ParkingSpaceService,
    private _parkingService: ParkingService
  ) {

    this.formParking = this.fb.group({
      plate: ['', Validators.required],
      cubicCentimeters: [null, Validators.nullValidator],
      vehicleType: ['', Validators.nullValidator],
      idParkingSpace: ['', Validators.required],
    });

    this._parkingSpace.getList().subscribe({
      next: (date) => {
        this.listSpace = date;
      },
      error: (e) => {},
    });
  }


  //changed Select tipo de vehiculo
  onVehicleTypeChange(event: Event) :void {
    const value = (event.target as HTMLSelectElement).value;
   this.filteredSpaces = this.listSpace.filter(x => x.vehicleType === Number(value));
  }

  //Agregar
  addParking() {

    console.log(this.formParking);
    console.log(this.formParking.value);

    const vehicle: ParkingRequest = {
      Plate: this.formParking.value.plate,
      CubicCentimeters: this.formParking.value.cubicCentimeters,
      VehicleType: this.formParking.value.vehicleType,
      idParkingSpace:this.formParking.value.idParkingSpace
    };

    this._parkingService.add(vehicle).subscribe(
      (data) => {
        this.openAlert('Se ha creado correctamente la entrada ', 'Listo');
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
  ngOnInit(): void {}
}
