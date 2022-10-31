import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PracticeExpertsChooseComponent } from './practice-experts-choose.component';

describe('PracticeExpertsChooseComponent', () => {
  let component: PracticeExpertsChooseComponent;
  let fixture: ComponentFixture<PracticeExpertsChooseComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PracticeExpertsChooseComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PracticeExpertsChooseComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
