import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SolutionGroupCreateComponent } from './solution-group-create.component';

describe('SolutionGroupCreateComponent', () => {
  let component: SolutionGroupCreateComponent;
  let fixture: ComponentFixture<SolutionGroupCreateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SolutionGroupCreateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SolutionGroupCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
