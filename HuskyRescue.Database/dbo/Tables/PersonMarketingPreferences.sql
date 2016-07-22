CREATE TABLE [dbo].[PersonMarketingPreferences] (
    [PersonId]               UNIQUEIDENTIFIER NOT NULL,
    [MarketingPreferencesId] UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_PersonMarketingPreferences] PRIMARY KEY CLUSTERED ([PersonId] ASC, [MarketingPreferencesId] ASC),
    CONSTRAINT [FK_MarketingPreferences_Id___PersonMarketingPreferences_MarketingPreferencesId] FOREIGN KEY ([MarketingPreferencesId]) REFERENCES [dbo].[MarketingPreferences] ([Id]),
    CONSTRAINT [FK_Person_Id___PersonMarketingPreferences_PersonId] FOREIGN KEY ([PersonId]) REFERENCES [dbo].[Person] ([Id])
);

