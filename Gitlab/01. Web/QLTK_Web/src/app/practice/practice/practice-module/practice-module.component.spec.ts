import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PracticeModuleComponent } from './practice-module.component';

describe('PracticeModuleComponent', () => {
  let component: PracticeModuleComponent;
  let fixture: ComponentFixture<PracticeModuleComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PracticeModuleComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PracticeModuleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
