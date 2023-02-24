import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { Parking } from 'app/Interface/parking';
import { ParkingService } from 'app/Service/Parking.service';

@Component({
  selector: 'app-garage',
  templateUrl: './garage.component.html',
  styleUrls: ['./garage.component.css']
})
export class GarageComponent implements OnInit {

  displayedColumns: string[] = [
    "vehicle",
    "parkingSpace",
    "entryDate",
    "vehicleTypeName",
    "duration",
    "totalValue"
  ];

  dataSource = new MatTableDataSource<Parking>();

  
  constructor(private ParkingService : ParkingService) { }

  ngOnInit(): void {
    this.GetAllExitVehicle();
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
  
   //Obtener los vehiculos que no han salido
   GetAllExitVehicle() {
    this.dataSource.data = [];
    this.ParkingService.GetAllVehicle().subscribe(
      (response) => {
        this.dataSource.data = response;
      },
      (error) => {
        console.log(error);
      }
    );
   }
  
}
