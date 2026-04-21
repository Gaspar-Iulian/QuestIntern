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
        (1, N'*RTR* NECTAR DE CAISE FARA ZAHAR 300ML', N'Nectar romanesc de caise, fara zahar adaugat, potrivit pentru un cos cu gusturi curate.', 14.00, N'https://dordegusturi.ro/wp-content/uploads/2025/12/NECTAR-DE-CAISE-600x600.jpg', 34),
        (2, N'SCORTISOARA PUDRA 70GR', N'Condiment aromat pentru deserturi, bauturi calde si retete de camara.', 6.00, N'https://dordegusturi.ro/wp-content/uploads/2025/12/SCORTISOARA-PUDRA-70GR-600x600.jpg', 50),
        (3, N'RISOTTO 750GR ECO', N'Produs de camara ecologic, potrivit pentru mese simple, consistente si naturale.', 44.00, N'https://dordegusturi.ro/wp-content/uploads/2025/12/RISOTTO-750GR-ECO-600x600.jpg', 18),
        (4, N'BRANZA DE BURDUF CU CHIMEN FERMA DE LA BRAN 100GR', N'Branza artizanala cu chimen, pentru platouri romanesti si gustari cu personalitate.', 11.70, N'https://dordegusturi.ro/wp-content/uploads/2025/12/BRANZA-DE-BURDUF-CU-CHIMEN-FERMA-DE-LA-BRAN-100GR-600x600.jpg', 28),
        (5, N'ARDEI IUTE IN OTET 290GR', N'Ardei iute in otet pentru mese traditionale, muraturi si preparate cu gust intens.', 22.00, N'https://dordegusturi.ro/wp-content/uploads/2025/12/ARDEI-IUTE-IN-OTET-290GR-600x600.jpg', 22),
        (6, N'SIROP DE SOC 500ML - DIN INIMA TARII', N'Sirop romanesc de soc, ideal pentru bauturi racoritoare si deserturi de casa.', 38.00, N'https://dordegusturi.ro/wp-content/uploads/2025/12/SIROP-DE-SOC-500ML-600x600.jpg', 16)
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
