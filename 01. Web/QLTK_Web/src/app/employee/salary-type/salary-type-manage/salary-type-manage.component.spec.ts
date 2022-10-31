import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SalaryTypeManageComponent } from './salary-type-manage.component';

describe('SalaryTypeManageComponent', () => {
  let component: SalaryTypeManageComponent;
  let fixture: ComponentFixture<SalaryTypeManageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SalaryTypeManageComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SalaryTypeManageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
