CREATE TABLE [dbo].[Event] (
    [Id]                    UNIQUEIDENTIFIER CONSTRAINT [DF_Event_Id] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    [OrganizationId]        UNIQUEIDENTIFIER NOT NULL,
    [EventDate]             DATETIME2 (0)    NULL,
    [IsActive]              BIT              NOT NULL,
    [IsDeleted]             BIT              NOT NULL,
    [DeletedOnDate]         DATETIME2 (0)    NULL,
    [AddedOnDate]           DATETIME2 (0)    NOT NULL,
    [AddedByUserId]         NVARCHAR (256)   NOT NULL,
    [UpdatedOnDate]         DATETIME2 (0)    NULL,
    [UpdatedByUserId]       NVARCHAR (256)   NULL,
    [Name]                  NVARCHAR (200)   NOT NULL,
    [StartTime]             TIME (0)         NOT NULL,
    [EndTime]               TIME (0)         NOT NULL,
    [IsAllDayEvent]         BIT              NOT NULL,
    [IsRoughRidersEvent]    BIT              NOT NULL,
    [IsGolfTournamentEvent] BIT              NOT NULL,
    [IsRaffleEvent]         BIT              NOT NULL,
    [IsMeetAndGreetEvent]   BIT              NOT NULL,
    [IsDogWashEvent]        BIT              NOT NULL,
    [Is5kEvent]             BIT              NOT NULL,
    [IsOtherEvent]          BIT              NOT NULL,
    [OtherEventDescription] NVARCHAR (200)   NULL,
    [AreTicketsSold]        BIT              NOT NULL,
    [TicketPrice]           MONEY            NOT NULL,
    [TicketPriceOther]      MONEY            NULL,
    [PublicDescription]     NVARCHAR (MAX)   NULL,
    [Notes]                 NVARCHAR (MAX)   NULL,
    CONSTRAINT [PK_Event] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Organization_Id___Event_OrganizationId] FOREIGN KEY ([OrganizationId]) REFERENCES [dbo].[Organization] ([Id])
);



