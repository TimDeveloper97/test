import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MeetingTypeCreateComponent } from './meeting-type-create.component';

describe('MeetingTypeCreateComponent', () => {
  let component: MeetingTypeCreateComponent;
  let fixture: ComponentFixture<MeetingTypeCreateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ MeetingTypeCreateComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(MeetingTypeCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
