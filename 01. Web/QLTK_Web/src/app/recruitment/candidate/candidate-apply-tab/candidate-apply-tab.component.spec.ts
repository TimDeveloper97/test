import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CandidateApplyTabComponent } from './candidate-apply-tab.component';

describe('CandidateApplyTabComponent', () => {
  let component: CandidateApplyTabComponent;
  let fixture: ComponentFixture<CandidateApplyTabComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CandidateApplyTabComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CandidateApplyTabComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
