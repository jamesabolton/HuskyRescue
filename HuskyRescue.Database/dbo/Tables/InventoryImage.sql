CREATE TABLE [dbo].[InventoryImage] (
    [Id]           UNIQUEIDENTIFIER CONSTRAINT [DF_InventoryImage_Id] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    [InventoryId]  UNIQUEIDENTIFIER NOT NULL,
    [RelativePath] NVARCHAR (1000)  NOT NULL,
    [PhysicalPath] NVARCHAR (1000)  NOT NULL,
    [AltText]      NVARCHAR (500)   NOT NULL,
    CONSTRAINT [PK_InventoryImage] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Inventory_Id___InventoryImage_InventoryId] FOREIGN KEY ([InventoryId]) REFERENCES [dbo].[Inventory] ([Id])
);

