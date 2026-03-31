export interface LoanSummary {
  loanId: number;
  principal: number;
  annualInterestRate: number;
  termYears: number;
}

export interface LoanPaymentResponse {
  loanId: number;
  principal: number;
  annualInterestRate: number;
  termYears: number;
  monthlyPayment: number;
}

export interface LoanCalculatorResult {
  principal: number;
  annualInterestRate: number;
  termYears: number;
  monthlyPayment: number;
  totalPayment: number;
  totalInterest: number;
}

export interface SendLoanCalculationEmailRequest {
  to: string;
  principal: number;
  annualInterestRate: number;
  termYears: number;
  monthlyPayment: number;
  totalPayment: number;
  totalInterest: number;
}