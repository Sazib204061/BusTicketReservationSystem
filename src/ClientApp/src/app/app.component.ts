import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'Bus Ticket Reservation System';
  currentView: 'search' | 'seats' = 'search';

  constructor() {
    // Listen for navigation events between components
    window.addEventListener('navigateToSeats', () => {
      this.currentView = 'seats';
    });

    window.addEventListener('navigateToSearch', () => {
      this.currentView = 'search';
    });
  }

  navigateTo(view: 'search' | 'seats') {
    this.currentView = view;
  }
}
