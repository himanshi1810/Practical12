# Practical12
# Employee Database Setup

This project contains SQL scripts to create and populate two tables: **Employee** and **Employee1**.

## 📌 Table: Employee



### **Insert Data For Task 1**
```sql
CREATE TABLE Employee (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    FirstName VARCHAR(50) NOT NULL,
    MiddleName VARCHAR(50) NULL,
    LastName VARCHAR(50) NOT NULL,
    DOB DATE NOT NULL,
    Address VARCHAR(100) NULL
);

INSERT INTO Employee (FirstName, MiddleName, LastName, DOB, Address) VALUES 
('John', 'A', 'Doe', '1990-05-21', 'New York'),
('Alice', NULL, 'Smith', '1985-08-15', 'Los Angeles'),
('Bob', 'C', 'Williams', '1992-02-10', 'Chicago'),
('David', 'M', 'Brown', '1988-11-25', 'Houston'),
('Emma', NULL, 'Johnson', '1995-07-19', 'San Francisco'),
('Michael', 'J', 'Miller', '1983-06-05', 'Boston'),
('Sophia', 'R', 'Davis', '1991-09-30', 'Seattle'),
('Liam', 'T', 'Wilson', '1994-04-10', 'Dallas'),
('Olivia', NULL, 'Anderson', '1989-03-14', 'Atlanta'),
('Ethan', 'K', 'Thomas', '1996-12-20', 'Denver');
```
### **Insert Data For Task 2**
```sql

CREATE TABLE Employee1 (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    FirstName VARCHAR(50) NOT NULL,
    MiddleName VARCHAR(50) NULL,
    LastName VARCHAR(50) NOT NULL,
    DOB DATE NOT NULL,
    MobileNumber VARCHAR(10) NOT NULL,
    Address VARCHAR(100) NULL,
    Salary DECIMAL(10,2) NOT NULL
);
INSERT INTO Employee1 (FirstName, MiddleName, LastName, DOB, MobileNumber, Address, Salary) VALUES 
('John', 'A', 'Doe', '1990-05-21', '9876543210', 'New York', 50000.00),
('Alice', NULL, 'Smith', '1985-08-15', '1234567890', 'Los Angeles', 60000.00),
('Bob', 'C', 'Williams', '1992-02-10', '5556667777', 'Chicago', 55000.00),
('David', 'M', 'Brown', '1988-11-25', '9988776655', 'Houston', 70000.00),
('Emma', NULL, 'Johnson', '1995-07-19', '7788994455', 'San Francisco', 62000.00),
('Michael', 'J', 'Miller', '1983-06-05', '6655443322', 'Boston', 80000.00),
('Sophia', 'R', 'Davis', '1991-09-30', '8899776655', 'Seattle', 48000.00),
('Liam', 'T', 'Wilson', '1994-04-10', '1122334455', 'Dallas', 53000.00),
('Olivia', NULL, 'Anderson', '1989-03-14', '2233445566', 'Atlanta', 75000.00),
('Ethan', 'K', 'Thomas', '1996-12-20', '3344556677', 'Denver', 67000.00);

```
### **Insert Data For Task 3**
```sql

CREATE TABLE Designation (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Designation VARCHAR(50) NOT NULL
);

CREATE TABLE Employee2 (
    Id INT PRIMARY KEY IDENTITY(1,1),
    FirstName VARCHAR(50) NOT NULL,
    MiddleName VARCHAR(50) NULL,
    LastName VARCHAR(50) NOT NULL,
    DOB DATE NOT NULL,
    MobileNumber VARCHAR(10) NOT NULL,
    Address VARCHAR(100) NULL,
    Salary DECIMAL(10,2) NOT NULL,
    DesignationId INT NULL,
    FOREIGN KEY (DesignationId) REFERENCES Designation(Id)
);
INSERT INTO Designation (Designation) 
VALUES ('Software Engineer'), ('Project Manager'), ('HR');

INSERT INTO Employee2 (FirstName, MiddleName, LastName, DOB, MobileNumber, Address, Salary, DesignationId) 
VALUES 
('John', 'A', 'Doe', '1990-05-10', '9876543210', '123 Street, NY', 75000, 1),
('Jane', NULL, 'Smith', '1985-08-15', '9123456789', '456 Road, LA', 90000, 2),
('Mike', 'B', 'Johnson', '1992-03-22', '9988776655', '789 Avenue, TX', 80000, 1),
('Sara', NULL, 'Wilson', '1995-07-18', '9776655443', NULL, 70000, 1);


CREATE VIEW vw_EmployeeDetails AS
SELECT 
    E.Id AS EmployeeId, E.FirstName, E.MiddleName, E.LastName,
    D.Designation, E.DOB, E.MobileNumber, E.Address, E.Salary
FROM Employee2 E
LEFT JOIN Designation D ON E.DesignationId = D.Id;

CREATE PROCEDURE sp_InsertDesignation
    @Designation VARCHAR(50)
AS
BEGIN
    INSERT INTO Designation (Designation) VALUES (@Designation);
END

CREATE PROCEDURE sp_InsertEmployee
    @FirstName VARCHAR(50),
    @MiddleName VARCHAR(50),
    @LastName VARCHAR(50),
    @DOB DATE,
    @MobileNumber VARCHAR(10),
    @Address VARCHAR(100),
    @Salary DECIMAL(18,2),
    @DesignationId INT
AS
BEGIN
    INSERT INTO Employee2 (FirstName, MiddleName, LastName, DOB, MobileNumber, Address, Salary, DesignationId)
    VALUES (@FirstName, @MiddleName, @LastName, @DOB, @MobileNumber, @Address, @Salary, @DesignationId);
END
CREATE PROCEDURE sp_GetEmployeesByDesignation
    @DesignationId INT
AS
BEGIN
    SELECT 
        E.Id AS EmployeeId,
        E.FirstName,
        E.MiddleName,
        E.LastName,
        E.DOB,
        E.MobileNumber,
        E.Address,
        E.Salary,
        D.Designation
    FROM Employee2 E
    INNER JOIN Designation D ON E.DesignationId = D.Id
    WHERE E.DesignationId = @DesignationId;
END
CREATE PROCEDURE sp_GetAllEmployees  
AS  
BEGIN  
    SELECT  
        E.Id,  
        E.FirstName,  
        E.MiddleName,  
        E.LastName,  
        D.Designation,  
        E.DOB,  
        E.MobileNumber,  
        E.Address,  
        E.Salary  
    FROM Employee2 E  
    INNER JOIN Designation D ON E.DesignationId = D.Id  
    ORDER BY E.DOB;  
END;


