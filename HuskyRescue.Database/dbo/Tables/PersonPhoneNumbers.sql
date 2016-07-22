CREATE TABLE [dbo].[PersonPhoneNumbers] (
    [PersonId]      UNIQUEIDENTIFIER NOT NULL,
    [PhoneNumberId] UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_PersonPhoneNumbers] PRIMARY KEY CLUSTERED ([PersonId] ASC, [PhoneNumberId] ASC),
    CONSTRAINT [FK_Person_Id___PersonPhoneNumbers_PersonId] FOREIGN KEY ([PersonId]) REFERENCES [dbo].[Person] ([Id]),
    CONSTRAINT [FK_PhoneNumber_Id___PersonPhoneNumbers_PhoneNumber_Id] FOREIGN KEY ([PhoneNumberId]) REFERENCES [dbo].[PhoneNumber] ([Id])
);

