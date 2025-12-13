# Variáveis de Ambiente - Guia de Configuração

Este documento descreve todas as variáveis de ambiente utilizadas no projeto EAM Platform e como configurá-las para diferentes ambientes (Desenvolvimento, Staging, Produção).

## 🔧 Configuração por Aplicação

### 1. EAM.Web.API (Backend API)

**Arquivo:** `appsettings.json` / Variáveis de Ambiente

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=EAM_Database;..."
  },
  "AppSettings": {
    "CompanyName": "EAM Company",
    "DefaultAuthor": "Emanuel Macêdo"
  },
  "JwtSettings": {
    "SecretKey": "Sua chave secreta aqui",
    "Issuer": "EAM.Web.API",
    "Audience": "EAM.Clients",
    "ExpirationHours": 24
  }
}
```

**Variáveis de Ambiente (Docker/Produção):**
- `ConnectionStrings__DefaultConnection` - String de conexão do banco de dados
- `AppSettings__CompanyName` - Nome da empresa
- `AppSettings__DefaultAuthor` - Nome do autor padrão para posts
- `JwtSettings__SecretKey` - **OBRIGATÓRIO** - Chave secreta JWT (mínimo 32 caracteres)
- `JwtSettings__Issuer` - Emissor do token JWT
- `JwtSettings__Audience` - Audiência do token JWT
- `JwtSettings__ExpirationHours` - Tempo de expiração do token em horas

---

### 2. EAM.Web.Public (Site Institucional)

**Arquivo:** `appsettings.json` / Variáveis de Ambiente

```json
{
  "ApiSettings": {
    "BaseUrl": "http://localhost:5000"
  },
  "AppSettings": {
    "PortalUrl": "http://localhost:5002",
    "CompanyName": "EAM Company",
    "AuthorName": "Emanuel Macêdo",
    "SocialMedia": {
      "LinkedIn": "https://www.linkedin.com/in/...",
      "GitHub": "https://github.com/...",
      "YouTube": "https://www.youtube.com/...",
      "Linktree": "https://linktr.ee/...",
      "Credly": "https://www.credly.com/...",
      "MicrosoftLearn": "https://learn.microsoft.com/...",
      "WhatsApp": "https://wa.me/..."
    }
  }
}
```

**Variáveis de Ambiente (Docker/Produção):**
- `ApiSettings__BaseUrl` - URL base da API (ex: `https://api.seudominio.com`)
- `AppSettings__PortalUrl` - URL do portal do cliente
- `AppSettings__CompanyName` - Nome da empresa
- `AppSettings__AuthorName` - Nome do autor
- `AppSettings__SocialMedia__LinkedIn` - URL do perfil LinkedIn
- `AppSettings__SocialMedia__GitHub` - URL do perfil GitHub
- `AppSettings__SocialMedia__YouTube` - URL do canal YouTube
- `AppSettings__SocialMedia__Linktree` - URL do Linktree
- `AppSettings__SocialMedia__Credly` - URL do perfil Credly
- `AppSettings__SocialMedia__MicrosoftLearn` - URL do perfil Microsoft Learn
- `AppSettings__SocialMedia__WhatsApp` - URL do WhatsApp Business

---

### 3. EAM.Web.Portal (Portal do Cliente)

**Arquivo:** `appsettings.json` / Variáveis de Ambiente

```json
{
  "ApiSettings": {
    "BaseUrl": "http://localhost:5000"
  },
  "AppSettings": {
    "PublicWebsiteUrl": "http://localhost:5001",
    "CompanyName": "EAM Company"
  }
}
```

**Variáveis de Ambiente (Docker/Produção):**
- `ApiSettings__BaseUrl` - URL base da API
- `AppSettings__PublicWebsiteUrl` - URL do site institucional
- `AppSettings__CompanyName` - Nome da empresa

---

## 🐳 Docker Compose - Configuração de Produção

### Exemplo de `.env` para Docker Compose:

```env
# Database
DB_NAME=EAM_Database
DB_PASSWORD=YourStrongPassword123!

# Environment
ASPNETCORE_ENVIRONMENT=Production

# API URLs
API_BASE_URL=https://api.seudominio.com
PORTAL_URL=https://portal.seudominio.com
PUBLIC_WEBSITE_URL=https://www.seudominio.com

# Company Info
COMPANY_NAME=Sua Empresa Ltda
AUTHOR_NAME=Seu Nome Completo

# JWT Settings
JWT_SECRET_KEY=SuaChaveSecretaSuperSeguraComMaisde32Caracteres!
JWT_ISSUER=SuaEmpresa.API
JWT_AUDIENCE=SuaEmpresa.Clients
JWT_EXPIRATION_HOURS=24

# Social Media URLs
SOCIAL_LINKEDIN=https://www.linkedin.com/in/seu-perfil/
SOCIAL_GITHUB=https://github.com/seu-usuario
SOCIAL_YOUTUBE=https://www.youtube.com/channel/seu-canal
SOCIAL_LINKTREE=https://linktr.ee/seu-usuario
SOCIAL_CREDLY=https://www.credly.com/users/seu-usuario
SOCIAL_MSLEARN=https://learn.microsoft.com/pt-br/users/seu-usuario
SOCIAL_WHATSAPP=https://wa.me/5511999999999
```

### Atualização do docker-compose.yml:

```yaml
services:
  api:
    environment:
      - ASPNETCORE_ENVIRONMENT=${ASPNETCORE_ENVIRONMENT}
      - ConnectionStrings__DefaultConnection=Server=sqlserver;Database=${DB_NAME};User Id=sa;Password=${DB_PASSWORD};...
      - AppSettings__CompanyName=${COMPANY_NAME}
      - AppSettings__DefaultAuthor=${AUTHOR_NAME}
      - JwtSettings__SecretKey=${JWT_SECRET_KEY}
      - JwtSettings__Issuer=${JWT_ISSUER}
      - JwtSettings__Audience=${JWT_AUDIENCE}
      - JwtSettings__ExpirationHours=${JWT_EXPIRATION_HOURS}

  web-public:
    environment:
      - ASPNETCORE_ENVIRONMENT=${ASPNETCORE_ENVIRONMENT}
      - ApiSettings__BaseUrl=${API_BASE_URL}
      - AppSettings__PortalUrl=${PORTAL_URL}
      - AppSettings__CompanyName=${COMPANY_NAME}
      - AppSettings__AuthorName=${AUTHOR_NAME}
      - AppSettings__SocialMedia__LinkedIn=${SOCIAL_LINKEDIN}
      - AppSettings__SocialMedia__GitHub=${SOCIAL_GITHUB}
      - AppSettings__SocialMedia__YouTube=${SOCIAL_YOUTUBE}
      - AppSettings__SocialMedia__Linktree=${SOCIAL_LINKTREE}
      - AppSettings__SocialMedia__Credly=${SOCIAL_CREDLY}
      - AppSettings__SocialMedia__MicrosoftLearn=${SOCIAL_MSLEARN}
      - AppSettings__SocialMedia__WhatsApp=${SOCIAL_WHATSAPP}

  web-portal:
    environment:
      - ASPNETCORE_ENVIRONMENT=${ASPNETCORE_ENVIRONMENT}
      - ApiSettings__BaseUrl=${API_BASE_URL}
      - AppSettings__PublicWebsiteUrl=${PUBLIC_WEBSITE_URL}
      - AppSettings__CompanyName=${COMPANY_NAME}
```

---

## 🔐 Segurança - Boas Práticas

### ⚠️ NUNCA faça:
- ❌ Commitar arquivos `.env` no repositório
- ❌ Usar valores padrão em produção
- ❌ Compartilhar chaves secretas por e-mail/chat
- ❌ Usar senhas fracas

### ✅ SEMPRE faça:
- ✅ Use Azure Key Vault ou AWS Secrets Manager em produção
- ✅ Mantenha chaves JWT com mínimo 32 caracteres
- ✅ Rotacione credenciais periodicamente
- ✅ Use diferentes valores por ambiente
- ✅ Configure variáveis de ambiente no seu provedor de nuvem

---

## 🚀 Deploy em Produção

### Azure App Service:
```bash
az webapp config appsettings set --name seu-app \
  --resource-group seu-grupo \
  --settings \
    ApiSettings__BaseUrl=https://api.seudominio.com \
    AppSettings__CompanyName="Sua Empresa"
```

### AWS ECS:
Configure as variáveis no `task-definition.json`:
```json
{
  "environment": [
    {"name": "ApiSettings__BaseUrl", "value": "https://api.seudominio.com"},
    {"name": "AppSettings__CompanyName", "value": "Sua Empresa"}
  ]
}
```

### Kubernetes:
Crie um ConfigMap e Secret:
```yaml
apiVersion: v1
kind: ConfigMap
metadata:
  name: eam-config
data:
  ApiSettings__BaseUrl: "https://api.seudominio.com"
  AppSettings__CompanyName: "Sua Empresa"
---
apiVersion: v1
kind: Secret
metadata:
  name: eam-secrets
type: Opaque
stringData:
  JwtSettings__SecretKey: "sua-chave-secreta"
  ConnectionStrings__DefaultConnection: "sua-connection-string"
```

---

## 📝 Checklist de Deploy

Antes de fazer deploy para produção, verifique:

- [ ] Todas as URLs apontam para domínios de produção
- [ ] Chave JWT é forte e única (32+ caracteres)
- [ ] Connection string usa credenciais de produção
- [ ] Variáveis de ambiente estão configuradas no provedor de nuvem
- [ ] Logs estão configurados corretamente
- [ ] HTTPS está habilitado
- [ ] Certificados SSL são válidos
- [ ] Firewall/Security Groups configurados
- [ ] Backup do banco de dados configurado

---

## 🆘 Troubleshooting

### Problema: "API não responde"
**Solução:** Verifique se `ApiSettings__BaseUrl` está correto e acessível

### Problema: "Erro de autenticação JWT"
**Solução:** Confirme que `JwtSettings__SecretKey` é a mesma na API e nos clientes

### Problema: "Links de redes sociais quebrados"
**Solução:** Verifique se todas as variáveis `AppSettings__SocialMedia__*` estão configuradas

---

## 📞 Suporte

Para dúvidas sobre configuração, consulte:
- **Documentação:** `/docs`
- **Issues:** GitHub Issues do projeto
- **Contato:** emanuel.macedo@email.com
