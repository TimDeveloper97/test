import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TechnologySolutionCreateComponent } from './technology-solution-create.component';

describe('TechnologySolutionCreateComponent', () => {
  let component: TechnologySolutionCreateComponent;
  let fixture: ComponentFixture<TechnologySolutionCreateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TechnologySolutionCreateComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(TechnologySolutionCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
