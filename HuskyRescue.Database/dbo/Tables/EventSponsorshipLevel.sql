CREATE TABLE [dbo].[EventSponsorshipLevel] (
    [Id]               UNIQUEIDENTIFIER CONSTRAINT [DF_EventSponsorshipLevel_Id] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    [Name]             NVARCHAR (200)   NOT NULL,
    [SponsorshipOrder] INT              NOT NULL,
    CONSTRAINT [PK_EventSponsorshipLevel] PRIMARY KEY CLUSTERED ([Id] ASC)
);

