import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ImportPrComponent } from './import-pr.component';

describe('ImportPrComponent', () => {
  let component: ImportPrComponent;
  let fixture: ComponentFixture<ImportPrComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ImportPrComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ImportPrComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
