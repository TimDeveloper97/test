import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DataDistributionComponent } from './data-distribution.component';

describe('DataDistributionComponent', () => {
  let component: DataDistributionComponent;
  let fixture: ComponentFixture<DataDistributionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DataDistributionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DataDistributionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
