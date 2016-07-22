CREATE TABLE [dbo].[StoreOrderDetail] (
    [Id]           UNIQUEIDENTIFIER CONSTRAINT [DF_StoreOrderDetail_Id] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    [StoreOrderId] UNIQUEIDENTIFIER NOT NULL,
    [InventoryId]  UNIQUEIDENTIFIER NOT NULL,
    [Quantity]     INT              NOT NULL,
    [Price]        MONEY            NOT NULL,
    CONSTRAINT [PK_StoreOrderDetail_1] PRIMARY KEY CLUSTERED ([Id] ASC, [StoreOrderId] ASC),
    CONSTRAINT [FK_Inventory_Id___StoreOrderDetail_InventoryId] FOREIGN KEY ([InventoryId]) REFERENCES [dbo].[Inventory] ([Id]),
    CONSTRAINT [FK_StoreOrder_Id___StoreOrderDetail_StoreOrderId] FOREIGN KEY ([StoreOrderId]) REFERENCES [dbo].[StoreOrder] ([Id])
);

