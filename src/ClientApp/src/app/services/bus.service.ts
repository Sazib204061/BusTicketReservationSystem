import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AvailableBus } from '../models/available-bus.model';
import { SeatPlan, BookSeatRequest, BookSeatResponse } from '../models/seat.model';

@Injectable({
  providedIn: 'root'
})
export class BusService {
  private apiUrl = '/api';

  constructor(private http: HttpClient) { }

  searchAvailableBuses(from: string, to: string, journeyDate: Date): Observable<AvailableBus[]> {
    const params = new HttpParams()
      .set('from', from)
      .set('to', to)
      .set('journeyDate', journeyDate.toISOString());

    return this.http.get<AvailableBus[]>(`${this.apiUrl}/search/available-buses`, { params });
  }

  getSeatPlan(busScheduleId: string): Observable<SeatPlan> {
    return this.http.get<SeatPlan>(`${this.apiUrl}/booking/seat-plan/${busScheduleId}`);
  }

  bookSeat(bookRequest: BookSeatRequest): Observable<BookSeatResponse> {
    return this.http.post<BookSeatResponse>(`${this.apiUrl}/booking/book-seat`, bookRequest);
  }
}
