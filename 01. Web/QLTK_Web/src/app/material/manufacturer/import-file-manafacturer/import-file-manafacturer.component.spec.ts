import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ImportFileManafacturerComponent } from './import-file-manafacturer.component';

describe('ImportFileManafacturerComponent', () => {
  let component: ImportFileManafacturerComponent;
  let fixture: ComponentFixture<ImportFileManafacturerComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ImportFileManafacturerComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ImportFileManafacturerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
