import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ExportAndKeepManageComponent } from './export-and-keep-manage.component';

describe('ExportAndKeepManageComponent', () => {
  let component: ExportAndKeepManageComponent;
  let fixture: ComponentFixture<ExportAndKeepManageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ExportAndKeepManageComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ExportAndKeepManageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
