
# 🛍️ Shoopi - E-Commerce Web Application

**Shoopi** is a simple and clean e-commerce web application built with **ASP.NET MVC** and **jQuery**. It provides essential online shopping features like product browsing, cart management, checkout, and user authentication.

---

## 🚀 Features

- 🧾 **Product Management** – Add, update, delete, and display products  
- 🛒 **Shopping Cart** – Add to cart, update quantity, remove items  
- 💳 **Checkout Process** – Basic order placement  
- 👤 **User Authentication** – Register, login, and manage roles , google login 
- 🔎 **Search & Filter** – Search by product name, category, and price  
- 📊 **Simple Admin Dashboard** – (Optional: extendable for admins)  

---

## 🛠️ Technologies Used

- **Backend**: ASP.NET MVC 5, C#  
- **Frontend**: HTML5, CSS3, JavaScript, jQuery  
- **Database**: SQL Server  
- **ORM**: Entity Framework  
- **UI Framework**: Bootstrap  
- **Other Tools**: AJAX, NuGet  

---

## 📦 Getting Started

### 1. Clone the repository

```bash
git clone https://github.com/MNTuas/Shoopi.git
```

### 2. Open in Visual Studio

- Open `Shoopi.sln` with **Visual Studio 2019** or newer.

### 3. Restore NuGet Packages

Go to:

```
Tools → NuGet Package Manager → Manage NuGet Packages for Solution
```

Then click **Restore**.

### 4. Configure SQL Server Connection

In `Web.config`, update your connection string:

```xml
<connectionStrings>
  <add name="DefaultConnection" connectionString="Data Source=.;Initial Catalog=ShoopiDB;Integrated Security=True" providerName="System.Data.SqlClient" />
</connectionStrings>
```

> ⚠️ Make sure your SQL Server instance is running and accessible.

### 5. Run the Application

- Press **F5** or go to `Debug → Start Debugging`

---

## 📊 Live Demo

🚧 Coming Soon...

You can deploy the project using:

- [Azure](https://azure.microsoft.com/)
- [Render](https://render.com/)
- [SmarterASP.NET](https://www.smarterasp.net/)

---

## 📁 Project Structure

```
Shoopi/
│
├── Controllers/        # MVC controllers
├── Models/             # Domain & view models
├── Views/              # Razor views (.cshtml)
├── Scripts/            # jQuery & JavaScript files
├── Content/            # CSS, fonts, etc.
├── Shoopi.sln          # Visual Studio solution
└── Web.config          # App configuration
```

---


## ✅ Notes

This is a demo project – feel free to extend it with:

- Order history  
- Admin panel  
- Payment gateway  
- Product images & categories  

> You can use **Database-First** or **Code-First** approach with Entity Framework.

---

## 📄 License

This project is licensed under the [MIT License](https://opensource.org/licenses/MIT).

---

## 🖼️ Screenshots
![image](https://github.com/user-attachments/assets/500e48e7-6164-4033-b0b4-f2bfb343e943)

![image](https://github.com/user-attachments/assets/734ee566-f521-43c9-bd36-f467707ead58)
