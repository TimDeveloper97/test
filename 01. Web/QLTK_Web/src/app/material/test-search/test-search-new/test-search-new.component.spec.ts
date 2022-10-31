import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TestSearchNewComponent } from './test-search-new.component';

describe('TestSearchNewComponent', () => {
  let component: TestSearchNewComponent;
  let fixture: ComponentFixture<TestSearchNewComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TestSearchNewComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TestSearchNewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
