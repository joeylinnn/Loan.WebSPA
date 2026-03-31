import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule, CurrencyPipe } from '@angular/common';
import { ActivatedRoute, RouterLink } from '@angular/router';

import { CardModule } from 'primeng/card';
import { ButtonModule } from 'primeng/button';

import { LoanApiService } from '../../../../core/services/loan-api.service';
import { LoanPaymentResponse } from '../../../../core/models/loan.model';

@Component({
  selector: 'app-loan-detail',
  standalone: true,
  imports: [
    CommonModule,
    RouterLink,
    CardModule,
    ButtonModule,
    CurrencyPipe
  ],
  templateUrl: './loan-detail.html',
  styleUrl: './loan-detail.css'
})
export class LoanDetail implements OnInit {
  private readonly route = inject(ActivatedRoute);
  private readonly loanApiService = inject(LoanApiService);

  loan = signal<LoanPaymentResponse | null>(null);
  loading = signal<boolean>(false);
  errorMessage = signal<string>('');

  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));

    if (!id) {
      this.errorMessage.set('Invalid loan id.');
      return;
    }

    this.loading.set(true);

    this.loanApiService.getLoanById(id).subscribe({
      next: (data) => {
        this.loan.set(data);
        this.loading.set(false);
      },
      error: (error) => {
        console.error(error);
        this.errorMessage.set('Failed to load loan details.');
        this.loading.set(false);
      }
    });
  }
}