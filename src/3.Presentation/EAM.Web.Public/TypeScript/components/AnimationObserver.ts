/**
 * Observer para animações on scroll
 */
export class AnimationObserver {
    private observer: IntersectionObserver | null = null;
    private elements: NodeListOf<Element>;

    constructor(
        elementSelector: string = '.animate-on-scroll',
        private threshold: number = 0.1,
        private animatedClass: string = 'animated'
    ) {
        this.elements = document.querySelectorAll(elementSelector);
    }

    /**
     * Inicializa o observer
     */
    public observe(): void {
        if (this.elements.length === 0) {
            console.info('Nenhum elemento encontrado para animação');
            return;
        }

        const options: IntersectionObserverInit = {
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
    public disconnect(): void {
        if (this.observer) {
            this.observer.disconnect();
            this.observer = null;
        }
    }

    /**
     * Retorna número de elementos observados
     */
    public getElementsCount(): number {
        return this.elements.length;
    }

    /**
     * Re-anima todos os elementos (remove classe e re-observa)
     */
    public reset(): void {
        this.elements.forEach(element => {
            element.classList.remove(this.animatedClass);
        });
        this.disconnect();
        this.observe();
    }
}
