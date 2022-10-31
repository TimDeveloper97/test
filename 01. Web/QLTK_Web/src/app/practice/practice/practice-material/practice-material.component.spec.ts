import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PracticeMaterialComponent } from './practice-material.component';

describe('PracticeMaterialComponent', () => {
  let component: PracticeMaterialComponent;
  let fixture: ComponentFixture<PracticeMaterialComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PracticeMaterialComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PracticeMaterialComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
