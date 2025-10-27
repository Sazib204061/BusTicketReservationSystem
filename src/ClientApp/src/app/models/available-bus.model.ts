export interface AvailableBus {
  busScheduleId: string;
  companyName: string;
  busName: string;
  busNumber: string;
  startTime: Date;
  arrivalTime: Date;
  seatsLeft: number;
  price: number;
  hasAC: boolean;
}
