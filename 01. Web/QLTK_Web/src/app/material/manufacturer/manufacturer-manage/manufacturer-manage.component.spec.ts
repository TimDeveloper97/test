import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ManufacturerManageComponent } from './manufacturer-manage.component';

describe('ManufacturerManageComponent', () => {
  let component: ManufacturerManageComponent;
  let fixture: ComponentFixture<ManufacturerManageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ManufacturerManageComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ManufacturerManageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
