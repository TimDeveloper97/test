import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SalaryLevelCreateComponent } from './salary-level-create.component';

describe('SalaryLevelCreateComponent', () => {
  let component: SalaryLevelCreateComponent;
  let fixture: ComponentFixture<SalaryLevelCreateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SalaryLevelCreateComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SalaryLevelCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
