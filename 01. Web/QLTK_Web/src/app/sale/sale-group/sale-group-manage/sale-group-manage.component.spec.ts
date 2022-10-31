import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SaleGroupManageComponent } from './sale-group-manage.component';

describe('SaleGroupManageComponent', () => {
  let component: SaleGroupManageComponent;
  let fixture: ComponentFixture<SaleGroupManageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SaleGroupManageComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SaleGroupManageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
