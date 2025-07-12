# 🏥 Hospital Management System

A full-stack **Hospital Management System** built with:

- 🧠 **ASP.NET Core Web API** (Backend)
- 🌐 **Angular** (Frontend)
- 📊 Dashboard Analytics with **Chart.js**
- 📂 Excel/CSV Export (ClosedXML, CsvHelper)
- 🔐 JWT-based Authentication & Role-based Authorization

---

## 🔧 Technologies Used

| Layer         | Tech Stack                                    |
|---------------|-----------------------------------------------|
| Frontend      | Angular 17+, TypeScript, SCSS, Chart.js       |
| Backend       | ASP.NET Core 8, Entity Framework Core, LINQ   |
| Database      | SQL Server                                    |
| Security      | JWT, Role-based access (Admin/User)           |
| Dev Tools     | Visual Studio 2022, Postman, Git, GitHub      |
| Export/Reports| ClosedXML for Excel, CsvHelper for CSV        |

---

## 📦 Features

### 👨‍⚕️ Doctors
- Add / Edit / Delete doctors
- Filter, sort, search, and paginate
- Export data to CSV and Excel

### 🧑‍🤝‍🧑 Patients
- Add / Edit / Delete patients
- Assign patients to doctors
- Filter, sort, search, and paginate

### 📊 Admin Dashboard
- Visual analytics using Chart.js (Bar, Pie)
- Real-time stats: Total Patients, Doctors, Patients per Doctor
- Date-based filtering (optional)

### 🔐 Authentication
- JWT-based secure login
- Admin-only endpoints (e.g., delete/update)
- Role-based dashboard and navigation

---

## ▶️ Getting Started

### ✅ Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [Node.js + Angular CLI](https://angular.io/guide/setup-local)
- SQL Server (or LocalDB)

## 📸 Dashboard Preview

### 💻 Admin Dashboard (Analytics Overview)
![Admin Dashboard - Charts](Charts.png)

### 📈 Patients Dashboard
![Patients Dashboard](<img width="1920" height="1080" alt="Patients DashBoard" src="https://github.com/user-attachments/assets/c80cad16-fc08-4b62-a3f0-80266ba40665" />)


### 👨‍⚕️ Doctors Dashboard
![Doctors Dashboard](<img width="1920" height="1080" alt="API2" src="https://github.com/user-attachments/assets/abd0ec1a-2a5f-416b-bac0-7d269bfac874" />)

### 🔑 Authentication Screens
#### Login
![Login Page](<img width="1920" height="1080" alt="Login" src="https://github.com/user-attachments/assets/24cf2fe8-564d-43f3-85a7-081ca1a79fee" />)

#### Register
![Register Page](Register.png)
<img width="1920" height="1080" alt="Register" src="https://github.com/user-attachments/assets/bb123ccb-e09e-411f-9350-8cdb1664f46b" />

### ⚙️ API Documentation (Swagger UI)
![API Documentation 1](API.png)
![API Documentation 2](API2.png)


  🙋‍♂️ Author
Amit Kushwanshi
👤 GitHub: @Amit78singh
✉️ Email: amit.kushwanshi0007@gmail.com

⭐️ Show your support
If you like this project, please ⭐️ the repo!

---

### 🧪 Backend Setup (ASP.NET Core)

```bash
cd HospitalManagementAPI
dotnet restore
dotnet ef database update
dotnet run



<img width="1920" height="1080" alt="Charts" src="https://github.com/user-attachments/assets/6f068bac-3783-4276-819c-be8bc4f5e32a" />
<img width="1920" height="1080" alt="API2" src="https://github.com/user-attachments/assets/abd0ec1a-2a5f-416b-bac0-7d269bfac874" />
<img width="1920" height="1080" alt="API" src="https://github.com/user-attachments/assets/c2c7cd5c-f9b5-44bd-8684-2f1db362affd" />
