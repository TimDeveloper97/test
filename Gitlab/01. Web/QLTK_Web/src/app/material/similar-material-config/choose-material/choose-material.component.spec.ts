import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ChooseMaterialComponent } from './choose-material.component';

describe('ChooseMaterialComponent', () => {
  let component: ChooseMaterialComponent;
  let fixture: ComponentFixture<ChooseMaterialComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ChooseMaterialComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ChooseMaterialComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
