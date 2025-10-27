import { Component, OnInit } from '@angular/core';
import { BusService } from '../../services/bus.service';
import { SeatPlan, Seat, SeatStatus, BookSeatRequest, BookSeatResponse } from '../../models/seat.model';

@Component({
  selector: 'app-seat-selection',
  templateUrl: './seat-selection.component.html',
  styleUrls: ['./seat-selection.component.css']
})
export class SeatSelectionComponent implements OnInit {
  seatPlan: SeatPlan | null = null;
  loading: boolean = false;
  error: string = '';
  successMessage: string = '';

  selectedSeat: Seat | null = null;
  boardingPoint: string = '';
  droppingPoint: string = '';
  passengerName: string = '';
  mobileNumber: string = '';

  constructor(private busService: BusService) { }

  ngOnInit() {
    const busScheduleId = localStorage.getItem('selectedBusScheduleId');
    if (busScheduleId) {
      this.loadSeatPlan(busScheduleId);
    } else {
      this.error = 'No bus selected. Please search for buses first.';
    }

    window.addEventListener('navigateToSeats', () => {
      const newBusScheduleId = localStorage.getItem('selectedBusScheduleId');
      if (newBusScheduleId) {
        this.loadSeatPlan(newBusScheduleId);
      }
    });
  }

  loadSeatPlan(busScheduleId: string) {
    this.loading = true;
    this.error = '';
    this.seatPlan = null;
    this.resetSelection();

    this.busService.getSeatPlan(busScheduleId)
      .subscribe({
        next: (plan) => {
          this.seatPlan = plan;
          this.loading = false;
        },
        error: (error) => {
          this.error = 'Error loading seat plan. Please try again.';
          this.loading = false;
          console.error('Seat plan error:', error);
        }
      });
  }

  selectSeat(seat: Seat) {
    if (seat.status === SeatStatus.Available) {
      this.selectedSeat = seat;
    }
  }

  bookSeat() {
    if (!this.selectedSeat) {
      this.error = 'Please select a seat';
      return;
    }

    if (!this.passengerName || !this.mobileNumber || !this.boardingPoint || !this.droppingPoint) {
      this.error = 'Please fill in all required fields';
      return;
    }

    if (!this.seatPlan) return;

    const bookRequest: BookSeatRequest = {
      busScheduleId: this.seatPlan.busScheduleId,
      seatId: this.selectedSeat.seatId,
      passengerName: this.passengerName,
      mobileNumber: this.mobileNumber,
      boardingPoint: this.boardingPoint,
      droppingPoint: this.droppingPoint
    };

    this.loading = true;
    this.error = '';

    this.busService.bookSeat(bookRequest)
      .subscribe({
        next: (response: BookSeatResponse) => {
          this.loading = false;
          if (response.success) {
            this.successMessage = `Booking successful! Ticket Number: ${response.ticketNumber}`;
            this.resetSelection();
            this.loadSeatPlan(this.seatPlan!.busScheduleId);
          } else {
            this.error = response.message;
          }
        },
        error: (error) => {
          this.loading = false;
          this.error = 'Booking failed. Please try again.';
          console.error('Booking error:', error);
        }
      });
  }

  resetSelection() {
    this.selectedSeat = null;
    this.boardingPoint = '';
    this.droppingPoint = '';
    this.passengerName = '';
    this.mobileNumber = '';
    this.successMessage = '';
  }

  getSeatClass(seat: Seat): string {
    if (this.selectedSeat?.seatId === seat.seatId) {
      return 'seat-selected';
    }

    switch (seat.status) {
      case SeatStatus.Available:
        return 'seat-available';
      case SeatStatus.Booked:
        return 'seat-booked';
      case SeatStatus.Sold:
        return 'seat-sold';
      default:
        return 'seat-available';
    }
  }

  goBack() {
    this.seatPlan = null;
    this.resetSelection();
    window.dispatchEvent(new CustomEvent('navigateToSearch'));
  }
}
