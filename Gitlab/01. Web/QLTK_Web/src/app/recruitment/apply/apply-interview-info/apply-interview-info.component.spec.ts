import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ApplyInterviewInfoComponent } from './apply-interview-info.component';

describe('ApplyInterviewInfoComponent', () => {
  let component: ApplyInterviewInfoComponent;
  let fixture: ComponentFixture<ApplyInterviewInfoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ApplyInterviewInfoComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ApplyInterviewInfoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
