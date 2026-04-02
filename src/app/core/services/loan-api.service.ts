import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import {
  LoanSummary,
  LoanPaymentResponse,
  SendLoanCalculationEmailRequest,
} from '../models/loan.model';
import { environment } from '../../../environments/environment.development';

@Injectable({
  providedIn: 'root',
})
export class LoanApiService {
  private readonly http = inject(HttpClient);
  // private readonly baseUrl = 'http://localhost/LoanServiceApp.Api/api/loans';
  //private readonly baseUrl = 'http://localhost:7071/api/loans';
  private readonly baseUrl = `${environment.AzureFunctionUrl}`

  getLoans(): Observable<LoanSummary[]> {
    return this.http.get<LoanSummary[]>(this.baseUrl);
  }

  getLoanById(id: number): Observable<LoanPaymentResponse> {
    return this.http.get<LoanPaymentResponse>(`${this.baseUrl}/${id}`);
  }

  sendLoanCalculationEmail(
    payload: SendLoanCalculationEmailRequest,
  ): Observable<{ message: string }> {
    return this.http.post<{ message: string }>(`${this.baseUrl}/sendEmail`, payload);
  }
}
