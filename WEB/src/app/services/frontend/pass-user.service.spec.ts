import { TestBed } from '@angular/core/testing';

import { PassUserService } from './pass-user.service';

describe('PassUserService', () => {
  let service: PassUserService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(PassUserService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
