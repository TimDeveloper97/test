import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TestMaterialPriceComponent } from './test-material-price.component';

describe('TestMaterialPriceComponent', () => {
  let component: TestMaterialPriceComponent;
  let fixture: ComponentFixture<TestMaterialPriceComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TestMaterialPriceComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TestMaterialPriceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
