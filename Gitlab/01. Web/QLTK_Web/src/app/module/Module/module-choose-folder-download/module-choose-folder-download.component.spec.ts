import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ModuleChooseFolderDownloadComponent } from './module-choose-folder-download.component';

describe('ModuleChooseFolderDownloadComponent', () => {
  let component: ModuleChooseFolderDownloadComponent;
  let fixture: ComponentFixture<ModuleChooseFolderDownloadComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ModuleChooseFolderDownloadComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ModuleChooseFolderDownloadComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
