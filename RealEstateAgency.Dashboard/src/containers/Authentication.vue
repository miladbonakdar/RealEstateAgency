<template>
  <div class="app">
    <AppHeader fixed>
      <b-link class="navbar-brand" to="#">
        <img class="navbar-brand-full" src="/img/logo.png" width="120" height="35" alt="Home&Share Logo">

      </b-link>

      <b-navbar-nav class="d-md-down">

        <b-nav-item class="d-md-down">
          <b-dropdown variant="outline-none">
            <template slot="button-content">
              <span class="fa fa-language"></span>
              {{selectedLocale.name}}
            </template>
            <b-dropdown-item v-for="locale in locales" :key="locale.id"
                             @click="changeLocale(locale.id)">{{locale.name}} - {{locale.country}}
            </b-dropdown-item>
          </b-dropdown>
        </b-nav-item>
      </b-navbar-nav>
      <!--<AsideToggler class="d-lg-none" mobile />-->
    </AppHeader>
    <div class="app-body">

      <main class="main">

        <div class="container-fluid">
          <router-view></router-view>
        </div>
      </main>
    </div>
    <TheFooter>
      <!--footer-->
      <div class="w-100 clearfix">
        <a href="">{{siteInfo.appName}}</a>
        <span class="ml-1">&copy; 2019 {{siteInfo.createdBy}}</span>
        <span class="float-right">Version : <i class="font-xs badge-danger badge mr-1">{{siteInfo.version}}-{{siteInfo.versionType}}</i></span>
        <span class="float-right">Build number : <i class="font-xs badge-warning badge mr-1">{{siteInfo.buildNumber}}</i></span>
      </div>


      <!-- <div class="ml-auto">
        <span class="mr-1">Powered by</span>
        <a href="https://coreui.io">CoreUI for Vue</a>
      </div> -->
    </TheFooter>
  </div>
</template>

<script>
    import {
        Header as AppHeader,
        Footer as TheFooter,
    } from '@coreui/vue'
    import {locales} from '../implementation/i18n'
    import {mapGetters} from "vuex"
    import {statics} from '../store/types'

    export default {
        name: 'Authentication',
        components: {
            AppHeader,
            TheFooter
        },
        data() {
            return {
                nav: null,
                selectedLocale: null,
                locales: locales,
            }
        },
        created() {
            this.selectedLocale = this.locales.find(i => i.id === this.$route.params.lang);
        },
        computed: {
            ...mapGetters({
                siteInfo: statics.getters.siteInfo
            }),
            name() {
                return this.$route.name
            },
            list() {
                return this.$route.matched.filter((route) => route.name || route.meta.label)
            }
        },
        methods: {
            changeLocale(locale) {
                if (this.$route.params.lang === locale)
                    return;
                const path = this.$route.path.replace(this.$route.params.lang + '/', `${locale}/`);
                location.replace(path);
            }
        }
    }
</script>
