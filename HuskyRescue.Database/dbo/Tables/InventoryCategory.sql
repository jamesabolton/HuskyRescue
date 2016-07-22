CREATE TABLE [dbo].[InventoryCategory] (
    [Id]          UNIQUEIDENTIFIER CONSTRAINT [DF_InventoryCategory_Id] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    [Name]        NVARCHAR (200)   NOT NULL,
    [Description] NVARCHAR (MAX)   NULL,
    CONSTRAINT [PK_InventoryCategory] PRIMARY KEY CLUSTERED ([Id] ASC)
);

