import { Component, OnInit } from "@angular/core";

//Importar alerta
import { MatSnackBar } from "@angular/material/snack-bar";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";

//Parking Space
import { ParkingSpace } from "app/Interface/parking-space";
import { ParkingSpaceService } from "app/Service/parking-space.service";
import { ParkingRequest } from "app/Interface/parkingRequest";
import { ParkingService } from "app/Service/parking.service";

@Component({
  selector: "app-parking",
  templateUrl: "./parking.component.html",
  styleUrls: ["./parking.component.css"],
})
export class ParkingComponent implements OnInit {
  formParking: FormGroup;
  listSpace: ParkingSpace[] = [];
  filteredSpaces: any[] = [];

  constructor(
    private Form: FormBuilder,
    private snackBar: MatSnackBar,
    private ParkingSpaceService: ParkingSpaceService,
    private ParkingService: ParkingService
  ) {
    //Formulario
    this.formParking = this.Form.group({
      plate: ["", Validators.required],
      cubicCentimeters: [null, Validators.nullValidator],
      vehicleType: ["", Validators.nullValidator],
      idParkingSpace: ["", Validators.required],
    });

    //Obtener todos los espacios del parqueadero
    this.ParkingSpaceService.Get().subscribe((listSpace: ParkingSpace[]) => {
      this.listSpace = listSpace;
    });
  }

  ngOnInit(): void {}

  //filtrar el espacio segun el tipo de vehiculo
  onVehicleTypeChange(event: number): void {
    this.filteredSpaces = this.listSpace.filter(
      (x) => x.vehicleType === Number(event)
    );
  }

  //Agregar al parqueadero
  AddParking() {
    const vehicle: ParkingRequest = {
      Plate: this.formParking.value.plate,
      CubicCentimeters: this.formParking.value.cubicCentimeters,
      VehicleType: this.formParking.value.vehicleType,
      idParkingSpace: this.formParking.value.idParkingSpace,
    };

    
    this.ParkingService.ParkingVehicle(vehicle).subscribe(data  => {
      this.openAlert('Se ha ingresado correctamente al parqueadero', 'Exito!');
      this.clearForm();
    }, (e) => {
      this.openAlert(e.error.detail, 'Error!');
    });
  }


  clearForm() {
    this.formParking.reset();
    Object.keys(this.formParking.controls).forEach(key => {
      this.formParking.get(key).setErrors(null) ;
    });
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
