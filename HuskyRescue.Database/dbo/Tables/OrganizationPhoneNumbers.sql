CREATE TABLE [dbo].[OrganizationPhoneNumbers] (
    [OrganizationId] UNIQUEIDENTIFIER NOT NULL,
    [PhoneNumberId]  UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_OrganizationPhoneNumbers] PRIMARY KEY CLUSTERED ([OrganizationId] ASC, [PhoneNumberId] ASC),
    CONSTRAINT [FK_Organization_Id___OrganizationPhoneNumbers_OrganizationId] FOREIGN KEY ([OrganizationId]) REFERENCES [dbo].[Organization] ([Id]),
    CONSTRAINT [FK_PhoneNumber_Id___OrganizationPhoneNumbers_PhoneNumberId] FOREIGN KEY ([PhoneNumberId]) REFERENCES [dbo].[PhoneNumber] ([Id])
);

