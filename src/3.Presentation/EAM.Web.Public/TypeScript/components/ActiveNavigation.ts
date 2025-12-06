import { ISectionObserver } from '../interfaces/INavigationLink';

/**
 * Gerenciador de navegação ativa baseado no scroll
 */
export class ActiveNavigation implements ISectionObserver {
    private navLinks: NodeListOf<HTMLAnchorElement>;
    private sections: Element[] = [];
    private observer: IntersectionObserver | null = null;
    private currentActiveLink: HTMLAnchorElement | null = null;

    constructor(
        navSelector: string = '.navbar-nav .nav-link[href^="#"]',
        private threshold: number = 0.5
    ) {
        this.navLinks = document.querySelectorAll(navSelector);
        this.initializeSections();
    }

    /**
     * Inicializa as seções baseadas nos links de navegação
     */
    private initializeSections(): void {
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
    public observe(): void {
        const options: IntersectionObserverInit = {
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
    private setActiveLink(sectionId: string): void {
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
    public disconnect(): void {
        if (this.observer) {
            this.observer.disconnect();
            this.observer = null;
        }
    }

    /**
     * Retorna número de seções observadas
     */
    public getSectionsCount(): number {
        return this.sections.length;
    }
}
