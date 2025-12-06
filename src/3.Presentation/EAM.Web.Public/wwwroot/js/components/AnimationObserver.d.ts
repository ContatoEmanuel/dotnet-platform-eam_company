/**
 * Observer para animações on scroll
 */
export declare class AnimationObserver {
    private threshold;
    private animatedClass;
    private observer;
    private elements;
    constructor(elementSelector?: string, threshold?: number, animatedClass?: string);
    /**
     * Inicializa o observer
     */
    observe(): void;
    /**
     * Para de observar todos os elementos
     */
    disconnect(): void;
    /**
     * Retorna número de elementos observados
     */
    getElementsCount(): number;
    /**
     * Re-anima todos os elementos (remove classe e re-observa)
     */
    reset(): void;
}
//# sourceMappingURL=AnimationObserver.d.ts.map