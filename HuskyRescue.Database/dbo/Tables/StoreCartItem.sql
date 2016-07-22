CREATE TABLE [dbo].[StoreCartItem] (
    [Id]          UNIQUEIDENTIFIER CONSTRAINT [DF_StoreCartItem_Id] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    [StoreCartId] UNIQUEIDENTIFIER NOT NULL,
    [InventoryId] UNIQUEIDENTIFIER NOT NULL,
    [Quantity]    INT              NOT NULL,
    CONSTRAINT [PK_StoreCartItem_1] PRIMARY KEY CLUSTERED ([Id] ASC, [StoreCartId] ASC),
    CONSTRAINT [FK_Inventory_Id___StoreCartItem_InventoryId] FOREIGN KEY ([InventoryId]) REFERENCES [dbo].[Inventory] ([Id]),
    CONSTRAINT [FK_StoreCart_Id___StoreCartItem_StoreCartId] FOREIGN KEY ([StoreCartId]) REFERENCES [dbo].[StoreCart] ([Id])
);

