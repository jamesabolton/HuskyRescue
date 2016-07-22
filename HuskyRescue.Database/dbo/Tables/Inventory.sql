CREATE TABLE [dbo].[Inventory] (
    [Id]                  UNIQUEIDENTIFIER CONSTRAINT [DF_Inventory_Id] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    [InventoryCategoryId] UNIQUEIDENTIFIER NOT NULL,
    [IsActive]            BIT              NOT NULL,
    [IsDeleted]           BIT              NOT NULL,
    [Name]                NVARCHAR (200)   NOT NULL,
    [Quantity]            INT              NOT NULL,
    [CostToBuy]           MONEY            NOT NULL,
    [IsAvailableInStore]  BIT              NOT NULL,
    [StoreSellPrice]      MONEY            NOT NULL,
    [ShippingCost]        MONEY            NOT NULL,
    [AddedOnDate]         DATETIME2 (0)    NOT NULL,
    [AddedByUserId]       UNIQUEIDENTIFIER NOT NULL,
    [UpdatedOnDate]       DATETIME2 (0)    NULL,
    [UpdatedByUserId]     UNIQUEIDENTIFIER NULL,
    [Brand]               NVARCHAR (200)   NULL,
    [Model]               NVARCHAR (50)    NULL,
    [Notes]               NVARCHAR (MAX)   NULL,
    CONSTRAINT [PK_Inventory] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_InventoryCategory_Id___Inventory_InventoryCategoryId] FOREIGN KEY ([InventoryCategoryId]) REFERENCES [dbo].[InventoryCategory] ([Id])
);

