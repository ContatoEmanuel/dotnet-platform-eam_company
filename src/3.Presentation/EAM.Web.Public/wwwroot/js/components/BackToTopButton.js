import { ScrollManager } from '../utils/ScrollManager';
/**
 * Componente de botão "Voltar ao topo"
 */
export class BackToTopButton {
    constructor(buttonSelector = '#backToTop', showThreshold = 300) {
        this.isVisible = false;
        this.button = document.querySelector(buttonSelector);
        this.scrollManager = new ScrollManager();
        this.showThreshold = showThreshold;
        if (this.button) {
            this.initialize();
        }
        else {
            console.warn(`Botão não encontrado: ${buttonSelector}`);
        }
    }
    /**
     * Inicializa o componente
     */
    initialize() {
        this.attachScrollListener();
        this.attachClickListener();
        this.updateVisibility(); // Verifica estado inicial
    }
    /**
     * Adiciona listener de scroll
     */
    attachScrollListener() {
        let ticking = false;
        window.addEventListener('scroll', () => {
            if (!ticking) {
                window.requestAnimationFrame(() => {
                    this.updateVisibility();
                    ticking = false;
                });
                ticking = true;
            }
        }, { passive: true });
    }
    /**
     * Adiciona listener de click
     */
    attachClickListener() {
        this.button?.addEventListener('click', (e) => {
            e.preventDefault();
            this.scrollManager.scrollToTop();
        });
    }
    /**
     * Atualiza visibilidade do botão baseado no scroll
     */
    updateVisibility() {
        const scrollPosition = this.scrollManager.getScrollPosition();
        const shouldShow = scrollPosition > this.showThreshold;
        if (shouldShow !== this.isVisible) {
            this.isVisible = shouldShow;
            this.toggleVisibility(shouldShow);
        }
    }
    /**
     * Alterna visibilidade do botão
     */
    toggleVisibility(show) {
        if (!this.button)
            return;
        if (show) {
            this.button.classList.add('visible');
        }
        else {
            this.button.classList.remove('visible');
        }
    }
    /**
     * Destrói o componente
     */
    destroy() {
        // Remove listeners se necessário
        this.button = null;
    }
}
//# sourceMappingURL=BackToTopButton.js.map