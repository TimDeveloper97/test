import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ModuleTabDesignDocumentComponent } from './module-tab-design-document.component';

describe('ModuleTabDesignDocumentComponent', () => {
  let component: ModuleTabDesignDocumentComponent;
  let fixture: ComponentFixture<ModuleTabDesignDocumentComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ModuleTabDesignDocumentComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ModuleTabDesignDocumentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
