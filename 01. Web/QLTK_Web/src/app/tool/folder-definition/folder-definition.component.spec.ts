import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FolderDefinitionComponent } from './folder-definition.component';

describe('FolderDefinitionComponent', () => {
  let component: FolderDefinitionComponent;
  let fixture: ComponentFixture<FolderDefinitionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FolderDefinitionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FolderDefinitionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
