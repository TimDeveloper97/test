import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PracticeSupModuleChooseComponent } from './practice-sup-module-choose.component';

describe('PracticeSupModuleChooseComponent', () => {
  let component: PracticeSupModuleChooseComponent;
  let fixture: ComponentFixture<PracticeSupModuleChooseComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PracticeSupModuleChooseComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PracticeSupModuleChooseComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
