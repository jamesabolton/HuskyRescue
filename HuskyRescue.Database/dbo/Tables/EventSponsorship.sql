CREATE TABLE [dbo].[EventSponsorship] (
    [Id]                      UNIQUEIDENTIFIER CONSTRAINT [DF_EventSponsorship_Id] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    [EventId]                 UNIQUEIDENTIFIER NOT NULL,
    [IsActive]                BIT              NOT NULL,
    [IsDeleted]               BIT              NOT NULL,
    [Name]                    NVARCHAR (200)   NOT NULL,
    [Cost]                    MONEY            NOT NULL,
    [NumberOfAllowedSponsors] INT              NOT NULL,
    [NumberOfSponsors]        INT              NOT NULL,
    [AddedOnDate]             DATETIME2 (0)    NOT NULL,
    [AddedByUserId]           NVARCHAR (256)   NOT NULL,
    [UpdatedOnDate]           DATETIME2 (0)    NULL,
    [UpdatedByUserId]         NVARCHAR (256)   NULL,
    [DeletedOnDate]           DATETIME2 (0)    NULL,
    [PublicDescription]       NVARCHAR (MAX)   NULL,
    [Notes]                   NVARCHAR (MAX)   NULL,
    CONSTRAINT [PK_EventSponsorship] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Event_Id___EventSponsorship_EventId] FOREIGN KEY ([EventId]) REFERENCES [dbo].[Event] ([Id])
);



