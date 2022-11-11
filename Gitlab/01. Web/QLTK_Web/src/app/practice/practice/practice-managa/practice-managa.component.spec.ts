import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PracticeManagaComponent } from './practice-managa.component';

describe('PracticeManagaComponent', () => {
  let component: PracticeManagaComponent;
  let fixture: ComponentFixture<PracticeManagaComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PracticeManagaComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PracticeManagaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
