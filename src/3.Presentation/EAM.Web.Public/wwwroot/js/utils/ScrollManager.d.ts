import { IScrollable, IScrollConfig } from '../interfaces/IScrollable';
/**
 * Gerenciador de scroll suave para navegação
 */
export declare class ScrollManager implements IScrollable {
    private readonly defaultConfig;
    private readonly finalConfig;
    constructor(config?: Partial<IScrollConfig>);
    /**
     * Navega para um elemento com scroll suave
     */
    scrollToElement(target: Element | string, offset?: number): void;
    /**
     * Navega para o topo da página
     */
    scrollToTop(): void;
    /**
     * Verifica se um elemento está visível no viewport
     */
    isElementInViewport(element: Element): boolean;
    /**
     * Obtém a posição de scroll atual
     */
    getScrollPosition(): number;
}
//# sourceMappingURL=ScrollManager.d.ts.map