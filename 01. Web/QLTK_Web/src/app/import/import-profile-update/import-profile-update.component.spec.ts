import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ImportProfileUpdateComponent } from './import-profile-update.component';

describe('ImportProfileUpdateComponent', () => {
  let component: ImportProfileUpdateComponent;
  let fixture: ComponentFixture<ImportProfileUpdateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ImportProfileUpdateComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ImportProfileUpdateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
