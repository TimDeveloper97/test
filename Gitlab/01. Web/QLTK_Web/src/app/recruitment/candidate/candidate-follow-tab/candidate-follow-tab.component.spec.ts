import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CandidateFollowTabComponent } from './candidate-follow-tab.component';

describe('CandidateFollowTabComponent', () => {
  let component: CandidateFollowTabComponent;
  let fixture: ComponentFixture<CandidateFollowTabComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CandidateFollowTabComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CandidateFollowTabComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
