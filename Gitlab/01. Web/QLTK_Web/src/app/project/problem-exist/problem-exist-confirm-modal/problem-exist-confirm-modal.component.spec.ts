import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProblemExistConfirmModalComponent } from './problem-exist-confirm-modal.component';

describe('ProblemExistConfirmModalComponent', () => {
  let component: ProblemExistConfirmModalComponent;
  let fixture: ComponentFixture<ProblemExistConfirmModalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProblemExistConfirmModalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProblemExistConfirmModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
