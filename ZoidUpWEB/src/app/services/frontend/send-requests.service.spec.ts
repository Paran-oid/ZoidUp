import { TestBed } from '@angular/core/testing';

import { SendRequestsService } from './send-requests.service';

describe('SendRequestsService', () => {
  let service: SendRequestsService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(SendRequestsService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
