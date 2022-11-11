import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProblemExistConfirmComponent } from './problem-exist-confirm.component';

describe('ProblemExistConfirmComponent', () => {
  let component: ProblemExistConfirmComponent;
  let fixture: ComponentFixture<ProblemExistConfirmComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProblemExistConfirmComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProblemExistConfirmComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
