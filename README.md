# SPARK 🔥 — Resume & Portfolio Builder Powered by AI

**SPARK** is a modern, AI-assisted resume and portfolio builder built with **.NET Core MVC**, **Entity Framework**, and  **OpenAI API**. It empowers users to craft professional resumes and  portfolios with ease, combining manual customization and AI-generated content in one seamless platform.

---

## ✨ Key Features

- ✍️ **AI-Assisted Resume & Portfolio Builder**  
  Create content manually or generate it using OpenAI for tailored, professional results.

- 👁️‍🗨️ **PDF Export**  
  Instantly export resumes in PDF format.

- 🌐 **Public Portfolio and Resume Sharing**  
  Publish personal portfolios and Resumes with flexible visibility options.

- 🔐 **Secure User Authentication**  
  Role-based access control for users and admins with account protection features.

- 📊 **Admin Dashboard**  
  View platform-wide stats and manage users, resumes, and portfolios with full oversight and control.

## 🛠 Technologies Used

- **.NET Core MVC** (C#)
- **Entity Framework Core**
- **ASP.NET Core Identity**
- **OpenAI API**
- **Bootstrap 5**
- **Razor Views**
- **SQL Server**  

---

## 📸 Screenshots

- **🏠 Home Page**  
  ![Home Page](screenshots/home.png)

- **ℹ️ About Page**  
  ![About Page](screenshots/about.png)

  - **ℹ️ Profile Page**  
  ![About Page](screenshots/profile.png)

- **📝 Manual Resume Creation**  
  ![Manual Create](screenshots/manual-Rcreate.png) 

- **🤖 AI-Assisted Resume Creation**  
  ![AI Generate](screenshots/ai-Rgenerate.png)

  - **🛠 Edit Resume Page**  
  ![Edit Resume](screenshots/edit-resume.png)

- **🧾 Resume List Page (Private View)**  
  ![Resume Page](screenshots/resume.png)
  
- **🧾 Resume Details Page (Public/Private View)**  
  ![Resume Details](screenshots/resumeD.png)

   - **📝 Manual Portfolio Creation**  
  ![Manual Create](screenshots/manual-Pcreate.png)

  - **🤖 AI-Assisted Portfolio Creation**  
  ![AI Generate](screenshots/ai-Pgenerate.png)

- **🛠 Edit Portfolio Page**  
  ![Edit Portfolio](screenshots/edit-portfolio.png)

  - **🌐 Portfolio List Page (Private View)**  
  ![Portfolio Page](screenshots/portfolio.png)

  - **🌐 Portfolio Details Page (Public/Private View)**  
  ![Portfolio Details](screenshots/portfolioD.png)

- **👤 Dashboard Page (Admin only)**  
  ![Dashboard](screenshots/dashboard.png)


- **👤 Manage User Page (Admin only)**  
  ![Manage User](screenshots/manage-user.png)


- **👤 Add User Page (Admin only)**  
  ![Add User](screenshots/add-user.png)

- **✏️ Edit User Page (Admin only)**  
  ![Edit User](screenshots/edit-user.png)

- **🔍 View User Profile Page(Admin only)**  
  ![View Profile](screenshots/view-user.png)


- **🚫 Access Denied Page**  
  ![Access Denied](screenshots/access-denied.png)

  - ** Login Page**  
  ![Register](screenshots/login.png)

- ** Register Page**  
  ![Login](screenshots/Register.png)

---

## 📂 Pages & Routing Overview

| Page                        | Description                                     | Access Level         |
|-----------------------------|-------------------------------------------------|----------------------|
| **Home Page**               | Landing page with website overview              | Public               |
| **About Page**              | Description of purpose, vision, and contact     | Public               |
| **Access Denied Page**      | Displayed when unauthorized access is attempted | Public (System)      |
| **Create Resume (Manual)**  | Manual form to build a resume                   | Authenticated Users  |
| **Create Resume (AI)**      | Uses OpenAI API to generate content             | Authenticated Users  |
| **Edit Resume**             | Modify saved resumes                            | Authenticated Users  |
| **Resume Page**             | View List of resumes                            |Private               |
| **Resume Details**          | View  resume details                            |Public/Private        |
| **Create Portfolio(Manual)**| Manual form to build a portfolio                | Authenticated Users  |
| **Create Portfolio(AI)**    | Uses OpenAI API to generate content             | Authenticated Users  |
| **Edit Portfolio**          | Update portfolio projects/content               | Authenticated Users  |
| **Portfolio Page**          | View List of portfolios                         | Private              |
| **Portfolio Details**       | View  portfolio details                         |Public/Private        |
| **Add/Edit/View User**      | Admin panel for user management                 | Admin Only           |
| **User Profile**            | View user info                                  | User Only            |

---

