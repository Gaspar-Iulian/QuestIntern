USE QuestEcommerce;
GO

IF OBJECT_ID('dbo.Orders', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Orders
    (
        Id INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Orders PRIMARY KEY,
        UserId INT NULL,
        ShippingFullName NVARCHAR(120) NOT NULL,
        ShippingAddressLine1 NVARCHAR(240) NOT NULL,
        ShippingCity NVARCHAR(120) NOT NULL,
        ShippingPostalCode NVARCHAR(40) NOT NULL,
        ShippingCountry NVARCHAR(120) NOT NULL,
        TotalAmount DECIMAL(18, 2) NOT NULL,
        CreatedAtUtc DATETIME2 NOT NULL CONSTRAINT DF_Orders_CreatedAtUtc DEFAULT SYSUTCDATETIME(),
        CONSTRAINT FK_Orders_Users FOREIGN KEY (UserId) REFERENCES dbo.Users(Id)
    );
END;
GO

IF OBJECT_ID('dbo.OrderItems', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.OrderItems
    (
        Id INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_OrderItems PRIMARY KEY,
        OrderId INT NOT NULL,
        ProductId INT NOT NULL,
        ProductName NVARCHAR(120) NOT NULL,
        Quantity INT NOT NULL,
        UnitPrice DECIMAL(18, 2) NOT NULL,
        LineTotal DECIMAL(18, 2) NOT NULL,
        CONSTRAINT FK_OrderItems_Orders FOREIGN KEY (OrderId) REFERENCES dbo.Orders(Id),
        CONSTRAINT FK_OrderItems_Products FOREIGN KEY (ProductId) REFERENCES dbo.Products(Id)
    );
END;
GO
