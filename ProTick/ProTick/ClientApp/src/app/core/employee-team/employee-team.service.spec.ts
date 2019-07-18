import { TestBed } from '@angular/core/testing';

import { EmployeeTeamService } from './employee-team.service';

describe('EmployeeTeamService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: EmployeeTeamService = TestBed.get(EmployeeTeamService);
    expect(service).toBeTruthy();
  });
});
