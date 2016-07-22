CREATE TABLE [dbo].[ApplicantOwnedAnimals] (
    [Id]                    UNIQUEIDENTIFIER CONSTRAINT [DF_ApplicantOwnedAnimals_Id] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    [ApplicantId]           UNIQUEIDENTIFIER NOT NULL,
    [PersonId]              UNIQUEIDENTIFIER NOT NULL,
    [Name]                  NVARCHAR (50)    NOT NULL,
    [Breed]                 NVARCHAR (20)    NOT NULL,
    [Gender]                NVARCHAR (10)    NOT NULL,
    [AgeMonths]             NVARCHAR (100)   NULL,
    [OwnershipLengthMonths] NVARCHAR (100)   NULL,
    [IsAltered]             BIT              NOT NULL,
    [AlteredReason]         NVARCHAR (200)   NULL,
    [IsHwPrevention]        BIT              NOT NULL,
    [HwPreventionReason]    NVARCHAR (200)   NULL,
    [IsFullyVaccinated]     BIT              NOT NULL,
    [FullyVaccinatedReason] NVARCHAR (200)   NULL,
    [IsStillOwned]          BIT              NOT NULL,
    [IsStillOwnedReason]    NVARCHAR (200)   NULL,
    CONSTRAINT [PK_ApplicantOwnedAnimals] PRIMARY KEY CLUSTERED ([Id] ASC, [ApplicantId] ASC, [PersonId] ASC),
    CONSTRAINT [FK_Applicant_Id___ApplicantOwnedAnimals_ApplicantId] FOREIGN KEY ([ApplicantId]) REFERENCES [dbo].[Applicant] ([Id])
);

