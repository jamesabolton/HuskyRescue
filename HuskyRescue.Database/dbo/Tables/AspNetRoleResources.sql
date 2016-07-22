CREATE TABLE [dbo].[AspNetRoleResources] (
    [RoleId]     NVARCHAR (128) NOT NULL,
    [ResourceId] NVARCHAR (128) NOT NULL,
    [Operations] INT            NOT NULL,
    CONSTRAINT [PK_AspNetRoleResources] PRIMARY KEY CLUSTERED ([RoleId] ASC, [ResourceId] ASC),
    CONSTRAINT [FK_AspNetResources_Id__AspNetRoleResources_ResourceId] FOREIGN KEY ([ResourceId]) REFERENCES [dbo].[AspNetResources] ([Id]),
    CONSTRAINT [FK_AspNetRoles_Id__AspNetRoleResources_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[AspNetRoles] ([Id])
);

