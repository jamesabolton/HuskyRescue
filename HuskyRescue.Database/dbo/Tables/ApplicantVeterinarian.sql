CREATE TABLE [dbo].[ApplicantVeterinarian] (
    [Id]          UNIQUEIDENTIFIER CONSTRAINT [DF_ApplicantVeterinarians_ID] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    [NameOffice]  NVARCHAR (50)    NULL,
    [NameDr]      NVARCHAR (50)    NULL,
    [PhoneNumber] NVARCHAR (15)    NULL,
    CONSTRAINT [PK_ApplicantVeterinarians_1] PRIMARY KEY CLUSTERED ([Id] ASC)
);

