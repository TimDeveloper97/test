import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ImportProfileCreateComponent } from './import-profile-create.component';

describe('ImportProfileCreateComponent', () => {
  let component: ImportProfileCreateComponent;
  let fixture: ComponentFixture<ImportProfileCreateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ImportProfileCreateComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ImportProfileCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
