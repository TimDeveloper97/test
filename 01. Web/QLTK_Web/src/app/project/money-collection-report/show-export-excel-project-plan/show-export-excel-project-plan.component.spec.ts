import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ShowExportExcelProjectPlanComponent } from './show-export-excel-project-plan.component';

describe('ShowExportExcelProjectPlanComponent', () => {
  let component: ShowExportExcelProjectPlanComponent;
  let fixture: ComponentFixture<ShowExportExcelProjectPlanComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ShowExportExcelProjectPlanComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ShowExportExcelProjectPlanComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
