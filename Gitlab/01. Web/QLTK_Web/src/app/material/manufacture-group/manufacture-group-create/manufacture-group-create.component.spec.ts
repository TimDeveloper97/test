import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ManufactureGroupCreateComponent } from './manufacture-group-create.component';

describe('ManufactureGroupCreateComponent', () => {
  let component: ManufactureGroupCreateComponent;
  let fixture: ComponentFixture<ManufactureGroupCreateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ManufactureGroupCreateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ManufactureGroupCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
