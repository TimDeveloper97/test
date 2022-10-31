import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SalaryTypeCreateComponent } from './salary-type-create.component';

describe('SalaryTypeCreateComponent', () => {
  let component: SalaryTypeCreateComponent;
  let fixture: ComponentFixture<SalaryTypeCreateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SalaryTypeCreateComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SalaryTypeCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
