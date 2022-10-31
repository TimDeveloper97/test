import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SolutionOldVersionComponent } from './solution-old-version.component';

describe('SolutionOldVersionComponent', () => {
  let component: SolutionOldVersionComponent;
  let fixture: ComponentFixture<SolutionOldVersionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SolutionOldVersionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SolutionOldVersionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
