import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TestFileDmvtComponent } from './test-file-dmvt.component';

describe('TestFileDmvtComponent', () => {
  let component: TestFileDmvtComponent;
  let fixture: ComponentFixture<TestFileDmvtComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TestFileDmvtComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TestFileDmvtComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
