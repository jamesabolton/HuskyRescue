CREATE TABLE [dbo].[EventSponsorLogo] (
    [FileId]         UNIQUEIDENTIFIER NOT NULL,
    [EventSponsorId] UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_EventSponsorLogo] PRIMARY KEY CLUSTERED ([FileId] ASC, [EventSponsorId] ASC),
    CONSTRAINT [FK_EventSponsor_Id__EventSponsorLogo_EventSponsorId] FOREIGN KEY ([EventSponsorId]) REFERENCES [dbo].[EventSponsor] ([Id]),
    CONSTRAINT [FK_File_Id__EventSponsorLogo_Id] FOREIGN KEY ([FileId]) REFERENCES [dbo].[File] ([Id])
);

