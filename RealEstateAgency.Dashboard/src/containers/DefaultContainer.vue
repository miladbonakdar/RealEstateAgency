<template>
  <div class="app">
    <AppHeader fixed>
      <SidebarToggler class="d-lg-none" display="md" mobile />
      <b-link class="navbar-brand" to="#">
        <img
          class="navbar-brand-full"
          src="/img/logo.png"
          width="120"
          height="35"
          alt="HomeShare Logo"
        />
        <img
          class="navbar-brand-minimized"
          src="/img/logo.png"
          width="120"
          height="35"
          alt="HomeShare Logo"
        />
      </b-link>
      <SidebarToggler class="d-md-down-none" display="lg" :defaultOpen="true" />
      <b-navbar-nav class="d-md-down">
        <b-nav-item
          class="px-3 d-md-down-none"
          :to="'/' + $route.params.lang + '/dashboard'"
          >Dashboard</b-nav-item
        >
        <!-- <b-nav-item class="px-3">Settings</b-nav-item> -->
        <b-nav-item
          class="d-md-down"
          v-if="
            staticsLoaded &&
              user &&
              user.userGroup.staticCode === 'RealEstateAdministrator'
          "
        >
          <b-dropdown
            variant="outline-none"
            v-if="staticsLoaded && selectedLanguage"
          >
            <template slot="button-content">
              <span class="fa fa-language"></span>
              {{ selectedLanguage.code }}
            </template>
            <b-dropdown-item
              v-for="locale in locales"
              :key="locale.id"
              @click="changeLocale(locale)"
              >{{ locale.code }}
            </b-dropdown-item>
          </b-dropdown>
        </b-nav-item>
        <div v-else class="px-3 d-md-down-none">
          <span class="text-muted font-xs">
            <i class="fa fa-globe font-lg"></i>
            {{ selectedLanguage.code }}</span
          >
        </div>
        <div class="px-3 d-md-down-none" v-if="siteInfo.versionType !== 'RC'">
          <span class="badge badge-warning font-xs animated infinite slow pulse"
            >{{ siteInfo.versionType }} Version</span
          >
        </div>
      </b-navbar-nav>
      <b-navbar-nav class="ml-auto">
        <b-nav-item class="font-lg" v-if="staticsLoaded && user">
          <b-link :to="'/' + $route.params.lang + '/dashboard'">
            <i
              >{{ user.firstName | upperChar }}
              {{ user.lastName | upperChar }}</i
            >
          </b-link>
        </b-nav-item>

        <b-nav-item class="d-md-down pointer">
          <b-dropdown
            v-if="userEvents.items && userEvents.items.length !== 0"
            variant="outline-none"
            no-caret
            dropleft
          >
            <template slot="button-content">
              <i
                v-if="notCompletedEventCount === 0"
                class="icon-bell"
                style="font-size: 20px;"
              ></i>
              <i
                v-if="notCompletedEventCount !== 0"
                class="icon-bell animated tada delay-5s"
                @mouseenter="bellHovered = true"
                @mouseleave="bellHovered = false"
                :class="{ infinite: bellHovered }"
                style="font-size: 20px"
              ></i>
              <b-badge
                class="font-xs"
                v-if="notCompletedEventCount !== 0"
                pill
                variant="danger"
                @mouseenter="bellHovered = true"
                @mouseleave="bellHovered = false"
                >{{ notCompletedEventCount }}
              </b-badge>
            </template>
            <div>
              <div v-for="event in userEvents.items" :key="event.id">
                <b-dropdown-form class="clearfix">
                  <span
                    class="fa"
                    :class="{
                      'fa-square-o text-success': !event.isDone,
                      'fa-check-square-o text-danger': event.isDone
                    }"
                  ></span>
                  {{ event.actionType.name }}
                  <span
                    class="px-2"
                    :class="{
                      'text-success': !event.isDone,
                      'text-danger': event.isDone
                    }"
                  >
                    {{ event.isDone ? "Done" : "Pending" }}</span
                  >
                  <span
                    @click="onDeleteItem(event.id)"
                    v-if="!event.isDone"
                    class="float-right fa fa-trash-o hvr-pulse-grow font-xs text-danger"
                    v-b-tooltip.hover
                    title="Delete this reminder"
                  ></span>
                </b-dropdown-form>
                <b-dropdown-text style="width: 240px;">
                  <span
                    class="text-warning fa fa-clock-o animated"
                    :class="{ 'infinite heartBeat': !event.isDone }"
                  ></span>
                  {{ event.followUpDateTime | moment("from") }} at
                  <span
                    class="px-2 badge-pill font-xs"
                    :class="{
                      'bg-success': !event.isDone,
                      'bg-danger': event.isDone
                    }"
                  >
                    {{ event.followUpDateTime | moment(dateFormat) }}</span
                  >
                  <br />
                  <span v-if="event.followUpDateTimeSnooze">
                    <span class="text-info fa fa-bolt"> Snooze:</span>
                    {{ event.followUpDateTimeSnooze | moment("from") }}<br />
                  </span>
                  <span class="text-primary fa fa-sticky-note-o"></span>
                  {{ event.description }}
                </b-dropdown-text>
                <b-dropdown-divider></b-dropdown-divider>
              </div>
              <div
                v-if="(showNextPage || showPrevPage) && !loadingEvents"
                class="clearfix text-info my-3"
              >
                <i
                  v-if="showPrevPage"
                  @mouseenter="pageHover.left = true"
                  @mouseleave="pageHover.left = false"
                  :class="{ 'text-danger font-2xl': pageHover.left }"
                  @click.prevent.stop="prevPage"
                >
                  <span class="float-left fa fa-caret-left font-lg px-4"></span>
                </i>
                <i
                  v-if="showNextPage"
                  :class="{ 'text-danger font-2xl': pageHover.right }"
                  @mouseenter="pageHover.right = true"
                  @mouseleave="pageHover.right = false"
                  @click.prevent.stop="nextPage"
                >
                  <span
                    class="float-right fa fa-caret-right font-lg px-4"
                  ></span>
                </i>
              </div>
              <div
                v-else-if="userEvents.total != userEvents.count"
                class="text-danger text-center"
              >
                <span class="fa fa-refresh animated infinite flash my-3 mb-4">
                  Loading...</span
                >
              </div>
            </div>
          </b-dropdown>
          <b-dropdown v-else variant="outline-none" no-caret dropleft>
            <template slot="button-content">
              <i class="icon-bell" style="font-size: 20px;"></i>
            </template>
            <div class="text-center text-warning">
              <h6 class="my-3">There is no notification</h6>
            </div>
          </b-dropdown>
        </b-nav-item>
        <!-- <b-nav-item class="d-md-down-none">
          <i class="icon-location-pin"></i>
        </b-nav-item> -->
        <DefaultHeaderDropdownAccnt />
      </b-navbar-nav>
      <AsideToggler class="d-none d-lg-block" />
      <AsideToggler class="d-lg-none" mobile />
    </AppHeader>
    <div class="app-body">
      <AppSidebar fixed>
        <SidebarHeader />
        <SidebarForm />
        <SidebarNav v-if="staticsLoaded" :navItems="nav"></SidebarNav>
        <SidebarFooter />
        <SidebarMinimizer />
      </AppSidebar>
      <main class="main">
        <Breadcrumb :list="list" />
        <div class="container-fluid">
          <router-view v-if="staticsLoaded"></router-view>
        </div>
      </main>
      <AppAside class="overflow-auto" fixed>
        <DefaultAside />
        <!--aside-->
      </AppAside>
    </div>
    <TheFooter>
      <!--footer-->
      <div class="w-100 clearfix font-sm">
        <a href="">{{ siteInfo.appName }}</a>
        <span class="ml-1">&copy; 2019 {{ siteInfo.createdBy }}</span>
        <span class="float-right"
          >Version :
          <i class="font-xs badge-danger badge mr-1"
            >{{ siteInfo.version }}-{{ siteInfo.versionType }}</i
          ></span
        >
        <span class="float-right"
          >Build number :
          <i class="font-xs badge-warning badge mr-1">{{
            siteInfo.buildNumber
          }}</i></span
        >
      </div>
      <!-- <div class="ml-auto">
        <span class="mr-1">Powered by</span>
        <a href="https://coreui.io">CoreUI for Vue</a>
      </div> -->
    </TheFooter>
  </div>
</template>

<script>
import nav from "@/_nav";
import {
  Header as AppHeader,
  SidebarToggler,
  Sidebar as AppSidebar,
  SidebarFooter,
  SidebarForm,
  SidebarHeader,
  SidebarMinimizer,
  SidebarNav,
  Aside as AppAside,
  AsideToggler,
  Footer as TheFooter,
  Breadcrumb
} from "@coreui/vue";
import DefaultAside from "./DefaultAside";
import DefaultHeaderDropdownAccnt from "./DefaultHeaderDropdownAccnt";
import { mapActions, mapMutations, mapGetters } from "vuex";
import { statics, userEvents } from "../store/types";

export default {
  name: "DefaultContainer",
  components: {
    AsideToggler,
    AppHeader,
    AppSidebar,
    AppAside,
    TheFooter,
    Breadcrumb,
    DefaultAside,
    DefaultHeaderDropdownAccnt,
    SidebarForm,
    SidebarFooter,
    SidebarToggler,
    SidebarHeader,
    SidebarNav,
    SidebarMinimizer
  },
  data() {
    return {
      pageHover: {
        left: false,
        right: false
      },
      nav: null,
      bellHovered: false,
      ignoreForBreadCrumb: ["Estate detail"],
      staticsLoaded: false,
      reminderPage: 0,
      currentPage: 1,
      dateFormat: "M/d/YYYY",
      loadingEvents: false
    };
  },
  created() {
    this.showLoading(true);
    this.startSignalR(localStorage.getItem("token"));
    this.$usersHub.$on("user_event", this.onNewUserEvent);
    this.$usersHub.$on("update_signal", this.onNewUpdate);

    this.$errorBus.$on("internal-server", this.handleInternalServerError);
    this.$errorBus.$on("bad-request", this.handleBadRequestError);
    this.$errorBus.$on("access-denied", this.handleAccessDeniedError);
    this.loadStatics({
      $gate: this.$gate,
      done: err => {
        if (err) {
          console.error(err);
          this.$toasted.info("please refresh the page to sync again");
          this.$toasted.global.error("problem in syncing statics data.");
        } else {
          this.setLang(this.$route.params.lang);
          this.staticsLoaded = true;
          this.dateFormat = window.dateFormat;
          this.nav = nav(this.$i18n.locale, this.user.userGroup).items;
          this.$forceUpdate();
        }
        this.showLoading(false);
        this.loadAllSideObjects({
          $gate: this.$gate,
          done: err => {
            console.log(this.currentPage);
            if (err) console.error(err);
            else this.$forceUpdate();
          }
        });
      }
    });
  },
  mounted() {
    console.log(this.$el);
  },
  beforeDestroy() {
    this.$usersHub.$off("user_event", this.onNewUserEvent);
    this.$usersHub.$off("update_signal", this.onNewUpdate);
    this.$errorBus.$off("internal-server", this.handleInternalServerError);
    this.$errorBus.$off("bad-request", this.handleBadRequestError);
    this.$errorBus.$off("access-denied", this.handleAccessDeniedError);
    this.stopSignalR();
  },
  computed: {
    ...mapGetters({
      user: statics.getters.user,
      locales: statics.getters.language,
      selectedLanguage: statics.getters.selectedLanguage,
      userEvents: userEvents.getters.userEvents,
      notCompletedEventCount: userEvents.getters.notCompletedEventCount,
      siteInfo: statics.getters.siteInfo
    }),
    name() {
      return this.$route.name;
    },
    list() {
      return this.$route.matched.filter(
        route =>
          (route.name || route.meta.label) &&
          this.ignoreForBreadCrumb.indexOf(route.name) < 0
      );
    },
    showNextPage() {
      return (
        (this.currentPage - 1) * this.userEvents.pageSize +
          this.userEvents.count <
        this.userEvents.total
      );
    },
    showPrevPage() {
      return this.currentPage !== 1;
    }
  },
  watch: {
    currentPage(val) {
      this.changePageNumber(val);
    }
  },
  methods: {
    ...mapActions({
      loadStatics: statics.actions.loadStatics,
      loadAllSideObjects: userEvents.actions.loadAll,
      loadEvents: userEvents.actions.loadEvents
    }),
    ...mapMutations({
      showLoading: statics.mutations.loading,
      setLang: statics.mutations.selectedLanguage
    }),
    changeLocale(locale) {
      if (this.$route.params.lang === locale.urlCode) return;
      this.$gate.language
        .changeLanguage(locale)
        .then(res => {
          const path = this.$route.path.replace(
            this.$route.params.lang + "/",
            `${locale.urlCode}/`
          );
          location.replace(path);
        })
        .catch(err => this.$handleError(err));
    },
    changePageNumber(page) {
      this.loadingEvents = true;
      this.currentPage = page;
      this.loadEvents({
        $gate: this.$gate,
        page: this.currentPage - 1,
        done: () => {
          this.loadingEvents = false;
        }
      });
    },
    nextPage() {
      this.changePageNumber(this.currentPage + 1);
    },
    prevPage() {
      this.changePageNumber(this.currentPage - 1);
    },
    onNewUserEvent: function(event) {
      this.loadAllSideObjects({
        $gate: this.$gate,
        done: err => {
          if (err) console.error(err);
          else this.$forceUpdate();
        }
      });

      if (event.type === 0)
        this.$toasted.info(
          `You have new request action. [${event.actionTypeName}]: ${
            event.description
          }`,
          {
            action: [
              {
                text: "Show",
                onClick: (e, toastObject) => {
                  this.$router.push({
                    path: "/" + this.$route.params.lang + "/agent/requests",
                    query: { requestId: event.requestId }
                  });
                  toastObject.goAway(0);
                }
              }
            ],
            duration: 20000
          }
        );
      else
        this.$toasted.show(
          `Reminder!!  [${event.actionTypeName}]: ${event.description}`,
          {
            action: [
              {
                text: "Show",
                onClick: (e, toastObject) => {
                  this.$router.push({
                    path: "/" + this.$route.params.lang + "/agent/requests",
                    query: { requestId: event.requestId }
                  });
                  toastObject.goAway(0);
                }
              }
            ],
            duration: 20000
          }
        );
      window.notify();
    },
    onDeleteItem(id) {
      this.$toasted.show("Are you sure you want to delete this item?", {
        action: [
          {
            text: "Yes",
            onClick: (e, toastObject) => {
              this.deleteItemFromDb(id);
              toastObject.goAway(0);
            }
          },
          {
            text: "No",
            onClick: (e, toastObject) => {
              toastObject.goAway(0);
            }
          }
        ]
      });
    },
    deleteItemFromDb(id) {
      this.$gate.requestActionFollowUp
        .delete(id)
        .then(res => {
          this.$toasted.global.deleted();
          this.changePageNumber(this.currentPage);
        })
        .catch(err => this.$handleError(err));
    },
    handleInternalServerError(msg) {
      this.$toasted.global.error(msg);
    },
    handleBadRequestError(msg) {
      this.$toasted.global.error(msg);
    },
    handleAccessDeniedError(msg) {
      this.$toasted.global.error(msg);
    },
    onNewUpdate(update) {
      window.notify();
      this.$toasted.success(update.message);
    }
  }
};
</script>
<style scoped>
.btn-secondary {
  color: white !important;
  background-color: transparent !important;
  border-color: transparent !important;
}

.badge {
  top: 50% !important;
  left: 40% !important;
  font-size: 14px;
}

button {
  cursor: pointer !important;
}

.icon-item {
  font-size: 1.5rem;
  font-weight: bold;
  color: #20a8d8;
  cursor: pointer;
}
</style>
