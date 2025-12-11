# ÉPICO 2: Site Institucional (ASP.NET MVC)

## 📋 Visão Geral

Site institucional da EAM Company desenvolvido em ASP.NET MVC, baseado no design do portfólio profissional em React (https://eam-company.com.br).

## ✅ US-04: Layout Master e Home Page

### Objetivos
- [x] Criar layout master (_Layout.cshtml) com navegação e rodapé
- [x] Desenvolver home page (Index.cshtml) com todas as seções
- [x] Adaptar design React/Tailwind CSS para Bootstrap 5
- [x] Implementar responsividade e interatividade

### Componentes Implementados

#### 1. Layout Master (`Views/Shared/_Layout.cshtml`)
**Recursos:**
- ✅ Navbar fixa com fundo blur effect
- ✅ Smooth scrolling para navegação por âncoras
- ✅ Links: Início, Sobre, Projetos, Experiência, Contato
- ✅ Botão CTA "Portal do Cliente" (link para Blazor Portal)
- ✅ Rodapé com informações da empresa e redes sociais
- ✅ Botão "Voltar ao topo"

**Tecnologias:**
- Bootstrap 5.3.0
- Bootstrap Icons 1.11.1
- **TypeScript 5.7.2** (compilado para JavaScript ES2020)
- Custom CSS (site.css)

#### 2. Home Page (`Views/Home/Index.cshtml`)

##### Seção Hero
- Badge "🚀 Disponível para novos projetos"
- Foto de perfil (placeholder com iniciais "EM")
- Nome e título profissional
- Stack tecnológico principal
- Botões de redes sociais:
  - GitHub: https://github.com/ContatoEmanuel
  - LinkedIn: https://www.linkedin.com/in/emanuel-arrudas-macedo/
  - YouTube: https://www.youtube.com/channel/UCQbZlecPawGlx0F6Oo56LDw
  - Linktree: https://linktr.ee/ContatoEmanuel
  - Credly: https://www.credly.com/users/emanuel-a-macedo/badges
  - MS Learn: https://learn.microsoft.com/pt-br/users/emanuelarrudasmacedo-1105/
- CTAs: "Conheça mais" e "Download CV"

##### Seção Sobre
- **Descrição profissional:** Senior Software Engineer focado em ecossistema Microsoft
- **Principais Clientes:**
  - Gerdau (Metalurgia)
  - Suzano (Papel e Celulose)
  - Alesat (Varejo)
  - Cogna (Educação)
- **Competências:**
  1. ⚙️ Desenvolvimento Dynamics 365
  2. ⚡ Power Platform & Mobilidade
  3. ☁️ Integração & Azure
  4. 🔒 Dados & Segurança
- **Tech Stack:** Dynamics 365, Power Apps, Power Automate, C# .NET, Azure, Dataverse, SQL Server, TypeScript, React, Azure DevOps, Logic Apps, API Management

##### Seção Projetos
**Projeto Destaque 1:** Dynamics 365 WhatsApp Connector
- Extensão Chrome para integração WhatsApp Web + Dynamics 365
- GitHub: https://github.com/ContatoEmanuel/react-extension-dynamics_whatsapp
- Stack: TypeScript, Chrome Extension, Azure AD, Dynamics 365, React

**Projeto Destaque 2:** React Portfolio Website
- Website profissional com React, TypeScript e Tailwind CSS
- Live: https://eam-company.com.br
- GitHub: https://github.com/ContatoEmanuel/react-front-portfolio
- Stack: React, TypeScript, Tailwind CSS, Vite, Vercel

##### Seção Experiência
**Microsoft Dynamics 365 Consultant**
- Empresa: Tecno IT Group
- Cliente: Gerdau
- Período: mar/2023 - atualmente
- Local: Grande Vitória e Região, Espírito Santo, Brasil
- Stack: Dynamics 365, C# .NET, JavaScript, Power Platform, Azure

##### Seção Contato
- **WhatsApp:** +55 38 98860-1152
  - Link: https://wa.me/5538988601152
- **Email:** contato.emanuel97@gmail.com
  - Link: mailto:contato.emanuel97@gmail.com

### Estilização

#### CSS Customizado (`wwwroot/css/site.css`)
**Recursos implementados:**
- Smooth scroll behavior
- Navbar blur effect com backdrop-filter
- Gradientes (blue-to-indigo matching portfolio)
- Hover effects em cards (translateY + shadow)
- Animações fade-in
- Badges estilizados
- Botão back-to-top com gradiente
- Responsividade mobile-first

**Paleta de Cores:**
- Primary: #3B82F6 (Blue)
- Secondary: #6366F1 (Indigo)
- Success: #10B981 (Green)
- Warning: #F59E0B (Amber)

#### JavaScript (`wwwroot/js/site.js`)
**Funcionalidades:**
- Smooth scroll para links âncora
- Back to top button (aparece após 300px de scroll)
- Active navigation link highlighting
- Intersection Observer para animações on scroll

**⚠️ SUBSTITUÍDO POR TYPESCRIPT:**
O código JavaScript foi completamente reescrito em **TypeScript 5.7.2** com:
- ✅ Type safety completo
- ✅ Interfaces e classes organizadas
- ✅ Strict null checks
- ✅ IntelliSense avançado
- ✅ Código modular e manutenível
- ✅ Source maps para debugging
- ✅ Compilação automática no Docker build

**Estrutura TypeScript:**
```
TypeScript/
├── interfaces/
│   ├── IScrollable.ts
│   └── INavigationLink.ts
├── utils/
│   └── ScrollManager.ts
├── components/
│   ├── BackToTopButton.ts
│   ├── SmoothScrollLinks.ts
│   ├── ActiveNavigation.ts
│   └── AnimationObserver.ts
└── app.ts (entry point)
```
src/3.Presentation/EAM.Web.Public/
├── TypeScript/                           # ✨ Código TypeScript source
│   ├── interfaces/
│   │   ├── IScrollable.ts
│   │   └── INavigationLink.ts
│   ├── utils/
│   │   └── ScrollManager.ts
│   ├── components/
│   │   ├── BackToTopButton.ts
│   │   ├── SmoothScrollLinks.ts
│   │   ├── ActiveNavigation.ts
│   │   └── AnimationObserver.ts
│   └── app.ts
├── Views/
│   ├── Shared/
│   │   └── _Layout.cshtml               # Layout master com navbar e footer
│   └── Home/
│       └── Index.cshtml                  # Home page com todas as seções
├── wwwroot/
│   ├── css/
│   │   └── site.css                     # CSS customizado
│   └── js/
│       ├── app.js                       # ✨ TypeScript compilado
│       ├── app.js.map                   # Source map para debug
│       └── components/                  # Módulos compilados
├── package.json                          # NPM dependencies
├── tsconfig.json                         # TypeScript configuration
├── Program.cs                            # Configuração da aplicação
└── Dockerfile                            # Container configuration
```https://[seu-codespace-url]-5001.app.github.dev
   ```

### Localmente
```bash
# Reconstruir e executar
docker-compose up --build -d web-public

# Acessar
http://localhost:5001
```

## 📦 Estrutura de Arquivos

```
src/3.Presentation/EAM.Web.Public/
├── Views/
│   ├── Shared/
│   │   └── _Layout.cshtml           # Layout master com navbar e footer
│   └── Home/
│       └── Index.cshtml              # Home page com todas as seções
├── wwwroot/
│   ├── css/
│   │   └── site.css                 # CSS customizado
│   └── js/
│       └── site.js                  # JavaScript customizado
├── Program.cs                        # Configuração da aplicação
└── Dockerfile                        # Container configuration
```

## 🎨 Design System

### Componentes Bootstrap Utilizados
- Navbar
- Cards
- Badges
- Buttons
- Grid System
- Utilities (spacing, colors, shadows)

### Customizações
- Gradientes personalizados
## ✨ Tecnologias

- ASP.NET Core MVC 10.0
- **TypeScript 5.7.2** → JavaScript ES2020 (type-safe)
- Bootstrap 5.3.0
- Bootstrap Icons 1.11.1
- Node.js 20.x (build toolchain)
- Docker & Docker Compose

### 🎯 TypeScript Features
- ✅ Strict mode habilitado
- ✅ Interfaces para contratos
- ✅ Classes com modificadores de acesso
- ✅ Type guards e null safety
- ✅ Intersection Observer API tipado
- ✅ Source maps para debugging
- ✅ Declarations (.d.ts) geradas
- ✅ Compilação automática no Docker
**Breakpoints:**
- Mobile: < 768px
- Tablet: 768px - 1024px
- Desktop: > 1024px

**Ajustes Mobile:**
- Navbar colapsável
- Grid adaptativo (col-12 → col-md-6)
- Font-sizes reduzidos
- Spacing otimizado

## 🔄 Próximos Passos

### Melhorias Futuras
- [ ] Adicionar mais projetos na seção Portfolio
- [ ] Implementar timeline completo de experiência
- [ ] Criar página de blog
- [ ] Adicionar formulário de contato funcional
- [ ] Integrar com Google Analytics
- [ ] Implementar página de currículo completo
- [ ] Adicionar animações mais sofisticadas (AOS library)
- [ ] Criar modo dark/light theme

### Otimizações
- [ ] Lazy loading de imagens
- [ ] Minificação de CSS/JS
- [ ] CDN para assets estáticos
- [ ] Service Worker para PWA
- [ ] Image optimization (WebP)

## 📚 Referências

- **Design Original:** https://eam-company.com.br
- **GitHub Portfolio:** https://github.com/ContatoEmanuel/react-front-portfolio
- **Bootstrap 5:** https://getbootstrap.com/docs/5.3/
- **Bootstrap Icons:** https://icons.getbootstrap.com/

## ✨ Tecnologias

- ASP.NET Core MVC 10.0
- Bootstrap 5.3.0
- Bootstrap Icons 1.11.1
- JavaScript ES6+
- Docker & Docker Compose

## 👨‍💻 Autor

**Emanuel A Macêdo**
- GitHub: [@ContatoEmanuel](https://github.com/ContatoEmanuel)
- LinkedIn: [emanuel-arrudas-macedo](https://www.linkedin.com/in/emanuel-arrudas-macedo/)
- Email: contato.emanuel97@gmail.com

---

**Status:** ✅ Concluído
**Data:** 2025
**Versão:** 1.0.0
