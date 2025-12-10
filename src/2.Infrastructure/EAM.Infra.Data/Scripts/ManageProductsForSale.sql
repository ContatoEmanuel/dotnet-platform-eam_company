-- Script para gerenciar produtos à venda no Portal
-- Use este script para marcar/desmarcar projetos como produtos à venda

USE EAM_Database;
GO

-- ============================================
-- CONSULTAR PRODUTOS À VENDA
-- ============================================
SELECT 
    Id,
    Title,
    Description,
    ForSale,
    Price,
    IsFeatured,
    DisplayOrder,
    IsActive
FROM Projects
WHERE ForSale = 1
ORDER BY DisplayOrder;
GO

-- ============================================
-- ADICIONAR PRODUTO À VENDA
-- ============================================
-- Exemplo: Marcar projeto ID 2 como produto à venda
/*
UPDATE Projects
SET ForSale = 1,
    Price = 99.99,  -- Defina o preço ou NULL para grátis
    DisplayOrder = 2
WHERE Id = 2;
GO
*/

-- ============================================
-- REMOVER PRODUTO DA VENDA
-- ============================================
-- Exemplo: Remover projeto ID 3 da venda
/*
UPDATE Projects
SET ForSale = 0
WHERE Id = 3;
GO
*/

-- ============================================
-- ATUALIZAR PREÇO DO PRODUTO
-- ============================================
-- Exemplo: Atualizar preço do WhatsApp Extension
/*
UPDATE Projects
SET Price = 0.00  -- Grátis
WHERE Id = 1;
GO
*/

-- ============================================
-- REORDENAR PRODUTOS
-- ============================================
-- Exemplo: Definir ordem de exibição
/*
UPDATE Projects SET DisplayOrder = 1 WHERE Id = 1;  -- WhatsApp Extension primeiro
UPDATE Projects SET DisplayOrder = 2 WHERE Id = 5;  -- Outro produto segundo
GO
*/

-- ============================================
-- INSERIR NOVO PRODUTO PARA VENDA
-- ============================================
-- Exemplo de novo produto
/*
INSERT INTO Projects (
    Title,
    Description,
    Technologies,
    GithubUrl,
    LiveUrl,
    ThumbnailUrl,
    IconEmoji,
    IsFeatured,
    ForSale,
    Price,
    DisplayOrder,
    IsActive,
    CreatedAt
)
VALUES (
    'Power BI Integration for Dynamics',
    'Integração avançada entre Power BI e Dynamics 365 com dashboards personalizados.',
    '["Dashboards interativos","Relatórios em tempo real","Integração nativa"]',
    NULL,
    NULL,
    NULL,
    '📊',
    0,
    1,  -- ForSale = true
    149.99,
    2,
    1,
    GETUTCDATE()
);
GO
*/

PRINT 'Script de gerenciamento de produtos pronto para uso!';
PRINT 'Descomente os exemplos acima para executar as operações.';
GO
