import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DownloadListModuleComponent } from './download-list-module.component';

describe('DownloadListModuleComponent', () => {
  let component: DownloadListModuleComponent;
  let fixture: ComponentFixture<DownloadListModuleComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DownloadListModuleComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DownloadListModuleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
