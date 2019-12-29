<template>
  <div class="animated fadeIn">
    <b-card header-tag="header">
      <div slot="header" class="clearfix">
        <span class="align-middle">
          <i class="fa fa-list"></i> Create new reminder
        </span>

        <b-button @click="clearEditInfo" class="pull-right" size="sm" variant="danger">
          <span class="fa fa-arrow-left"></span> Back to actions
        </b-button>
      </div>
      <b-row>
        <div class="col-md-4 col-lg-4 col-sm-12">
          <label for="name">Reminder Type *</label>
          <multi-select
            class="pointer"
            placeholder="Select one"
            deselect-label="Can't remove this value"
            track-by="id"
            v-model="reminder.actionTypeId"
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
          <label for="name">Action create time *</label>
          <datetime
            type="datetime"
            value-zone="local"
            @focus="clear('followUpDateTime')"
            :min-datetime="followUpMinDate"
            input-class="form-control"
            v-model="reminder.followUpDateTime"
          ></datetime>
          <div
            v-if="errmsg.followUpDateTime && hasError"
            class="error vuelidate-invalid-feedback"
          >FollowUp Date Time is required.</div>
        </div>

        <div class="col-md-4 col-lg-4 col-sm-12">
          <label for="name">Action reminder time</label>
          <datetime
            type="datetime"
            value-zone="local"
            input-class="form-control"
            :min-datetime="followUpMinDate"
            :max-datetime="reminder.followUpDateTime"
            v-model="reminder.followUpDateTimeSnooze"
          ></datetime>
        </div>
      </b-row>

      <label for="name">Description:</label>
      <b-form-textarea
        :options="editorOptions"
        v-model="reminder.description"
        placeholder="Enter description here..."
        rows="3"
        max-rows="6"
        @focus="clear('description')"
      ></b-form-textarea>
      <div
        v-if="errmsg.description && hasError"
        class="error vuelidate-invalid-feedback"
      >Description is required.</div>
      <b-row>
        <div class="col-sm-12">
          <div class="py-2">
            <div class="float-right">
              <b-button @click="checkUpdate" type="submit" variant="primary" class="mx-2">
                <i class="fa fa-dot-circle-o"></i>
                {{$t("save")}}
              </b-button>
            </div>
          </div>
        </div>
      </b-row>
      <hr />
      <span v-if="capitanItems.length !== 0">Reminders :</span>
      <b-row class="p-2">
        <span v-for="item in capitanItems" :key="item.id" class="my-1" style="font-size: 16px">
          <b-badge
            class="mx-1 py-2"
            variant="danger"
            v-if="item.isDone"
          >{{item.description}} - {{item.followUpDateTime | moment("from")}}</b-badge>
          <b-badge class="mx-1 py-2 animated tada delay-1s" variant="success" v-else>
            <i class="fa fa-bell-o mr-1"></i>
            {{item.description}} - {{item.followUpDateTime| moment("from")}}
            <i
              @click="onDeleteItem(item.id)"
              class="fa fa-close mx-1 float-right pointer"
            ></i>
          </b-badge>
        </span>
      </b-row>
    </b-card>
    <!-- <b-card header="Reminder List">
      <b-table hover :items="capitanItems" :fields="capitanFields">
        <template slot-scope="row" slot="actions" v-if="!row.item.isDone">

           <crud-actions
              v-on:deleteItem="onDeleteItem(row.item.id, row.index)"
              v-on:showDetails="onUpdateItem(row.item.id)"
            />
        </template>
      </b-table>
    </b-card>-->
    <b-modal
      size="lg"
      :id="infoModal.id"
      :ok-title="$t('save')"
      :cancel-title="$t('cancel')"
      @ok="checkUpdate(infoModal.content.itemId)"
      @hide="resetInfoModal"
    ></b-modal>
  </div>
</template>

<script>
import { mapGetters, mapMutations, mapActions } from "vuex";
import { statics, userEvents } from "../../../store/types";
import AgentReminders from "./AgentReminderComponent";

export default {
  components: {
    AgentReminders
  },
  props: ["itemId"],
  data() {
    return {
      regex: /(<([^>]+)>)/gi,
      followUpMinDate: new Date().toLocalISOString(),
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
          max: 100,
          itemId: "",
          agentDescription: ""
        }
      },
      errmsg: {
        actionTypeId: false,
        description: false,
        followUpDateTime: false
      },
      hasError: false,
      reminder: {
        requestActionId: 0,
        actionTypeId: 0,
        agentIdFollowUp: 0,
        description: "",
        followUpDateTime: "",
        followUpDateTimeSnooze: "",
        isDone: false
      },
      options: {
        useCurrent: false,
        showClear: true,
        showClose: true
      },
      capitanFields: [
        { key: "actionType", sortable: true },
        { key: "followUpDateTime", sortable: true },
        { key: "isDone", sortable: true },
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
    this.getReminderList();
  },
  methods: {
    ...mapMutations({
      showLoading: statics.mutations.loading
    }),
    ...mapActions({
      loadAllSideObjects: userEvents.actions.loadAll
    }),
    clearEditInfo() {
      this.reminder.actionTypeId = 0;
      this.reminder.description = "";
      this.reminder.followUpDateTime = "";
      this.reminder.followUpDateTimeSnooze = false;
      this.updateStatus();
    },
    checkValidation() {
      if (this.reminder.actionTypeId == 0) {
        this.hasError = true;
        this.errmsg.actionTypeId = true;
      }
      if (this.reminder.description == "") {
        this.hasError = true;
        this.errmsg.description = true;
      }
      if (this.reminder.followUpDateTime == "") {
        this.hasError = true;
        this.errmsg.followUpDateTime = true;
      }
    },
    clear(item) {
      switch (item) {
        case "followUpDateTime":
          this.hasError = false;
          this.errmsg.followUpDateTime = false;
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
    updateStatus() {
      this.$emit("changeStatus", true);
    },
    checkUpdate() {
      this.updateMode ? this.updateOnDb() : this.onSubmit();
    },
    onSubmit() {
      this.checkValidation();
      if (!this.hasError) {
        this.showLoading(true);
        this.reminder.requestActionId = this.itemId;
        this.reminder.agentIdFollowUp = this.user.agentId;

        this.reminder.actionTypeId = this.reminder.actionTypeId.id;
        this.$gate.requestActionFollowUp
          .create(this.reminder)
          .then(res => {
            this.clearAction();
            this.updateReminderCount();
            this.$toasted.success(
              "Action for this request created successfully."
            );
            this.getReminderList();

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
    updateReminderCount() {
      this.$emit("addReminderCount");
    },
    clearAction() {
      this.reminder.followUpDateTime = "";

      this.reminder.followUpDateTimeSnooze = "";

      this.reminder.actionTypeId = 0;
      this.reminder.description = "";
      this.reminder.success = false;
    },
    getReminderList() {
      //we should pass request ID in the future
      this.$gate.requestActionFollowUp
        .getActionFollowUps(this.itemId)
        .then(res => {
          this.capitanItems = res.body;
          let that = this;
          that.capitanItems.forEach(element => {
            element["actionType"] = that.actionTypes.filter(
              i => i.id == element.actionTypeId
            )[0].name;
          });
        })
        .catch(err => this.$handleError(err))
        .finally(this.showLoading(false));
    },
    onCancel() {
      this.$router.pushLang("/profile");
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
      this.$gate.requestActionFollowUp
        .delete(id)
        .then(res => {
          this.$toasted.global.deleted();
          this.getReminderList();
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
    scrollToTop() {
      window.scrollTo(0, 50);
    },
    onUpdateItem(id) {
      this.updateMode = true;
      let that = this;
      let reminder = that.capitanItems.find(i => i.id == id);
      this.reminder = reminder;
      let actionTypeId = that.actionTypes.filter(
        i => i.id == reminder.actionTypeId
      )[0];
      this.reminder.actionTypeId = actionTypeId;
      this.scrollToTop();
    },
    updateOnDb() {
      this.checkValidation();
      if (!this.hasError) {
        let that = this;
        const data = {
          id: that.reminder.id,
          requestActionId: that.itemId,
          actionTypeId: that.actionTypes.filter(
            i => i.id == that.reminder.actionTypeId.id
          )[0].id,
          agentIdFollowUp: that.user.agentId,
          description: that.reminder.description,
          followUpDateTime: that.reminder.followUpDateTime,
          followUpDateTimeSnooze: that.reminder.followUpDateTimeSnooze,
          isDone: false
        };
        that.showLoading(true);

        this.$gate.requestActionFollowUp
          .update(data)
          .then(res => {
            this.clearAction();
            this.$toasted.success("Request action update successfully.");
            this.updateMode = false;
            this.getReminderList();
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
      this.infoModal.content.type = item.type;
      this.infoModal.content.phone = item.phone;
      this.infoModal.content.fullName = item.fullName;
      this.infoModal.content.email = item.email;
      this.infoModal.content.subject = item.subject;
      this.infoModal.content.description = item.description;
      this.infoModal.content.itemId = item.id;
      this.infoModal.content.agent = item.agent;
      this.$root.$emit("bv::show::modal", this.infoModal.id);
    },
    resetInfoModal() {
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
</style>
