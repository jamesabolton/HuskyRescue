CREATE TABLE [dbo].[EventSponsor] (
    [Id]                          UNIQUEIDENTIFIER CONSTRAINT [DF_EventSponsor_Id] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    [EventId]                     UNIQUEIDENTIFIER NOT NULL,
    [OrganizationId]              UNIQUEIDENTIFIER NULL,
    [PersonId]                    UNIQUEIDENTIFIER NULL,
    [EventSponsorshipId]          UNIQUEIDENTIFIER NOT NULL,
    [HaveReceivedSingage]         BIT              NOT NULL,
    [HaveReceviedLogo]            BIT              NOT NULL,
    [HasSponsorMoneyBeenReceived] BIT              NOT NULL,
    [AmountPaid]                  MONEY            NOT NULL,
    [AddedOnDate]                 DATETIME2 (0)    NOT NULL,
    [AddedByUserId]               NVARCHAR (256)   NOT NULL,
    [UpdatedOnDate]               DATETIME2 (0)    NULL,
    [UpdatedByUserId]             NVARCHAR (256)   NULL,
    [Notes]                       NVARCHAR (MAX)   NULL,
    CONSTRAINT [PK_EventSponsor] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Event_Id___EventSponsor_EventId] FOREIGN KEY ([EventId]) REFERENCES [dbo].[Event] ([Id]),
    CONSTRAINT [FK_EventSponsorship_Id___EventSponsor_EventSponsorshipId] FOREIGN KEY ([EventSponsorshipId]) REFERENCES [dbo].[EventSponsorship] ([Id]),
    CONSTRAINT [FK_Organization_Id___EventSponsor_OrganizationId] FOREIGN KEY ([OrganizationId]) REFERENCES [dbo].[Organization] ([Id]),
    CONSTRAINT [FK_Person_Id___EventSponsor_PersonId] FOREIGN KEY ([PersonId]) REFERENCES [dbo].[Person] ([Id])
);





