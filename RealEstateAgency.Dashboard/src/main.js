// The Vue build version to load with the `import` command
// (runtime-only or standalone) has been set in webpack.base.conf with an alias.
import 'core-js/es6/promise'
import 'core-js/es6/string'
import 'core-js/es7/array'

import Vue from 'vue'
import BootstrapVue from 'bootstrap-vue'
import vueMoment from 'vue-moment';
import App from './App'
import router from './router'
import filters from './implementation/filters'
import VueCrud from "./implementation/crudly/vueCrud";
import MyGate from "./implementation/crudly/CRUDlyInstance";
import {store} from "./store/store";
import {i18n} from "./implementation/i18n";//https://kazupon.github.io/vue-i18n/started.html#html
import Toasted from "vue-toasted";
import VueMoment from "vue-moment";
import './implementation/registerRootComponents';
import quillConfig from './implementation/quellConfiguration';
import * as VueGoogleMaps from 'vue2-google-maps';
import Vuelidate from 'vuelidate';
import vueFilterPrettyBytes from 'vue-filter-pretty-bytes';
import usersHub from './implementation/usersHub.js';
import globalExceptionHandling from './implementation/globalExceptionHandling';
import configureToastedTemplates from './implementation/configureToastedTemplates';
import Datetime from 'vue-datetime'

quillConfig();

Vue.use(BootstrapVue);
Vue.use(VueCrud, MyGate);

Vue.use(Toasted, {
  position: 'bottom-right',
  duration: 5000,
  iconPack: "fontawesome"
});

Vue.use(VueGoogleMaps, {
  load: {
    key: 'AIzaSyBcjQZGS2nmFiOeJu9G4ZpBiVOGLJ4YBDs',
    libraries: 'places',
  },
});

Vue.use(configureToastedTemplates);
Vue.use(globalExceptionHandling);
Vue.use(vueFilterPrettyBytes);
Vue.use(VueMoment);
Vue.use(Vuelidate);
Vue.use(usersHub);
Vue.use(filters);
Vue.use(Datetime);

new Vue({
  el: '#app',
  router,
  store,
  i18n,
  template: '<App/>',
  components: {
    App
  }
});
