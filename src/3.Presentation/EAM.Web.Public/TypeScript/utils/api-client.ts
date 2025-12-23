/**
 * API Client para buscar dados multilíngues
 * Sempre passa o idioma atual para a API
 */
export class ApiClient {
    private baseUrl: string;
    private currentLanguage: string;

    constructor(baseUrl: string = '') {
        this.baseUrl = baseUrl || window.location.origin;
        this.currentLanguage = this.getCurrentLanguage();
    }

    /**
     * Obtém idioma atual do cookie ou localStorage
     */
    private getCurrentLanguage(): string {
        // Tenta cookie primeiro
        const cookieLang = this.getCookie('eam_lang');
        if (cookieLang === 'pt-BR' || cookieLang === 'en-US') {
            return cookieLang;
        }

        // Tenta localStorage
        const storedLang = localStorage.getItem('eam_lang');
        if (storedLang === 'pt-BR' || storedLang === 'en-US') {
            return storedLang;
        }

        // Default
        return 'pt-BR';
    }

    /**
     * Atualiza o idioma e recarrega dados se necessário
     */
    setLanguage(language: string) {
        if (language === 'pt-BR' || language === 'en-US') {
            this.currentLanguage = language;
            // Atualiza cookie
            this.setCookie('eam_lang', language);
            // Atualiza localStorage
            localStorage.setItem('eam_lang', language);
        }
    }

    /**
     * Busca posts do blog
     */
    async getPosts(publishedOnly: boolean = true): Promise<any[]> {
        const url = `${this.baseUrl}/api/blog/posts?publishedOnly=${publishedOnly}&language=${this.currentLanguage}`;
        const response = await fetch(url);
        
        if (!response.ok) {
            throw new Error(`Erro ao buscar posts: ${response.statusText}`);
        }
        
        return await response.json();
    }

    /**
     * Busca um post específico
     */
    async getPost(id: number): Promise<any> {
        const url = `${this.baseUrl}/api/blog/posts/${id}?language=${this.currentLanguage}`;
        const response = await fetch(url);
        
        if (!response.ok) {
            throw new Error(`Erro ao buscar post: ${response.statusText}`);
        }
        
        return await response.json();
    }

    /**
     * Busca post por slug
     */
    async getPostBySlug(slug: string): Promise<any> {
        const url = `${this.baseUrl}/api/blog/posts/slug/${slug}?language=${this.currentLanguage}`;
        const response = await fetch(url);
        
        if (!response.ok) {
            throw new Error(`Erro ao buscar post: ${response.statusText}`);
        }
        
        return await response.json();
    }

    /**
     * Busca projetos
     */
    async getProjects(): Promise<any[]> {
        const url = `${this.baseUrl}/api/projects?language=${this.currentLanguage}`;
        const response = await fetch(url);
        
        if (!response.ok) {
            throw new Error(`Erro ao buscar projetos: ${response.statusText}`);
        }
        
        return await response.json();
    }

    /**
     * Busca um projeto específico
     */
    async getProject(id: number): Promise<any> {
        const url = `${this.baseUrl}/api/projects/${id}?language=${this.currentLanguage}`;
        const response = await fetch(url);
        
        if (!response.ok) {
            throw new Error(`Erro ao buscar projeto: ${response.statusText}`);
        }
        
        return await response.json();
    }

    /**
     * Busca projetos em destaque
     */
    async getFeaturedProjects(count: number = 3): Promise<any[]> {
        const url = `${this.baseUrl}/api/projects/featured/${count}?language=${this.currentLanguage}`;
        const response = await fetch(url);
        
        if (!response.ok) {
            throw new Error(`Erro ao buscar projetos em destaque: ${response.statusText}`);
        }
        
        return await response.json();
    }

    /**
     * Getter para idioma atual
     */
    getLanguage(): string {
        return this.currentLanguage;
    }

    /**
     * Helper para ler cookie
     */
    private getCookie(name: string): string | null {
        const nameEQ = name + '=';
        const cookies = document.cookie.split(';');
        
        for (let cookie of cookies) {
            cookie = cookie.trim();
            if (cookie.indexOf(nameEQ) === 0) {
                return cookie.substring(nameEQ.length);
            }
        }
        
        return null;
    }

    /**
     * Helper para salvar cookie
     */
    private setCookie(name: string, value: string, days: number = 365) {
        const date = new Date();
        date.setTime(date.getTime() + days * 24 * 60 * 60 * 1000);
        const expires = `expires=${date.toUTCString()}`;
        document.cookie = `${name}=${value}; ${expires}; path=/`;
    }
}

// Instância global
export const apiClient = new ApiClient();
