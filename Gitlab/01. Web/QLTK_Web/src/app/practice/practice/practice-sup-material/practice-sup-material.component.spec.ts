import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PracticeSupMaterialComponent } from './practice-sup-material.component';

describe('PracticeSupMaterialComponent', () => {
  let component: PracticeSupMaterialComponent;
  let fixture: ComponentFixture<PracticeSupMaterialComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PracticeSupMaterialComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PracticeSupMaterialComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
