import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NTSDropDownTreeComponent } from './nts-drop-down-tree.component';

describe('NTSDropDownTreeComponent', () => {
  let component: NTSDropDownTreeComponent;
  let fixture: ComponentFixture<NTSDropDownTreeComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NTSDropDownTreeComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NTSDropDownTreeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
