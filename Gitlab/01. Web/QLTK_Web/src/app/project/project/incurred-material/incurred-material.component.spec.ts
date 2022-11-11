import { ComponentFixture, TestBed } from '@angular/core/testing';

import { IncurredMaterialComponent } from './incurred-material.component';

describe('IncurredMaterialComponent', () => {
  let component: IncurredMaterialComponent;
  let fixture: ComponentFixture<IncurredMaterialComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ IncurredMaterialComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(IncurredMaterialComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
