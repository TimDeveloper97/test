import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PopupPracticeCreateComponent } from './popup-practice-create.component';

describe('PopupPracticeCreateComponent', () => {
  let component: PopupPracticeCreateComponent;
  let fixture: ComponentFixture<PopupPracticeCreateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PopupPracticeCreateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PopupPracticeCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
