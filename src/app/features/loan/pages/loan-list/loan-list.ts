import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule, CurrencyPipe } from '@angular/common';
import { RouterLink } from '@angular/router';

import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { CardModule } from 'primeng/card';
import { TagModule } from 'primeng/tag';
import { ToolbarModule } from 'primeng/toolbar';

import { LoanApiService } from '../../../../core/services/loan-api.service';
import { LoanSummary } from '../../../../core/models/loan.model';

@Component({
  selector: 'app-loan-list',
  imports: [CommonModule,
    RouterLink,
    TableModule,
    ButtonModule,
    CardModule,
    TagModule,
    ToolbarModule,
    CurrencyPipe,
    ],
  templateUrl: './loan-list.html',
  styleUrl: './loan-list.css',
})
export class LoanList implements OnInit{

  private readonly loanApiService = inject(LoanApiService);

  loans = signal<LoanSummary[]>([]);
  loading = signal<boolean>(false);
  errorMessage = signal<string>('');

  ngOnInit(): void {
    this.loadLoans();
  }

  loadLoans(): void {
    this.loading.set(true);
    this.errorMessage.set('');

    this.loanApiService.getLoans().subscribe({
      next: (data) => {
        this.loans.set(data);
        this.loading.set(false);
      },
      error: (error) => {
        console.error(error);
        this.errorMessage.set('Failed to load loans from API.');
        this.loading.set(false);
      }
    });
  }

  getRateSeverity(rate: number): 'success' | 'warn' | 'danger' {
    if (rate < 4) return 'success';
    if (rate < 7) return 'warn';
    return 'danger';
  }
}
