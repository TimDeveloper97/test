import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ImportProfileViewComponent } from './import-profile-view.component';

describe('ImportProfileViewComponent', () => {
  let component: ImportProfileViewComponent;
  let fixture: ComponentFixture<ImportProfileViewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ImportProfileViewComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ImportProfileViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
