import { apiClient } from '../utils/api-client';

/**
 * Renderizador de Posts do Blog
 * Busca dados da API e renderiza dinamicamente no frontend
 */
export class BlogRenderer {
    private container: HTMLElement | null;
    private posts: any[] = [];

    constructor(containerId: string = 'posts-grid') {
        this.container = document.getElementById(containerId);
    }

    /**
     * Carrega e renderiza posts
     */
    async loadAndRender() {
        try {
            if (!this.container) {
                console.error('Container não encontrado');
                return;
            }

            // Mostra loading
            this.container.innerHTML = '<p class="text-center py-8">Carregando posts...</p>';

            // Busca posts da API
            this.posts = await apiClient.getPosts();

            // Renderiza posts
            this.render();

            console.log(`✅ ${this.posts.length} posts carregados e renderizados`);
        } catch (error) {
            console.error('Erro ao carregar posts:', error);
            if (this.container) {
                this.container.innerHTML = '<p class="text-center py-8 text-red-500">Erro ao carregar posts. Tente novamente mais tarde.</p>';
            }
        }
    }

    /**
     * Renderiza os posts
     */
    private render() {
        if (!this.container || this.posts.length === 0) {
            if (this.container) {
                this.container.innerHTML = '<p class="text-center py-8">Nenhum post encontrado.</p>';
            }
            return;
        }

        this.container.innerHTML = this.posts
            .map(post => this.renderPost(post))
            .join('');

        // Re-aplica filters se existirem
        this.applyFilters();
    }

    /**
     * Renderiza um post individual
     */
    private renderPost(post: any): string {
        const language = apiClient.getLanguage();
        const postTitle = post.titleEn && language === 'en-US' ? post.titleEn : post.title;
        const postExcerpt = post.excerptEn && language === 'en-US' ? post.excerptEn : post.excerpt;
        const categoryName = language === 'en-US' && post.category?.nameEn ? post.category.nameEn : post.category?.name;
        const categoryColor = post.category?.color || '#3B82F6';
        const postTags = post.tags ? post.tags.join(',') : '';

        return `
            <article class="blog-post bg-white dark:bg-gray-800 rounded-lg shadow-md overflow-hidden hover:shadow-xl transition-shadow duration-300 dark:border dark:border-gray-700" 
                     data-category="${categoryName || 'Geral'}" 
                     data-tags="${postTags}">
                <!-- Post Image -->
                <div class="aspect-video bg-gradient-to-br from-blue-500 to-indigo-600 relative overflow-hidden">
                    ${post.imageUrl ? `<img src="${post.imageUrl}" alt="${postTitle}" class="w-full h-full object-cover">` : ''}
                    <div class="absolute top-4 left-4">
                        <span class="px-3 py-1 bg-white text-xs font-semibold rounded-full" style="color: ${categoryColor};">
                            ${categoryName || 'Geral'}
                        </span>
                    </div>
                </div>
                
                <!-- Post Content -->
                <div class="p-6">
                    <!-- Meta Info -->
                    <div class="flex items-center text-sm text-gray-500 dark:text-gray-400 mb-3">
                        <i class="bi bi-calendar3 mr-2"></i>
                        <span>${this.formatDate(post.publishedAt)}</span>
                        <span class="mx-2">•</span>
                        <i class="bi bi-clock mr-2"></i>
                        <span>${post.readTimeMinutes || 5} min</span>
                    </div>
                    
                    <!-- Title -->
                    <h2 class="text-xl font-bold text-gray-900 dark:text-white mb-3 line-clamp-2 hover:text-blue-600 dark:hover:text-blue-400 transition-colors">
                        <a href="/blog/post/${post.id}/${post.slug}">
                            ${postTitle}
                        </a>
                    </h2>
                    
                    <!-- Excerpt -->
                    <p class="text-gray-600 dark:text-gray-300 line-clamp-3 mb-4">
                        ${postExcerpt}
                    </p>
                    
                    <!-- Tags -->
                    <div class="flex flex-wrap gap-2 mb-4">
                        ${post.tags?.slice(0, 3).map((tag: string) => `
                            <span class="px-2 py-1 bg-blue-50 dark:bg-blue-900 text-blue-700 dark:text-blue-300 text-xs rounded-full">
                                ${tag}
                            </span>
                        `).join('') || ''}
                    </div>
                    
                    <!-- Read More Button -->
                    <a href="/blog/post/${post.id}/${post.slug}" class="inline-block mt-auto text-blue-600 dark:text-blue-400 font-semibold hover:text-blue-800 dark:hover:text-blue-300 transition-colors">
                        Leia mais →
                    </a>
                </div>
            </article>
        `;
    }

    /**
     * Formata data
     */
    private formatDate(dateString: string): string {
        if (!dateString) return new Date().toLocaleDateString('pt-BR');
        const date = new Date(dateString);
        return date.toLocaleDateString('pt-BR');
    }

    /**
     * Re-aplica filtros de categoria e tag
     */
    private applyFilters() {
        if (!this.container) return;

        const filterButtons = document.querySelectorAll('.category-filter, .tag-filter');
        filterButtons.forEach(button => {
            button.addEventListener('click', () => this.filter(button as HTMLElement));
        });
    }

    /**
     * Filtra posts
     */
    private filter(button: HTMLElement) {
        const posts = this.container?.querySelectorAll('.blog-post');
        if (!posts) return;

        // Remove active de todos
        document.querySelectorAll('.category-filter, .tag-filter').forEach(btn => {
            btn.classList.remove('bg-blue-600', 'text-white');
            btn.classList.add('bg-gray-100', 'dark:bg-gray-700', 'text-gray-700', 'dark:text-gray-300');
        });

        // Adiciona active ao clicado
        button.classList.add('bg-blue-600', 'text-white');
        button.classList.remove('bg-gray-100', 'dark:bg-gray-700', 'text-gray-700', 'dark:text-gray-300');

        const filterType = button.classList.contains('category-filter') ? 'category' : 'tag';
        const filterValue = button.getAttribute(`data-${filterType}`);

        // Filtra posts
        posts.forEach(post => {
            if (!(post instanceof HTMLElement)) return;

            const postData = post.getAttribute(`data-${filterType}s`);
            const matches = filterValue === 'all' || 
                          (filterType === 'category' && post.getAttribute('data-category') === filterValue) ||
                          (filterType === 'tag' && postData?.includes(filterValue || ''));

            post.style.display = matches ? 'block' : 'none';
        });
    }

    /**
     * Atualiza idioma e recarrega
     */
    async setLanguage(language: string) {
        apiClient.setLanguage(language);
        await this.loadAndRender();
        console.log(`✅ Posts recarregados em ${language}`);
    }
}
