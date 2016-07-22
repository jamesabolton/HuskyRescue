CREATE TABLE [dbo].[EventRegistrationPerson] (
    [Id]                  UNIQUEIDENTIFIER CONSTRAINT [DF_EventRegistrationPerson_Id] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    [EventRegistrationId] UNIQUEIDENTIFIER NOT NULL,
    [PersonId]            UNIQUEIDENTIFIER NOT NULL,
    [IsPrimaryPerson]     BIT              NOT NULL,
    [TicketPrice]         MONEY            NOT NULL,
    [AttendeeType]        NVARCHAR (20)    NULL,
    CONSTRAINT [PK_EventRegistrationPerson] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_EventRegistration_Id___EventRegistrationPerson_EventRegistrationId] FOREIGN KEY ([EventRegistrationId]) REFERENCES [dbo].[EventRegistration] ([Id]),
    CONSTRAINT [FK_Person_Id___EventRegistrationPerson_PersonId] FOREIGN KEY ([PersonId]) REFERENCES [dbo].[Person] ([Id])
);



