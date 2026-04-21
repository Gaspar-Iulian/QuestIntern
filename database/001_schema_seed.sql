IF DB_ID('QuestEcommerce') IS NULL
BEGIN
    CREATE DATABASE QuestEcommerce;
END;
GO

USE QuestEcommerce;
GO

IF OBJECT_ID('dbo.Products', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Products
    (
        Id INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Products PRIMARY KEY,
        Name NVARCHAR(120) NOT NULL,
        Description NVARCHAR(500) NOT NULL,
        Price DECIMAL(18, 2) NOT NULL,
        ImageUrl NVARCHAR(500) NOT NULL,
        StockQuantity INT NOT NULL,
        CreatedAtUtc DATETIME2 NOT NULL CONSTRAINT DF_Products_CreatedAtUtc DEFAULT SYSUTCDATETIME()
    );
END;
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Products)
BEGIN
    INSERT INTO dbo.Products (Name, Description, Price, ImageUrl, StockQuantity)
    VALUES
        ('Blue Top', 'Women blue cotton top.', 29.99, 'https://automationexercise.com/get_product_picture/1', 25),
        ('Men Tshirt', 'Comfortable casual tshirt.', 19.99, 'https://automationexercise.com/get_product_picture/2', 40),
        ('Sleeveless Dress', 'Light sleeveless summer dress.', 49.99, 'https://automationexercise.com/get_product_picture/3', 15),
        ('Stylish Dress', 'Elegant dress for daily wear.', 59.99, 'https://automationexercise.com/get_product_picture/4', 10);
END;
GO
