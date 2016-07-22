CREATE TABLE [dbo].[PersonWebsites] (
    [PersonId]  UNIQUEIDENTIFIER NOT NULL,
    [WebsiteId] UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_PersonWebsites] PRIMARY KEY CLUSTERED ([PersonId] ASC, [WebsiteId] ASC),
    CONSTRAINT [FK_Person_Id___PersonWebsites_PersonId] FOREIGN KEY ([PersonId]) REFERENCES [dbo].[Person] ([Id]),
    CONSTRAINT [FK_Website_Id___PersonWebsites_WebsiteId] FOREIGN KEY ([WebsiteId]) REFERENCES [dbo].[Website] ([Id])
);

