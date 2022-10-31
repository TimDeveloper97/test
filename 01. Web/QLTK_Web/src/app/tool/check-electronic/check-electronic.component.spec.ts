import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CheckElectronicComponent } from './check-electronic.component';

describe('CheckElectronicComponent', () => {
  let component: CheckElectronicComponent;
  let fixture: ComponentFixture<CheckElectronicComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CheckElectronicComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CheckElectronicComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
