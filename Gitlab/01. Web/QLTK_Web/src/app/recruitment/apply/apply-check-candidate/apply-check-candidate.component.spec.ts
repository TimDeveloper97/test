import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ApplyCheckCandidateComponent } from './apply-check-candidate.component';

describe('ApplyCheckCandidateComponent', () => {
  let component: ApplyCheckCandidateComponent;
  let fixture: ComponentFixture<ApplyCheckCandidateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ApplyCheckCandidateComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ApplyCheckCandidateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
