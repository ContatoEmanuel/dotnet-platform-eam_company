/**
 * EAM Company - Site Institucional
 * TypeScript Entry Point
 * @author Emanuel A Macêdo
 */

import { BackToTopButton } from './components/BackToTopButton';
import { SmoothScrollLinks } from './components/SmoothScrollLinks';
import { ActiveNavigation } from './components/ActiveNavigation';
import { AnimationObserver } from './components/AnimationObserver';

/**
 * Classe principal da aplicação
 */
class EAMPublicSite {
    private backToTopButton: BackToTopButton | null = null;
    private smoothScrollLinks: SmoothScrollLinks | null = null;
    private activeNavigation: ActiveNavigation | null = null;
    private animationObserver: AnimationObserver | null = null;

    /**
     * Inicializa a aplicação
     */
    public initialize(): void {
        console.log('🚀 Inicializando EAM Public Site...');

        // Aguarda DOM estar pronto
        if (document.readyState === 'loading') {
            document.addEventListener('DOMContentLoaded', () => this.setup());
        } else {
            this.setup();
        }
    }

    /**
     * Configura todos os componentes
     */
    private setup(): void {
        try {
            // Inicializa Back to Top Button
            this.backToTopButton = new BackToTopButton('#backToTop', 300);
            console.log('✅ Back to Top Button inicializado');

            // Inicializa Smooth Scroll Links
            this.smoothScrollLinks = new SmoothScrollLinks('a[href^="#"]', 80);
            console.log(`✅ Smooth Scroll Links inicializado (${this.smoothScrollLinks.getLinksCount()} links)`);

            // Inicializa Active Navigation
            this.activeNavigation = new ActiveNavigation('.navbar-nav .nav-link[href^="#"]', 0.5);
            this.activeNavigation.observe();
            console.log(`✅ Active Navigation inicializado (${this.activeNavigation.getSectionsCount()} seções)`);

            // Inicializa Animation Observer
            this.animationObserver = new AnimationObserver('.animate-on-scroll', 0.1, 'animated');
            this.animationObserver.observe();
            console.log(`✅ Animation Observer inicializado (${this.animationObserver.getElementsCount()} elementos)`);

            console.log('✨ EAM Public Site inicializado com sucesso!');
        } catch (error) {
            console.error('❌ Erro ao inicializar aplicação:', error);
        }
    }

    /**
     * Destrói todos os componentes
     */
    public destroy(): void {
        this.backToTopButton?.destroy();
        this.activeNavigation?.disconnect();
        this.animationObserver?.disconnect();
        
        console.log('🔄 Componentes destruídos');
    }
}

// Inicializa aplicação
const app = new EAMPublicSite();
app.initialize();

// Expõe globalmente para debugging (opcional)
if (typeof window !== 'undefined') {
    (window as any).EAMApp = app;
}

export default app;
