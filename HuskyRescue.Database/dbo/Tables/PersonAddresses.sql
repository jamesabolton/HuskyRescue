CREATE TABLE [dbo].[PersonAddresses] (
    [PersonId]  UNIQUEIDENTIFIER NOT NULL,
    [AddressId] UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_PersonAddresses] PRIMARY KEY CLUSTERED ([PersonId] ASC, [AddressId] ASC),
    CONSTRAINT [FK_Address_Id___PersonAddresses_AddressId] FOREIGN KEY ([AddressId]) REFERENCES [dbo].[Address] ([Id]),
    CONSTRAINT [FK_Person_Id___PersonAddresses_PersonId] FOREIGN KEY ([PersonId]) REFERENCES [dbo].[Person] ([Id])
);

