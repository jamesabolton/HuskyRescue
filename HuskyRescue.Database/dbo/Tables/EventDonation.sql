CREATE TABLE [dbo].[EventDonation] (
    [Id]                   UNIQUEIDENTIFIER CONSTRAINT [DF_EventDonation_Id] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    [EventId]              UNIQUEIDENTIFIER NOT NULL,
    [PaymentDonationId]    UNIQUEIDENTIFIER NOT NULL,
    [HaveReceivedDonation] BIT              NOT NULL,
    CONSTRAINT [PK_EventDonation] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Event_Id___EventDonation_EventId] FOREIGN KEY ([EventId]) REFERENCES [dbo].[Event] ([Id]),
    CONSTRAINT [FK_PaymentDonation_Id___EventDonation_PaymentDonationId] FOREIGN KEY ([PaymentDonationId]) REFERENCES [dbo].[PaymentDonation] ([Id])
);

