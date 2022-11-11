import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ImportEmployeeComponent } from './import-employee.component';

describe('ImportEmployeeComponent', () => {
  let component: ImportEmployeeComponent;
  let fixture: ComponentFixture<ImportEmployeeComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ImportEmployeeComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ImportEmployeeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
