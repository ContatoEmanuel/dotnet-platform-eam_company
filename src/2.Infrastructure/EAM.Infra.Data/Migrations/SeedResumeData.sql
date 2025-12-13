-- Migration: AddResumeEntities
-- Criação das tabelas de Currículo

-- PersonalInfos
CREATE TABLE PersonalInfos (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    FullName NVARCHAR(200) NOT NULL,
    Title NVARCHAR(200) NOT NULL,
    Location NVARCHAR(200),
    Email NVARCHAR(200),
    Phone NVARCHAR(50),
    LinkedIn NVARCHAR(500),
    GitHub NVARCHAR(500),
    Summary NVARCHAR(2000),
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
    UpdatedAt DATETIME2 NULL
);

-- Experiences
CREATE TABLE Experiences (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    JobTitle NVARCHAR(200) NOT NULL,
    Company NVARCHAR(200) NOT NULL,
    Location NVARCHAR(200),
    StartDate DATETIME2 NOT NULL,
    EndDate DATETIME2 NULL,
    IsCurrentJob BIT NOT NULL DEFAULT 0,
    Description NVARCHAR(2000),
    Technologies NVARCHAR(MAX), -- Stored as CSV
    DisplayOrder INT NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
    UpdatedAt DATETIME2 NULL
);

-- Educations
CREATE TABLE Educations (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Degree NVARCHAR(300) NOT NULL,
    Institution NVARCHAR(200) NOT NULL,
    StartDate DATETIME2 NOT NULL,
    EndDate DATETIME2 NULL,
    IsInProgress BIT NOT NULL DEFAULT 0,
    Description NVARCHAR(1000),
    DisplayOrder INT NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
    UpdatedAt DATETIME2 NULL
);

-- Certifications
CREATE TABLE Certifications (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(300) NOT NULL,
    Issuer NVARCHAR(200) NOT NULL,
    IssueDate DATETIME2 NOT NULL,
    CredentialCode NVARCHAR(100),
    CredentialUrl NVARCHAR(500),
    Description NVARCHAR(1000),
    Technologies NVARCHAR(MAX), -- Stored as CSV
    Type INT NOT NULL, -- 1=MicrosoftCertified, 2=MicrosoftAppliedSkills, 3=Other
    DisplayOrder INT NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
    UpdatedAt DATETIME2 NULL
);

-- Skills
CREATE TABLE Skills (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(200) NOT NULL,
    Category INT NOT NULL, -- 1=PowerPlatform, 2=Development, 3=Other
    DisplayOrder INT NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
    UpdatedAt DATETIME2 NULL
);

-- Languages
CREATE TABLE Languages (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    ProficiencyLevel NVARCHAR(100) NOT NULL,
    DisplayOrder INT NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
    UpdatedAt DATETIME2 NULL
);

-- Seed Personal Info
INSERT INTO PersonalInfos (FullName, Title, Location, Email, Phone, LinkedIn, GitHub, Summary, IsActive)
VALUES 
('Emanuel A Macêdo', 
 'Senior Software Engineer | Dynamics 365 | Power Platform | .NET C# | Azure',
 'Bebedouro, SP',
 'contato.emanuel97@gmail.com',
 '',
 'https://www.linkedin.com/in/emanuel-a-macedo/',
 'https://github.com/ContatoEmanuel',
 'Desenvolvedor sênior especializado em Dynamics 365, Power Platform e .NET, com experiência em cloud computing e arquitetura de software.',
 1);

-- Seed Experiences
INSERT INTO Experiences (JobTitle, Company, Location, StartDate, EndDate, IsCurrentJob, Description, Technologies, DisplayOrder)
VALUES
('Software Engineer', 'Confidential Company', 'Remoto', '2024-10-01', NULL, 1, 
 'Desenvolvimento de soluções empresariais utilizando Microsoft Dynamics 365 e Power Platform.',
 'Dynamics 365,Power Apps,Power Automate,Azure DevOps,C#', 1);

-- Seed Education
INSERT INTO Educations (Degree, Institution, StartDate, EndDate, IsInProgress, Description, DisplayOrder)
VALUES
('Curso Superior de Tecnologia (CST) em Análise e Desenvolvimento de Sistemas', 
 'Universidade de Franca', 
 '2024-08-01', 
 '2026-07-31', 
 1, 
 NULL, 
 1);

-- Seed Certifications (Microsoft Certified)
INSERT INTO Certifications (Name, Issuer, IssueDate, CredentialCode, Description, Technologies, Type, DisplayOrder)
VALUES
('Microsoft Certified: Dynamics 365 Fundamentals (CRM)', 'Microsoft', '2022-12-01', '6DDFF53C4C7BE301', 
 'Fundamentos da plataforma Microsoft Dynamics 365 Customer Engagement.',
 'Microsoft Dynamics 365,Microsoft CRM,Microsoft Power Platform', 1, 1),
('Microsoft Certified: Power Platform Fundamentals', 'Microsoft', '2022-08-01', 'AFA32BEF041EA6E5',
 'Fundamentos da Microsoft Power Platform incluindo Power Apps, Power Automate, Power BI e Power Virtual Agents.',
 'Microsoft Power Automate,Microsoft Power Virtual Agents,Microsoft Power Platform,Microsoft Power BI', 1, 2),
('Microsoft Certified: Azure Fundamentals', 'Microsoft', '2022-04-01', '74EDD934A9DDB798',
 'Fundamentos da plataforma Microsoft Azure incluindo serviços de nuvem, DevOps e Functions.',
 'Azure DevOps,Azure Functions,Windows Azure', 1, 3);

-- Seed Certifications (Microsoft Applied Skills)
INSERT INTO Certifications (Name, Issuer, IssueDate, CredentialCode, Description, Technologies, Type, DisplayOrder)
VALUES
('Microsoft Applied Skills: Criar e gerenciar aplicativos baseados em modelo', 'Microsoft', '2025-01-01', 'C8FF6F7BA37C5B36',
 'Criar e gerenciar aplicativos baseados em modelo com o Power Apps e o Dataverse.',
 'Dataverse,Microsoft Dynamics 365,Power Apps', 2, 4),
('Microsoft Applied Skills: Criar e gerenciar processos automatizados', 'Microsoft', '2024-12-01', '2DD0B21C91F261F8',
 'Criar e gerenciar processos automatizados usando o Power Automate.',
 'Microsoft Power Automate', 2, 5),
('Microsoft Applied Skills: Criar e gerenciar aplicativos de tela', 'Microsoft', '2024-10-01', 'C1693C7A38490D4A',
 'Criar e gerenciar aplicativos de tela com o Power Apps.',
 'Microsoft Power Apps', 2, 6);

-- Seed Certifications (Other)
INSERT INTO Certifications (Name, Issuer, IssueDate, CredentialCode, Description, Technologies, Type, DisplayOrder)
VALUES
('CEFR Level A1', 'EF English Live', '2023-04-01', NULL,
 'Certificação de proficiência em inglês como segunda língua - Nível A1 (Iniciante).',
 'ESL (Inglês como segunda língua)', 3, 7);

-- Seed Skills (Power Platform)
INSERT INTO Skills (Name, Category, DisplayOrder)
VALUES
('Microsoft Power Apps (Canvas & Model-Driven)', 1, 1),
('Microsoft Power Automate (Cloud Flows & Desktop Flows)', 1, 2),
('Microsoft Power Pages', 1, 3),
('Microsoft Dataverse', 1, 4),
('Microsoft Dynamics 365', 1, 5),
('PCF (Power Apps Component Framework)', 1, 6),
('Power Query / DAX', 1, 7);

-- Seed Skills (Development)
INSERT INTO Skills (Name, Category, DisplayOrder)
VALUES
('C# .NET Framework/Core', 2, 8),
('JavaScript/TypeScript', 2, 9),
('React.js', 2, 10),
('REST APIs & Web Services', 2, 11),
('SQL Server & T-SQL', 2, 12);

-- Seed Languages
INSERT INTO Languages (Name, ProficiencyLevel, DisplayOrder)
VALUES
('Português', 'Nativo', 1),
('Inglês', 'Básico (A1 - CEFR)', 2);

PRINT 'Resume data seeded successfully!';
