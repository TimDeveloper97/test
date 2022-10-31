import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SolutionTabDesignDocumentComponent } from './solution-tab-design-document.component';

describe('SolutionTabDesignDocumentComponent', () => {
  let component: SolutionTabDesignDocumentComponent;
  let fixture: ComponentFixture<SolutionTabDesignDocumentComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SolutionTabDesignDocumentComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SolutionTabDesignDocumentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
