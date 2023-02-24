import { Component, Inject, OnInit, ViewChild } from "@angular/core";

import { Parking } from "app/Interface/parking";
import { ParkingService } from "app/Service/Parking.service";

//Table
import { MatTableDataSource } from "@angular/material/table";
//Paginador
import { MatPaginator } from "@angular/material/paginator";
import { MatDialog } from "@angular/material/dialog";
import { ExitVehicleComponent } from "app/Dialog/exit-vehicle/exit-vehicle.component";

import { ParkingSpaceService } from "app/Service/parking-space.service";
import { ParkingSpace } from "app/Interface/parking-space";

@Component({
  selector: "app-dashboard",
  templateUrl: "./dashboard.component.html",
  styleUrls: ["./dashboard.component.css"],
})
export class DashboardComponent implements OnInit {
  displayedColumns: string[] = [
    "vehicle",
    "parkingSpace",
    "entryDate",
    "vehicleTypeName",
    "actions",
  ];
  dataSource = new MatTableDataSource<Parking>();
  
  spaceMot: number[]=[0,0];
  spaceCar: number[] = [0, 0];
  total: number = 0;

  constructor(
    private ParkingSpace: ParkingSpaceService,
    private ParkingService: ParkingService,
    public dialog: MatDialog
  ) {}

  ngOnInit(): void {
    this.GetNotExitVehicle();
    this.GetSpace();

  }

  //#region  Paginador
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
  }

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }
  //#endregion

  //Obtener los espacios ocupados y los disponibles por tipo de vehiculo
  GetSpace() {
    var spaceMot: ParkingSpace[];
    var spaceCar: ParkingSpace[];
  
    this.ParkingSpace.Get().subscribe((data) => {        
      
      spaceMot = data.filter((x) => x.vehicleType === Number(0) );      
      spaceCar = data.filter((x) => x.vehicleType === Number(1));
        
      const [busySpaceMot, totalMot] = [
        spaceMot.reduce((sum, current) => sum + current.busySpace, 0),
        spaceMot.reduce((sum, current) => sum + current.space, 0)
      ];
      const [busySpaceCar, totalCar] = [
        spaceCar.reduce((sum, current) => sum + current.busySpace, 0),
        spaceCar.reduce((sum, current) => sum + current.space, 0)
      ];
      
      this.spaceMot = [busySpaceMot, totalMot];
      this.spaceCar = [busySpaceCar, totalCar];

    });
  }

  //Obtener los vehiculos que no han salido
  GetNotExitVehicle() {
    this.dataSource.data = [];
    this.ParkingService.GetNoExitVehicle().subscribe(
      (response) => {
        
        this.dataSource.data = response;
      },
      (error) => {
        console.log(error);
      }
    );
  }

  //Dar salida a un vehiculo, abrir modal
  ValuePay(data: Parking) {
    this.dialog
      .open(ExitVehicleComponent, {
        disableClose: false,
        data: data,
        width: "420px",
      })
      .afterClosed()
      .subscribe((result) => {
        this.GetSpace();
      
        if (result)
          this.DeleteVehicleExit(result);
      });
  }

  DeleteVehicleExit(id?: string) {    
    this.dataSource.data = this.dataSource.data.filter(
      (x)=> x.id != id
    )
  }
}
