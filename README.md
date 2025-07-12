# 🏥 Hospital Management System

A full-stack **Hospital Management System** built with:

- 🧠 **ASP.NET Core Web API** (Backend)
- 🌐 **Angular** (Frontend)
- 📊 Dashboard Analytics with **Chart.js**
- 📂 Excel/CSV Export (ClosedXML, CsvHelper)
- 🔐 JWT-based Authentication & Role-based Authorization

---

## 🔧 Technologies Used

| Layer         | Tech Stack                                    |
|---------------|-----------------------------------------------|
| Frontend      | Angular 17+, TypeScript, SCSS, Chart.js       |
| Backend       | ASP.NET Core 8, Entity Framework Core, LINQ   |
| Database      | SQL Server                                    |
| Security      | JWT, Role-based access (Admin/User)           |
| Dev Tools     | Visual Studio 2022, Postman, Git, GitHub      |
| Export/Reports| ClosedXML for Excel, CsvHelper for CSV        |

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
The backend will run on https://localhost:7064

🌐 Frontend Setup (Angular)
cd hospital-management
npm install
ng serve --open
The frontend will run on http://localhost:4200

🧪 Testing
✅ Integration testing with xUnit

Test patient & doctor endpoints

Login/auth test coverage (manual + automation)
  HospitalManagementSystem/
├── HospitalManagementAPI/           # ASP.NET Core backend
├── hospital-management/             # Angular frontend



🛡️ Security
JWT Token Authentication

Role-Based Access (Admin/User)

Secure endpoints with [Authorize]
