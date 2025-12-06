/**
 * Interface para elementos que suportam scroll suave
 */
export interface IScrollable {
    /**
     * Navega para um elemento específico com scroll suave
     * @param target Elemento de destino ou seletor
     * @param offset Offset em pixels (padrão: 80)
     */
    scrollToElement(target: Element | string, offset?: number): void;

    /**
     * Navega para o topo da página
     */
    scrollToTop(): void;
}

/**
 * Interface para configuração de scroll
 */
export interface IScrollConfig {
    offset: number;
    behavior: ScrollBehavior;
    duration?: number;
}
