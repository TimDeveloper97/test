import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PlanCopyComponent } from './plan-copy.component';

describe('PlanCopyComponent', () => {
  let component: PlanCopyComponent;
  let fixture: ComponentFixture<PlanCopyComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PlanCopyComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PlanCopyComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
