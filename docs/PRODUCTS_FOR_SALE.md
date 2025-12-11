# Sistema de Produtos à Venda - Portal Blazor

## Visão Geral

O Portal Blazor agora consome produtos diretamente do banco de dados, filtrando apenas projetos marcados com a flag `ForSale = true`. Isso permite controlar quais projetos são exibidos como produtos comerciais na vitrine pública.

## Estrutura de Dados

### Novas Colunas na Tabela `Projects`

- **`ForSale`** (bit, NOT NULL, default: 0)
  - Indica se o projeto está disponível para venda no Portal
  - `true` = Exibido na vitrine pública
  - `false` = Apenas no portfólio do site MVC

- **`Price`** (decimal(18,2), NULL)
  - Preço do produto
  - `NULL` ou `0.00` = Produto gratuito (exibe "Grátis")
  - Valor > 0 = Exibe preço formatado (ex: R$ 99,99)

## Como Funciona

### 1. Serviço de Produtos (`ProductService`)

```csharp
public interface IProductService
{
    Task<List<Project>> GetProductsForSaleAsync();
    Task<Project?> GetProductByIdAsync(int id);
}
```

O serviço filtra automaticamente:
- `IsActive = true`
- `ForSale = true`
- Ordenado por `DisplayOrder`

### 2. Página Home do Portal

A página `/` (Home.razor) consome o `IProductService` e renderiza:
- Cards dinâmicos para cada produto
- Ícone especial para WhatsApp (detecta "WhatsApp" no título)
- Features do produto (parseadas do campo `Technologies`)
- Preço formatado
- Botão "Teste Grátis"

### 3. Diferenças entre Site Público e Portal

| Característica | Site Público (MVC) | Portal Blazor |
|---|---|---|
| URL | http://localhost:5001 | http://localhost:5002 |
| Filtro | `IsFeatured = true` | `ForSale = true` |
| Finalidade | Portfólio pessoal | Vitrine comercial |
| Projetos exibidos | 7 projetos em destaque | Apenas produtos à venda |

## Gerenciamento de Produtos

### Adicionar Produto à Venda

```sql
-- Marcar projeto existente como produto
UPDATE Projects
SET ForSale = 1,
    Price = 99.99,
    DisplayOrder = 2
WHERE Id = 2;
```

### Remover Produto da Venda

```sql
UPDATE Projects
SET ForSale = 0
WHERE Id = 3;
```

### Atualizar Preço

```sql
UPDATE Projects
SET Price = 0.00  -- Grátis
WHERE Id = 1;
```

### Consultar Produtos à Venda

```sql
SELECT Id, Title, ForSale, Price, DisplayOrder
FROM Projects
WHERE ForSale = 1
ORDER BY DisplayOrder;
```

## Script de Gerenciamento

Use o script completo em:
```
src/2.Infrastructure/EAM.Infra.Data/Scripts/ManageProductsForSale.sql
```

Este script contém exemplos comentados para todas as operações.

## Exemplo Prático

### Produto Atual (WhatsApp Extension)

```sql
Id: 1
Title: WhatsApp Extension for Dynamics
ForSale: true
Price: 0.00 (Grátis)
Technologies: [
  "Integração nativa com Dynamics 365",
  "Interface React moderna e responsiva",
  "Histórico de conversas sincronizado"
]
```

### Adicionar Novo Produto

```sql
INSERT INTO Projects (
    Title,
    Description,
    Technologies,
    IconEmoji,
    ForSale,
    Price,
    DisplayOrder,
    IsActive,
    CreatedAt
)
VALUES (
    'Power BI Integration for Dynamics',
    'Integração avançada entre Power BI e Dynamics 365.',
    '["Dashboards interativos","Relatórios em tempo real"]',
    '📊',
    1,  -- ForSale
    149.99,
    2,
    1,
    GETUTCDATE()
);
```

## Migrations

- **Migration**: `20251209_AddForSaleToProjects.sql`
- **Localização**: `src/2.Infrastructure/EAM.Infra.Data/Migrations/`
- **Status**: ✅ Executada

## Deploy

Após alterar produtos no banco:
1. As mudanças aparecem imediatamente (sem rebuild)
2. O Blazor Server atualiza na próxima requisição
3. Não é necessário reiniciar containers

## Notas Importantes

- ✅ Apenas projetos com `ForSale = true` aparecem no Portal
- ✅ O campo `IsFeatured` ainda é usado pelo Site Público MVC
- ✅ Você pode ter o mesmo projeto em ambos (Featured + ForSale)
- ✅ DisplayOrder controla a ordem de exibição
