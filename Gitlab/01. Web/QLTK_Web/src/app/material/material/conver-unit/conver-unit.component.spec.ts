import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ConverUnitComponent } from './conver-unit.component';

describe('ConverUnitComponent', () => {
  let component: ConverUnitComponent;
  let fixture: ComponentFixture<ConverUnitComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ConverUnitComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ConverUnitComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
