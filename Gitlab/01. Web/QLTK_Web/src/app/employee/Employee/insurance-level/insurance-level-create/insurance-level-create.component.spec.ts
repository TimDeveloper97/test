import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InsuranceLevelCreateComponent } from './insurance-level-create.component';

describe('InsuranceLevelCreateComponent', () => {
  let component: InsuranceLevelCreateComponent;
  let fixture: ComponentFixture<InsuranceLevelCreateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ InsuranceLevelCreateComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(InsuranceLevelCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
