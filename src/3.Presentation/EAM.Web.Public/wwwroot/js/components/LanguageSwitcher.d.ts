import { SupportedLang } from '../utils/i18n';
export declare class LanguageSwitcher {
    private lang;
    private readonly LANG_KEY;
    private selectElement;
    constructor(selectElement?: HTMLSelectElement | null);
    getCurrentLang(): SupportedLang;
    setLang(lang: SupportedLang): void;
    getSavedLang(): SupportedLang | null;
    getBrowserLang(): SupportedLang | null;
    applyLang(): void;
}
//# sourceMappingURL=LanguageSwitcher.d.ts.map