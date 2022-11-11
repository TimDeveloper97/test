import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MasterEmployeeLevelComponent } from './master-employee-level.component';

describe('MasterEmployeeLevelComponent', () => {
  let component: MasterEmployeeLevelComponent;
  let fixture: ComponentFixture<MasterEmployeeLevelComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MasterEmployeeLevelComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MasterEmployeeLevelComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
