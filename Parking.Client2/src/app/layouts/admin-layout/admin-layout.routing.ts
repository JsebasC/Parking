import { Routes } from '@angular/router';

import { DashboardComponent } from '../../dashboard/dashboard.component';
import { ParkingComponent } from 'app/parking/parking.component';
import { GarageComponent } from 'app/garage/garage.component';

export const AdminLayoutRoutes: Routes = [
  
    { path: 'dashboard',      component: DashboardComponent },    
    { path: 'garage',     component: GarageComponent },
    { path: 'parking',     component: ParkingComponent },
  
];
