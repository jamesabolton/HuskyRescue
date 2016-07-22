CREATE TABLE [dbo].[StoreShippingMethod] (
    [Id]                      UNIQUEIDENTIFIER CONSTRAINT [DF_StoreShipmentMethod_Id] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    [Name]                    NVARCHAR (200)   NOT NULL,
    [IsActive]                BIT              NOT NULL,
    [IsDeleted]               BIT              NOT NULL,
    [IsFlatAmountPerItem]     BIT              NOT NULL,
    [FlatAmountPerItem]       MONEY            NOT NULL,
    [IsFlatAmountPerOrder]    BIT              NOT NULL,
    [FlatAmountPerOrder]      MONEY            NOT NULL,
    [IsPercentAmountPerItem]  BIT              NOT NULL,
    [PercentAmountPerItem]    DECIMAL (4, 2)   NOT NULL,
    [IsPercentAmountPerOrder] BIT              NOT NULL,
    [PercentAmountPerOrder]   DECIMAL (4, 2)   NOT NULL,
    [BaseAmount]              MONEY            NOT NULL,
    [AddOnDate]               DATETIME2 (0)    NOT NULL,
    [AddedByUserId]           UNIQUEIDENTIFIER NOT NULL,
    [UpdatedOnDate]           DATETIME2 (0)    NULL,
    [UpdatedByUserId]         UNIQUEIDENTIFIER NULL,
    [DeletedOnDate]           DATETIME2 (0)    NULL,
    [TrackingWebsite]         NVARCHAR (1000)  NULL,
    [Notes]                   NVARCHAR (MAX)   NULL,
    CONSTRAINT [PK_StoreShipmentMethod] PRIMARY KEY CLUSTERED ([Id] ASC)
);

