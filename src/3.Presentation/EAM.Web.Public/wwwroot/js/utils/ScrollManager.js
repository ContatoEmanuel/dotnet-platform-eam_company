/**
 * Gerenciador de scroll suave para navegação
 */
export class ScrollManager {
    constructor(config = {}) {
        this.defaultConfig = {
            offset: 80,
            behavior: 'smooth'
        };
        this.finalConfig = { ...this.defaultConfig, ...config };
    }
    /**
     * Navega para um elemento com scroll suave
     */
    scrollToElement(target, offset) {
        const element = typeof target === 'string'
            ? document.querySelector(target)
            : target;
        if (!element) {
            console.warn(`Elemento não encontrado: ${target}`);
            return;
        }
        const headerOffset = offset ?? this.finalConfig.offset;
        const elementPosition = element.getBoundingClientRect().top;
        const offsetPosition = elementPosition + window.pageYOffset - headerOffset;
        window.scrollTo({
            top: offsetPosition,
            behavior: this.finalConfig.behavior
        });
    }
    /**
     * Navega para o topo da página
     */
    scrollToTop() {
        window.scrollTo({
            top: 0,
            behavior: this.finalConfig.behavior
        });
    }
    /**
     * Verifica se um elemento está visível no viewport
     */
    isElementInViewport(element) {
        const rect = element.getBoundingClientRect();
        return (rect.top >= 0 &&
            rect.left >= 0 &&
            rect.bottom <= (window.innerHeight || document.documentElement.clientHeight) &&
            rect.right <= (window.innerWidth || document.documentElement.clientWidth));
    }
    /**
     * Obtém a posição de scroll atual
     */
    getScrollPosition() {
        return window.pageYOffset || document.documentElement.scrollTop;
    }
}
//# sourceMappingURL=ScrollManager.js.map