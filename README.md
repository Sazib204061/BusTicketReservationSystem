Setup Guide:

  Prerequisites:
    1. .NET 9.0 SDK
    2. PostgreSQL
    3. Node.js 18+
    4. Angular CLI 17

  Database Setup: 
    1. Update connection string in appsettings.json: 
      "ConnectionStrings": {
        "DefaultConnection": "Host=localhost;Database=BusTicketReservation;Username=postgres;Password=Sazib123*#;Port=5432"
        },
      }
    2. Run Database Commands in Package Manager Console: (Update-Database) then migrations is applyed properly and create database and tables.

  Backend Setup (Visual Studio)
    1. Set WebApi as startup project
    2. Click Start button (or F5) to run
    3. API will run on: https://localhost:7000

  Frontend Setup
    1. Open new Visual Studio window
    2. File → Open → Folder
    3. Select ClientApp folder
    4. Open Terminal in VS: View → Terminal
    5. Run commands:
        npm install
        npm start
    6. Frontend running on: http://localhost:4200

Test Application
  1. Go to: http://localhost:4200
  2. Search: From Station → To Station → Select date then click Search Buses button.
  3. If bus is avilable then get list of bus with some information. Then click View Seats.
  4. Then you see all seat status and you can abel to book a seat.
  5. Finally book seats.

NB: Keep both Visual Studio windows running!


        
