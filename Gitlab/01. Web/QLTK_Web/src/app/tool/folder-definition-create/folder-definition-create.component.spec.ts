import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FolderDefinitionCreateComponent } from './folder-definition-create.component';

describe('FolderDefinitionCreateComponent', () => {
  let component: FolderDefinitionCreateComponent;
  let fixture: ComponentFixture<FolderDefinitionCreateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FolderDefinitionCreateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FolderDefinitionCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
