import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PlanProjectCreateComponent } from './plan-project-create.component';

describe('PlanProjectCreateComponent', () => {
  let component: PlanProjectCreateComponent;
  let fixture: ComponentFixture<PlanProjectCreateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PlanProjectCreateComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PlanProjectCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
