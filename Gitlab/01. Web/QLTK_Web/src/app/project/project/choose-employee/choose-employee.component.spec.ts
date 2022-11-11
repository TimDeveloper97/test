import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ChooseEmployeeComponent } from './choose-employee.component';

describe('ChooseEmployeeComponent', () => {
  let component: ChooseEmployeeComponent;
  let fixture: ComponentFixture<ChooseEmployeeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ChooseEmployeeComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ChooseEmployeeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
