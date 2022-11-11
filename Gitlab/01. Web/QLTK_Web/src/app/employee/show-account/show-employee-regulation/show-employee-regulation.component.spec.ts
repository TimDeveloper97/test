import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ShowEmployeeRegulationComponent } from './show-employee-regulation.component';

describe('ShowEmployeeRegulationComponent', () => {
  let component: ShowEmployeeRegulationComponent;
  let fixture: ComponentFixture<ShowEmployeeRegulationComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ShowEmployeeRegulationComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ShowEmployeeRegulationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
