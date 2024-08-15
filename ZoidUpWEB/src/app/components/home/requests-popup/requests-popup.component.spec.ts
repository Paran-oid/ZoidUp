import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RequestsPopupComponent } from './requests-popup.component';

describe('RequestsPopupComponent', () => {
  let component: RequestsPopupComponent;
  let fixture: ComponentFixture<RequestsPopupComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RequestsPopupComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RequestsPopupComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
