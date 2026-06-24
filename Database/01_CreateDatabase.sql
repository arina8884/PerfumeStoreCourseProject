USE master;
GO


IF EXISTS (SELECT * FROM sys.databases WHERE name = 'PerfumeStore')
BEGIN
    ALTER DATABASE PerfumeStore SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE PerfumeStore;
END
GO

CREATE DATABASE PerfumeStore;
GO

USE PerfumeStore;
GO


CREATE TABLE Users (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    FullName NVARCHAR(150) NOT NULL,
    Email NVARCHAR(150) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(255) NOT NULL,
    Phone NVARCHAR(20) NULL,
    Role NVARCHAR(50) NOT NULL DEFAULT 'Client',
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
    CONSTRAINT CHK_UserRole CHECK (Role IN ('Client', 'OrderManager', 'ContentManager', 'Admin'))
);
GO


CREATE TABLE Categories (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL UNIQUE,
    Description NVARCHAR(MAX) NULL
);
GO

CREATE TABLE Products (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    CategoryId INT NOT NULL,
    Name NVARCHAR(150) NOT NULL,
    Brand NVARCHAR(100) NOT NULL,
    Price DECIMAL(18,2) NOT NULL,
    Volume INT NOT NULL,
    Description NVARCHAR(MAX) NULL,
    StockQuantity INT NOT NULL DEFAULT 0,
    ImageUrl NVARCHAR(500) NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_Products_Categories FOREIGN KEY (CategoryId) REFERENCES Categories(Id) ON DELETE CASCADE,
    CONSTRAINT UQ_Product UNIQUE(Name, Brand, Volume),
    CONSTRAINT CHK_ProductPrice CHECK (Price >= 0),
    CONSTRAINT CHK_ProductStock CHECK (StockQuantity >= 0)
);
GO


CREATE TABLE Cart (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NOT NULL UNIQUE,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_Cart_Users FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE
);
GO

CREATE TABLE CartItems (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    CartId INT NOT NULL,
    ProductId INT NOT NULL,
    Quantity INT NOT NULL DEFAULT 1,
    CONSTRAINT FK_CartItems_Cart FOREIGN KEY (CartId) REFERENCES Cart(Id) ON DELETE CASCADE,
    CONSTRAINT FK_CartItems_Products FOREIGN KEY (ProductId) REFERENCES Products(Id) ON DELETE CASCADE,
    CONSTRAINT UQ_Cart_Product UNIQUE(CartId, ProductId),
    CONSTRAINT CHK_CartItemQuantity CHECK (Quantity > 0)
);
GO


CREATE TABLE Orders (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NOT NULL,
    OrderDate DATETIME2 NOT NULL DEFAULT GETDATE(),
    UpdatedAt DATETIME2 NULL,
    TotalAmount DECIMAL(18,2) NOT NULL,
    DeliveryAddress NVARCHAR(500) NOT NULL,
    PaymentMethod NVARCHAR(100) NOT NULL,
    DeliveryMethod NVARCHAR(100) NOT NULL,
    Status NVARCHAR(50) NOT NULL DEFAULT 'New',
    CONSTRAINT FK_Orders_Users FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE,
    CONSTRAINT CHK_OrderTotal CHECK (TotalAmount >= 0),
    CONSTRAINT CHK_OrderStatus CHECK (Status IN ('New', 'Confirmed', 'Assembling', 'InDelivery', 'OnTheWay', 'Delivered', 'Canceled'))
);
GO
CREATE TABLE OrderItems (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    OrderId INT NOT NULL,
    ProductId INT NOT NULL,
    Quantity INT NOT NULL DEFAULT 1,
    Price DECIMAL(18,2) NOT NULL,
    CONSTRAINT FK_OrderItems_Orders FOREIGN KEY (OrderId) REFERENCES Orders(Id) ON DELETE CASCADE,
    CONSTRAINT FK_OrderItems_Products FOREIGN KEY (ProductId) REFERENCES Products(Id) ON DELETE NO ACTION,
    CONSTRAINT UQ_Order_Product UNIQUE(OrderId, ProductId),
    CONSTRAINT CHK_OrderItemQuantity CHECK (Quantity > 0),
    CONSTRAINT CHK_OrderItemPrice CHECK (Price >= 0)
);
GO

CREATE TABLE Favorites (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NOT NULL,
    ProductId INT NOT NULL,
    CONSTRAINT FK_Favorites_Users FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE,
    CONSTRAINT FK_Favorites_Products FOREIGN KEY (ProductId) REFERENCES Products(Id) ON DELETE CASCADE,
    CONSTRAINT UQ_User_Product UNIQUE (UserId, ProductId)
);
GO


CREATE TABLE Reviews (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NOT NULL,
    ProductId INT NOT NULL,
    Rating INT NOT NULL,
    Comment NVARCHAR(1000) NULL, 
    ReviewDate DATETIME2 NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_Reviews_Users FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE,
    CONSTRAINT FK_Reviews_Products FOREIGN KEY (ProductId) REFERENCES Products(Id) ON DELETE CASCADE,
    CONSTRAINT UQ_UserReview UNIQUE(UserId, ProductId),
    CONSTRAINT CHK_ReviewRating CHECK (Rating BETWEEN 1 AND 5)
);
GO


CREATE TABLE Suppliers (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(150) NOT NULL,
    Email NVARCHAR(150) NULL,
    Phone NVARCHAR(20) NULL
);
GO


CREATE TABLE SupplyRequests (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    SupplierId INT NOT NULL,
    CreatedByUserId INT NOT NULL, 
    RequestDate DATETIME2 NOT NULL DEFAULT GETDATE(),
    Status NVARCHAR(50) NOT NULL DEFAULT 'Pending',
    Comment NVARCHAR(500) NULL,
    CONSTRAINT FK_SupplyRequests_Suppliers FOREIGN KEY (SupplierId) REFERENCES Suppliers(Id) ON DELETE CASCADE,
    CONSTRAINT FK_SupplyRequests_Users FOREIGN KEY (CreatedByUserId) REFERENCES Users(Id) ON DELETE NO ACTION, 
    CONSTRAINT CHK_SupplyRequestStatus CHECK (Status IN ('Pending', 'Approved', 'Completed', 'Rejected'))
);
GO

CREATE TABLE SupplyRequestItems (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    SupplyRequestId INT NOT NULL,
    ProductId INT NOT NULL,
    Quantity INT NOT NULL DEFAULT 1,
    CONSTRAINT FK_SupplyRequestItems_Requests FOREIGN KEY (SupplyRequestId) REFERENCES SupplyRequests(Id) ON DELETE CASCADE,
    CONSTRAINT FK_SupplyRequestItems_Products FOREIGN KEY (ProductId) REFERENCES Products(Id) ON DELETE CASCADE,
    CONSTRAINT UQ_SupplyRequest_Product UNIQUE(SupplyRequestId, ProductId),
    CONSTRAINT CHK_SupplyRequestItemQuantity CHECK (Quantity > 0)
);
GO
