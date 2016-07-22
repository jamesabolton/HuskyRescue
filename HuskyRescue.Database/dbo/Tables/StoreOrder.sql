CREATE TABLE [dbo].[StoreOrder] (
    [Id]                    UNIQUEIDENTIFIER CONSTRAINT [DF_StoreOrder_Id] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    [CreatedOnDate]         DATETIME2 (0)    NOT NULL,
    [UpdatedOnDate]         DATETIME2 (0)    NULL,
    [SubmittedOnDate]       DATETIME2 (7)    NULL,
    [PaymentStoreId]        UNIQUEIDENTIFIER NULL,
    [StoreShippingMethodId] UNIQUEIDENTIFIER NULL,
    [UserId]                UNIQUEIDENTIFIER NOT NULL,
    [Status]                UNIQUEIDENTIFIER NOT NULL,
    [AmountDue]             MONEY            NOT NULL,
    [CustomerComments]      NVARCHAR (MAX)   NULL,
    CONSTRAINT [PK_StoreOrder] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_PaymentStore_Id___StoreOrder_PaymentStoreId] FOREIGN KEY ([PaymentStoreId]) REFERENCES [dbo].[PaymentStore] ([Id]),
    CONSTRAINT [FK_StoreShippingMethod_Id___StoreOrder_StoreShippingMethodId] FOREIGN KEY ([StoreShippingMethodId]) REFERENCES [dbo].[StoreShippingMethod] ([Id])
);

