CREATE TABLE [dbo].[PersonEmailAddresses] (
    [PersonId]       UNIQUEIDENTIFIER NOT NULL,
    [EmailAddressId] UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_PersonEmailAddresses] PRIMARY KEY CLUSTERED ([PersonId] ASC, [EmailAddressId] ASC),
    CONSTRAINT [FK_EmailAddress_Id___PersonEmailAddresses_EmailAddressId] FOREIGN KEY ([EmailAddressId]) REFERENCES [dbo].[EmailAddress] ([Id]),
    CONSTRAINT [FK_Person_Id___PersonEmailAddresses_PersonId] FOREIGN KEY ([PersonId]) REFERENCES [dbo].[Person] ([Id])
);

