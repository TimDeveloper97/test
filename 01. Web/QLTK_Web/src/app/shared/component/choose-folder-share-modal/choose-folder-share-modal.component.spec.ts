import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ChooseFolderShareModalComponent } from './choose-folder-share-modal.component';

describe('ChooseFolderShareModalComponent', () => {
  let component: ChooseFolderShareModalComponent;
  let fixture: ComponentFixture<ChooseFolderShareModalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ChooseFolderShareModalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ChooseFolderShareModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
