import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LaborContractManageComponent } from './labor-contract-manage.component';

describe('LaborContractManageComponent', () => {
  let component: LaborContractManageComponent;
  let fixture: ComponentFixture<LaborContractManageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ LaborContractManageComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(LaborContractManageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
