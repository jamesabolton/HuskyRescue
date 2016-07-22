CREATE TABLE [dbo].[PaymentDonation] (
    [Id]                      UNIQUEIDENTIFIER CONSTRAINT [DF_PaymentDonation_Id] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    [PaymentId]               UNIQUEIDENTIFIER NULL,
    [HasThankYouCardBeenSent] BIT              NOT NULL,
    [IsDonatedItem]           BIT              NOT NULL,
    [DonatedItemCashValue]    MONEY            NULL,
    [DonatedItemDescription]  NVARCHAR (MAX)   NULL,
    CONSTRAINT [PK_PaymentDonation] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Payment_Id___PaymentDonation_PaymentId] FOREIGN KEY ([PaymentId]) REFERENCES [dbo].[Payment] ([Id])
);

