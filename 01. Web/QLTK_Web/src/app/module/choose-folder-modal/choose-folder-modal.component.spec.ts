import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ChooseFolderModalComponent } from './choose-folder-modal.component';

describe('ChooseFolderModalComponent', () => {
  let component: ChooseFolderModalComponent;
  let fixture: ComponentFixture<ChooseFolderModalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ChooseFolderModalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ChooseFolderModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
