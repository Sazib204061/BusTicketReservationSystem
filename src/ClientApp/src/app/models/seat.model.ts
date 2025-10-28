export interface SeatPlan {
  busScheduleId: string;
  busName: string;
  companyName: string;
  seats: Seat[];
  boardingPoints: string[];
  droppingPoints: string[];
}

export interface Seat {
  seatId: string;
  seatNumber: string;
  row: number;
  column: number;
  status: SeatStatus;
}

export enum SeatStatus {
  Available = 1,
  Booked = 2,
  Sold = 3
}

export interface BookSeatRequest {
  busScheduleId: string;
  seatId: string;
  passengerName: string;
  mobileNumber: string;
  boardingPoint: string;
  droppingPoint: string;
}

export interface BookSeatResponse {
  success: boolean;
  message: string;
  ticketId: string;
  ticketNumber: string;
}
