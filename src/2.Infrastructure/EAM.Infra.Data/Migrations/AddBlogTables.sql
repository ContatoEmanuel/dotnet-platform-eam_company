-- Migration: Adicionar tabelas do Blog
-- Data: 2024-12-13

-- Criar tabela BlogCategories
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'BlogCategories')
BEGIN
    CREATE TABLE BlogCategories (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Name NVARCHAR(100) NOT NULL,
        Slug NVARCHAR(150) NOT NULL UNIQUE,
        Description NVARCHAR(500) NULL,
        Color NVARCHAR(20) NOT NULL DEFAULT '#3B82F6',
        DisplayOrder INT NOT NULL DEFAULT 0,
        CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        IsActive BIT NOT NULL DEFAULT 1
    );

    CREATE INDEX IX_BlogCategories_Slug ON BlogCategories(Slug);
    CREATE INDEX IX_BlogCategories_IsActive ON BlogCategories(IsActive);
END
GO

-- Criar tabela BlogPosts
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'BlogPosts')
BEGIN
    CREATE TABLE BlogPosts (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Title NVARCHAR(200) NOT NULL,
        Slug NVARCHAR(250) NOT NULL UNIQUE,
        Excerpt NVARCHAR(500) NOT NULL,
        Content NVARCHAR(MAX) NOT NULL,
        ImageUrl NVARCHAR(500) NULL,
        Author NVARCHAR(100) NOT NULL DEFAULT 'Emanuel Macêdo',
        ReadTimeMinutes INT NOT NULL DEFAULT 5,
        IsPublished BIT NOT NULL DEFAULT 0,
        PublishedAt DATETIME2 NULL,
        ViewCount INT NOT NULL DEFAULT 0,
        DisplayOrder INT NOT NULL DEFAULT 0,
        CategoryId INT NOT NULL,
        Tags NVARCHAR(MAX) NOT NULL DEFAULT '[]',
        CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        UpdatedAt DATETIME2 NULL,
        IsActive BIT NOT NULL DEFAULT 1,
        CONSTRAINT FK_BlogPosts_BlogCategories FOREIGN KEY (CategoryId) 
            REFERENCES BlogCategories(Id) ON DELETE CASCADE
    );

    CREATE INDEX IX_BlogPosts_Slug ON BlogPosts(Slug);
    CREATE INDEX IX_BlogPosts_CategoryId ON BlogPosts(CategoryId);
    CREATE INDEX IX_BlogPosts_IsPublished ON BlogPosts(IsPublished);
    CREATE INDEX IX_BlogPosts_IsActive ON BlogPosts(IsActive);
    CREATE INDEX IX_BlogPosts_PublishedAt ON BlogPosts(PublishedAt DESC);
END
GO

-- Seed de categorias
IF NOT EXISTS (SELECT * FROM BlogCategories WHERE Slug = 'dynamics-365')
BEGIN
    INSERT INTO BlogCategories (Name, Slug, Description, Color, DisplayOrder, CreatedAt)
    VALUES 
        ('Dynamics 365', 'dynamics-365', 'Artigos sobre Dynamics 365 Customer Engagement, Sales, Marketing e muito mais', '#3B82F6', 1, '2024-01-01'),
        ('Power Platform', 'power-platform', 'Tutoriais e dicas sobre Power Apps, Power Automate, Power BI e Power Pages', '#6366F1', 2, '2024-01-01'),
        ('.NET', 'dotnet', 'Desenvolvimento com .NET, C#, ASP.NET Core e arquitetura de software', '#8B5CF6', 3, '2024-01-01');
END
GO

-- Seed de posts
IF NOT EXISTS (SELECT * FROM BlogPosts WHERE Slug = 'comecando-com-dynamics-365-ce')
BEGIN
    DECLARE @CategoryD365 INT = (SELECT Id FROM BlogCategories WHERE Slug = 'dynamics-365');
    DECLARE @CategoryPower INT = (SELECT Id FROM BlogCategories WHERE Slug = 'power-platform');
    DECLARE @CategoryDotnet INT = (SELECT Id FROM BlogCategories WHERE Slug = 'dotnet');

    INSERT INTO BlogPosts (Title, Slug, Excerpt, Content, ImageUrl, Author, ReadTimeMinutes, IsPublished, PublishedAt, CategoryId, Tags, DisplayOrder, CreatedAt)
    VALUES 
    (
        'Começando com Dynamics 365 Customer Engagement',
        'comecando-com-dynamics-365-ce',
        'Aprenda os conceitos fundamentais do Dynamics 365 CE e como começar seu primeiro projeto.',
        '<p class="lead">O Dynamics 365 Customer Engagement é uma plataforma poderosa para gerenciar relacionamentos com clientes.</p><h2>O que é Dynamics 365 CE?</h2><p>Dynamics 365 Customer Engagement (CE) é uma solução CRM (Customer Relationship Management) que ajuda organizações a gerenciar vendas, marketing, atendimento ao cliente e operações de campo.</p><h2>Principais Componentes</h2><ul><li><strong>Sales:</strong> Gerenciamento de oportunidades e pipeline de vendas</li><li><strong>Marketing:</strong> Automação de marketing e gestão de campanhas</li><li><strong>Customer Service:</strong> Atendimento ao cliente e gestão de casos</li><li><strong>Field Service:</strong> Operações de campo e agendamento</li></ul><h2>Por onde começar?</h2><p>Para iniciar com Dynamics 365, recomendo seguir estes passos:</p><ol><li>Criar uma conta de trial no Microsoft 365</li><li>Acessar o Power Platform Admin Center</li><li>Provisionar um ambiente Dynamics 365</li><li>Explorar os aplicativos model-driven disponíveis</li></ol><p>Nos próximos artigos, vamos aprofundar em cada um desses componentes e criar soluções práticas.</p>',
        'https://via.placeholder.com/1200x400/3B82F6/FFFFFF?text=Dynamics+365',
        'Emanuel Macêdo',
        5,
        1,
        '2024-12-10 10:00:00',
        @CategoryD365,
        '["Dynamics 365","CRM","Microsoft","Tutorial"]',
        1,
        '2024-12-10 10:00:00'
    ),
    (
        'Power Platform: Automatizando processos com Power Automate',
        'power-platform-automatizando-com-power-automate',
        'Descubra como criar automações poderosas usando Power Automate e integrar com diversas aplicações.',
        '<p class="lead">Power Automate é a ferramenta de automação da Microsoft que permite conectar aplicativos e automatizar fluxos de trabalho.</p><h2>O que é Power Automate?</h2><p>Power Automate (anteriormente conhecido como Microsoft Flow) é uma plataforma de automação low-code que permite criar workflows automatizados entre aplicativos e serviços.</p><h2>Tipos de Fluxos</h2><ul><li><strong>Cloud Flows:</strong> Automações na nuvem disparadas por eventos</li><li><strong>Desktop Flows:</strong> RPA (Robotic Process Automation) para automação de desktop</li><li><strong>Business Process Flows:</strong> Guias visuais para processos padronizados</li></ul><h2>Casos de Uso Comuns</h2><ol><li>Aprovação de documentos e workflows</li><li>Sincronização de dados entre sistemas</li><li>Notificações e alertas automatizados</li><li>Coleta e processamento de dados</li></ol><p>Em breve publicarei tutoriais práticos mostrando como criar seus primeiros fluxos.</p>',
        'https://via.placeholder.com/1200x400/6366F1/FFFFFF?text=Power+Automate',
        'Emanuel Macêdo',
        7,
        1,
        '2024-12-08 14:30:00',
        @CategoryPower,
        '["Power Platform","Power Automate","Automação","Low-Code"]',
        2,
        '2024-12-08 14:30:00'
    ),
    (
        'Arquitetura Clean em .NET: Princípios e Práticas',
        'arquitetura-clean-dotnet-principios-praticas',
        'Entenda os conceitos de Clean Architecture e como aplicar em projetos .NET Core.',
        '<p class="lead">Clean Architecture é um padrão arquitetural que promove a separação de responsabilidades e independência de frameworks.</p><h2>O que é Clean Architecture?</h2><p>Proposta por Robert C. Martin (Uncle Bob), Clean Architecture organiza o código em camadas concêntricas, onde as dependências apontam sempre para dentro, em direção às regras de negócio.</p><h2>Camadas Principais</h2><ul><li><strong>Domain:</strong> Entidades e regras de negócio core</li><li><strong>Application:</strong> Casos de uso e interfaces de serviço</li><li><strong>Infrastructure:</strong> Implementações de persistência e serviços externos</li><li><strong>Presentation:</strong> UI, APIs e interfaces de usuário</li></ul><h2>Benefícios</h2><ol><li>Testabilidade: código facilmente testável</li><li>Manutenibilidade: mudanças isoladas em camadas específicas</li><li>Flexibilidade: troca de frameworks sem impacto no core</li><li>Independência: não acoplamento com tecnologias específicas</li></ol><p>Este projeto é um exemplo prático de Clean Architecture em .NET!</p>',
        'https://via.placeholder.com/1200x400/8B5CF6/FFFFFF?text=Clean+Architecture',
        'Emanuel Macêdo',
        10,
        1,
        '2024-12-05 09:00:00',
        @CategoryDotnet,
        '[".NET","Clean Architecture","Design Patterns","Boas Práticas"]',
        3,
        '2024-12-05 09:00:00'
    );
END
GO

PRINT 'Tabelas do Blog criadas com sucesso!'
GO
