import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CandidateApplysListComponent } from './candidate-applys-list.component';

describe('CandidateApplysListComponent', () => {
  let component: CandidateApplysListComponent;
  let fixture: ComponentFixture<CandidateApplysListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CandidateApplysListComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CandidateApplysListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
