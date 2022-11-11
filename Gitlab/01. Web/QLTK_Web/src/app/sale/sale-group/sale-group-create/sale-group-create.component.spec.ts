import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SaleGroupCreateComponent } from './sale-group-create.component';

describe('SaleGroupCreateComponent', () => {
  let component: SaleGroupCreateComponent;
  let fixture: ComponentFixture<SaleGroupCreateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SaleGroupCreateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SaleGroupCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
