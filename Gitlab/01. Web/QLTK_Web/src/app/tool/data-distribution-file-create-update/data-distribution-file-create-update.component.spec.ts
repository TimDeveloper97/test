import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DataDistributionFileCreateUpdateComponent } from './data-distribution-file-create-update.component';

describe('DataDistributionFileCreateUpdateComponent', () => {
  let component: DataDistributionFileCreateUpdateComponent;
  let fixture: ComponentFixture<DataDistributionFileCreateUpdateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DataDistributionFileCreateUpdateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DataDistributionFileCreateUpdateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
