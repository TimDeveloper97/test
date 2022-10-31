import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ImportSupplierComponent } from './import-supplier.component';

describe('ImportSupplierComponent', () => {
  let component: ImportSupplierComponent;
  let fixture: ComponentFixture<ImportSupplierComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ImportSupplierComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ImportSupplierComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
