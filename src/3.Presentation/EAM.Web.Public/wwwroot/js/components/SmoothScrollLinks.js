import { ScrollManager } from '../utils/ScrollManager';
/**
 * Gerenciador de links com smooth scroll
 */
export class SmoothScrollLinks {
    constructor(linkSelector = 'a[href^="#"]', scrollOffset = 80) {
        this.scrollManager = new ScrollManager({ offset: scrollOffset });
        this.links = document.querySelectorAll(linkSelector);
        this.initialize();
    }
    /**
     * Inicializa os links
     */
    initialize() {
        this.links.forEach(anchor => {
            anchor.addEventListener('click', (e) => {
                this.handleClick(e, anchor);
            });
        });
    }
    /**
     * Manipula o click no link
     */
    handleClick(e, anchor) {
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
        }
        else {
            console.warn(`Target não encontrado para: ${href}`);
        }
    }
    /**
     * Retorna o número de links encontrados
     */
    getLinksCount() {
        return this.links.length;
    }
}
//# sourceMappingURL=SmoothScrollLinks.js.map