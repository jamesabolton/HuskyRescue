CREATE TABLE [dbo].[EventSponsorshipItem] (
    [Id]                 INT              IDENTITY (1, 1) NOT NULL,
    [EventSponsorshipId] UNIQUEIDENTIFIER NOT NULL,
    [Value]              NVARCHAR (MAX)   NOT NULL,
    CONSTRAINT [PK_EventSponsorshipItem] PRIMARY KEY CLUSTERED ([Id] ASC, [EventSponsorshipId] ASC)
);

