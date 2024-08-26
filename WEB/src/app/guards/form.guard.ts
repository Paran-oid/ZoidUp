import { CanDeactivateFn } from '@angular/router';
import { RegisterComponent } from '../components/register/register.component';

export const formGuard: CanDeactivateFn<RegisterComponent> = (
  component,
  currentRoute,
  currentState,
  nextState
) => {
  if (component.HasUnsavedChanges()) {
    let result = confirm('are you sure?');
    return result;
  }
  return true;
};