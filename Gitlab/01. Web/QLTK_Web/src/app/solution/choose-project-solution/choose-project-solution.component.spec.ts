import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ChooseProjectSolutionComponent } from './choose-project-solution.component';

describe('ChooseProjectSolutionComponent', () => {
  let component: ChooseProjectSolutionComponent;
  let fixture: ComponentFixture<ChooseProjectSolutionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ChooseProjectSolutionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ChooseProjectSolutionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
