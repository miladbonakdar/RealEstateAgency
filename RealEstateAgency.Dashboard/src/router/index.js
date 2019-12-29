import Vue from 'vue';
import Router from 'vue-router';
import {locales} from "../implementation/i18n"
import estateRoute from './partials/estate.route'
import featureRoute from './partials/basicInformation/feature.route'
import labelRoute from './partials/basicInformation/label.route'
import statusRoute from './partials/basicInformation/status.route'
import typeRoute from './partials/basicInformation/type.route'
import importantPlaceRoute from './partials/basicInformation/importantPlace.route'
import agentProjectsComponentRoute from './partials/agentProjects/agentProjectsComponent.route'
import userListRoute from './partials/user.route'
import agentListRoute from './partials/agents.route'




import AgentProfile from '../views/userAccount/AgentProfile'
import {i18n} from '../implementation/i18n';

// Containers
const DefaultContainer = () => import('@/containers/DefaultContainer')
const Authentication = () => import('@/containers/Authentication')


// Views
const Dashboard = () => import('@/views/Dashboard');
// Views - Pages
const Page404 = () => import('@/views/pages/Page404');
const FakeRequest = () => import('@/views/pages/FakeRequest');
//const Page500 = () => import('@/views/pages/Page500');
const Login = () => import('@/views/pages/Login');

// const Register = () => import('@/views/pages/Register');

Vue.use(Router);

Router.prototype.replaceLang = function (path) {
  if (!path.startsWith('/'))
    path = '/' + path;
  this.replace(`/${this.app.$i18n.locale}${path}`);
};

Router.prototype.goLang = function (path) {
  if (!path.startsWith('/'))
    path = '/' + path;
  this.go(`/${this.app.$i18n.locale}${path}`);
};

Router.prototype.pushLang = function (path) {
  if (!path.startsWith('/'))
    path = '/' + path;
  this.push(`/${this.app.$i18n.locale}${path}`);
};

const router = new Router({
  mode: 'history', // https://router.vuejs.org/api/#mode
  linkActiveClass: 'open active',
  scrollBehavior: () => ({y: 0}),
  routes: [
    {
      path: '/:lang',
      redirect: '/:lang/dashboard',
      beforeEnter(to, from, next) {
        const lang = to.params.lang;
        if (!locales.map(l => l.id).includes(lang)) return next('en');
        if (i18n.locale !== lang)
          i18n.locale = lang;
        return next();
      },
      name: 'Home',
      component: DefaultContainer,
      children: [
        {
          path: 'dashboard',
          name: 'Dashboard',
          component: Dashboard
        },
        {
          path: 'profile',
          name: 'Profile',
          component: AgentProfile
        },
        userListRoute,
        estateRoute,
        ...featureRoute,
        ...statusRoute,
        ...typeRoute,
        ...labelRoute,
        ...importantPlaceRoute,
        ...agentProjectsComponentRoute,
        ...agentListRoute,
        {
          path: '404',
          name: '',
          component: Page404
        },
        {
          path: 'fakeRequest',
          name: 'fake request',
          component: FakeRequest
        }
      ]
    },
    {
      path: '/:lang/auth',
      redirect: '/:lang/auth/login',
      beforeEnter(to, from, next) {
        const lang = to.params.lang;
        if (!locales.map(l => l.id).includes(lang)) return next('en');
        if (i18n.locale !== lang)
          i18n.locale = lang;
        return next();
      },
      name: 'Authentication',
      component: Authentication,
      children: [
        {
          path: 'login',
          name: 'Login',
          component: Login
        },
        // {
        //   path: 'register',
        //   name: 'Register',
        //   component: Register
        // }
      ]
    },
    {path: '/:lang/*', redirect: '/:lang/auth/login'},
    {path: '*', redirect: '/en/auth/login'}
  ]
});


router.beforeEach((to, from, next) => {
  window.currentRoute = to.name;
  window.lastRoute = from.name;
  document.title = `${to.name} - Home & Share`;
  next();
});

export default router;
