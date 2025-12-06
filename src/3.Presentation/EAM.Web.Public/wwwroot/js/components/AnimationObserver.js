/**
 * Observer para animações on scroll
 */
export class AnimationObserver {
    constructor(elementSelector = '.animate-on-scroll', threshold = 0.1, animatedClass = 'animated') {
        this.threshold = threshold;
        this.animatedClass = animatedClass;
        this.observer = null;
        this.elements = document.querySelectorAll(elementSelector);
    }
    /**
     * Inicializa o observer
     */
    observe() {
        if (this.elements.length === 0) {
            console.info('Nenhum elemento encontrado para animação');
            return;
        }
        const options = {
            root: null,
            rootMargin: '0px',
            threshold: this.threshold
        };
        this.observer = new IntersectionObserver((entries) => {
            entries.forEach(entry => {
                if (entry.isIntersecting) {
                    entry.target.classList.add(this.animatedClass);
                    // Para de observar após animar (one-time animation)
                    if (this.observer) {
                        this.observer.unobserve(entry.target);
                    }
                }
            });
        }, options);
        this.elements.forEach(element => {
            if (this.observer) {
                this.observer.observe(element);
            }
        });
    }
    /**
     * Para de observar todos os elementos
     */
    disconnect() {
        if (this.observer) {
            this.observer.disconnect();
            this.observer = null;
        }
    }
    /**
     * Retorna número de elementos observados
     */
    getElementsCount() {
        return this.elements.length;
    }
    /**
     * Re-anima todos os elementos (remove classe e re-observa)
     */
    reset() {
        this.elements.forEach(element => {
            element.classList.remove(this.animatedClass);
        });
        this.disconnect();
        this.observe();
    }
}
//# sourceMappingURL=AnimationObserver.js.map