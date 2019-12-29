<template>
  <div>
    <template slot="title">
      <i class="icon-list"></i>
    </template>
    <b-list-group class="list-group-accent">
      <b-list-group-item
        class="list-group-item-accent-secondary bg-light text-center font-weight-bold text-muted text-uppercase small"
      >Actions</b-list-group-item>
      <div v-if="userAction.items && userAction.items.length !== 0">
      <div
        
        v-for="action in userAction.items"
        :key="action.id"
      >
        <b-list-group-item
          v-if="!action.actionIsSuccess"
          class="list-group-item-accent-success list-group-item-divider"
        >
          <!-- <div class="avatar float-right">
            <img class="img-avatar" src="/img/avatars/7.jpg" alt="admin@bootstrapmaster.com">
          </div>-->
          <span class="fa fa-square-o text-success mr-1"></span>
          <b-badge variant="warning">
            <span class="value-item">{{action.actionType.name}}</span>
          </b-badge>
          <br />
          <small class="text-muted mr-3">
            <i class="fa fa-calendar-check-o text-warning animated infinite heartBeat"></i>
            &nbsp;&nbsp;{{action.actionDate | moment("from")}} at
            <b-badge pill variant="light">{{action.actionDate | moment("YYYY-MM-DD")}}</b-badge>
          </small>
          <div>
            <i class="fa fa-sticky-note-o text-info mr-1"></i>
            <span class="value-item">{{action.description}}</span>
          </div>

          <b-row class="px-2">
            <div
              class="badge-item my-1"
              v-for="followUp in action.actionFollowUps"
              :key="followUp.id"
            >
              <b-badge
                class="mx-1 my-1"
                variant="success"
                v-if="!followUp.isDone"
                v-b-tooltip.hover
                :title="`${followUp.description} - ${$moment(followUp.followUpDateTime).from()} at ${$moment(followUp.followUpDateTime).format(dateFormat)}`"
              >
                <span>{{followUp.description | sub}}</span>
                <i @click="deleteReminder(followUp.id)" class="fa fa-close mx-1 float-right"></i>
              </b-badge>
            </div>
          </b-row>

          <div class="clearfix">
            <i
              class="text-info pointer float-left"
              @click="seeRequest(action.requestId)"
              v-b-tooltip.hover
              title="See Request"
            >
              <span class="fa fa-eye hvr-pulse-grow font-lg"></span>
            </i>

            <i
              @click="deleteAction(action.id)"
              class="text-danger pointer float-right"
              v-b-tooltip.hover
              title="Delete This Action?"
            >
              <span class="fa fa-trash-o hvr-grow font-lg"></span>
            </i>
          </div>
        </b-list-group-item>
        <b-list-group-item v-else class="list-group-item-accent-danger list-group-item-divider">
          <!-- <div class="avatar float-right">
            <img class="img-avatar" src="/img/avatars/7.jpg" alt="admin@bootstrapmaster.com">
          </div>-->
          <i class="fa fa-check-square-o text-danger mr-1"></i>
          <b-badge variant="warning">
            <span class="value-item">{{action.actionType.name}}</span>
          </b-badge>
          <br />
          <small class="text-muted mr-3">
            <i class="fa fa-calendar-check-o text-warning"></i>
            &nbsp;&nbsp;{{action.actionDate | moment("from")}} at
            <b-badge pill variant="light">{{action.actionDate | moment("YYYY-MM-DD")}}</b-badge>
          </small>
          <div>
            <i class="fa fa-sticky-note-o text-info mr-1"></i>
            <span class="value-item">{{action.description}}</span>
          </div>
        </b-list-group-item>
      </div>
      </div>
      <div
        v-if="!userAction.items || userAction.items.length === 0"
        class="text-center text-warning"
      >
        <h6 class="mt-3">There is no action</h6>
      </div>
    </b-list-group>
    <div v-if="(showNextPage || showPrevPage) && !loadingEvents" class="clearfix text-info my-3">
      <i
        v-if="showPrevPage"
        @mouseenter="pageHover.left = true"
        @mouseleave="pageHover.left = false"
        :class="{'text-danger font-2xl' :pageHover.left }"
        @click.prevent.stop="prevPage"
      >
        <span class="float-left fa fa-caret-left font-lg px-4"></span>
      </i>
      <i
        v-if="showNextPage"
        :class="{'text-danger font-2xl' :pageHover.right }"
        @mouseenter="pageHover.right = true"
        @mouseleave="pageHover.right = false"
        @click.prevent.stop="nextPage"
      >
        <span class="float-right fa fa-caret-right font-lg px-4"></span>
      </i>
    </div>
    <div v-else-if="userAction.total != userAction.count" class="text-danger text-center">
      <span class="fa fa-refresh animated infinite flash my-3 mb-4">Loading...</span>
    </div>
  </div>
</template>

<script>
import { Switch as cSwitch } from "@coreui/vue";
import { mapActions, mapGetters, mapMutations } from "vuex";
import { statics, userEvents } from "../store/types";

export default {
  name: "DefaultAside",
  components: {
    cSwitch
  },
  data() {
    return {
      currentPage: 1,
      pageHover: {
        left: false,
        right: false
      },
      actions: [],
      dateFormat: window.dateFormat,
      loadingEvents: false
    };
  },
  created() {},
  computed: {
    ...mapGetters({
      userAction: userEvents.getters.userActions
    }),
    showNextPage() {
      return (
        (this.currentPage - 1) * this.userAction.pageSize +
          this.userAction.count <
        this.userAction.total
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
    ...mapMutations({
      showLoading: statics.mutations.loading
    }),
    ...mapActions({
      loadEvents: userEvents.actions.loadEvents,
      loadActions: userEvents.actions.loadActions,
      loadAllSideObjects: userEvents.actions.loadAll
    }),
    seeRequest(reqId) {
      debugger;
      this.$router.push({
        path: "/" + this.$route.params.lang + "/agent/requests",
        query: { requestId: reqId }
      });
    },
    deleteAction(id) {
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
    seeRequest(id) {
      this.$router.push({
        path: "/" + this.$route.params.lang + "/agent/requests",
        query: { requestId: id }
      });
    },
    deleteReminder(id) {
      this.$toasted.show("Are you sure you want to delete this item?", {
        action: [
          {
            text: "Yes",
            onClick: (e, toastObject) => {
              this.deleteReminderFromDb(id);
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
    deleteReminderFromDb(id) {
      this.showLoading(true);
      this.$gate.requestActionFollowUp
        .delete(id)
        .then(res => {
          this.$toasted.success("Reminder deleted successfully.");
          this.loadAllSideObjects({
            $gate: this.$gate,
            done: err => {
              if (err) console.error(err);
              else this.$forceUpdate();
            }
          });
        })
        .catch(err => this.$toasted.error("Please try again later."))
        .finally(this.showLoading(false));
    },
    deleteItemFromDb(id) {
      this.showLoading(true);
      this.$gate.requestAction
        .delete(id)
        .then(res => {
          this.$toasted.success("Item deleted successfully.");
          this.loadAllSideObjects({
            $gate: this.$gate,
            done: err => {
              if (err) console.error(err);
              else this.$forceUpdate();
            }
          });
        })
        .catch(err => {
          this.$toasted.error("Please try again later.");
        })
        .finally(this.showLoading(false));
    },
    changePageNumber(page) {
      this.loadingEvents = true;
      this.currentPage = page;
      this.loadActions({
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
    }
  }
};
</script>
<style scoped>
.badge-item {
  font-size: 16px;
  cursor: pointer;
}

.item-title {
  font-weight: bold;
  font-size: 12px;
}

.value-item {
  font-size: 12px;
}
</style>
