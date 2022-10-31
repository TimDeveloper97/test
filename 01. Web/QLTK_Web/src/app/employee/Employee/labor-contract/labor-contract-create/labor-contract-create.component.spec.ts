import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LaborContractCreateComponent } from './labor-contract-create.component';

describe('LaborContractCreateComponent', () => {
  let component: LaborContractCreateComponent;
  let fixture: ComponentFixture<LaborContractCreateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ LaborContractCreateComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(LaborContractCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
