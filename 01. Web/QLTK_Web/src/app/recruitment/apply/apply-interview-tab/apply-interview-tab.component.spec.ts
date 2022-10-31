import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ApplyInterviewTabComponent } from './apply-interview-tab.component';

describe('ApplyInterviewTabComponent', () => {
  let component: ApplyInterviewTabComponent;
  let fixture: ComponentFixture<ApplyInterviewTabComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ApplyInterviewTabComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ApplyInterviewTabComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
