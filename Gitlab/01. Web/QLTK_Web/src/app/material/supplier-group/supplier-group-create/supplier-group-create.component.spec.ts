import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SupplierGroupCreateComponent } from './supplier-group-create.component';

describe('SupplierGroupCreateComponent', () => {
  let component: SupplierGroupCreateComponent;
  let fixture: ComponentFixture<SupplierGroupCreateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SupplierGroupCreateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SupplierGroupCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
