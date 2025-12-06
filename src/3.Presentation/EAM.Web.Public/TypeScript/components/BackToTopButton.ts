import { ScrollManager } from '../utils/ScrollManager';

/**
 * Componente de botão "Voltar ao topo"
 */
export class BackToTopButton {
    private button: HTMLElement | null;
    private scrollManager: ScrollManager;
    private showThreshold: number;
    private isVisible: boolean = false;

    constructor(
        buttonSelector: string = '#backToTop',
        showThreshold: number = 300
    ) {
        this.button = document.querySelector(buttonSelector);
        this.scrollManager = new ScrollManager();
        this.showThreshold = showThreshold;

        if (this.button) {
            this.initialize();
        } else {
            console.warn(`Botão não encontrado: ${buttonSelector}`);
        }
    }

    /**
     * Inicializa o componente
     */
    private initialize(): void {
        this.attachScrollListener();
        this.attachClickListener();
        this.updateVisibility(); // Verifica estado inicial
    }

    /**
     * Adiciona listener de scroll
     */
    private attachScrollListener(): void {
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
    private attachClickListener(): void {
        this.button?.addEventListener('click', (e: Event) => {
            e.preventDefault();
            this.scrollManager.scrollToTop();
        });
    }

    /**
     * Atualiza visibilidade do botão baseado no scroll
     */
    private updateVisibility(): void {
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
    private toggleVisibility(show: boolean): void {
        if (!this.button) return;

        if (show) {
            this.button.classList.add('visible');
        } else {
            this.button.classList.remove('visible');
        }
    }

    /**
     * Destrói o componente
     */
    public destroy(): void {
        // Remove listeners se necessário
        this.button = null;
    }
}
