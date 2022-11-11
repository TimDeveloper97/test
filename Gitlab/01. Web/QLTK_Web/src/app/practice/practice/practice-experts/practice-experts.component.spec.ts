import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PracticeExpertsComponent } from './practice-experts.component';

describe('PracticeExpertsComponent', () => {
  let component: PracticeExpertsComponent;
  let fixture: ComponentFixture<PracticeExpertsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PracticeExpertsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PracticeExpertsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
