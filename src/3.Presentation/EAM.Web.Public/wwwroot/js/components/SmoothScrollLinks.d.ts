/**
 * Gerenciador de links com smooth scroll
 */
export declare class SmoothScrollLinks {
    private scrollManager;
    private links;
    constructor(linkSelector?: string, scrollOffset?: number);
    /**
     * Inicializa os links
     */
    private initialize;
    /**
     * Manipula o click no link
     */
    private handleClick;
    /**
     * Retorna o número de links encontrados
     */
    getLinksCount(): number;
}
//# sourceMappingURL=SmoothScrollLinks.d.ts.map