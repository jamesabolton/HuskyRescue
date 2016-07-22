CREATE TABLE [dbo].[PaymentStore] (
    [Id]        UNIQUEIDENTIFIER CONSTRAINT [DF_PaymentStore_Id] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    [PaymentId] UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_PaymentStore] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Payment_Id___PaymentStore_PaymentId] FOREIGN KEY ([PaymentId]) REFERENCES [dbo].[Payment] ([Id])
);

