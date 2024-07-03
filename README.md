
# Out Of Office Application

## About the Application

The Out Of Office Application is designed to streamline the management of employee out-of-office schedules, approval requests, and project assignments. It provides a comprehensive system for HR managers and administrators to add new employees, manage existing schedules, handle approval requests, and assign employees to projects efficiently. The application offers a user-friendly interface for viewing a calendar of all out of office schedules and simplifies scheduling and project management tasks.

## Features

- **Employee Management:** Add new employees to the system and manage their information.
- **Schedule Management:** Manage and view out-of-office schedules for all employees.
- **Approval Requests:** View and manage approval requests for out-of-office schedules.
- **Project Assignments:** Assign employees to projects or unassign them as needed.
- **User-friendly Interface:** A clean and intuitive interface for easy navigation and usage.

## How to Run the Application Locally

To run the Out Of Office Application locally, both the client-side and server-side components need to be set up and run. Follow these steps to get started:

### Prerequisites

Ensure you have the following installed:
- Node.js (Preferably the latest LTS version)
- Angular CLI
- .NET 8.0 SDK

### Client-Side Setup

1. **Clone the Repository**

   First, clone the repository to your local machine using Git:

   ```bash
   git clone https://github.com/HlibPavlyk/out-of-office-app.git
   ```

2. **Navigate to the Client Application Directory**

   Change directory to the client application:

   ```bash
   cd out-of-office-app\src\OutOfOfficeApp.Client
   ```

3. **Install Dependencies**

   Install the necessary dependencies:

   ```bash
   npm install
   ```

4. **Run the Client Application**

   Start the application:

   ```bash
   ng serve
   ```

5. **Access the Application**

   Open your web browser and go to `http://localhost:4200` to view the application.

### Server-Side Setup

1. **Navigate to the Server Application Directory**

   If you're not already there, change directory to the server application:

   ```bash
   cd out-of-office-app
   ```

2. **Restore Dependencies**

   Restore the .NET dependencies:

   ```bash
   dotnet restore
   ```

3. **Move to the Server Application Directory**

   Change directory to the server application:

   ```bash
   cd out-of-office-app\src\OutOfOfficeApp.API
   ```

4. **Run the Server Application**

   Start the server:

   ```bash
   dotnet run --urls "https://localhost:7082"
   ```

5. **Server Running**

   The server will start, and the API will be available at `https://localhost:7082`.

## Database Schema

## User Instructions

## Conclusion

The Out Of Office Application is a powerful tool for managing employee schedules and project assignments. By following the steps above, you can set up and run the application locally for development or testing purposes.
