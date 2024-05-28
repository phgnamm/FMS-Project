CREATE DATABASE [FMSNextBeanEdition]
USE [FMSNextBeanEdition]
       
IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [AspNetRoles] (
    [Id] uniqueidentifier NOT NULL,
    [Description] nvarchar(256) NULL,
    [Name] nvarchar(256) NULL,
    [NormalizedName] nvarchar(256) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetRoles] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [AspNetUsers] (
    [Id] uniqueidentifier NOT NULL,
    [FirstName] nvarchar(50) NOT NULL,
    [LastName] nvarchar(50) NOT NULL,
    [Gender] int NOT NULL,
    [DateOfBirth] datetime2 NOT NULL,
    [Address] nvarchar(max) NULL,
    [Image] nvarchar(max) NULL,
    [RefreshToken] nvarchar(max) NULL,
    [RefreshTokenExpiryTime] datetime2 NULL,
    [VerificationCode] nvarchar(6) NULL,
    [VerificationCodeExpiryTime] datetime2 NULL,
    [CreationDate] datetime2 NOT NULL,
    [CreatedBy] uniqueidentifier NULL,
    [ModificationDate] datetime2 NULL,
    [ModifiedBy] uniqueidentifier NULL,
    [DeletionDate] datetime2 NULL,
    [DeletedBy] uniqueidentifier NULL,
    [IsDeleted] bit NOT NULL,
    [UserName] nvarchar(256) NULL,
    [NormalizedUserName] nvarchar(256) NULL,
    [Email] nvarchar(256) NULL,
    [NormalizedEmail] nvarchar(256) NULL,
    [EmailConfirmed] bit NOT NULL,
    [PasswordHash] nvarchar(max) NULL,
    [SecurityStamp] nvarchar(max) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    [PhoneNumber] nvarchar(15) NULL,
    [PhoneNumberConfirmed] bit NOT NULL,
    [TwoFactorEnabled] bit NOT NULL,
    [LockoutEnd] datetimeoffset NULL,
    [LockoutEnabled] bit NOT NULL,
    [AccessFailedCount] int NOT NULL,
    CONSTRAINT [PK_AspNetUsers] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [AspNetRoleClaims] (
    [Id] int NOT NULL IDENTITY,
    [RoleId] uniqueidentifier NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [AspNetUserClaims] (
    [Id] int NOT NULL IDENTITY,
    [UserId] uniqueidentifier NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [AspNetUserLogins] (
    [LoginProvider] nvarchar(450) NOT NULL,
    [ProviderKey] nvarchar(450) NOT NULL,
    [ProviderDisplayName] nvarchar(max) NULL,
    [UserId] uniqueidentifier NOT NULL,
    CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey]),
    CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [AspNetUserRoles] (
    [UserId] uniqueidentifier NOT NULL,
    [RoleId] uniqueidentifier NOT NULL,
    CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY ([UserId], [RoleId]),
    CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [AspNetUserTokens] (
    [UserId] uniqueidentifier NOT NULL,
    [LoginProvider] nvarchar(450) NOT NULL,
    [Name] nvarchar(450) NOT NULL,
    [Value] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY ([UserId], [LoginProvider], [Name]),
    CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE INDEX [IX_AspNetRoleClaims_RoleId] ON [AspNetRoleClaims] ([RoleId]);
GO

CREATE UNIQUE INDEX [RoleNameIndex] ON [AspNetRoles] ([NormalizedName]) WHERE [NormalizedName] IS NOT NULL;
GO

CREATE INDEX [IX_AspNetUserClaims_UserId] ON [AspNetUserClaims] ([UserId]);
GO

CREATE INDEX [IX_AspNetUserLogins_UserId] ON [AspNetUserLogins] ([UserId]);
GO

CREATE INDEX [IX_AspNetUserRoles_RoleId] ON [AspNetUserRoles] ([RoleId]);
GO

CREATE INDEX [EmailIndex] ON [AspNetUsers] ([NormalizedEmail]);
GO

CREATE UNIQUE INDEX [UserNameIndex] ON [AspNetUsers] ([NormalizedUserName]) WHERE [NormalizedUserName] IS NOT NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240518095844_Initial', N'8.0.4');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [AspNetUsers] ADD [Code] nvarchar(450) NULL;
GO

CREATE TABLE [DeliverableType] (
    [Id] uniqueidentifier NOT NULL,
    [Name] nvarchar(256) NULL,
    [Description] nvarchar(max) NULL,
    [CreationDate] datetime2 NOT NULL,
    [CreatedBy] uniqueidentifier NULL,
    [ModificationDate] datetime2 NULL,
    [ModifiedBy] uniqueidentifier NULL,
    [DeletionDate] datetime2 NULL,
    [DeletedBy] uniqueidentifier NULL,
    [IsDeleted] bit NOT NULL,
    CONSTRAINT [PK_DeliverableType] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Freelancer] (
    [Id] uniqueidentifier NOT NULL,
    [Code] nvarchar(450) NULL,
    [FirstName] nvarchar(50) NULL,
    [LastName] nvarchar(50) NULL,
    [Email] nvarchar(256) NULL,
    [PhoneNumber] nvarchar(15) NULL,
    [Gender] int NOT NULL,
    [Dob] datetime2 NULL,
    [Address] nvarchar(max) NULL,
    [Wallet] real NULL,
    [Image] nvarchar(max) NULL,
    [Status] nvarchar(50) NULL,
    [CreationDate] datetime2 NOT NULL,
    [CreatedBy] uniqueidentifier NULL,
    [ModificationDate] datetime2 NULL,
    [ModifiedBy] uniqueidentifier NULL,
    [DeletionDate] datetime2 NULL,
    [DeletedBy] uniqueidentifier NULL,
    [IsDeleted] bit NOT NULL,
    CONSTRAINT [PK_Freelancer] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [ProjectCategory] (
    [Id] uniqueidentifier NOT NULL,
    [Name] nvarchar(256) NULL,
    [Description] nvarchar(max) NULL,
    [CreationDate] datetime2 NOT NULL,
    [CreatedBy] uniqueidentifier NULL,
    [ModificationDate] datetime2 NULL,
    [ModifiedBy] uniqueidentifier NULL,
    [DeletionDate] datetime2 NULL,
    [DeletedBy] uniqueidentifier NULL,
    [IsDeleted] bit NOT NULL,
    CONSTRAINT [PK_ProjectCategory] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Skill] (
    [Id] uniqueidentifier NOT NULL,
    [Name] nvarchar(256) NULL,
    [Description] nvarchar(max) NULL,
    [Type] nvarchar(50) NULL,
    [CreationDate] datetime2 NOT NULL,
    [CreatedBy] uniqueidentifier NULL,
    [ModificationDate] datetime2 NULL,
    [ModifiedBy] uniqueidentifier NULL,
    [DeletionDate] datetime2 NULL,
    [DeletedBy] uniqueidentifier NULL,
    [IsDeleted] bit NOT NULL,
    CONSTRAINT [PK_Skill] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Project] (
    [Id] uniqueidentifier NOT NULL,
    [Code] nvarchar(450) NULL,
    [Name] nvarchar(256) NULL,
    [Description] nvarchar(max) NULL,
    [Duration] int NOT NULL,
    [Price] real NULL,
    [Status] nvarchar(50) NULL,
    [AccountId] uniqueidentifier NULL,
    [CategoryId] uniqueidentifier NULL,
    [CreationDate] datetime2 NOT NULL,
    [CreatedBy] uniqueidentifier NULL,
    [ModificationDate] datetime2 NULL,
    [ModifiedBy] uniqueidentifier NULL,
    [DeletionDate] datetime2 NULL,
    [DeletedBy] uniqueidentifier NULL,
    [IsDeleted] bit NOT NULL,
    CONSTRAINT [PK_Project] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Project_AspNetUsers_AccountId] FOREIGN KEY ([AccountId]) REFERENCES [AspNetUsers] ([Id]),
    CONSTRAINT [FK_Project_ProjectCategory_CategoryId] FOREIGN KEY ([CategoryId]) REFERENCES [ProjectCategory] ([Id])
);
GO

CREATE TABLE [FreelancerSkill] (
    [Id] uniqueidentifier NOT NULL,
    [FreelancerId] uniqueidentifier NULL,
    [SkillId] uniqueidentifier NULL,
    [CreationDate] datetime2 NOT NULL,
    [CreatedBy] uniqueidentifier NULL,
    [ModificationDate] datetime2 NULL,
    [ModifiedBy] uniqueidentifier NULL,
    [DeletionDate] datetime2 NULL,
    [DeletedBy] uniqueidentifier NULL,
    [IsDeleted] bit NOT NULL,
    CONSTRAINT [PK_FreelancerSkill] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_FreelancerSkill_Freelancer_FreelancerId] FOREIGN KEY ([FreelancerId]) REFERENCES [Freelancer] ([Id]),
    CONSTRAINT [FK_FreelancerSkill_Skill_SkillId] FOREIGN KEY ([SkillId]) REFERENCES [Skill] ([Id])
);
GO

CREATE TABLE [ProjectApply] (
    [Id] uniqueidentifier NOT NULL,
    [ProjectId] uniqueidentifier NULL,
    [FreelancerId] uniqueidentifier NULL,
    [Status] nvarchar(50) NULL,
    [CreationDate] datetime2 NOT NULL,
    [CreatedBy] uniqueidentifier NULL,
    [ModificationDate] datetime2 NULL,
    [ModifiedBy] uniqueidentifier NULL,
    [DeletionDate] datetime2 NULL,
    [DeletedBy] uniqueidentifier NULL,
    [IsDeleted] bit NOT NULL,
    CONSTRAINT [PK_ProjectApply] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ProjectApply_Freelancer_FreelancerId] FOREIGN KEY ([FreelancerId]) REFERENCES [Freelancer] ([Id]),
    CONSTRAINT [FK_ProjectApply_Project_ProjectId] FOREIGN KEY ([ProjectId]) REFERENCES [Project] ([Id])
);
GO

CREATE TABLE [ProjectDeliverable] (
    [Id] uniqueidentifier NOT NULL,
    [Name] nvarchar(256) NULL,
    [SubmissionDate] datetime2 NULL,
    [Status] nvarchar(50) NULL,
    [ProjectId] uniqueidentifier NULL,
    [DeliverableTypeId] uniqueidentifier NULL,
    [CreationDate] datetime2 NOT NULL,
    [CreatedBy] uniqueidentifier NULL,
    [ModificationDate] datetime2 NULL,
    [ModifiedBy] uniqueidentifier NULL,
    [DeletionDate] datetime2 NULL,
    [DeletedBy] uniqueidentifier NULL,
    [IsDeleted] bit NOT NULL,
    CONSTRAINT [PK_ProjectDeliverable] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ProjectDeliverable_DeliverableType_DeliverableTypeId] FOREIGN KEY ([DeliverableTypeId]) REFERENCES [DeliverableType] ([Id]),
    CONSTRAINT [FK_ProjectDeliverable_Project_ProjectId] FOREIGN KEY ([ProjectId]) REFERENCES [Project] ([Id])
);
GO

CREATE TABLE [Transaction] (
    [Id] uniqueidentifier NOT NULL,
    [Code] nvarchar(450) NULL,
    [Description] nvarchar(max) NULL,
    [Status] bit NOT NULL,
    [ProjectId] uniqueidentifier NULL,
    [FreelancerId] uniqueidentifier NULL,
    [CreationDate] datetime2 NOT NULL,
    [CreatedBy] uniqueidentifier NULL,
    [ModificationDate] datetime2 NULL,
    [ModifiedBy] uniqueidentifier NULL,
    [DeletionDate] datetime2 NULL,
    [DeletedBy] uniqueidentifier NULL,
    [IsDeleted] bit NOT NULL,
    CONSTRAINT [PK_Transaction] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Transaction_Freelancer_FreelancerId] FOREIGN KEY ([FreelancerId]) REFERENCES [Freelancer] ([Id]),
    CONSTRAINT [FK_Transaction_Project_ProjectId] FOREIGN KEY ([ProjectId]) REFERENCES [Project] ([Id])
);
GO

CREATE TABLE [DeliverableProduct] (
    [Id] uniqueidentifier NOT NULL,
    [Name] nvarchar(256) NULL,
    [URL] nvarchar(max) NULL,
    [Status] nvarchar(50) NULL,
    [Feedback] nvarchar(max) NULL,
    [ProjectApplyId] uniqueidentifier NULL,
    [ProjectDeliverableId] uniqueidentifier NULL,
    [CreationDate] datetime2 NOT NULL,
    [CreatedBy] uniqueidentifier NULL,
    [ModificationDate] datetime2 NULL,
    [ModifiedBy] uniqueidentifier NULL,
    [DeletionDate] datetime2 NULL,
    [DeletedBy] uniqueidentifier NULL,
    [IsDeleted] bit NOT NULL,
    CONSTRAINT [PK_DeliverableProduct] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_DeliverableProduct_ProjectApply_ProjectApplyId] FOREIGN KEY ([ProjectApplyId]) REFERENCES [ProjectApply] ([Id]),
    CONSTRAINT [FK_DeliverableProduct_ProjectDeliverable_ProjectDeliverableId] FOREIGN KEY ([ProjectDeliverableId]) REFERENCES [ProjectDeliverable] ([Id])
);
GO

CREATE UNIQUE INDEX [IX_AspNetUsers_Code] ON [AspNetUsers] ([Code]) WHERE [Code] IS NOT NULL;
GO

CREATE INDEX [IX_DeliverableProduct_ProjectApplyId] ON [DeliverableProduct] ([ProjectApplyId]);
GO

CREATE INDEX [IX_DeliverableProduct_ProjectDeliverableId] ON [DeliverableProduct] ([ProjectDeliverableId]);
GO

CREATE UNIQUE INDEX [IX_Freelancer_Code] ON [Freelancer] ([Code]) WHERE [Code] IS NOT NULL;
GO

CREATE INDEX [IX_FreelancerSkill_FreelancerId] ON [FreelancerSkill] ([FreelancerId]);
GO

CREATE INDEX [IX_FreelancerSkill_SkillId] ON [FreelancerSkill] ([SkillId]);
GO

CREATE INDEX [IX_Project_AccountId] ON [Project] ([AccountId]);
GO

CREATE INDEX [IX_Project_CategoryId] ON [Project] ([CategoryId]);
GO

CREATE UNIQUE INDEX [IX_Project_Code] ON [Project] ([Code]) WHERE [Code] IS NOT NULL;
GO

CREATE INDEX [IX_ProjectApply_FreelancerId] ON [ProjectApply] ([FreelancerId]);
GO

CREATE INDEX [IX_ProjectApply_ProjectId] ON [ProjectApply] ([ProjectId]);
GO

CREATE INDEX [IX_ProjectDeliverable_DeliverableTypeId] ON [ProjectDeliverable] ([DeliverableTypeId]);
GO

CREATE INDEX [IX_ProjectDeliverable_ProjectId] ON [ProjectDeliverable] ([ProjectId]);
GO

CREATE UNIQUE INDEX [IX_Transaction_Code] ON [Transaction] ([Code]) WHERE [Code] IS NOT NULL;
GO

CREATE INDEX [IX_Transaction_FreelancerId] ON [Transaction] ([FreelancerId]);
GO

CREATE INDEX [IX_Transaction_ProjectId] ON [Transaction] ([ProjectId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240519074651_UpdateEntities', N'8.0.4');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

EXEC sp_rename N'[Freelancer].[Dob]', N'RefreshTokenExpiryTime', N'COLUMN';
GO

ALTER TABLE [Freelancer] ADD [DateOfBirth] datetime2 NULL;
GO

ALTER TABLE [Freelancer] ADD [RefreshToken] nvarchar(max) NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240520103242_UpdateEntitiesV2', N'8.0.4');
GO

COMMIT;
GO

