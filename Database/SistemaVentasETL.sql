USE master;
GO

IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'SistemaVentasETL')
BEGIN
    CREATE DATABASE SistemaVentasETL
    ON PRIMARY (
        NAME = N'SistemaVentasETL_Data',
        FILENAME = N'C:\MisBasesDeDatosVentasETL\SistemaVentasETL_Data.mdf',
        SIZE = 50MB,
        MAXSIZE = UNLIMITED,
        FILEGROWTH = 10MB
    )
    LOG ON (
        NAME = N'SistemaVentasETL_Log',
        FILENAME = N'C:\MisBasesDeDatosVentasETL\SistemaVentasETL_Log.ldf',
        SIZE = 10MB,
        MAXSIZE = 2GB,
        FILEGROWTH = 5MB
    );
END
GO

USE SistemaVentasETL;
GO

CREATE TABLE Categories (
    CategoryID      INT IDENTITY(1,1) NOT NULL,
    CategoryName    NVARCHAR(50)      NOT NULL,
    CONSTRAINT PK_Categories PRIMARY KEY (CategoryID)
);
GO

CREATE TABLE Products (
    ProductID       INT IDENTITY(1,1) NOT NULL,
    ProductName     NVARCHAR(100)     NOT NULL,
    CategoryID      INT               NOT NULL,
    Price           DECIMAL(10,2)     NOT NULL,
    Stock           INT               NOT NULL,
    CONSTRAINT PK_Products PRIMARY KEY (ProductID),
    CONSTRAINT FK_Products_Categories FOREIGN KEY (CategoryID)
        REFERENCES Categories (CategoryID),
    CONSTRAINT CK_Products_Price CHECK (Price >= 0),
    CONSTRAINT CK_Products_Stock CHECK (Stock >= 0)
);
GO

CREATE TABLE Countries (
    CountryID       INT IDENTITY(1,1) NOT NULL,
    CountryName     NVARCHAR(50)      NOT NULL,
    CONSTRAINT PK_Countries PRIMARY KEY (CountryID)
);
GO

CREATE TABLE Cities (
    CityID          INT IDENTITY(1,1) NOT NULL,
    CityName        NVARCHAR(50)      NOT NULL,
    CountryID       INT               NOT NULL,
    CONSTRAINT PK_Cities PRIMARY KEY (CityID),
    CONSTRAINT FK_Cities_Countries FOREIGN KEY (CountryID)
        REFERENCES Countries (CountryID)
);
GO

CREATE TABLE Customers (
    CustomerID      INT IDENTITY(1,1) NOT NULL,
    FirstName       NVARCHAR(50)      NOT NULL,
    LastName        NVARCHAR(50)      NOT NULL,
    Email           VARCHAR(100)      NOT NULL,
    Phone           VARCHAR(20)       NULL,
    CityID          INT               NOT NULL,
    CONSTRAINT PK_Customers PRIMARY KEY (CustomerID),
    CONSTRAINT FK_Customers_Cities FOREIGN KEY (CityID)
        REFERENCES Cities (CityID),
    CONSTRAINT UQ_Customers_Email UNIQUE (Email)
);
GO

CREATE TABLE OrderStatus (
    StatusID        INT IDENTITY(1,1) NOT NULL,
    StatusName      NVARCHAR(30)      NOT NULL,
    CONSTRAINT PK_OrderStatus PRIMARY KEY (StatusID)
);
GO

CREATE TABLE Orders (
    OrderID         INT IDENTITY(1,1) NOT NULL,
    CustomerID      INT               NOT NULL,
    StatusID        INT               NOT NULL,
    OrderDate       DATETIME          NOT NULL DEFAULT GETDATE(),
    CONSTRAINT PK_Orders PRIMARY KEY (OrderID),
    CONSTRAINT FK_Orders_Customers FOREIGN KEY (CustomerID)
        REFERENCES Customers (CustomerID),
    CONSTRAINT FK_Orders_OrderStatus FOREIGN KEY (StatusID)
        REFERENCES OrderStatus (StatusID)
);
GO

CREATE TABLE Order_Details (
    DetailID        INT IDENTITY(1,1) NOT NULL,
    OrderID         INT               NOT NULL,
    ProductID       INT               NOT NULL,
    Quantity        INT               NOT NULL,
    UnitPrice       DECIMAL(10,2)     NOT NULL,
    TotalPrice      AS (Quantity * UnitPrice) PERSISTED,
    CONSTRAINT PK_OrderDetails PRIMARY KEY (DetailID),
    CONSTRAINT FK_OrderDetails_Orders FOREIGN KEY (OrderID)
        REFERENCES Orders (OrderID),
    CONSTRAINT FK_OrderDetails_Products FOREIGN KEY (ProductID)
        REFERENCES Products (ProductID),
    CONSTRAINT CK_OrderDetails_Quantity CHECK (Quantity > 0),
    CONSTRAINT CK_OrderDetails_UnitPrice CHECK (UnitPrice >= 0)
);
GO

SET IDENTITY_INSERT [Categories] ON;
INSERT INTO [Categories] (CategoryID, CategoryName) VALUES (1, 'Tecnologia');
INSERT INTO [Categories] (CategoryID, CategoryName) VALUES (2, 'Mobiliario');
SET IDENTITY_INSERT [Categories] OFF;
GO

SET IDENTITY_INSERT [Products] ON;
INSERT INTO [Products] (ProductID, ProductName, CategoryID, Price, Stock) VALUES (1, 'Laptop HP Pavilion', 1, 650.00, 25);
INSERT INTO [Products] (ProductID, ProductName, CategoryID, Price, Stock) VALUES (2, 'Mouse Inalambrico Logitech', 1, 15.50, 100);
INSERT INTO [Products] (ProductID, ProductName, CategoryID, Price, Stock) VALUES (3, 'Teclado Mecanico RGB', 1, 45.99, 60);
INSERT INTO [Products] (ProductID, ProductName, CategoryID, Price, Stock) VALUES (4, 'Monitor Samsung 24 pulgadas', 1, 180.00, 30);
INSERT INTO [Products] (ProductID, ProductName, CategoryID, Price, Stock) VALUES (5, 'Silla Ergonomica Oficina', 2, 220.00, 15);
INSERT INTO [Products] (ProductID, ProductName, CategoryID, Price, Stock) VALUES (6, 'Escritorio de Madera', 2, 150.00, 10);
INSERT INTO [Products] (ProductID, ProductName, CategoryID, Price, Stock) VALUES (7, 'Impresora Epson L3250', 1, 190.00, 20);
INSERT INTO [Products] (ProductID, ProductName, CategoryID, Price, Stock) VALUES (8, 'Camara Web Full HD', 1, 35.00, 40);
INSERT INTO [Products] (ProductID, ProductName, CategoryID, Price, Stock) VALUES (9, 'Auriculares Bluetooth', 1, 28.75, 80);
INSERT INTO [Products] (ProductID, ProductName, CategoryID, Price, Stock) VALUES (10, 'Lampara de Escritorio LED', 2, 22.00, 50);
INSERT INTO [Products] (ProductID, ProductName, CategoryID, Price, Stock) VALUES (11, 'Pizarra Blanca', 2, 60.00, 12);
SET IDENTITY_INSERT [Products] OFF;
GO

SET IDENTITY_INSERT [Countries] ON;
INSERT INTO [Countries] (CountryID, CountryName) VALUES (1, 'Republica Dominicana');
SET IDENTITY_INSERT [Countries] OFF;
GO

SET IDENTITY_INSERT [Cities] ON;
INSERT INTO [Cities] (CityID, CityName, CountryID) VALUES (1, 'Santo Domingo', 1);
INSERT INTO [Cities] (CityID, CityName, CountryID) VALUES (2, 'Santiago', 1);
INSERT INTO [Cities] (CityID, CityName, CountryID) VALUES (3, 'La Vega', 1);
INSERT INTO [Cities] (CityID, CityName, CountryID) VALUES (4, 'Puerto Plata', 1);
INSERT INTO [Cities] (CityID, CityName, CountryID) VALUES (5, 'Bavaro', 1);
SET IDENTITY_INSERT [Cities] OFF;
GO

SET IDENTITY_INSERT [Customers] ON;
INSERT INTO [Customers] (CustomerID, FirstName, LastName, Email, Phone, CityID) VALUES (1, 'Juan', 'Perez', 'juan.perez@email.com', '8091234567', 1);
INSERT INTO [Customers] (CustomerID, FirstName, LastName, Email, Phone, CityID) VALUES (2, 'Maria', 'Gomez', 'maria.gomez@email.com', '8092345678', 2);
INSERT INTO [Customers] (CustomerID, FirstName, LastName, Email, Phone, CityID) VALUES (3, 'Carlos', 'Rodriguez', 'carlos.rodriguez@email.com', '8093456789', 1);
INSERT INTO [Customers] (CustomerID, FirstName, LastName, Email, Phone, CityID) VALUES (4, 'Ana', 'Martinez', 'ana.martinez@email.com', '8094567890', 3);
INSERT INTO [Customers] (CustomerID, FirstName, LastName, Email, Phone, CityID) VALUES (5, 'Pedro', 'Sanchez', 'pedro.sanchez@email.com', NULL, 4);
INSERT INTO [Customers] (CustomerID, FirstName, LastName, Email, Phone, CityID) VALUES (6, 'Laura', 'Fernandez', 'laura.fernandez@email.com', '8095678901', 2);
INSERT INTO [Customers] (CustomerID, FirstName, LastName, Email, Phone, CityID) VALUES (7, 'Jose', 'Ramirez', 'jose.ramirez@email.com', '8096789012', 1);
INSERT INTO [Customers] (CustomerID, FirstName, LastName, Email, Phone, CityID) VALUES (8, 'Sofia', 'Diaz', 'sofia.diaz@email.com', '8097890123', 5);
INSERT INTO [Customers] (CustomerID, FirstName, LastName, Email, Phone, CityID) VALUES (9, 'Miguel', 'Torres', 'miguel.torres@email.com', '8098901234', 1);
INSERT INTO [Customers] (CustomerID, FirstName, LastName, Email, Phone, CityID) VALUES (10, 'Elena', 'Castillo', 'elena.castillo@email.com', '8099012345', 2);
SET IDENTITY_INSERT [Customers] OFF;
GO

SET IDENTITY_INSERT [OrderStatus] ON;
INSERT INTO [OrderStatus] (StatusID, StatusName) VALUES (1, 'Completado');
INSERT INTO [OrderStatus] (StatusID, StatusName) VALUES (2, 'Pendiente');
INSERT INTO [OrderStatus] (StatusID, StatusName) VALUES (3, 'Cancelado');
SET IDENTITY_INSERT [OrderStatus] OFF;
GO

SET IDENTITY_INSERT [Orders] ON;
INSERT INTO [Orders] (OrderID, CustomerID, StatusID, OrderDate) VALUES (1, 1, 1, '2026-06-01 00:00:00');
INSERT INTO [Orders] (OrderID, CustomerID, StatusID, OrderDate) VALUES (2, 2, 2, '2026-06-02 00:00:00');
INSERT INTO [Orders] (OrderID, CustomerID, StatusID, OrderDate) VALUES (3, 3, 1, '2026-06-03 00:00:00');
INSERT INTO [Orders] (OrderID, CustomerID, StatusID, OrderDate) VALUES (4, 4, 3, '2026-06-04 00:00:00');
INSERT INTO [Orders] (OrderID, CustomerID, StatusID, OrderDate) VALUES (5, 6, 1, '2026-06-06 00:00:00');
INSERT INTO [Orders] (OrderID, CustomerID, StatusID, OrderDate) VALUES (6, 7, 1, '2026-06-07 00:00:00');
INSERT INTO [Orders] (OrderID, CustomerID, StatusID, OrderDate) VALUES (7, 9, 1, '2026-06-09 00:00:00');
INSERT INTO [Orders] (OrderID, CustomerID, StatusID, OrderDate) VALUES (8, 10, 1, '2026-06-10 00:00:00');
INSERT INTO [Orders] (OrderID, CustomerID, StatusID, OrderDate) VALUES (9, 1, 1, '2026-06-01 00:00:00');
INSERT INTO [Orders] (OrderID, CustomerID, StatusID, OrderDate) VALUES (10, 2, 2, '2026-06-02 00:00:00');
INSERT INTO [Orders] (OrderID, CustomerID, StatusID, OrderDate) VALUES (11, 3, 1, '2026-06-03 00:00:00');
INSERT INTO [Orders] (OrderID, CustomerID, StatusID, OrderDate) VALUES (12, 4, 3, '2026-06-04 00:00:00');
INSERT INTO [Orders] (OrderID, CustomerID, StatusID, OrderDate) VALUES (13, 6, 1, '2026-06-06 00:00:00');
INSERT INTO [Orders] (OrderID, CustomerID, StatusID, OrderDate) VALUES (14, 7, 1, '2026-06-07 00:00:00');
INSERT INTO [Orders] (OrderID, CustomerID, StatusID, OrderDate) VALUES (15, 9, 1, '2026-06-09 00:00:00');
INSERT INTO [Orders] (OrderID, CustomerID, StatusID, OrderDate) VALUES (16, 10, 1, '2026-06-10 00:00:00');
SET IDENTITY_INSERT [Orders] OFF;
GO

SET IDENTITY_INSERT [Order_Details] ON;
INSERT INTO [Order_Details] (DetailID, OrderID, ProductID, Quantity, UnitPrice) VALUES (1, 1, 1, 1, 650.00);
INSERT INTO [Order_Details] (DetailID, OrderID, ProductID, Quantity, UnitPrice) VALUES (2, 1, 2, 2, 15.50);
INSERT INTO [Order_Details] (DetailID, OrderID, ProductID, Quantity, UnitPrice) VALUES (3, 2, 3, 1, 45.99);
INSERT INTO [Order_Details] (DetailID, OrderID, ProductID, Quantity, UnitPrice) VALUES (4, 3, 4, 1, 180.00);
INSERT INTO [Order_Details] (DetailID, OrderID, ProductID, Quantity, UnitPrice) VALUES (5, 3, 5, 2, 220.00);
INSERT INTO [Order_Details] (DetailID, OrderID, ProductID, Quantity, UnitPrice) VALUES (6, 3, 8, 1, 35.00);
INSERT INTO [Order_Details] (DetailID, OrderID, ProductID, Quantity, UnitPrice) VALUES (7, 4, 6, 1, 150.00);
INSERT INTO [Order_Details] (DetailID, OrderID, ProductID, Quantity, UnitPrice) VALUES (8, 5, 9, 3, 28.75);
INSERT INTO [Order_Details] (DetailID, OrderID, ProductID, Quantity, UnitPrice) VALUES (9, 7, 9, 1, 28.75);
INSERT INTO [Order_Details] (DetailID, OrderID, ProductID, Quantity, UnitPrice) VALUES (10, 9, 1, 1, 650.00);
INSERT INTO [Order_Details] (DetailID, OrderID, ProductID, Quantity, UnitPrice) VALUES (11, 9, 2, 2, 15.50);
INSERT INTO [Order_Details] (DetailID, OrderID, ProductID, Quantity, UnitPrice) VALUES (12, 10, 3, 1, 45.99);
INSERT INTO [Order_Details] (DetailID, OrderID, ProductID, Quantity, UnitPrice) VALUES (13, 11, 4, 1, 180.00);
INSERT INTO [Order_Details] (DetailID, OrderID, ProductID, Quantity, UnitPrice) VALUES (14, 11, 5, 2, 220.00);
INSERT INTO [Order_Details] (DetailID, OrderID, ProductID, Quantity, UnitPrice) VALUES (15, 11, 8, 1, 35.00);
INSERT INTO [Order_Details] (DetailID, OrderID, ProductID, Quantity, UnitPrice) VALUES (16, 12, 6, 1, 150.00);
INSERT INTO [Order_Details] (DetailID, OrderID, ProductID, Quantity, UnitPrice) VALUES (17, 13, 9, 3, 28.75);
INSERT INTO [Order_Details] (DetailID, OrderID, ProductID, Quantity, UnitPrice) VALUES (18, 15, 9, 1, 28.75);
SET IDENTITY_INSERT [Order_Details] OFF;
GO


