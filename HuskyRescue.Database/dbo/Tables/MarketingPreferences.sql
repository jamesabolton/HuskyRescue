CREATE TABLE [dbo].[MarketingPreferences] (
    [Id]          UNIQUEIDENTIFIER CONSTRAINT [DF_MarketingPreferences_Id] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    [IsEmailable] BIT              NOT NULL,
    [IsMailable]  BIT              NOT NULL,
    CONSTRAINT [PK_MarketingPreferences] PRIMARY KEY CLUSTERED ([Id] ASC)
);

