IF NOT EXISTS(SELECT * FROM sys.databases WHERE NAME = 'AntCommerceDb')
  BEGIN
    CREATE DATABASE [AntCommerceDb]
	END
GO

USE [AntCommerceDb]
GO
IF OBJECT_ID(N'[dbo].[Products]', N'U') IS NULL
BEGIN
	CREATE TABLE dbo.Products
	(
		Id INT IDENTITY (1, 1),
		SKU VARCHAR(5) NOT NULL,
		[Name] NVARCHAR(500) NOT NULL,
		[Description] NVARCHAR(MAX) NULL,
		[Price] DECIMAL(18),
		CreateAt DATETIME CONSTRAINT DF_Product_CreateAt DEFAULT GETDATE(),
		CreatedBy NVARCHAR(500) NULL,
		UpdateAt DATETIME CONSTRAINT DF_Product_UpdateAt DEFAULT GETDATE(),
		UpdatedBy NVARCHAR(500) NULL,
		CONSTRAINT PK_Product PRIMARY KEY (Id),
		CONSTRAINT PK_Product_SKU UNIQUE (SKU)
	)
END

INSERT INTO dbo.Products(SKU, Name, Description, Price) VALUES('IP013', 'iPhone 13 ProMax', 'Nice', 130000)
INSERT INTO dbo.Products(SKU, Name, Description, Price) VALUES('IP014', 'iPhone 13 ProMax', 'Nice', 130000)
INSERT INTO dbo.Products(SKU, Name, Description, Price) VALUES('IP015', 'iPhone 13 ProMax', 'Nice', 130000)
INSERT INTO dbo.Products(SKU, Name, Description, Price) VALUES('IP016', 'iPhone 13 ProMax', 'Nice', 130000)
INSERT INTO dbo.Products(SKU, Name, Description, Price) VALUES('IP017', 'iPhone 13 ProMax', 'Nice', 130000)
INSERT INTO dbo.Products(SKU, Name, Description, Price) VALUES('IP018', 'iPhone 13 ProMax', 'Nice', 130000)
INSERT INTO dbo.Products(SKU, Name, Description, Price) VALUES('IP019', 'iPhone 13 ProMax', 'Nice', 130000)
INSERT INTO dbo.Products(SKU, Name, Description, Price) VALUES('IP020', 'iPhone 13 ProMax', 'Nice', 130000)
INSERT INTO dbo.Products(SKU, Name, Description, Price) VALUES('IP021', 'iPhone 13 ProMax', 'Nice', 150000)
INSERT INTO dbo.Products(SKU, Name, Description, Price) VALUES('IP022', 'iPhone 13 ProMax', 'Nice', 180000)
INSERT INTO dbo.Products(SKU, Name, Description, Price) VALUES('IP023', 'iPhone 13 ProMax', 'Nice', 200000)
INSERT INTO dbo.Products(SKU, Name, Description, Price) VALUES('IP024', 'Samsung Galaxy', 'Nice', 200000)
-- select * from dbo.Products