import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ChooseSolutionComponent } from './choose-solution.component';

describe('ChooseSolutionComponent', () => {
  let component: ChooseSolutionComponent;
  let fixture: ComponentFixture<ChooseSolutionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ChooseSolutionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ChooseSolutionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
