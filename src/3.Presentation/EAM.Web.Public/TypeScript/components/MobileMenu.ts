/**
 * Mobile Menu Handler
 * Controla o comportamento do menu hambúrguer mobile
 */

export class MobileMenu {
    private hamburgerBtn: HTMLElement | null;
    private closeBtn: HTMLElement | null;
    private mobileMenu: HTMLElement | null;
    private overlay: HTMLElement | null;
    private menuIconClosed: HTMLElement | null;
    private menuIconOpen: HTMLElement | null;
    private isOpen: boolean = false;

    constructor() {
        this.hamburgerBtn = document.getElementById('hamburger-btn');
        this.closeBtn = document.getElementById('close-menu-btn');
        this.mobileMenu = document.getElementById('mobile-menu');
        this.overlay = document.getElementById('mobile-menu-overlay');
        this.menuIconClosed = document.getElementById('menu-icon-closed');
        this.menuIconOpen = document.getElementById('menu-icon-open');

        this.init();
    }

    private init(): void {
        if (!this.hamburgerBtn || !this.mobileMenu || !this.overlay) {
            console.warn('Mobile menu elements not found');
            return;
        }

        // Event listeners
        this.hamburgerBtn.addEventListener('click', () => this.toggle());
        this.closeBtn?.addEventListener('click', () => this.close());
        this.overlay.addEventListener('click', () => this.close());

        // Close menu when clicking on menu items
        const menuItems = this.mobileMenu.querySelectorAll('.mobile-menu-item');
        menuItems.forEach(item => {
            item.addEventListener('click', () => {
                // Small delay to allow navigation
                setTimeout(() => this.close(), 150);
            });
        });

        // Close on ESC key
        document.addEventListener('keydown', (e) => {
            if (e.key === 'Escape' && this.isOpen) {
                this.close();
            }
        });

        console.log('✅ Mobile Menu initialized');
    }

    private toggle(): void {
        if (this.isOpen) {
            this.close();
        } else {
            this.open();
        }
    }

    private open(): void {
        if (!this.mobileMenu || !this.overlay) return;

        this.isOpen = true;
        
        // Show overlay
        this.overlay.classList.remove('hidden');
        setTimeout(() => {
            this.overlay?.classList.add('opacity-100');
        }, 10);

        // Slide menu in
        this.mobileMenu.classList.remove('translate-x-full');
        
        // Toggle icons
        this.menuIconClosed?.classList.add('hidden');
        this.menuIconOpen?.classList.remove('hidden');

        // Prevent body scroll
        document.body.style.overflow = 'hidden';
    }

    private close(): void {
        if (!this.mobileMenu || !this.overlay) return;

        this.isOpen = false;
        
        // Hide overlay
        this.overlay.classList.remove('opacity-100');
        setTimeout(() => {
            this.overlay?.classList.add('hidden');
        }, 300);

        // Slide menu out
        this.mobileMenu.classList.add('translate-x-full');
        
        // Toggle icons
        this.menuIconClosed?.classList.remove('hidden');
        this.menuIconOpen?.classList.add('hidden');

        // Restore body scroll
        document.body.style.overflow = '';
    }

    public destroy(): void {
        // Clean up event listeners if needed
        this.close();
    }
}
