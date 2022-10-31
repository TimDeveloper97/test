import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DataDistributionFileChooseComponent } from './data-distribution-file-choose.component';

describe('DataDistributionFileChooseComponent', () => {
  let component: DataDistributionFileChooseComponent;
  let fixture: ComponentFixture<DataDistributionFileChooseComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DataDistributionFileChooseComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DataDistributionFileChooseComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
