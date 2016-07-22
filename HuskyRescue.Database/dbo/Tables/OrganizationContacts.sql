CREATE TABLE [dbo].[OrganizationContacts] (
    [OrganizationId] UNIQUEIDENTIFIER NOT NULL,
    [PersonId]       UNIQUEIDENTIFIER NOT NULL,
    [Role]           NVARCHAR (50)    NOT NULL,
    CONSTRAINT [PK_OrganizationContacts] PRIMARY KEY CLUSTERED ([OrganizationId] ASC, [PersonId] ASC),
    CONSTRAINT [FK_Organization_Id___OrganizationContacts_OrganizationId] FOREIGN KEY ([OrganizationId]) REFERENCES [dbo].[Organization] ([Id]),
    CONSTRAINT [FK_Person_Id___OrganizationContacts_PersonId] FOREIGN KEY ([PersonId]) REFERENCES [dbo].[Person] ([Id])
);

