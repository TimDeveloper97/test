import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TestFolderIgsComponent } from './test-folder-igs.component';

describe('TestFolderIgsComponent', () => {
  let component: TestFolderIgsComponent;
  let fixture: ComponentFixture<TestFolderIgsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TestFolderIgsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TestFolderIgsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
