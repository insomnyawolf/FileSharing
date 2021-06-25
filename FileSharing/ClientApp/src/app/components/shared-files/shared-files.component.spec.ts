import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SharedFilesComponent } from './shared-files.component';

describe('SharedFilesComponent', () => {
  let component: SharedFilesComponent;
  let fixture: ComponentFixture<SharedFilesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SharedFilesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SharedFilesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
