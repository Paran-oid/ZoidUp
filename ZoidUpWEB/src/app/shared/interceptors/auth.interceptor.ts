import { HttpInterceptorFn } from '@angular/common/http';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const token = window.localStorage.getItem('Token');
  if (token) {
    req = req.clone({
      setHeaders: { Authorisation: `Bearer ${token}` },
    });
  }
  return next(req);
};
