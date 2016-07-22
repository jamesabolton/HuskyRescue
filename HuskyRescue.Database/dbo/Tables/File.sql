CREATE TABLE [dbo].[File] (
    [Id]          UNIQUEIDENTIFIER CONSTRAINT [DF_File_Id] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    [Name]        NVARCHAR (500)   NOT NULL,
    [FileTypeId]  INT              NOT NULL,
    [ContentType] NVARCHAR (100)   NOT NULL,
    [Content]     VARBINARY (MAX)  NOT NULL,
    CONSTRAINT [PK_File] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_FileType_Id__File_FileTypeId] FOREIGN KEY ([FileTypeId]) REFERENCES [dbo].[FileType] ([Id])
);

