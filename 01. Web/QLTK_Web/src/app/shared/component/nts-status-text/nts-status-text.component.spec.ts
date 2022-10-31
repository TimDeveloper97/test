import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NtsStatusTextComponent } from './nts-status-text.component';

describe('NtsStatusTextComponent', () => {
  let component: NtsStatusTextComponent;
  let fixture: ComponentFixture<NtsStatusTextComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NtsStatusTextComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NtsStatusTextComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
