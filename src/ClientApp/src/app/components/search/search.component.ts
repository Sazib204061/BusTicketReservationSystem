import { Component } from '@angular/core';
import { BusService } from '../../services/bus.service';
import { AvailableBus } from '../../models/available-bus.model';

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.css']
})
export class SearchComponent {
  from: string = '';
  to: string = '';
  journeyDate: string = '';

  availableBuses: AvailableBus[] = [];
  loading: boolean = false;
  error: string = '';

  cities: string[] = [
    'Dhaka', 'Chittagong', 'Rajshahi', 'Khulna', 'Sylhet',
    'Barisal', 'Rangpur', 'Dinajpur', 'Bogra', 'Comilla', 'Cox\'s Bazar'
  ];

  constructor(private busService: BusService) {
    // Set default date to tomorrow
    const tomorrow = new Date();
    tomorrow.setDate(tomorrow.getDate() + 1);
    this.journeyDate = tomorrow.toISOString().split('T')[0];
  }

  searchBuses() {
    if (!this.from || !this.to || !this.journeyDate) {
      this.error = 'Please fill in all fields';
      return;
    }

    if (this.from === this.to) {
      this.error = 'From and To cities cannot be the same';
      return;
    }

    this.loading = true;
    this.error = '';
    this.availableBuses = [];

    const journeyDate = new Date(this.journeyDate);

    this.busService.searchAvailableBuses(this.from, this.to, journeyDate)
      .subscribe({
        next: (buses) => {
          this.availableBuses = buses;
          this.loading = false;
        },
        error: (error) => {
          this.error = 'Error searching buses. Please try again.';
          this.loading = false;
          console.error('Search error:', error);
        }
      });
  }

  viewSeats(busScheduleId: string) {
    localStorage.setItem('selectedBusScheduleId', busScheduleId);
    window.dispatchEvent(new CustomEvent('navigateToSeats'));
  }

  swapCities() {
    [this.from, this.to] = [this.to, this.from];
  }
}
