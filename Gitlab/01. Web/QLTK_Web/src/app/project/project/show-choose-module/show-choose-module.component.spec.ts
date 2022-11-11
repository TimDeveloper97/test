import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ShowChooseModuleComponent } from './show-choose-module.component';

describe('ShowChooseModuleComponent', () => {
  let component: ShowChooseModuleComponent;
  let fixture: ComponentFixture<ShowChooseModuleComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ShowChooseModuleComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ShowChooseModuleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
