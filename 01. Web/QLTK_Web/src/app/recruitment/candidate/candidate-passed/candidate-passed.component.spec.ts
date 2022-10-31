import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CandidatePassedComponent } from './candidate-passed.component';

describe('CandidatePassedComponent', () => {
  let component: CandidatePassedComponent;
  let fixture: ComponentFixture<CandidatePassedComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CandidatePassedComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CandidatePassedComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
