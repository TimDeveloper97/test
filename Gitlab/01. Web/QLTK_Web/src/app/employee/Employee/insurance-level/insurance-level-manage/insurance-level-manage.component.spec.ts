import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InsuranceLevelManageComponent } from './insurance-level-manage.component';

describe('InsuranceLevelManageComponent', () => {
  let component: InsuranceLevelManageComponent;
  let fixture: ComponentFixture<InsuranceLevelManageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ InsuranceLevelManageComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(InsuranceLevelManageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
