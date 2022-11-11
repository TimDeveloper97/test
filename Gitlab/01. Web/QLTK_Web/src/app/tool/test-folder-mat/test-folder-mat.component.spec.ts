import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TestFolderMatComponent } from './test-folder-mat.component';

describe('TestFolderMatComponent', () => {
  let component: TestFolderMatComponent;
  let fixture: ComponentFixture<TestFolderMatComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TestFolderMatComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TestFolderMatComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
