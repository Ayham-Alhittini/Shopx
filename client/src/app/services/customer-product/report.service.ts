import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ProductReport } from 'src/app/models/productReport';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ReportService {

  private baseUrl = environment.apiBase;

  constructor(private http: HttpClient) { }

  reportProduct(report: ProductReport) {
    return this.http.post(this.baseUrl + 'customer/report', report);
  }

  getReportReasons() {
    return this.http.get<string[]>(this.baseUrl + 'initialization/get-report-reasons');
  }

}
