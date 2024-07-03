
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

![Microsoft SQL Server (JetBrains)](https://github.com/HlibPavlyk/out-of-office-app/assets/135625402/c89bc6c4-7159-4e58-bcec-af8af8644b1d)

## User Instructions

### Initial Screen and Application Features

When you first open the application, you will see the initial screen. This screen introduces you to the application and its main features, helping you get familiar with the functionalities and navigation.

![image](https://github.com/HlibPavlyk/out-of-office-app/assets/135625402/038632c3-471f-467f-ac10-9454169d83ed)

### User Authentication

Next, you need to log in. Navigate to the login page. Please note that login is performed using a username in lowercase letters with no spaces, followed by @example.com, and the password is "password". For instance, if the user is Hlib Pavlyk, the login would be hlibpavlyk@example.com, and the password would be "password". Initially, there is one account for each role:

- Administrator: administrator@example.com
- HR Manager: hrmanager@example.com
- Project Manager: projectmanager@example.com
- Employee: employee@example.com

![image](https://github.com/HlibPavlyk/out-of-office-app/assets/135625402/34a3db43-a665-461c-9553-f360a345bae3)

### Employee Page

After logging in, you will be redirected to the Employee page. Here, you can see detailed information about each user. Additionally, you have the ability to perform various actions, which will be explained in the following sections.

![image](https://github.com/HlibPavlyk/out-of-office-app/assets/135625402/941208b0-761b-44b9-aaf3-4e67d9897a63)

### Adding a New Employee

Next, click on the button to add a new user and complete the necessary fields. Once the user is successfully added, you will be redirected back to the initial page. Here, you can sort users by column names and utilize the search field to find specific users.

![image](https://github.com/HlibPavlyk/out-of-office-app/assets/135625402/c0721573-78e5-47c2-ad8c-bc9bdf092b06)

![image](https://github.com/HlibPavlyk/out-of-office-app/assets/135625402/0b3e5b96-a0d4-4257-91b4-06332c097ccb)

### Employee Deactivation and Editing

Additionally, there are buttons available for deactivating and editing a user. These functions are similar to the previous user management functionalities, allowing you to update user information or deactivate their account as needed.

### Project Management and Assignment

Now, let's try logging in as a Project Manager. You'll notice that there are similar tabs available, but with additional functionality to assign a user to a specific project. Before we can assign a user, we need to create a project. Navigate to the "Projects" tab, click on "Add New Project," and complete the necessary details. Once the project is created, we can successfully assign an employee to this project.

![image](https://github.com/HlibPavlyk/out-of-office-app/assets/135625402/adfa7077-20ae-4639-bc43-95cbb7aa39b6)

![image](https://github.com/HlibPavlyk/out-of-office-app/assets/135625402/a272441d-6a7c-4822-a9c5-f55ab91ed8b1)

![image](https://github.com/HlibPavlyk/out-of-office-app/assets/135625402/9de71d81-218b-4ff0-9b94-e10c917b5db7)

![image](https://github.com/HlibPavlyk/out-of-office-app/assets/135625402/0b6f2e5b-a822-4ec5-b8b7-ef7155e95370)

### Project Dates and Details

For projects, their dates are set automatically based on the project's status. Additionally, we can update the project status and view detailed information about each project. This ensures that project timelines are managed efficiently and accurately.

![image](https://github.com/HlibPavlyk/out-of-office-app/assets/135625402/f31ab35b-010e-4210-a166-9815aabd7949)

### Leave Request for Hlib Pavlyk

Now, let's switch to our user Hlib Pavlyk and submit a leave request. Navigate to the appropriate section and create the request. While we can edit the request initially, it's important to note that once submitted, it will no longer be available for editing.


![image](https://github.com/HlibPavlyk/out-of-office-app/assets/135625402/9465b8f5-31f0-4acf-9d9f-0d747834a18e)

![image](https://github.com/HlibPavlyk/out-of-office-app/assets/135625402/71ca2f7a-9f68-46e6-af32-6eff40327ac3)

![image](https://github.com/HlibPavlyk/out-of-office-app/assets/135625402/6f4294ee-41ba-4594-b01c-a0bdf58d226d)

### Approve Leave Request as Project Manager

Next, switch to the project manager overseeing the project Hlib Pavlyk is assigned to. Approve Hlib's leave request by navigating to the detailed view of the request. Ensure that the number of leave days is updated and the status of the request is correctly reflected.

![image](https://github.com/HlibPavlyk/out-of-office-app/assets/135625402/df69fc43-2e64-41ad-b2b4-4692358bbbd6)

![image](https://github.com/HlibPavlyk/out-of-office-app/assets/135625402/130793f6-6164-4783-9df0-6ca5ad8e3350)

![image](https://github.com/HlibPavlyk/out-of-office-app/assets/135625402/cee2f2e0-e118-440e-8af8-f5e4cbce1497)

![image](https://github.com/HlibPavlyk/out-of-office-app/assets/135625402/6a71bd90-ea73-4247-960b-c05a14751046)

![image](https://github.com/HlibPavlyk/out-of-office-app/assets/135625402/006323ad-d909-411a-9a13-20cf67f6ea2b)


### Explore Additional Features

In addition to the core functionalities mentioned, our application offers a wide range of features. We encourage you to explore the application further and discover all the capabilities it provides.


## Conclusion

The Out Of Office Application is a powerful tool for managing employee schedules and project assignments. By following the steps above, you can set up and run the application locally for development or testing purposes.
