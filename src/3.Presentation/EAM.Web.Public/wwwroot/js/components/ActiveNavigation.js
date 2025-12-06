/**
 * Gerenciador de navegação ativa baseado no scroll
 */
export class ActiveNavigation {
    constructor(navSelector = '.navbar-nav .nav-link[href^="#"]', threshold = 0.5) {
        this.threshold = threshold;
        this.sections = [];
        this.observer = null;
        this.currentActiveLink = null;
        this.navLinks = document.querySelectorAll(navSelector);
        this.initializeSections();
    }
    /**
     * Inicializa as seções baseadas nos links de navegação
     */
    initializeSections() {
        this.navLinks.forEach(link => {
            const href = link.getAttribute('href');
            if (href && href !== '#') {
                const section = document.querySelector(href);
                if (section) {
                    this.sections.push(section);
                }
            }
        });
    }
    /**
     * Observa as seções para atualizar navegação ativa
     */
    observe() {
        const options = {
            root: null,
            rootMargin: '-100px 0px -66%',
            threshold: this.threshold
        };
        this.observer = new IntersectionObserver((entries) => {
            entries.forEach(entry => {
                if (entry.isIntersecting) {
                    this.setActiveLink(entry.target.id);
                }
            });
        }, options);
        this.sections.forEach(section => {
            if (this.observer) {
                this.observer.observe(section);
            }
        });
    }
    /**
     * Define o link ativo
     */
    setActiveLink(sectionId) {
        // Remove active da navegação atual
        if (this.currentActiveLink) {
            this.currentActiveLink.classList.remove('active');
        }
        // Encontra e ativa o novo link
        this.navLinks.forEach(link => {
            const href = link.getAttribute('href');
            if (href === `#${sectionId}`) {
                link.classList.add('active');
                this.currentActiveLink = link;
            }
        });
    }
    /**
     * Para de observar
     */
    disconnect() {
        if (this.observer) {
            this.observer.disconnect();
            this.observer = null;
        }
    }
    /**
     * Retorna número de seções observadas
     */
    getSectionsCount() {
        return this.sections.length;
    }
}
//# sourceMappingURL=ActiveNavigation.js.map