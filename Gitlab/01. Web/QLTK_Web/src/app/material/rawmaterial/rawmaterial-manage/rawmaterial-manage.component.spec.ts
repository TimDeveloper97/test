import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RawmaterialManageComponent } from './rawmaterial-manage.component';

describe('RawmaterialManageComponent', () => {
  let component: RawmaterialManageComponent;
  let fixture: ComponentFixture<RawmaterialManageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RawmaterialManageComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RawmaterialManageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
