import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ShowChooseEmployeeComponent } from './show-choose-employee.component';

describe('ShowChooseEmployeeComponent', () => {
  let component: ShowChooseEmployeeComponent;
  let fixture: ComponentFixture<ShowChooseEmployeeComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ShowChooseEmployeeComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ShowChooseEmployeeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
