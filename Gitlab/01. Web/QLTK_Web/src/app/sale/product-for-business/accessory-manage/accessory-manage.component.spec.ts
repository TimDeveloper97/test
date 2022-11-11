import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AccessoryManageComponent } from './accessory-manage.component';

describe('AccessoryManageComponent', () => {
  let component: AccessoryManageComponent;
  let fixture: ComponentFixture<AccessoryManageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AccessoryManageComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AccessoryManageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
