CREATE TABLE [dbo].[InventoryPlacements] (
    [Id]                 UNIQUEIDENTIFIER CONSTRAINT [DF_InventoryPlacements_Id] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    [InventoryId]        UNIQUEIDENTIFIER NOT NULL,
    [QuantityAtLocation] INT              NOT NULL,
    [AddedOnDate]        DATETIME2 (0)    NOT NULL,
    [AddedByUserId]      UNIQUEIDENTIFIER NOT NULL,
    [UpdatedOnDate]      DATETIME2 (0)    NULL,
    [UpdatedByUserId]    UNIQUEIDENTIFIER NULL,
    [PersonId]           UNIQUEIDENTIFIER NULL,
    [OrganizationId]     UNIQUEIDENTIFIER NULL,
    [Notes]              NVARCHAR (MAX)   NULL,
    CONSTRAINT [PK_InventoryPlacements] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Inventory_Id___InventoryPlacements_InventoryId] FOREIGN KEY ([InventoryId]) REFERENCES [dbo].[Inventory] ([Id]),
    CONSTRAINT [FK_Organization_Id___InventoryPlacements_OrganizationId] FOREIGN KEY ([OrganizationId]) REFERENCES [dbo].[Organization] ([Id]),
    CONSTRAINT [FK_Person_Id___InventoryPlacements_PersonId] FOREIGN KEY ([PersonId]) REFERENCES [dbo].[Person] ([Id])
);

