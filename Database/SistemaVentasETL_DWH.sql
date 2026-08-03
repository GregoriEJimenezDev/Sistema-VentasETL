USE SistemaVentasETL;
GO

-- ============================================================
-- Script del Data Warehouse (DWH) del proyecto AnalisisVentas ETL
-- Crea los esquemas, las tablas dimensionales, la tabla de hechos
-- y puebla DimFecha con 1,461 fechas (2023-01-01 a 2026-12-31).
-- Ejecutar DESPUÉS de Database/SistemaVentasETL.sql
-- ============================================================

CREATE SCHEMA Dimensiones;
GO

CREATE SCHEMA Hechos;
GO

-- Dimensión Producto (SCD Tipo 1)
CREATE TABLE Dimensiones.DimProducto (
    ProductoKey       INT IDENTITY(1,1) NOT NULL,
    Codigo            NVARCHAR(50)      NOT NULL,
    NombreProducto    NVARCHAR(100)     NOT NULL,
    Categoria         NVARCHAR(50)      NOT NULL,
    Precio            DECIMAL(10,2)     NOT NULL,
    Stock             INT               NOT NULL,
    FechaCreacionDW   DATETIME          NOT NULL DEFAULT GETDATE(),
    CONSTRAINT PK_DimProducto PRIMARY KEY (ProductoKey),
    CONSTRAINT UQ_DimProducto_Codigo UNIQUE (Codigo)
);
GO

-- Dimensión Cliente (SCD Tipo 1)
CREATE TABLE Dimensiones.DimCliente (
    ClienteKey        INT IDENTITY(1,1) NOT NULL,
    ClienteIdOrigen   NVARCHAR(50)      NOT NULL,
    NombreCompleto    NVARCHAR(100)     NOT NULL,
    Email             VARCHAR(100)      NOT NULL,
    Telefono          VARCHAR(20)       NULL,
    Ciudad            NVARCHAR(50)      NOT NULL,
    FechaCreacionDW   DATETIME          NOT NULL DEFAULT GETDATE(),
    CONSTRAINT PK_DimCliente PRIMARY KEY (ClienteKey),
    CONSTRAINT UQ_DimCliente_IdOrigen UNIQUE (ClienteIdOrigen)
);
GO

-- Dimensión Suplidor (SCD Tipo 1)
CREATE TABLE Dimensiones.DimSuplidor (
    SuplidorKey       INT IDENTITY(1,1) NOT NULL,
    SuplidorIdOrigen  NVARCHAR(50)      NOT NULL,
    NombreSuplidor    NVARCHAR(100)     NOT NULL,
    Email             VARCHAR(100)      NULL,
    Telefono          VARCHAR(20)       NULL,
    Ciudad            NVARCHAR(50)      NULL,
    FechaCreacionDW   DATETIME          NOT NULL DEFAULT GETDATE(),
    CONSTRAINT PK_DimSuplidor PRIMARY KEY (SuplidorKey),
    CONSTRAINT UQ_DimSuplidor_IdOrigen UNIQUE (SuplidorIdOrigen)
);
GO

-- Dimensión Fecha (FechaKey = yyyyMMdd)
CREATE TABLE Dimensiones.DimFecha (
    FechaKey          INT            NOT NULL,
    Fecha             DATE           NOT NULL,
    Anio              INT            NOT NULL,
    Mes               INT            NOT NULL,
    Dia               INT            NOT NULL,
    NombreMes         NVARCHAR(20)   NOT NULL,
    Trimestre         INT            NOT NULL,
    Semana            INT            NOT NULL,
    DiaNombre         NVARCHAR(20)   NOT NULL,
    EsFinSemana       BIT            NOT NULL,
    FechaCreacionDW   DATETIME       NOT NULL DEFAULT GETDATE(),
    CONSTRAINT PK_DimFecha PRIMARY KEY (FechaKey)
);
GO

-- Población de DimFecha: 2023-01-01 a 2026-12-31 (1,461 fechas)
;WITH Fechas AS (
    SELECT CAST('2023-01-01' AS DATE) AS Fecha
    UNION ALL
    SELECT DATEADD(DAY, 1, Fecha)
    FROM Fechas
    WHERE Fecha < '2026-12-31'
)
INSERT INTO Dimensiones.DimFecha (FechaKey, Fecha, Anio, Mes, Dia, NombreMes, Trimestre, Semana, DiaNombre, EsFinSemana)
SELECT
    CAST(CONVERT(VARCHAR(8), Fecha, 112) AS INT)          AS FechaKey,
    Fecha,
    YEAR(Fecha)                                           AS Anio,
    MONTH(Fecha)                                          AS Mes,
    DAY(Fecha)                                            AS Dia,
    DATENAME(MONTH, Fecha)                                AS NombreMes,
    DATEPART(QUARTER, Fecha)                              AS Trimestre,
    DATEPART(WEEK, Fecha)                                 AS Semana,
    DATENAME(WEEKDAY, Fecha)                              AS DiaNombre,
    CASE WHEN DATEPART(WEEKDAY, Fecha) IN (1, 7) THEN 1 ELSE 0 END AS EsFinSemana
FROM Fechas
OPTION (MAXRECURSION 0);
GO

-- Tabla de hechos: Ventas
CREATE TABLE Hechos.FactVentas (
    VentaKey         INT IDENTITY(1,1) NOT NULL,
    ProductoKey      INT               NOT NULL,
    ClienteKey       INT               NOT NULL,
    FechaKey         INT               NOT NULL,
    Cantidad         INT               NOT NULL,
    PrecioUnitario   DECIMAL(10,2)     NOT NULL,
    TotalVenta       DECIMAL(12,2)     NOT NULL,
    CONSTRAINT PK_FactVentas PRIMARY KEY (VentaKey),
    CONSTRAINT FK_FactVentas_Producto FOREIGN KEY (ProductoKey) REFERENCES Dimensiones.DimProducto (ProductoKey),
    CONSTRAINT FK_FactVentas_Cliente  FOREIGN KEY (ClienteKey)  REFERENCES Dimensiones.DimCliente (ClienteKey),
    CONSTRAINT FK_FactVentas_Fecha    FOREIGN KEY (FechaKey)    REFERENCES Dimensiones.DimFecha (FechaKey)
);
GO
