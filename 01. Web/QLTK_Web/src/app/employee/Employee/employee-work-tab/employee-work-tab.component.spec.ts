import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EmployeeWorkTabComponent } from './employee-work-tab.component';

describe('EmployeeWorkTabComponent', () => {
  let component: EmployeeWorkTabComponent;
  let fixture: ComponentFixture<EmployeeWorkTabComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ EmployeeWorkTabComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(EmployeeWorkTabComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
