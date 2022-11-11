import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProblemExistCreateChangePlanComponent } from './problem-exist-create-change-plan.component';

describe('ProblemExistCreateChangePlanComponent', () => {
  let component: ProblemExistCreateChangePlanComponent;
  let fixture: ComponentFixture<ProblemExistCreateChangePlanComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProblemExistCreateChangePlanComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ProblemExistCreateChangePlanComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
