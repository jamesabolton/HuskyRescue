CREATE TABLE [dbo].[OrganizationWebsites] (
    [OrganizationId] UNIQUEIDENTIFIER NOT NULL,
    [WebsiteId]      UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_OrganizationWebsites] PRIMARY KEY CLUSTERED ([OrganizationId] ASC, [WebsiteId] ASC),
    CONSTRAINT [FK_Organization_Id___OrganizationWebsites_OrganizationId] FOREIGN KEY ([OrganizationId]) REFERENCES [dbo].[Organization] ([Id]),
    CONSTRAINT [FK_Website_Id___OrganizationWebsites_WebsiteId] FOREIGN KEY ([WebsiteId]) REFERENCES [dbo].[Website] ([Id])
);

