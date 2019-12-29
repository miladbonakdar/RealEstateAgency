import Vue from 'vue';
//https://kazupon.github.io/vue-i18n/started.html#html
import VueI18n from 'vue-i18n';

import huLocale from './locales/hu.locale';
import enLocale from './locales/en.locale';

Vue.use(VueI18n);

export const i18n = new VueI18n({
  locale: 'en',
  fallbackLocale: 'en',
  messages: {
    en: enLocale,
    hu: huLocale
  }
});

export const locales = [
  {id: 'en', name: 'English', country: 'English'},
  {id: 'hu', name: 'Hungarian', country: 'Hungary'}];
