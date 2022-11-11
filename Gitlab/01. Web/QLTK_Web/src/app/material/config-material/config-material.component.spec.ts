import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ConfigMaterialComponent } from './config-material.component';

describe('ConfigMaterialComponent', () => {
  let component: ConfigMaterialComponent;
  let fixture: ComponentFixture<ConfigMaterialComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ConfigMaterialComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ConfigMaterialComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
