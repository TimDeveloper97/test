import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ChooseManufactureComponent } from './choose-manufacture.component';

describe('ChooseManufactureComponent', () => {
  let component: ChooseManufactureComponent;
  let fixture: ComponentFixture<ChooseManufactureComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ChooseManufactureComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ChooseManufactureComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
