import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ChooseConfigScanFileComponent } from './choose-config-scan-file.component';

describe('ChooseConfigScanFileComponent', () => {
  let component: ChooseConfigScanFileComponent;
  let fixture: ComponentFixture<ChooseConfigScanFileComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ChooseConfigScanFileComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ChooseConfigScanFileComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
