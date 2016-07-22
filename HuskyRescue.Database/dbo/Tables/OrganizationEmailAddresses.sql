CREATE TABLE [dbo].[OrganizationEmailAddresses] (
    [OrganizationId] UNIQUEIDENTIFIER NOT NULL,
    [EmailAddressId] UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_OrganizationEmailAddresses] PRIMARY KEY CLUSTERED ([OrganizationId] ASC, [EmailAddressId] ASC),
    CONSTRAINT [FK_EmailAddress_Id___OrganizationEmailAddresses_EmailAddressId] FOREIGN KEY ([EmailAddressId]) REFERENCES [dbo].[EmailAddress] ([Id]),
    CONSTRAINT [FK_Organization_Id___OrganizationEmailAddresses_OrganizationId] FOREIGN KEY ([OrganizationId]) REFERENCES [dbo].[Organization] ([Id])
);

