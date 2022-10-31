import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ShowSelectModuleComponent } from './show-select-module.component';

describe('ShowSelectModuleComponent', () => {
  let component: ShowSelectModuleComponent;
  let fixture: ComponentFixture<ShowSelectModuleComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ShowSelectModuleComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ShowSelectModuleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
