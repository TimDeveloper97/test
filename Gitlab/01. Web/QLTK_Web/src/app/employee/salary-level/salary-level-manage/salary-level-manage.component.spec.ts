import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SalaryLevelManageComponent } from './salary-level-manage.component';

describe('SalaryLevelManageComponent', () => {
  let component: SalaryLevelManageComponent;
  let fixture: ComponentFixture<SalaryLevelManageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SalaryLevelManageComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SalaryLevelManageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
