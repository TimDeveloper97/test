import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SalaryGroupManageComponent } from './salary-group-manage.component';

describe('SalaryGroupManageComponent', () => {
  let component: SalaryGroupManageComponent;
  let fixture: ComponentFixture<SalaryGroupManageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SalaryGroupManageComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SalaryGroupManageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
