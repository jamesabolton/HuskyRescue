﻿CREATE TABLE [dbo].[Website] (
    [Id]      UNIQUEIDENTIFIER CONSTRAINT [DF_Website_Id] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    [Name]    NVARCHAR (200)   NOT NULL,
    [Website] NVARCHAR (1000)  NOT NULL,
    CONSTRAINT [PK_Website] PRIMARY KEY CLUSTERED ([Id] ASC)
);
