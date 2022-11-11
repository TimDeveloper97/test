import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ShowProjectSolutionComponent } from './show-project-solution.component';

describe('ShowProjectSolutionComponent', () => {
  let component: ShowProjectSolutionComponent;
  let fixture: ComponentFixture<ShowProjectSolutionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ShowProjectSolutionComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ShowProjectSolutionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
