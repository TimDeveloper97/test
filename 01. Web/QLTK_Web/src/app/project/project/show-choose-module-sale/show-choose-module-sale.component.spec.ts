import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ShowChooseModuleSaleComponent } from './show-choose-module-sale.component';

describe('ShowChooseModuleSaleComponent', () => {
  let component: ShowChooseModuleSaleComponent;
  let fixture: ComponentFixture<ShowChooseModuleSaleComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ShowChooseModuleSaleComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ShowChooseModuleSaleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
