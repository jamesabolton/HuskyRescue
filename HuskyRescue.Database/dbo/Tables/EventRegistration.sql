CREATE TABLE [dbo].[EventRegistration] (
    [Id]                         UNIQUEIDENTIFIER CONSTRAINT [DF_EventRegistration_Id] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    [EventId]                    UNIQUEIDENTIFIER NOT NULL,
    [DateSubmitted]              DATETIME2 (0)    NOT NULL,
    [HasPaid]                    BIT              NOT NULL,
    [PaymentEventRegistrationId] UNIQUEIDENTIFIER NULL,
    [NumberTicketsBought]        INT              NOT NULL,
    [AmountPaid]                 MONEY            NOT NULL,
    [UpdatedOnDate]              DATETIME2 (0)    NULL,
    [UpdatedByUserId]            UNIQUEIDENTIFIER NULL,
    [RegistrationComments]       NVARCHAR (MAX)   NULL,
    [Notes]                      NVARCHAR (MAX)   NULL,
    CONSTRAINT [PK_EventRegistration] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Event_Id___EventRegistration_EventId] FOREIGN KEY ([EventId]) REFERENCES [dbo].[Event] ([Id]),
    CONSTRAINT [FK_PaymentEventRegistration_Id___EventRegistration_PaymentEventRegistrationId] FOREIGN KEY ([PaymentEventRegistrationId]) REFERENCES [dbo].[PaymentEventRegistration] ([Id])
);



