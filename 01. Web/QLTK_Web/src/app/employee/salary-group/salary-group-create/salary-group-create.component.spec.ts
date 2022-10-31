import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SalaryGroupCreateComponent } from './salary-group-create.component';

describe('SalaryLevelCreateComponent', () => {
  let component: SalaryGroupCreateComponent;
  let fixture: ComponentFixture<SalaryGroupCreateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SalaryGroupCreateComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SalaryGroupCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
