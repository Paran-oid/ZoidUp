import { HttpInterceptorFn } from '@angular/common/http';
import { SpinnerService } from '../../services/spinner.service';
import { inject } from '@angular/core';
import { delay, finalize } from 'rxjs';

export const loadingInterceptor: HttpInterceptorFn = (req, next) => {
  let loadingService = inject(SpinnerService);
  loadingService.Loading();
  return next(req).pipe(
    delay(1000),
    finalize(() => {
      loadingService.Inactive();
    })
  );
};
