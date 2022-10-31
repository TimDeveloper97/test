import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { UnitManagerComponent } from './unit-manager.component';

describe('UnitManagerComponent', () => {
  let component: UnitManagerComponent;
  let fixture: ComponentFixture<UnitManagerComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ UnitManagerComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(UnitManagerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
