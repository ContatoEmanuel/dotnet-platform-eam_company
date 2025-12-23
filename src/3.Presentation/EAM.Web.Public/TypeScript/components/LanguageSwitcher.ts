import { translations, SupportedLang } from '../utils/i18n';

export class LanguageSwitcher {
  private lang: SupportedLang;
  private readonly LANG_KEY = 'eam_lang';
  private selectElement: HTMLSelectElement | null = null;

  constructor(selectElement?: HTMLSelectElement | null) {
    this.lang = this.getSavedLang() || this.getBrowserLang() || 'pt-BR';
    this.selectElement = selectElement || null;
    this.applyLang();
  }

  getCurrentLang(): SupportedLang {
    return this.lang;
  }

  setLang(lang: SupportedLang) {
    this.lang = lang;
    localStorage.setItem(this.LANG_KEY, lang);
    this.setCookie(lang);
    this.applyLang();
  }

  getSavedLang(): SupportedLang | null {
    const saved = localStorage.getItem(this.LANG_KEY);
    if (saved === 'pt-BR' || saved === 'en-US') return saved;
    return null;
  }

  getBrowserLang(): SupportedLang | null {
    const nav = navigator.language || (navigator as any).userLanguage;
    if (nav.startsWith('en')) return 'en-US';
    if (nav.startsWith('pt')) return 'pt-BR';
    return null;
  }

  private setCookie(lang: SupportedLang) {
    // Define cookie com 1 ano de validade
    const date = new Date();
    date.setTime(date.getTime() + 365 * 24 * 60 * 60 * 1000);
    const expires = `expires=${date.toUTCString()}`;
    document.cookie = `eam_lang=${lang}; ${expires}; path=/`;
    console.log(`🍪 Cookie 'eam_lang' definido com valor: ${lang}`);
  }

  applyLang() {
    document.documentElement.lang = this.lang;
    document.documentElement.setAttribute('lang', this.lang);
    
    // Atualiza textos do footer
    const copyright = document.getElementById('footer-copyright');
    const developed = document.getElementById('footer-developed');
    const label = document.getElementById('footer-language-label');
    const select = this.selectElement || document.getElementById('footer-language-select') as HTMLSelectElement | null;
    
    if (copyright) copyright.textContent = `© ${new Date().getFullYear()} EAM Company - ${translations[this.lang].copyright}`;
    if (developed) developed.textContent = translations[this.lang].developed;
    if (label) label.textContent = translations[this.lang].language + ':';
    if (select) {
      select.options[0].text = translations[this.lang].portuguese;
      select.options[1].text = translations[this.lang].english;
      select.value = this.lang;
    }

    // Publica evento para notificar mudança de idioma
    window.dispatchEvent(new CustomEvent('languageChanged', { detail: { language: this.lang } }));

    // Log para debug
    console.log(`✅ Idioma aplicado: ${this.lang}`);
    console.log(`📦 localStorage['${this.LANG_KEY}']:`, localStorage.getItem(this.LANG_KEY));
  }
}
