import { Routes } from '@angular/router';

export const routes: Routes = [
     {
    path: '',
    redirectTo: 'loans',
    pathMatch: 'full'
  },
  {
    path: 'loans',
    loadComponent: () =>
      import('./features/loan/pages/loan-list/loan-list').then(
        m => m.LoanList
      )
  },
  {
    path: 'calculator',
    loadComponent: () =>
      import('./features/loan/pages/loan-calculator/loan-calculator').then(
        m => m.LoanCalculator
      )
  },
  {
    path: 'loans/:id',
    loadComponent: () =>
      import('./features/loan/pages/loan-detail/loan-detail').then(
        m => m.LoanDetail
      )
  },
  {
    path: '**',
    redirectTo: 'loans'
  }
];
