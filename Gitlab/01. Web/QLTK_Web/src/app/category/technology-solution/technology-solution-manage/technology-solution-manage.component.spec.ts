import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TechnologySolutionManageComponent } from './technology-solution-manage.component';

describe('TechnologySolutionManageComponent', () => {
  let component: TechnologySolutionManageComponent;
  let fixture: ComponentFixture<TechnologySolutionManageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TechnologySolutionManageComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(TechnologySolutionManageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
