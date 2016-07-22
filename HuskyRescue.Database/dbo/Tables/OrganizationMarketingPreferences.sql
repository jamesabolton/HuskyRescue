CREATE TABLE [dbo].[OrganizationMarketingPreferences] (
    [OrganizationId]         UNIQUEIDENTIFIER NOT NULL,
    [MarketingPreferencesId] UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_OrganizationMarketingPreferences] PRIMARY KEY CLUSTERED ([OrganizationId] ASC, [MarketingPreferencesId] ASC),
    CONSTRAINT [FK_MarketingPreferences_Id___OrganizationMarketingPreferences_MarketingPreferencesId] FOREIGN KEY ([MarketingPreferencesId]) REFERENCES [dbo].[MarketingPreferences] ([Id]),
    CONSTRAINT [FK_Organization_Id___OrganizationMarketingPreferences_OrganizationId] FOREIGN KEY ([OrganizationId]) REFERENCES [dbo].[Organization] ([Id])
);

