import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TestDesignStructureFolderComponent } from './test-design-structure-folder.component';

describe('TestDesignStructureFolderComponent', () => {
  let component: TestDesignStructureFolderComponent;
  let fixture: ComponentFixture<TestDesignStructureFolderComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TestDesignStructureFolderComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TestDesignStructureFolderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
