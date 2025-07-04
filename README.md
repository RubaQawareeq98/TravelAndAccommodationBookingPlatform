# ✈️ Travel and Accommodation Booking Platform

The **Travel and Accommodation Booking Platform** is a comprehensive **RESTful API** built to simplify and streamline hotel booking and management. It offers a powerful set of features for both **administrators** and **users**, enabling efficient control over hotels, rooms, bookings, payments, and user accounts.

---

##  Key Features

### 1. User Authentication & Authorization
- **User Registration**: Allows users to create accounts with a unique email and a strong password.
- **User Login**: Enables secure access to user accounts using valid credentials.

---

### 2. Administration Interface
- Provides administrators with full access to manage system entities.
- Supports **searching**, **creating**, **updating**, and **deleting**:
  - Users
  - Hotels
  - Rooms
  - Bookings
  - Reviews

---

### 3. Hotel & Room Management
- Add hotels with detailed information.
- Associate hotels with multiple room categories and rooms.
- Manage room availability and pricing dynamically.

---

### 4. Email Notifications
- Sends confirmation emails to users after booking is successfully created.
- Ensures users stay informed about their reservations.

---

### 5. PDF Invoice Generation (via QuestPDF)
- Generates a **PDF invoice** after a booking is made.
- Includes full booking details and total cost **before and after discounts**.

---

### 6. Logging & Monitoring (with Serilog + Elasticsearch)
- Implements structured logging using **Serilog**.
- Optionally integrates with **Elasticsearch** for advanced log analysis.
- Logs can be visualized in **Kibana** for monitoring and debugging.

---

### 7. Concurrency-Safe Booking System
- Handles concurrency during room bookings.
- Prevents multiple users from booking the same room at the same time.

---

### 8. Global Filtering, Sorting, and Pagination (Sieve)
- Integrates the **Sieve** library to provide:
  - Flexible filtering
  - Sorting
  - Pagination across all endpoints

---

### 9. Unit & Integration Testing
- Ensures system stability with:
  - **Unit tests** for individual components
  - **Integration tests** for end-to-end API behavior
- Built using **xUnit** and **Moq**

---

### 10. Result Pattern for Clean Error Handling
- Implements a **Result pattern** to encapsulate success/failure of operations.
- Avoids throwing exceptions for flow control.
- Each service method returns a clear outcome with either:
  - `Success`: with data or confirmation
  - `Failure`: with error message or domain-specific error

## Tech Stack

- **.NET 8 Web API**
- **Entity Framework Core**
- **SQL Server**
- **Serilog** (for logging)
- **QuestPDF** (for invoice generation)
- **Sieve** (for filtering/sorting/pagination)
- **Elasticsearch** + **Kibana** (optional log visualization)
- **Brevo** (for email messages)
- **Cloudinary** (for Images)
---

## Getting Started

1. **Clone the repository:**

   ```bash
   git clone https://github.com/RubaQawareeq98/TravelAndAccommodationBookingPlatform.git
   
2. **Navigate to the project directory:**
   ```bash
   cd TravelAndAccommodationBookingPlatform
   
3. **Run the project:**
   Make sure to have & run the Docker descktop.

   ```bash
   docker-compose up --build
###
  The API will be accessable using http://localhost:8080.
  
  The swagger UI will open automatically where you can try and explore the endpoints or you can open it using http://localhost:8080/swagger/index.html.
  
  The Kibana elastic search will be accessable using http://localhost:5601

## 📬 Contact

[![Email](https://img.shields.io/badge/Email-rubaqawareeq2@gmail.com-blue?style=flat&logo=gmail&logoColor=white)](mailto:rubaqawareeq2@gmail.com)
[![LinkedIn](https://img.shields.io/badge/LinkedIn-RubaQawareeq-blue?style=flat&logo=linkedin)](https://www.linkedin.com/in/ruba-qawareeq-919b7b24a/)

