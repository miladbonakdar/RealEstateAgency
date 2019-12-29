<template>
  <div>
    <b-card header-tag="header" v-if="user.isResponsible">
      <div slot="header" class="clearfix">
        <b-row>
          <div class="col-md-4">
            <span class="align-middle">
              <i class="fa fa-list"></i> New Requests
            </span>
          </div>
          <div class="col-md-4">
            <multi-select
              class="pointer"
              :internal-search="false"
              :allow-empty="false"
              track-by="value"
              label="name"
              :options="requestCategories"
              v-model="value"
              @select="setFilter"
            ></multi-select>
          </div>
          <div class="col-md-4">
            <b-input-group class="float-right">
              <b-form-input
                @keyup="searchTimeOut"
                v-model="filter"
                placeholder="Type to Search"
              ></b-form-input>
              <b-input-group-append>
                <b-button
                  class="pointer"
                  :disabled="!filter"
                  :class="{ 'bg-info': filter }"
                  @click="clearSearch"
                  v-html="
                    filter ? 'Clear' : `<span class='fa fa-search'> </span>`
                  "
                >
                </b-button>
                <b-button
                  class="pointer"
                  :disabled="!filter"
                  :class="{ 'bg-info': filter }"
                  @click="clearSearch"
                  v-html="
                    filter ? 'Clear' : `<span class='fa fa-search'> </span>`
                  "
                ></b-button>
              </b-input-group-append>
            </b-input-group>
          </div>
        </b-row>
      </div>
      <b-table
        show-empty
        empty-html="<h6>There are no requests to show!</h6>"
        hover
        :items="items"
        :fields="fields"
      >
        <template slot="type" slot-scope="row">
          <span v-html="$options.filters.highlight(row.item.type, filter)">{{
            row.item.type
          }}</span>
        </template>
        <template slot="title" slot-scope="row">
          <span v-html="$options.filters.highlight(row.item.title, filter)">{{
            row.item.title
          }}</span>
        </template>
        <template slot="agent" slot-scope="row">
          <b-badge v-if="row.item.isAssigned == false" variant="danger">{{
            row.item.agent
          }}</b-badge>
          <div v-else>{{ row.item.agent }}</div>
        </template>
        <template slot="date" slot-scope="row">
          <span>
            {{ row.item.dateCreated | moment("from") }}
            <b-badge>{{ row.item.dateCreated | moment(dateFormat) }}</b-badge>
          </span>
        </template>
        <template slot="phone" slot-scope="row">
          <b-badge
            variant="success"
            v-if="row.item.phone"
            v-html="$options.filters.highlight(row.item.phone, filter)"
            >{{ row.item.phone }}</b-badge
          >
          <b-badge variant="warning" v-if="!row.item.phone">Empty</b-badge>
        </template>
        <template slot="fullName" slot-scope="row">
          <span
            variant="success"
            v-if="row.item.fullName"
            v-html="$options.filters.highlight(row.item.fullName, filter)"
            >{{ row.item.fullName }}</span
          >
        </template>
        <template slot="email" slot-scope="row">
          <span
            variant="success"
            v-if="row.item.email"
            v-html="$options.filters.highlight(row.item.email, filter)"
            >{{ row.item.email }}</span
          >
        </template>
        <template slot="actions" slot-scope="row">
          <!-- <b-button size="sm" @click="info(row.item, row.index, $event.target)" class="mr-1">
            Info modal
          </b-button>-->
          <div class="col-sm-12 col-lg-4 col-md-4" style="min-width: 100px;">
            <span
              @click="info(row.item, row.index)"
              class="pointer text-primary mx-1 action-item"
              v-b-tooltip.hover
              title="More info!"
            >
              <i class="fa fa-info-circle"></i>
            </span>
            <span
              @click="changeOrAssignAgent(row.item, row.index)"
              class="pointer text-warning mx-1 action-item"
              v-b-tooltip.hover
              title="Change or assign agent!"
            >
              <i
                class="fa fa fa-user-circle"
                :class="{
                  'animated infinite slower heartBeat': !row.item.isAssigned
                }"
              ></i>
            </span>
            <span
              @click="deleteItem(row.item.id)"
              class="pointer text-danger mx-1 action-item"
              v-b-tooltip.hover
              title="Delete this item?"
            >
              <i class="fa fa-trash-o mx-1"></i>
            </span>
          </div>
        </template>
      </b-table>
      <b-row class="clearfix">
        <b-pagination
          v-model="currentPage"
          :total-rows="total"
          :per-page="propertyPage.pageSize"
          class="my-1 ml-3 float-left"
        ></b-pagination>
        <b-form-group label-cols-sm="1" class="my-1 perPage">
          <b-form-select
            v-model="propertyPage.pageSize"
            :options="pageOptions"
            @change="changePage"
          ></b-form-select>
        </b-form-group>
      </b-row>
    </b-card>
    <AgentProjectsComponent ref="list" />
    <b-modal
      :title="infoModalReq.title"
      :id="infoModalReq.id"
      :ok-title="$t('save')"
      :cancel-title="$t('cancel')"
      @ok="updateItem(infoModalReq.content.itemId)"
      @hide="resetInfoModal"
    >
      <div>
        <span class="font-lg">
          <i class="fa fa-home pr-2 text-muted"></i>Type:
        </span>
        {{ infoModalReq.content.type }}
      </div>
      <br />
      <div>
        <span class="font-lg">
          <i class="fa fa-phone pr-2 text-primary"></i>Phone:
        </span>
        <b-badge variant="danger" v-if="infoModalReq.content.phone !== null">{{
          infoModalReq.content.phone
        }}</b-badge>
        <b-badge variant="danger" v-else>Empty</b-badge>
      </div>
      <br />
      <div>
        <span class="font-lg">
          <i class="fa fa-envelope pr-2 text-info"></i>Email:
        </span>
        <b-badge variant="warning" class="animated pulse">{{
          infoModalReq.content.email
        }}</b-badge>
      </div>
      <br />
      <div>
        <span class="font-lg"> <i class="fa fa-user pr-2"></i>Full Name: </span>
        {{ infoModalReq.content.fullName }}
      </div>
      <br />
      <div>
        <span class="font-lg">
          <i class="fa fa-file pr-2 text-success"></i>Title:
        </span>
        <span v-if="infoModalReq.content.title !== null">{{
          infoModalReq.content.title
        }}</span>
        <b-badge variant="danger" v-else>Empty</b-badge>
      </div>
      <br />
      <div>
        <span class="font-lg">
          <i class="fa fa-info pr-2 text-primary"></i>Description:
        </span>
        {{ infoModalReq.content.description }}
      </div>
      <br />
    </b-modal>
    <b-modal
      :id="assignAgentModal.id"
      :title="assignAgentModal.title"
      :ok-title="$t('save')"
      :cancel-title="$t('cancel')"
      @ok="updateItem(assignAgentModal.content.itemId)"
      @hide="resetInfoModal"
    >
      <div>
        <label for="name">
          <span class="font-lg">
            <i class="fa fa-user-secret pr-2"></i>Agent:
          </span>
        </label>

        <multi-select
          class="pointer"
          placeholder="Select one"
          deselect-label="Can't remove this value"
          track-by="id"
          v-model="assignAgentModal.content.agent"
          label="agentName"
          :options="agents"
          :searchable="false"
          :allow-empty="false"
        >
          <template slot="singleLabel" slot-scope="props"
            >{{ props.option.firstName }} {{ props.option.lastName }} -
            {{ props.option.email }}</template
          >
          <template slot="option" slot-scope="props"
            >{{ props.option.firstName }} {{ props.option.lastName }} -
            {{ props.option.email }}</template
          >
        </multi-select>
      </div>
      <div class="py-1">
        <label for="name">
          <span class="font-lg">
            <i class="fa fa-sticky-note pr-2 text-success"></i>Note:
          </span>
        </label>
        <b-form-textarea
          v-model="assignAgentModal.content.agentDescription"
          rows="3"
          max-rows="6"
          placeholder="Enter note here..."
        ></b-form-textarea>
      </div>
    </b-modal>
  </div>
</template>

<script>
import AgentProjectsComponent from "./AgnetProjectsComponent";
import { mapGetters, mapMutations } from "vuex";
import { statics } from "../../../store/types";

export default {
  name: "AgentRequests",
  components: {
    AgentProjectsComponent
  },
  data() {
    return {
      dateFormat: window.dateFormat,
      agentObj: {},
      filter: null,
      infoModalReq: {
        id: "info-modal",
        title: "Request details",
        total: 0,
        content: {
          type: "",
          phone: "",
          fullName: "",
          email: "",
          description: "",
          agent: null,
          title: "",
          itemId: "",
          agentDescription: ""
        }
      },
      assignAgentModal: {
        id: "change-agent-modal",
        title: "Change or assign agent",
        total: 0,
        content: {
          agent: null,
          itemId: "",
          agentDescription: ""
        }
      },
      requestCategories: [
        { name: "Unassigned Requests", value: "is:unassigned" },
        { name: "Assigned Requests", value: "is:assigned" },
        { name: "All Requests", value: "" }
      ],
      currentPage: 1,
      total: 0,
      propertyPage: {
        pageNumber: 0,
        pageSize: 5,
        filter: {}
      },
      fields: [
        { key: "type", sortable: true },
        { key: "title", sortable: true },
        { key: "date", sortable: true },
        { key: "phone", sortable: true },
        { key: "fullName", sortable: true },
        { key: "email", sortable: true },
        { key: "agent", sortable: true },
        { key: "actions" }
      ],
      pageOptions: [5, 10, 15],
      items: [],
      value: ""
    };
  },
  created() {
    if (this.user.isResponsible === true) {
      this.getOpenRequestsList();
      this.$usersHub.$on("request_update_signal", this.getOpenRequestsList);
    }
  },
  beforeDestroy() {
    this.$usersHub.$off("request_update_signal", this.getOpenRequestsList);
  },
  methods: {
    setFilter(item) {
      this.filter = item.value;
      this.getOpenRequestsList();
    },
    clearSearch() {
      this.filter = null;
      this.getOpenRequestsList();
    },
    searchTimeOut() {
      if (this.timer) {
        clearTimeout(this.timer);
        this.timer = null;
      }
      this.timer = setTimeout(() => {
        this.getOpenRequestsList();
      }, 500);
    },
    changeOrAssignAgent(item, index) {
      this.assignAgentModal.content.agent = this.agents.filter(
        i => i.id == item.agentId
      )[0];
      this.assignAgentModal.content.itemId = item.id;
      this.assignAgentModal.content.title = item.title;
      this.$root.$emit("bv::show::modal", this.assignAgentModal.id);
    },
    info(item, index) {
      this.infoModalReq.content.type = item.type;
      this.infoModalReq.content.phone = item.phone;
      this.infoModalReq.content.fullName = item.fullName;
      this.infoModalReq.content.email = item.email;
      this.infoModalReq.content.description = item.description;
      this.infoModalReq.content.agent = this.agents.filter(
        i => i.id == item.agentId
      )[0];
      this.infoModalReq.content.itemId = item.id;
      this.infoModalReq.content.title = item.title;
      this.$root.$emit("bv::show::modal", this.infoModalReq.id);
    },
    getOpenRequestsList() {
      this.propertyPage.filter = { searchText: this.filter };
      this.propertyPage.pageNumber = this.currentPage - 1;
      this.showLoading(true);
      let that = this;
      this.$gate.request
        .getOpenRequests(this.propertyPage)
        .then(res => {
          this.total = res.body.total;
          this.items = res.body.items;
          this.items.forEach(element => {
            element["type"] = that.requestTypes.filter(
              i => i.id == element.requestTypeId
            )[0].name;
            element["fullName"] =
              element.user.firstName + " " + element.user.lastName;
            element["phone"] = element.user.phone01;
            element["email"] = element.user.email;
            if (element.isAssigned) {
              let agent = that.agents.filter(i => i.id == element.agentId)[0];
              element["agent"] = agent.firstName + " " + agent.lastName;
            } else {
              element["agent"] = "Not assigned";
            }
          });
        })
        .catch(err => this.$handleError(err))
        .finally(() => this.showLoading(false));
    },
    resetInfoModal() {
      this.infoModalReq.content.type = "";
      this.infoModalReq.content.phone = "";
      this.infoModalReq.content.fullName = "";
      this.infoModalReq.content.email = "";
      this.infoModalReq.content.agent = null;
      this.infoModalReq.content.itemId = "";
      this.infoModalReq.content.title = "";
      this.infoModalReq.content.description = "";
    },
    deleteItem(id) {
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
      this.$gate.request
        .delete(id)
        .then(res => {
          this.$toasted.success("Request deleted successfully.");
          this.getOpenRequestsList();
          this.$refs.list.updateListByDeleting();
        })
        .catch(err => this.$handleError(err))
        .finally(() => this.showLoading(false));
    },
    updateItem(id) {
      if (this.infoModalReq.content.agent === undefined) {
        this.$toasted.global.warn(
          "Please select an agent to assign a request."
        );
      } else {
        const data = {
          newAgentId: this.infoModalReq.content.agent.agentId,
          requestId: id,
          description: this.infoModalReq.content.agentDescription
        };
        this.showLoading(true);
        this.$gate.request
          .changeRequestAgent(data)
          .then(res => {
            this.$toasted.success("Request assigned to agent successfully.");
            this.getOpenRequestsList();
          })
          .catch(err => this.$handleError(err))
          .finally(() => this.showLoading(false));
      }
    },
    changePage(value) {
      this.propertyPage.pageSize = value;
      this.currentPage = 1;
      this.getOpenRequestsList();
    },
    changePageNumber(page) {
      this.currentPage = page;
      this.getOpenRequestsList();
    },
    ...mapMutations({
      showLoading: statics.mutations.loading
    })
  },
  watch: {
    currentPage(val) {
      this.changePageNumber(val);
    },
    items() {
      this.$forceUpdate();
    }
  },
  computed: {
    ...mapGetters({
      requestTypes: statics.getters.requestType,
      agents: statics.getters.agent,
      user: statics.getters.user
    })
  }
};
</script>
