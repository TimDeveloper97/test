import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MessageconfirmComponent } from './messageconfirm.component';

describe('MessageconfirmComponent', () => {
  let component: MessageconfirmComponent;
  let fixture: ComponentFixture<MessageconfirmComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MessageconfirmComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MessageconfirmComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
