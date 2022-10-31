import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MessageconfirmcodeComponent } from './messageconfirmcode.component';

describe('MessageconfirmcodeComponent', () => {
  let component: MessageconfirmcodeComponent;
  let fixture: ComponentFixture<MessageconfirmcodeComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MessageconfirmcodeComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MessageconfirmcodeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
