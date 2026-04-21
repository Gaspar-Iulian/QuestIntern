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

SET IDENTITY_INSERT dbo.Products ON;

MERGE dbo.Products AS target
USING
(
    VALUES
        (1, N'*RTR* NECTAR DE CAISE FARA ZAHAR 300ML', N'Nectar romanesc de caise, fara zahar adaugat, potrivit pentru un cos cu gusturi curate.', 11.00, N'https://dordegusturi.ro/wp-content/uploads/2025/12/Produs-_391-9-600x600.png', 34),
        (2, N'*RTR* NECTAR DE CAPSUNI FARA ZAHAR 300ML', N'Nectar de capsuni fara zahar adaugat, pentru bauturi simple si deserturi de casa.', 11.00, N'https://dordegusturi.ro/wp-content/uploads/2025/12/Produs-_393-1-600x600.jpg', 30),
        (3, N'*RTR* NECTAR DE PIERSICI FARA ZAHAR 300ML', N'Nectar de piersici fara zahar adaugat, cu gust fructat si textura fina.', 11.00, N'https://dordegusturi.ro/wp-content/uploads/2025/12/Produs-_392-1-600x600.jpg', 28),
        (4, N'*RTR* NECTAR DE PRUNE FARA ZAHAR 300ML', N'Nectar de prune fara zahar adaugat, potrivit pentru un mic dejun rapid.', 11.00, N'https://dordegusturi.ro/wp-content/uploads/2025/12/Produs-_394-1-600x600.jpg', 24),
        (5, N'ACADEA CIOCOLATA - MIISIMII', N'Acadea de ciocolata pentru un desert mic, artizanal si usor de pus in cos.', 10.00, N'https://dordegusturi.ro/wp-content/uploads/2025/12/WhatsApp-Image-2025-12-23-at-11.31.15-3-600x600.jpeg', 22),
        (6, N'ANTRICOT DE MANZAT FARA OS BIO SKIN 100GR', N'Antricot de manzat fara os, portionat pentru un catalog gourmet variat.', 18.00, N'https://dordegusturi.ro/wp-content/uploads/2025/12/Produs-_102-1-600x600.jpg', 16)
) AS source (Id, Name, Description, Price, ImageUrl, StockQuantity)
ON target.Id = source.Id
WHEN MATCHED THEN
    UPDATE SET
        Name = source.Name,
        Description = source.Description,
        Price = source.Price,
        ImageUrl = source.ImageUrl,
        StockQuantity = source.StockQuantity
WHEN NOT MATCHED THEN
    INSERT (Id, Name, Description, Price, ImageUrl, StockQuantity)
    VALUES (source.Id, source.Name, source.Description, source.Price, source.ImageUrl, source.StockQuantity);

SET IDENTITY_INSERT dbo.Products OFF;
GO
