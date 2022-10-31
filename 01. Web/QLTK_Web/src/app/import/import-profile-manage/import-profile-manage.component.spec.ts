import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ImportProfileManageComponent } from './import-profile-manage.component';

describe('ImportProfileManageComponent', () => {
  let component: ImportProfileManageComponent;
  let fixture: ComponentFixture<ImportProfileManageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ImportProfileManageComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ImportProfileManageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
