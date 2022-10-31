import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ModulegroupManageComponent } from './modulegroup-manage.component';

describe('ModulegroupManageComponent', () => {
  let component: ModulegroupManageComponent;
  let fixture: ComponentFixture<ModulegroupManageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ModulegroupManageComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ModulegroupManageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
