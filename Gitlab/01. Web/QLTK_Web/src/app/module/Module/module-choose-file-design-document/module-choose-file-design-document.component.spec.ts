import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ModuleChooseFileDesignDocumentComponent } from './module-choose-file-design-document.component';

describe('ModuleChooseFileDesignDocumentComponent', () => {
  let component: ModuleChooseFileDesignDocumentComponent;
  let fixture: ComponentFixture<ModuleChooseFileDesignDocumentComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ModuleChooseFileDesignDocumentComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ModuleChooseFileDesignDocumentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
