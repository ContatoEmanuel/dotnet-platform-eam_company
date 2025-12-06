import { IScrollable, IScrollConfig } from '../interfaces/IScrollable';

/**
 * Gerenciador de scroll suave para navegação
 */
export class ScrollManager implements IScrollable {
    private readonly defaultConfig: IScrollConfig = {
        offset: 80,
        behavior: 'smooth'
    };

    private readonly finalConfig: Required<IScrollConfig>;

    constructor(config: Partial<IScrollConfig> = {}) {
        this.finalConfig = { ...this.defaultConfig, ...config } as Required<IScrollConfig>;
    }

    /**
     * Navega para um elemento com scroll suave
     */
    public scrollToElement(target: Element | string, offset?: number): void {
        const element = typeof target === 'string' 
            ? document.querySelector(target) 
            : target;

        if (!element) {
            console.warn(`Elemento não encontrado: ${target}`);
            return;
        }

        const headerOffset: number = offset ?? this.finalConfig.offset;
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
    public scrollToTop(): void {
        window.scrollTo({
            top: 0,
            behavior: this.finalConfig.behavior
        });
    }

    /**
     * Verifica se um elemento está visível no viewport
     */
    public isElementInViewport(element: Element): boolean {
        const rect = element.getBoundingClientRect();
        return (
            rect.top >= 0 &&
            rect.left >= 0 &&
            rect.bottom <= (window.innerHeight || document.documentElement.clientHeight) &&
            rect.right <= (window.innerWidth || document.documentElement.clientWidth)
        );
    }

    /**
     * Obtém a posição de scroll atual
     */
    public getScrollPosition(): number {
        return window.pageYOffset || document.documentElement.scrollTop;
    }
}
