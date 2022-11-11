import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProblemExistCreateComponent } from './problem-exist-create.component';

describe('ProblemExistCreateComponent', () => {
  let component: ProblemExistCreateComponent;
  let fixture: ComponentFixture<ProblemExistCreateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProblemExistCreateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProblemExistCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
