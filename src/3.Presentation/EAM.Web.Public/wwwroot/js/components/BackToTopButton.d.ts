/**
 * Componente de botão "Voltar ao topo"
 */
export declare class BackToTopButton {
    private button;
    private scrollManager;
    private showThreshold;
    private isVisible;
    constructor(buttonSelector?: string, showThreshold?: number);
    /**
     * Inicializa o componente
     */
    private initialize;
    /**
     * Adiciona listener de scroll
     */
    private attachScrollListener;
    /**
     * Adiciona listener de click
     */
    private attachClickListener;
    /**
     * Atualiza visibilidade do botão baseado no scroll
     */
    private updateVisibility;
    /**
     * Alterna visibilidade do botão
     */
    private toggleVisibility;
    /**
     * Destrói o componente
     */
    destroy(): void;
}
//# sourceMappingURL=BackToTopButton.d.ts.map