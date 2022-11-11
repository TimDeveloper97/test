import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MoreInterviewsComponent } from './more-interviews.component';

describe('MoreInterviewsComponent', () => {
  let component: MoreInterviewsComponent;
  let fixture: ComponentFixture<MoreInterviewsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ MoreInterviewsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(MoreInterviewsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
