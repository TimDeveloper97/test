import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NtsStatusComponent } from './nts-status.component';

describe('NtsStatusComponent', () => {
  let component: NtsStatusComponent;
  let fixture: ComponentFixture<NtsStatusComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NtsStatusComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NtsStatusComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
