/**
 * Interface para links de navegação
 */
export interface INavigationLink {
    element: HTMLAnchorElement;
    target: string;
    isActive: boolean;
}

/**
 * Interface para observer de seções
 */
export interface ISectionObserver {
    /**
     * Observa as seções da página para atualizar navegação ativa
     */
    observe(): void;

    /**
     * Para de observar
     */
    disconnect(): void;
}
