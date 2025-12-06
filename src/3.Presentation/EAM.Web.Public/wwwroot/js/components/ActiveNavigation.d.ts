import { ISectionObserver } from '../interfaces/INavigationLink';
/**
 * Gerenciador de navegação ativa baseado no scroll
 */
export declare class ActiveNavigation implements ISectionObserver {
    private threshold;
    private navLinks;
    private sections;
    private observer;
    private currentActiveLink;
    constructor(navSelector?: string, threshold?: number);
    /**
     * Inicializa as seções baseadas nos links de navegação
     */
    private initializeSections;
    /**
     * Observa as seções para atualizar navegação ativa
     */
    observe(): void;
    /**
     * Define o link ativo
     */
    private setActiveLink;
    /**
     * Para de observar
     */
    disconnect(): void;
    /**
     * Retorna número de seções observadas
     */
    getSectionsCount(): number;
}
//# sourceMappingURL=ActiveNavigation.d.ts.map