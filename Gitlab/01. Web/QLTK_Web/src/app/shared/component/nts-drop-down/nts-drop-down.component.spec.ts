import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NTSDropDownComponent } from './nts-drop-down.component';

describe('NTSDropDownComponent', () => {
  let component: NTSDropDownComponent;
  let fixture: ComponentFixture<NTSDropDownComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NTSDropDownComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NTSDropDownComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
