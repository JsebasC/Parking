import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AdminLayoutRoutes } from './admin-layout.routing';
import { DashboardComponent } from '../../dashboard/dashboard.component';
import { TableListComponent } from '../../table-list/table-list.component';
import { IconsComponent } from '../../icons/icons.component';
import {MatButtonModule} from '@angular/material/button';
import {MatInputModule} from '@angular/material/input';
import {MatRippleModule} from '@angular/material/core';
import {MatFormFieldModule} from '@angular/material/form-field';
import {MatTooltipModule} from '@angular/material/tooltip';
import {MatSelectModule} from '@angular/material/select';
import { ParkingComponent } from '../../parking/parking.component';

//Consumir APIS
import { HttpClientModule } from '@angular/common/http';

//Dialogos
import { MatDialogModule } from '@angular/material/dialog';
//Paginador
import { MatPaginatorModule } from '@angular/material/paginator';
//Alertar material desing
import { MatSnackBarModule } from '@angular/material/snack-bar';
//Table para marcar el datasource
import { MatTableModule } from '@angular/material/table';
import { GarageComponent } from 'app/garage/garage.component';


@NgModule({
  imports: [
    CommonModule,
    RouterModule.forChild(AdminLayoutRoutes),
    FormsModule,
    ReactiveFormsModule,
    MatButtonModule,
    MatRippleModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatTooltipModule,
    MatDialogModule,
    MatPaginatorModule,
    HttpClientModule,
    MatSnackBarModule,
    MatTableModule
  ],
  declarations: [
    DashboardComponent,    
    TableListComponent,  
    IconsComponent,     
    ParkingComponent,
    GarageComponent
  ]
})

export class AdminLayoutModule {}
