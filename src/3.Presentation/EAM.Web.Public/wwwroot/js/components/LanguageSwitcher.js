import { translations } from '../utils/i18n';
export class LanguageSwitcher {
    constructor(selectElement) {
        this.LANG_KEY = 'eam_lang';
        this.selectElement = null;
        this.lang = this.getSavedLang() || this.getBrowserLang() || 'pt-BR';
        this.selectElement = selectElement || null;
        this.applyLang();
    }
    getCurrentLang() {
        return this.lang;
    }
    setLang(lang) {
        this.lang = lang;
        localStorage.setItem(this.LANG_KEY, lang);
        this.applyLang();
    }
    getSavedLang() {
        const saved = localStorage.getItem(this.LANG_KEY);
        if (saved === 'pt-BR' || saved === 'en-US')
            return saved;
        return null;
    }
    getBrowserLang() {
        const nav = navigator.language || navigator.userLanguage;
        if (nav.startsWith('en'))
            return 'en-US';
        if (nav.startsWith('pt'))
            return 'pt-BR';
        return null;
    }
    applyLang() {
        document.documentElement.lang = this.lang;
        document.documentElement.setAttribute('lang', this.lang);
        // Atualiza textos do footer
        const copyright = document.getElementById('footer-copyright');
        const developed = document.getElementById('footer-developed');
        const label = document.getElementById('footer-language-label');
        const select = this.selectElement || document.getElementById('footer-language-select');
        if (copyright)
            copyright.textContent = `© ${new Date().getFullYear()} EAM Company - ${translations[this.lang].copyright}`;
        if (developed)
            developed.textContent = translations[this.lang].developed;
        if (label)
            label.textContent = translations[this.lang].language + ':';
        if (select) {
            select.options[0].text = translations[this.lang].portuguese;
            select.options[1].text = translations[this.lang].english;
            select.value = this.lang;
        }
        // Log para debug
        console.log(`✅ Idioma aplicado: ${this.lang}`);
        console.log(`📦 localStorage['${this.LANG_KEY}']:`, localStorage.getItem(this.LANG_KEY));
    }
}
//# sourceMappingURL=LanguageSwitcher.js.map