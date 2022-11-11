import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DataDistributionCreateFolderComponent } from './data-distribution-create-folder.component';

describe('DataDistributionCreateFolderComponent', () => {
  let component: DataDistributionCreateFolderComponent;
  let fixture: ComponentFixture<DataDistributionCreateFolderComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DataDistributionCreateFolderComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DataDistributionCreateFolderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
