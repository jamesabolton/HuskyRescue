CREATE TABLE [dbo].[PaymentEventRegistration] (
    [Id]        UNIQUEIDENTIFIER CONSTRAINT [DF_PaymentEventRegistration_Id] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    [PaymentId] UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_PaymentEventRegistration] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Payment_Id___PaymentEventRegistration_PaymentId] FOREIGN KEY ([PaymentId]) REFERENCES [dbo].[Payment] ([Id])
);

