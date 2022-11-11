import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RawmaterialCreateComponent } from './rawmaterial-create.component';

describe('RawmaterialCreateComponent', () => {
  let component: RawmaterialCreateComponent;
  let fixture: ComponentFixture<RawmaterialCreateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RawmaterialCreateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RawmaterialCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
