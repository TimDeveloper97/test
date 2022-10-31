import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MeetingCreateRequirmentModelComponent } from './meeting-create-requirment-model.component';

describe('MeetingCreateRequirmentModelComponent', () => {
  let component: MeetingCreateRequirmentModelComponent;
  let fixture: ComponentFixture<MeetingCreateRequirmentModelComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ MeetingCreateRequirmentModelComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(MeetingCreateRequirmentModelComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
