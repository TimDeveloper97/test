import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ConfigScanFileComponent } from './config-scan-file.component';

describe('ConfigScanFileComponent', () => {
  let component: ConfigScanFileComponent;
  let fixture: ComponentFixture<ConfigScanFileComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ConfigScanFileComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ConfigScanFileComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
