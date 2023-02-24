import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ExitVehicleComponent } from './exit-vehicle.component';

describe('ExitVehicleComponent', () => {
  let component: ExitVehicleComponent;
  let fixture: ComponentFixture<ExitVehicleComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ExitVehicleComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ExitVehicleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
