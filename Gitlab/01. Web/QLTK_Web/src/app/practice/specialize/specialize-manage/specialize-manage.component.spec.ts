import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SpecializeManageComponent } from './specialize-manage.component';

describe('SpecializeManageComponent', () => {
  let component: SpecializeManageComponent;
  let fixture: ComponentFixture<SpecializeManageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SpecializeManageComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SpecializeManageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
