import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NtsStatusBadgeComponent } from './nts-status-badge.component';

describe('NtsStatusBadgeComponent', () => {
  let component: NtsStatusBadgeComponent;
  let fixture: ComponentFixture<NtsStatusBadgeComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NtsStatusBadgeComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NtsStatusBadgeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
