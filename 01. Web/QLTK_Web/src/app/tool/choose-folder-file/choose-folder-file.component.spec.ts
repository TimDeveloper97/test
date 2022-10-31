import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ChooseFolderFileComponent } from './choose-folder-file.component';

describe('ChooseFolderFileComponent', () => {
  let component: ChooseFolderFileComponent;
  let fixture: ComponentFixture<ChooseFolderFileComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ChooseFolderFileComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ChooseFolderFileComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
