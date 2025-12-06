import { ScrollManager } from '../utils/ScrollManager';

/**
 * Gerenciador de links com smooth scroll
 */
export class SmoothScrollLinks {
    private scrollManager: ScrollManager;
    private links: NodeListOf<HTMLAnchorElement>;

    constructor(
        linkSelector: string = 'a[href^="#"]',
        scrollOffset: number = 80
    ) {
        this.scrollManager = new ScrollManager({ offset: scrollOffset });
        this.links = document.querySelectorAll(linkSelector);
        this.initialize();
    }

    /**
     * Inicializa os links
     */
    private initialize(): void {
        this.links.forEach(anchor => {
            anchor.addEventListener('click', (e: Event) => {
                this.handleClick(e, anchor);
            });
        });
    }

    /**
     * Manipula o click no link
     */
    private handleClick(e: Event, anchor: HTMLAnchorElement): void {
        const href = anchor.getAttribute('href');
        
        // Ignora links vazios ou apenas '#'
        if (!href || href === '#' || href === '') {
            return;
        }

        e.preventDefault();

        const target = document.querySelector(href);
        if (target) {
            this.scrollManager.scrollToElement(target);
            
            // Atualiza URL sem scroll
            if (history.pushState) {
                history.pushState(null, '', href);
            }
        } else {
            console.warn(`Target não encontrado para: ${href}`);
        }
    }

    /**
     * Retorna o número de links encontrados
     */
    public getLinksCount(): number {
        return this.links.length;
    }
}
