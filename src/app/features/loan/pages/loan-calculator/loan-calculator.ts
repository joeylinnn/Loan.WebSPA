import { Component, inject, signal } from '@angular/core';
import { CommonModule, CurrencyPipe } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';

import { ButtonModule } from 'primeng/button';
import { CardModule } from 'primeng/card';
import { InputNumberModule } from 'primeng/inputnumber';
import { InputTextModule } from 'primeng/inputtext';
import { ToastModule } from 'primeng/toast';
import { MessageService } from 'primeng/api';

import { LoanCalculatorResult, SendLoanCalculationEmailRequest } from '../../../../core/models/loan.model';
import { LoanApiService } from '../../../../core/services/loan-api.service';

@Component({
  selector: 'app-loan-calculator',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    CardModule,
    ButtonModule,
    InputNumberModule,
    InputTextModule,
    ToastModule,
    CurrencyPipe
  ],
  providers: [MessageService],
  templateUrl: './loan-calculator.html',
  styleUrl: './loan-calculator.css'
})
export class LoanCalculator {
  private readonly fb = inject(FormBuilder);
  private readonly messageService = inject(MessageService);
  private readonly loanApiService = inject(LoanApiService);
  
  result = signal<LoanCalculatorResult | null>(null);
  sending = signal<boolean>(false);

  calculatorForm = this.fb.group({
    principal: [250000, [Validators.required, Validators.min(1)]],
    annualInterestRate: [6.25, [Validators.required, Validators.min(0)]],
    termYears: [30, [Validators.required, Validators.min(1)]],
    recipientEmail: ['']
  });


  calculateLoan(): void {
    if (this.calculatorForm.invalid) {
      this.calculatorForm.markAllAsTouched();
      return;
    }

    const principal = Number(this.calculatorForm.value.principal ?? 0);
    const annualInterestRate = Number(this.calculatorForm.value.annualInterestRate ?? 0);
    const termYears = Number(this.calculatorForm.value.termYears ?? 0);

    const monthlyRate = annualInterestRate / 100 / 12;
    const totalMonths = termYears * 12;

    let monthlyPayment = 0;

    if (monthlyRate === 0) {
      monthlyPayment = principal / totalMonths;
    } else {
      monthlyPayment =
        (principal * monthlyRate * Math.pow(1 + monthlyRate, totalMonths)) /
        (Math.pow(1 + monthlyRate, totalMonths) - 1);
    }

    const totalPayment = monthlyPayment * totalMonths;
    const totalInterest = totalPayment - principal;

    this.result.set({
      principal,
      annualInterestRate,
      termYears,
      monthlyPayment,
      totalPayment,
      totalInterest
    });
  }

sendEmail(): void {
    const result = this.result();
    const recipientEmail = this.calculatorForm.value.recipientEmail ?? '';

    if (!result) {
      this.messageService.add({
        severity: 'warn',
        summary: 'No calculation result',
        detail: 'Please calculate the loan first.'
      });
      return;
    }

    if (!recipientEmail) {
      this.messageService.add({
        severity: 'warn',
        summary: 'Recipient email required',
        detail: 'Please enter a valid email address.'
      });
      return;
    }

    const payload: SendLoanCalculationEmailRequest = {
      to: recipientEmail,
      principal: result.principal,
      annualInterestRate: result.annualInterestRate,
      termYears: result.termYears,
      monthlyPayment: result.monthlyPayment,
      totalPayment: result.totalPayment,
      totalInterest: result.totalInterest
    };

    this.sending.set(true);

    this.loanApiService.sendLoanCalculationEmail(payload).subscribe({
      next: (response) => {
        this.sending.set(false);
        this.messageService.add({
          severity: 'success',
          summary: 'Email sent',
          detail: response.message
        });
      },
      error: (error) => {
        this.sending.set(false);
        console.error(error);
        this.messageService.add({
          severity: 'error',
          summary: 'Send failed',
          detail: 'Unable to send the email.'
        });
      }
    });
  }
}