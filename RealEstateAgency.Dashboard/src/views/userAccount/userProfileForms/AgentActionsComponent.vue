<template>
  <div>
    <b-card class="animated fadeIn" header-tag="header" v-if="componentStatus">
      <div slot="header" class="clearfix">
        <span class="align-middle">
          <i class="fa fa-list"></i>&nbsp;
          <span v-if="updateMode">Edit selected</span>
          <span v-else>Create new</span> action
        </span>
        <span v-b-tooltip.hover title="Discard edit?">
          <b-button
            v-if="updateMode"
            @click="clearEditInfo"
            size="sm"
            variant="danger"
            class="float-right"
          >
            <span class="fa fa-close"></span> Discard edit
          </b-button>
        </span>
      </div>
      <b-row>
        <div class="col-md-4 col-lg-4 col-sm-12">
          <label for="name">Action Type:</label>
          <multi-select
            class="pointer"
            placeholder="Select one"
            deselect-label="Can't remove this value"
            track-by="id"
            v-model="action.actionTypeId"
            label="name"
            :options="actionTypes"
            :searchable="false"
            :allow-empty="false"
            @select="clear('actionType')"
          ></multi-select>
          <div
            v-if="errmsg.actionTypeId && hasError"
            class="error vuelidate-invalid-feedback"
          >Action Type is required.</div>
        </div>
        <div class="col-md-4 col-lg-4 col-sm-12">
          <label for="name">Action Date Time:</label>
          <datetime
            type="datetime"
            value-zone="local"
            @focus="clear('actionDate')"
            input-class="form-control"
            v-model="action.actionDate"
          ></datetime>
          <div
            v-if="errmsg.actionDate && hasError"
            class="error vuelidate-invalid-feedback"
          >Action date and time is required.</div>
        </div>

        <div class="col-md-4 col-lg-4 col-sm-12">
          <label for="name"></label>
          <b-form-checkbox v-model="action.actionIsSuccess" name="checkbox-1" class="my-3">Success</b-form-checkbox>
        </div>
      </b-row>

      <label for="name">Description:</label>

      <b-form-textarea
        class="editor-example bubble"
        :options="editorOptions"
        v-model="action.description"
        @focus="clear('description')"
        rows="3"
        max-rows="6"
        placeholder="Enter description here..."
      ></b-form-textarea>
      <div
        v-if="errmsg.description && hasError"
        class="error vuelidate-invalid-feedback"
      >Description is required.</div>
      <submit-group v-on:onCancel="onCancel" v-on:onSubmit="checkUpdate" />
    </b-card>
    <AgentReminders
      v-else
      v-bind:itemId="infoModal.content.itemId"
      v-on:addReminderCount="addReminderCountItem"
      v-on:changeStatus="changeStatusComponent"
    />

    <b-card header="Action List" style="position: inherit">
      <b-table
        show-empty
        empty-html="<h6>There are no actions to show!</h6>"
        hover
        :items="capitanItems"
        :fields="capitanFields"
        class="dastan"
      >
        <template slot-scope="row" slot="actionDate">
          {{row.item.actionDate | moment("from")}}
          <b-badge>{{row.item.actionDate | moment("YYYY-MM-DD") }}</b-badge>
        </template>
        <template slot="description" slot-scope="row">
          <span v-b-tooltip.hover :title="row.item.description">{{row.item.description | sub}}</span>
        </template>
        <template slot-scope="row" slot="actions">
          <b-row>
            <span
              @click="onDeleteItem(row.item.id)"
              class="text-danger mx-1 action-item pointer"
              v-b-tooltip.hover
              title="Delete this item?"
            >
              <i class="fa fa-trash-o mx-1"></i>
            </span>
            <span
              @click="onUpdateItem(row.item.id)"
              v-if="row.item.isSuccess === 'No'"
              class="text-primary mx-1 action-item pointer"
              v-b-tooltip.hover
              title="More info and edit!"
            >
              <i class="fa fa-pencil-square-o mx-1"></i>
            </span>
            <span
              @click="addReminder(row.item.id)"
              class="text-warning mx-1 action-item pointer"
              v-if="row.item.isSuccess === 'No'"
              v-b-tooltip.hover
              title="Add reminder!"
            >
              <i class="icon-bell mx-1"></i>
              <b-badge
                pill
                variant="warning"
                class="followup-badge-item"
              >{{row.item.followUpBadges}}</b-badge>
            </span>
          </b-row>
        </template>
      </b-table>
    </b-card>
    <b-modal title="Add reminder for this action!" v-model="show" size="lg">
      <div slot="modal-footer" class="w-100">
        <b-button
          variant="secondary"
          style="color:#6e6c6c"
          class="mx-2 float-right"
          @click="resetInfoModal"
        >
          <i class="fa fa-ban"></i>
          Close
        </b-button>
      </div>
    </b-modal>
  </div>
</template>

<script>
import { mapGetters, mapMutations, mapActions } from "vuex";
import { statics, userEvents } from "../../../store/types";
import AgentReminders from "./AgentReminderComponent";

export default {
  name: "AgentActionsComponent",
  components: {
    AgentReminders
  },
  props: ["itemId", "requestId"],
  data() {
    return {
      dateFormat: window.dateFormat,
      componentStatus: true,
      lastActionId: [],
      reminderStatus: false,
      editorOptions: {
        placeholder: "Enter description here ...",
        modules: {
          toolbar: [
            ["bold", "italic", "underline", "strike"],
            ["blockquote", "code-block"],
            [{ header: 1 }, { header: 2 }],
            [{ list: "ordered" }, { list: "bullet" }],
            [{ script: "sub" }, { script: "super" }],
            [{ indent: "-1" }, { indent: "+1" }],
            [{ direction: "rtl" }],
            [{ size: ["small", false, "large", "huge"] }],
            [{ header: [1, 2, 3, 4, 5, 6, false] }],
            [{ color: [] }],
            [{ align: [] }],
            ["clean"]
          ]
        }
      },
      actionId: 0,
      show: false,
      updateMode: false,
      infoModal: {
        id: "info-modal-reminder",
        content: {
          type: "",
          phone: "",
          fullName: "",
          email: "",
          subject: "",
          description: "",
          agent: "",
          value: 45,
          max: 100,
          itemId: "",
          agentDescription: ""
        }
      },
      errmsg: {
        actionTypeId: false,
        description: false,
        actionDate: false
      },
      hasError: false,
      action: {
        requestId: 0,
        actionTypeId: 0,
        agentId: 0,
        description: "",
        actionDate: "",
        actionTime: "",
        actionIsSuccess: false
      },
      options: {
        useCurrent: false,
        showClear: true,
        showClose: true
      },
      capitanFields: [
        { key: "actionType", sortable: true },
        { key: "actionDate", sortable: true },
        { key: "isSuccess", sortable: true },
        { key: "description", sortable: true },
        { key: "actions" }
      ],
      capitanItems: []
    };
  },
  computed: {
    ...mapGetters({
      user: statics.getters.user,
      actionTypes: statics.getters.actionType
    })
  },

  created() {
    this.getActionList();
    this.$usersHub.$on("requestaction_update_signal", this.getActionList);
  },
  methods: {
    ...mapMutations({
      showLoading: statics.mutations.loading
    }),
    ...mapActions({
      loadAllSideObjects: userEvents.actions.loadAll
    }),
    addReminderCountItem() {
      debugger;
      this.getActionList();
    },
    addReminder(id) {
      this.reminderStatus = true;
      this.clearAction();
      this.infoModal.content.itemId = id;
      this.changeStatusComponent(false);
    },
    changeStatusComponent(state) {
      this.componentStatus = state;
    },
    checkValidation() {
      if (this.action.actionTypeId == 0) {
        this.hasError = true;
        this.errmsg.actionTypeId = true;
      }
      if (this.action.description == "") {
        this.hasError = true;
        this.errmsg.description = true;
      }
      if (this.action.actionDate == "") {
        this.hasError = true;
        this.errmsg.actionDate = true;
      }
    },
    clear(item) {
      switch (item) {
        case "actionDate":
          this.hasError = false;
          this.errmsg.actionDate = false;
          break;
        case "description":
          this.hasError = false;
          this.errmsg.description = false;
          break;
        case "actionType":
          this.hasError = false;
          this.errmsg.actionTypeId = false;
          break;
      }
    },
    checkUpdate() {
      this.updateMode ? this.updateOnDb() : this.onSubmit();
    },
    clearEditInfo() {
      this.updateMode = false;
      this.action.actionTypeId = 0;
      this.action.description = "";
      this.action.actionDate = "";
      this.action.actionIsSuccess = false;
    },
    onSubmit() {
      this.checkValidation();
      if (!this.hasError) {
        this.showLoading(true);
        this.action.requestId = this.requestId;
        this.action.agentId = this.user.agentId;
        this.action.actionTypeId = this.action.actionTypeId.id;
        this.$gate.requestAction
          .create(this.action)
          .then(res => {
            this.clearAction();
            this.$toasted.success(
              "Action for this request created successfully."
            );
            this.updateActionCount();
            this.getActionList();
            this.loadAllSideObjects({
              $gate: this.$gate,
              done: err => {
                if (err) console.error(err);
                else this.$forceUpdate();
              }
            });
          })
          .catch(err => this.$handleError(err))
          .finally(this.showLoading(false));
      }
    },
    updateActionCount() {
      this.$emit("addCount", true);
    },
    clearAction() {
      debugger;
      this.action.actionDate = "";
      this.action.actionTypeId = 0;
      this.action.description = "";
      this.action.actionIsSuccess = false;
      this.hasError = false;
      this.errmsg.actionTypeId = false;
      this.errmsg.description = false;
      this.errmsg.actionDate = false;
    },
    getActionList() {
      //we should pass request ID in the future
      this.$gate.requestAction
        .getRequestActions(this.requestId)
        .then(res => {
          this.capitanItems = res.body;
          let that = this;
          that.capitanItems.forEach(element => {
            element["actionType"] = that.actionTypes.filter(
              i => i.id == element.actionTypeId
            )[0].name;
            element["isSuccess"] = element.actionIsSuccess ? "Yes" : "No";
            if (element.isSuccess) {
              element["followUpBadges"] = element.actionFollowUps.filter(
                i => i.isDone === false
              ).length;
            }
          });
        })
        .catch(err => this.$handleError(err))
        .finally(this.showLoading(false));
    },
    onCancel() {
      this.clearAction();
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
      this.showLoading(true);
      this.$gate.requestAction
        .delete(id)
        .then(res => {
          this.$toasted.global.deleted();
          this.changeStatusComponent(true);
          this.updateMode = false;
          this.clearAction();
          this.getActionList();
          this.updateActionCount();
          this.updateOtherActionsList();
          this.loadAllSideObjects({
            $gate: this.$gate,
            done: err => {
              if (err) console.error(err);
              else this.$forceUpdate();
            }
          });
        })
        .catch(err => this.$handleError(err))
        .finally(this.showLoading(false));
    },
    updateOtherActionsList() {
      this.$emit("updateByDelete");
    },
    scrollToTop() {
      window.scrollTo(0, 50);
    },
    onUpdateItem(id) {
      debugger;
      this.componentStatus = true;
      this.updateMode = true;
      let that = this;
      let action = { ...that.capitanItems.find(i => i.id == id) };
      this.action = action;
      let actionTypeId;
      if (
        this.lastActionId.length !== 0 &&
        this.lastActionId.find(i => i.id == id)
      ) {
        actionTypeId = that.actionTypes.filter(
          i => i.id == action.actionTypeId
        )[0];
      } else {
        actionTypeId = that.actionTypes.filter(
          i => i.id == action.actionTypeId
        )[0];
      }
      this.lastActionId.push({ id: id });
      this.action.actionTypeId = actionTypeId;
      this.scrollToTop();
    },
    updateOnDb() {
      this.checkValidation();
      if (!this.hasError) {
        let that = this;
        const data = {
          id: that.action.id,
          requestId: this.requestId,
          actionTypeId: that.actionTypes.filter(
            i => i.id == that.action.actionTypeId.id
          )[0].id,
          agentId: that.user.agentId,
          description: that.action.description,
          actionDate: that.action.actionDate,
          actionTime: that.action.actionTime,
          actionIsSuccess: that.action.actionIsSuccess
        };
        that.showLoading(true);

        this.$gate.requestAction
          .update(data)
          .then(res => {
            this.clearAction();
            this.$toasted.success("Request action update successfully.");
            this.updateMode = false;
            this.getActionList();
            this.loadAllSideObjects({
              $gate: this.$gate,
              done: err => {
                if (err) console.error(err);
                else this.$forceUpdate();
              }
            });
          })
          .catch(err => this.$handleError(err))
          .finally(this.showLoading(false));
      }
    },
    info(id, item) {
      if (!item.actionIsSuccess) {
        this.infoModal.content.type = item.type;
        this.infoModal.content.phone = item.phone;
        this.infoModal.content.fullName = item.fullName;
        this.infoModal.content.email = item.email;
        this.infoModal.content.subject = item.subject;
        this.infoModal.content.description = item.description;
        this.infoModal.content.itemId = item.id;
        this.infoModal.content.agent = item.agent;

        this.$root.$emit("bv::show::modal", this.infoModal.id);
      } else {
        this.$toasted.global.warn(
          "Action is completed and you can't add reminder for this."
        );
      }
    },
    resetInfoModal() {
      this.show = false;
      this.infoModal.title = "";
      this.infoModal.content.type = "";
      this.infoModal.content.phone = "";
      this.infoModal.content.fullName = "";
      this.infoModal.content.email = "";
      this.infoModal.content.itemId = "";
    }
  }
};
</script>

<style>
.vuelidate-invalid-feedback {
  width: 100%;
  margin-top: 0.25rem;
  font-size: 80%;
  color: #f86c6b;
}
/*

  .table.b-table > thead > tr > th, .table.b-table > thead > tr > td, .table.b-table > tfoot > tr > th, .table.b-table > tfoot > tr > td {
    position: inherit !important;
  }
 */

.table.b-table.dastan > thead > tr > th {
  position: inherit !important;
}
.followup-badge-item {
  position: absolute;
  margin-top: -6px;
  margin-left: -12px;
}
</style>
