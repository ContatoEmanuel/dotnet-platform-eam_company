# EAM.Web.Public - TypeScript Setup

## 📦 Estrutura TypeScript

```
TypeScript/
├── interfaces/          # Interfaces TypeScript
│   ├── IScrollable.ts
│   └── INavigationLink.ts
├── utils/              # Utilitários
│   └── ScrollManager.ts
├── components/         # Componentes
│   ├── BackToTopButton.ts
│   ├── SmoothScrollLinks.ts
│   ├── ActiveNavigation.ts
│   └── AnimationObserver.ts
└── app.ts             # Entry point
```

## 🚀 Setup

### 1. Instalar dependências

```bash
cd src/3.Presentation/EAM.Web.Public
npm install
```

### 2. Compilar TypeScript

```bash
# Build única vez
npm run build

# Watch mode (recompila automaticamente)
npm run watch

# Build production (minificado)
npm run build:prod
```

### 3. Output

Os arquivos compilados vão para:
```
wwwroot/js/
├── site.js           # Compilado do TypeScript
├── site.js.map       # Source map
└── site.min.js       # Versão minificada (prod)
```

## 📚 Componentes TypeScript

### BackToTopButton
Gerencia botão "Voltar ao topo" com scroll suave.

**Uso:**
```typescript
const button = new BackToTopButton('#backToTop', 300);
```

### SmoothScrollLinks
Adiciona scroll suave a todos os links âncora.

**Uso:**
```typescript
const smoothScroll = new SmoothScrollLinks('a[href^="#"]', 80);
```

### ActiveNavigation
Destaca link ativo baseado na seção visível.

**Uso:**
```typescript
const nav = new ActiveNavigation('.navbar-nav .nav-link', 0.5);
nav.observe();
```

### AnimationObserver
Anima elementos quando aparecem no viewport.

**Uso:**
```typescript
const animator = new AnimationObserver('.animate-on-scroll');
animator.observe();
```

## 🔧 Configuração TypeScript

### tsconfig.json
- **Target:** ES2020
- **Module:** ES2020
- **Strict:** true
- **Source Maps:** Habilitado
- **Declarations:** Habilitado

### Recursos TypeScript Utilizados
- ✅ Interfaces
- ✅ Classes
- ✅ Generics
- ✅ Strict null checks
- ✅ Type guards
- ✅ Intersection Observer API
- ✅ Private/Public modifiers
- ✅ Optional parameters
- ✅ Union types

## 🐛 Debug

O código TypeScript inclui console.log para debug:

```javascript
🚀 Inicializando EAM Public Site...
✅ Back to Top Button inicializado
✅ Smooth Scroll Links inicializado (X links)
✅ Active Navigation inicializado (X seções)
✅ Animation Observer inicializado (X elementos)
✨ EAM Public Site inicializado com sucesso!
```

Para acessar a instância da aplicação no console:
```javascript
window.EAMApp
```

## 📝 Scripts NPM

| Script | Descrição |
|--------|-----------|
| `npm run build` | Compila TypeScript uma vez |
| `npm run watch` | Watch mode (auto-recompila) |
| `npm run build:prod` | Build + minificação |
| `npm run minify` | Minifica JS existente |

## 🔄 Integração com ASP.NET

### _Layout.cshtml
```html
<!-- Development -->
<script src="~/js/site.js" asp-append-version="true"></script>

<!-- Production -->
<script src="~/js/site.min.js" asp-append-version="true"></script>
```

## 📦 Dependências

### DevDependencies
- `typescript` ^5.7.2 - Compilador TypeScript
- `@types/bootstrap` ^5.2.10 - Tipagens Bootstrap
- `terser` ^5.36.0 - Minificador JavaScript

## 🎯 Benefícios da Migração

### ✅ Antes (JavaScript)
```javascript
function scrollToTop() {
    window.scrollTo({ top: 0, behavior: 'smooth' });
}
```

### ✨ Depois (TypeScript)
```typescript
public scrollToTop(): void {
    window.scrollTo({
        top: 0,
        behavior: this.config.behavior
    });
}
```

**Melhorias:**
- ✅ Type safety
- ✅ IntelliSense completo
- ✅ Detecção de erros em tempo de desenvolvimento
- ✅ Refatoração segura
- ✅ Documentação inline (JSDoc)
- ✅ Código mais organizado (classes/interfaces)
- ✅ Melhor manutenibilidade

## 🚀 Próximos Passos

- [ ] Adicionar testes unitários (Jest)
- [ ] Configurar ESLint
- [ ] Adicionar Prettier
- [ ] Criar build automático no Docker
- [ ] Adicionar mais componentes interativos
