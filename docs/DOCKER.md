# Como Executar o Projeto com Docker no Linux

## Pré-requisitos

- Docker instalado
- Docker Compose instalado

## Configuração

1. **Configure as variáveis de ambiente**

   O arquivo `.env` já está configurado com valores padrão. Você pode alterá-los conforme necessário:
   
   ```bash
   # Visualizar o arquivo
   cat .env
   ```

## Executar o Projeto

### Iniciar todos os serviços

```bash
docker-compose up -d
```

Este comando irá:
- Baixar e configurar o SQL Server 2022
- Construir e iniciar a API (porta 5000)
- Construir e iniciar o Site Institucional (porta 5001)
- Construir e iniciar o Portal do Cliente (porta 5002)

### Verificar o status dos containers

```bash
docker-compose ps
```

### Visualizar os logs

```bash
# Todos os serviços
docker-compose logs -f

# Serviço específico
docker-compose logs -f api
docker-compose logs -f web-portal
docker-compose logs -f web-public
docker-compose logs -f sqlserver
```

## Acessar as Aplicações

Após iniciar os containers, você pode acessar:

- **API (JSON puro)**: http://localhost:5000/weatherforecast
- **Documentação da API (Scalar)**: http://localhost:5000/scalar/v1
- **OpenAPI Spec**: http://localhost:5000/openapi/v1.json
- **Site Institucional (MVC) - COM interface gráfica**: http://localhost:5001
- **Portal do Cliente (Blazor) - COM interface gráfica**: http://localhost:5002
- **SQL Server**: localhost:1433 (usuário: sa, senha: definida no .env)

### 📝 Importante
- A **API (porta 5000)** retorna dados JSON puros (sem interface HTML)
- Para visualizar a documentação interativa da API, use `/scalar/v1`
- Os sites **MVC (5001)** e **Blazor (5002)** têm interface gráfica completa

## Comandos Úteis

### Parar os serviços

```bash
docker-compose stop
```

### Parar e remover os containers

```bash
docker-compose down
```

### Parar e remover containers e volumes (ATENÇÃO: isso apaga o banco de dados)

```bash
docker-compose down -v
```

### Reconstruir as imagens

```bash
docker-compose build
```

### Reconstruir e reiniciar

```bash
docker-compose up --build -d
```

### Executar migrations do banco de dados

```bash
# Quando você criar migrations, execute:
docker-compose exec api dotnet ef database update
```

## Estrutura de Portas

| Serviço | Porta Externa | Porta Interna |
|---------|---------------|---------------|
| API | 5000 | 8080 |
| Web Public (MVC) | 5001 | 8080 |
| Web Portal (Blazor) | 5002 | 8080 |
| SQL Server | 1433 | 1433 |

## Problemas Comuns

### Container reiniciando constantemente

Verifique os logs:
```bash
docker-compose logs [nome-do-servico]
```

### Porta já em uso

Se alguma porta já estiver em uso, você pode alterar no `docker-compose.yml`:
```yaml
ports:
  - "NOVA_PORTA:8080"  # Altere NOVA_PORTA
```

### Limpar tudo e começar do zero

```bash
docker-compose down -v
docker system prune -a
docker-compose up --build -d
```

## Variáveis de Ambiente (.env)

```env
# Ambiente de execução
ASPNETCORE_ENVIRONMENT=Development

# Banco de Dados
DB_PASSWORD=YourStrong@Password123
DB_NAME=EAM_Database

# JWT Configuration
JWT_SECRET_KEY=YourSuperSecretKeyForJWT_MinimumLength32Characters!
JWT_ISSUER=EAM.API
JWT_AUDIENCE=EAM.Client
JWT_EXPIRATION_HOURS=24
```

**IMPORTANTE**: Nunca commite o arquivo `.env` com senhas reais. Use `.env.example` como template.
