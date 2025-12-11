# 🔒 SECURITY - Diretrizes de Segurança

## ⚠️ AVISO CRÍTICO

Este repositório contém código **CONFIDENCIAL** e **PROPRIETÁRIO**. Consulte [CONFIDENTIAL-NOTICE.md](CONFIDENTIAL-NOTICE.md) para restrições legais.

---

## 🚫 Dados Sensíveis - NUNCA Committar

### Credenciais e Senhas
- ❌ Senhas em texto plano
- ❌ Hashes de senha (exceto em seeds de desenvolvimento)
- ❌ Tokens de API
- ❌ Chaves privadas (`.pfx`, `.p12`, `.key`, `.pem`)
- ❌ Certificados SSL

### Configurações de Ambiente
- ❌ `appsettings.json` (produção)
- ❌ `appsettings.Production.json`
- ❌ `appsettings.Staging.json`
- ❌ Arquivos `.env` (exceto `.env.example`)

### Dados de Banco
- ❌ Arquivos `.db`, `.mdf`, `.ldf`
- ❌ Backups de banco (`.bak`)
- ❌ Scripts SQL com dados reais de clientes
- ❌ Migrations com dados sensíveis

### Ferramentas e Temporários
- ❌ Diretório `temp/`
- ❌ Scripts de geração de senha (`PasswordHasher`)
- ❌ Logs com informações sensíveis

---

## ✅ O que PODE ser commitado

### Estrutura
- ✅ Migrations de schema (sem dados)
- ✅ Scripts SQL de exemplo (`.example.sql`)
- ✅ Configurações de exemplo (`.example.json`)

### Documentação
- ✅ README.md
- ✅ CONFIDENTIAL-NOTICE.md
- ✅ LICENSE
- ✅ Documentação técnica (sem credenciais)

---

## 🛡️ Boas Práticas

### 1. Variáveis de Ambiente
Use `.env` para credenciais (já está no `.gitignore`):
```bash
DB_PASSWORD=senha_forte
JWT_SECRET=token_secreto
API_KEY=chave_api
```

### 2. User Secrets (.NET)
Para desenvolvimento local:
```bash
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:Default" "Server=..."
```

### 3. Azure Key Vault / AWS Secrets Manager
Para produção, use serviços de gerenciamento de secrets.

### 4. Hashes de Senha
- Sempre use `PasswordHasher<T>` do ASP.NET Identity
- Nunca armazene senhas em texto plano
- Seeds de desenvolvimento devem usar hashes pré-computados

---

## 📋 Checklist Antes do Commit

- [ ] Removi todas as senhas/tokens do código
- [ ] Arquivos de configuração estão no `.gitignore`
- [ ] Não há dados reais de clientes
- [ ] Scripts SQL não contêm informações confidenciais
- [ ] Diretório `temp/` não está sendo trackeado
- [ ] Logs não expõem informações sensíveis

---

## 🔍 Auditoria de Segurança

### Verificar arquivos sensíveis trackeados:
```bash
git ls-files | grep -E "\.(env|sql|key|pfx|p12)$"
```

### Remover arquivo do histórico:
```bash
git rm --cached arquivo_sensivel
git commit -m "security: remove arquivo sensível"
```

### Verificar commits anteriores:
```bash
git log --all --full-history -- "*senha*"
```

---

## 📞 Incidente de Segurança

Se você identificou exposição de dados sensíveis:

1. **PARE** - Não faça mais commits
2. **COMUNIQUE** - contato@eam-company.com.br
3. **ROTACIONE** - Troque todas as credenciais expostas
4. **LIMPE** - Remova dados do histórico do git

---

**Copyright © 2025 Emanuel Arrudas de Macêdo - EAM Company**  
**Confidencial e Proprietário**
