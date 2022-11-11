import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DataDistributionFileManageModalComponent } from './data-distribution-file-manage-modal.component';

describe('DataDistributionFileManageModalComponent', () => {
  let component: DataDistributionFileManageModalComponent;
  let fixture: ComponentFixture<DataDistributionFileManageModalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DataDistributionFileManageModalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DataDistributionFileManageModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
