CREATE TABLE [dbo].[SystemErrorLog] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [AddedOnDate] DATETIME2 (7)  NOT NULL,
    [Level]       NVARCHAR (5)   NOT NULL,
    [Type]        NVARCHAR (500) NOT NULL,
    [Message]     NVARCHAR (MAX) NOT NULL,
    [Source]      NVARCHAR (500) NOT NULL,
    [StackTrace]  NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_SystemErrorLog] PRIMARY KEY CLUSTERED ([Id] ASC)
);

