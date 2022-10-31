import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ChooseProductToolComponent } from './choose-product-tool.component';

describe('ChooseProductToolComponent', () => {
  let component: ChooseProductToolComponent;
  let fixture: ComponentFixture<ChooseProductToolComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ChooseProductToolComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ChooseProductToolComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
