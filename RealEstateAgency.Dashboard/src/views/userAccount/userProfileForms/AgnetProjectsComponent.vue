<template>
  <b-card header-tag="header">
    <div slot="header" class="clearfix">
      <b-row>
        <div class="col-md-4">
            <span class="align-middle">
              <i class="fa fa-list"></i> Your Requests
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
          >
            <template slot="singleLabel" slot-scope="props">
              <span :class="props.option.class"></span>&nbsp;{{props.option.name}}
            </template>
            <template slot="option" slot-scope="props">
              <span :class="props.option.class"></span>&nbsp;{{props.option.name}}
            </template>
          </multi-select>
        </div>
        <div class="col-md-4">
          <b-input-group class="float-right">
            <b-form-input @keyup="searchTimeOut" v-model="filter" placeholder="Type to Search"></b-form-input>
            <b-input-group-append>
              <b-button class="pointer" :disabled="!filter" :class="{'bg-info': filter}" @click="clearSearch"
                        v-html='filter ? "Clear" : `<span class="fa fa-search"> </span>`'></b-button>
            </b-input-group-append>
          </b-input-group>
        </div>
      </b-row>
    </div>
    <div>
      <b-table
        hover
        :items="capitanItems"
        show-empty
        empty-html="<h6>There are no requests to show!</h6>"
        :fields="capitanFields"
      >

        <template slot="type" slot-scope="row">

          <span class="fa fa-sign-in text-success" v-if="!row.item.canAddProperty" v-b-tooltip.hover
                title="Apply"></span>
          <span class="fa fa-sign-out text-danger" v-else title="Submit" v-b-tooltip.hover></span>&nbsp;
          <span v-html="$options.filters.highlight(row.item.type, filter)">{{ row.item.type }}</span>
        </template>
        <template slot="title" slot-scope="row">
          <span v-html="$options.filters.highlight(row.item.title, filter)">{{ row.item.title }}</span>
        </template>
        <template slot="date" slot-scope="row">
          <span>
            {{row.item.dateCreated | moment("from")}}
            <b-badge>{{row.item.dateCreated | moment(dateFormat) }}</b-badge>
          </span>
        </template>

        <template slot="phone" slot-scope="row">
          <b-badge variant="success" v-if="row.item.phone" v-html="$options.filters.highlight(row.item.phone, filter)">
            {{row.item.phone}}
          </b-badge>
          <b-badge variant="warning" v-if="!row.item.phone">Empty</b-badge>
        </template>
        <template slot="fullName" slot-scope="row">
          <span variant="success" v-if="row.item.fullName"
                v-html="$options.filters.highlight(row.item.fullName, filter)">{{row.item.fullName}}</span>
        </template>
        <template slot="email" slot-scope="row">
          <span variant="success" v-if="row.item.email" v-html="$options.filters.highlight(row.item.email, filter)">{{row.item.email}}</span>
        </template>


        <template slot="actions" slot-scope="row">
          <div class="col-sm-12 col-lg-12 col-md-12" style="min-width: 100px;">
            <span
              @click="infoMyAgent(row.item, row.index)"
              class="pointer text-primary mx-1 action-item"
              v-b-tooltip.hover
              title="More info!"
            >
              <i class="fa fa-info-circle"></i>
            </span>
            <span
              @click="changeOrAssignMyAgent(row.item, row.index)"
              class="pointer text-warning mx-1 action-item"
              v-b-tooltip.hover
              title="Change or assign agent!"
            >
              <i
                class="fa fa fa-user-circle"
                :class="{'animated infinite slower heartBeat' : !row.item.isAssigned}"
              ></i>
            </span>
            <span
              @click="addActions(row.item.id)"
              class="pointer text-danger mx-1 action-item action-badge"
              v-b-tooltip.hover
              title="Reminders and Actions"
            >
              <i class="icon-bell mx-1"></i>
              <b-badge pill class="action-badge-item" variant="danger">{{row.item.actionBadges}}</b-badge>
            </span>
            <span
              v-if="row.item.property"
              @click="goToProperty(row.item.property)"
              class="pointer text-black-50 mx-1 action-item"
              v-b-tooltip.hover
              title="Show property"
            >
              <i class="fa fa-home mx-1"></i>
            </span>
            <span
              v-if="!row.item.property && row.item.canAddProperty"
              @click="createProperty(row.item.id)"
              class="pointer text-success mx-1 action-item"
              v-b-tooltip.hover
              title="Create property"
            >
              <i class="fa fa-plus-circle mx-1 animated infinite swing slow"></i>
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
      <b-modal
        v-model="show"
        :title="assignToAgentModal.title"
        :id="assignToAgentModal.id"
        @hide="resetActionModal"
        :no-close-on-esc="false"
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
            v-model="assignToAgentModal.content.agent"
            label="firstName"
            :options="cAgents"
            :searchable="false"
            :allow-empty="false"
            @select="clear('agent')"
          >
            <template
              slot="singleLabel"
              slot-scope="props"
            >{{props.option.firstName}} {{props.option.lastName}} - {{props.option.email}}
            </template>
            <template
              slot="option"
              slot-scope="props"
            >{{props.option.firstName}} {{props.option.lastName}} - {{props.option.email}}
            </template>
          </multi-select>
        </div>
        <div
          v-if="errmsg.agent && hasError"
          class="error vuelidate-invalid-feedback"
        >Please select agent.
        </div>
        <div class="py-1">
          <label for="name">
          <span class="font-lg">
            <i class="fa fa-sticky-note pr-2 text-success"></i>Note:
          </span>
          </label>
          <b-form-textarea
            v-model="assignToAgentModal.content.agentDescription"
            rows="3"
            max-rows="6"
            placeholder="Enter note here..."
          ></b-form-textarea>
        </div>
        <div
          v-if="errmsg.description && hasError"
          class="error vuelidate-invalid-feedback"
        >Note is required.
        </div>
        <div slot="modal-footer" class="w-100">
          <b-button
            variant="primary"
            class="float-right mx-2"
            @click="updateItem(assignToAgentModal.content.itemId)"
          >{{$t('save')}}
          </b-button>
          <b-button variant="secondary" class="float-right mx-2" @click="resetInfoModal">{{$t('cancel')}}</b-button>
        </div>
      </b-modal>
      <b-modal
        v-model="show"
        :title="infoModal.title"
        :id="infoModal.id"
        @hide="resetActionModal"
        :no-close-on-esc="false"
      >
        <div>
        <span class="font-lg">
          <i class="fa fa-home pr-2 text-muted"></i>Type:
        </span>
          {{ infoModal.content.type }}
        </div>
        <br/>
        <div>
        <span class="font-lg">
          <i class="fa fa-phone pr-2 text-primary"></i>Phone:
        </span>
          <b-badge variant="danger" v-if="infoModal.content.phone !== null">{{ infoModal.content.phone }}
          </b-badge>
          <b-badge variant="danger" v-else>Empty</b-badge>
        </div>
        <br/>
        <div>
        <span class="font-lg">
          <i class="fa fa-envelope pr-2 text-info"></i>Email:
        </span>
          <b-badge variant="warning" class="animated pulse">{{ infoModal.content.email }}</b-badge>
        </div>
        <br/>
        <div>
        <span class="font-lg">
          <i class="fa fa-user pr-2"></i>Full Name:
        </span>
          {{ infoModal.content.fullName }}
        </div>
        <br/>
        <div>
        <span class="font-lg">
          <i class="fa fa-file pr-2 text-success"></i>Title:
        </span>
          <span v-if="infoModal.content.title !== null">{{ infoModal.content.title }}</span>
          <b-badge variant="danger" v-else>Empty</b-badge>
        </div>
        <br/>
        <div>
        <span class="font-lg">
          <i class="fa fa-info pr-2 text-primary"></i>Description:
        </span>
          {{ infoModal.content.description }}
        </div>
        <br/>
        <div slot="modal-footer" class="w-100">
          <b-button
            variant="primary"
            class="float-right mx-2"
            @click="updateItem(infoModal.content.itemId)"
          >{{$t('save')}}
          </b-button>
          <b-button variant="secondary" class="float-right mx-2" @click="resetInfoModal">{{$t('cancel')}}</b-button>
        </div>
      </b-modal>
      <b-modal
        size="lg"
        :title="actionModal.title"
        :id="actionModal.id"
        :ok-title="$t('save')"
        :cancel-title="$t('cancel')"
        @hide="resetActionModal"
      >
        <AgentActionsComponent
          v-on:addCount="addActionCount"
          v-bind:requestId="actionModal.content.requestId"
        />
      </b-modal>
    </div>
  </b-card>
</template>


<script>
    import {mapGetters, mapMutations} from "vuex";
    import {statics} from "../../../store/types";
    import AgentActionsComponent from "./AgentActionsComponent";

    export default {
        name: "AgentProjectsComponent",
        components: {
            AgentActionsComponent
        },
        props: ["requestId", "status"],
        data() {
            return {
              value: null,
                dateFormat: window.dateFormat,
                requestCategories: [
                    {name: "All Requests", value: "", class:"fa fa-list text-muted"},
                    {name: "Apply Requests", value: "is:apply", class:"fa fa-sign-in text-success"},
                    {name: "Submit Requests", value: "is:submit", class:"fa fa-sign-out text-danger"},
                    {name: "Rental Requests", value: "is:rent", class:"fa fa-handshake-o text-info"},
                    {name: "Sell Requests", value: "is:sell", class:"fa fa-money text-success"}
                ],
                cAgents: [],
                show: false,
                editorOptions: {
                    placeholder: "Enter description here ...",
                    modules: {
                        toolbar: [
                            ["bold", "italic", "underline", "strike"],
                            ["blockquote", "code-block"],
                            [{header: 1}, {header: 2}],
                            [{list: "ordered"}, {list: "bullet"}],
                            [{script: "sub"}, {script: "super"}],
                            [{indent: "-1"}, {indent: "+1"}],
                            [{direction: "rtl"}],
                            [{size: ["small", false, "large", "huge"]}],
                            [{header: [1, 2, 3, 4, 5, 6, false]}],
                            [{color: []}],
                            [{align: []}],
                            ["clean"]
                        ]
                    }
                },
                filter: null,
                capitanFields: [
                    {key: "type", sortable: true},
                    {key: "title", sortable: true},
                    {key: "date", sortable: true},
                    {key: "phone", sortable: true},
                    {key: "fullName", sortable: true},
                    {key: "email", sortable: true},
                    {key: "actions"}
                ],
                capitanItems: [],
                assignToAgentModal:{
                    id: "assign-modal-agent",
                    title: "Change or assign agent",
                    content: {
                        agent: "",
                        itemId: "",
                        agentDescription: ""
                    }
                },
                infoModal: {
                    id: "info-modal-agent",
                    title: "Request details",
                    content: {
                        type: "",
                        phone: "",
                        fullName: "",
                        email: "",
                        description: "",
                        itemId: "",
                    }
                },
                actionModal: {
                    id: "info-modal-action",
                    title: "Action details and it's reminders",
                    content: {
                        requestId: ""
                    }
                },
                currentPage: 1,
                total: 0,
                propertyPage: {
                    pageNumber: 0,
                    pageSize: 5,
                    filter: {}
                },
                pageOptions: [5, 10, 15],
                errmsg: {
                    agent: false,
                    description: false
                },
                hasError: false
            };
        },
        created() {
            //status true: action component, else reminder componnet
            this.cAgents = this.agents.filter(i => i.agentId !== this.user.agentId);
            this.$usersHub.$on("request_update_signal", this.getRequestList);
            if (this.$route.query && this.$route.query.requestId)
                this.filter = `id:${this.$route.query.requestId}`;
            this.getRequestList();
            this.completeRequestCategories();
        },
        methods: {
            completeRequestCategories() {
                this.requestTypes.forEach(r => {
                    this.requestCategories.push({name: r.name, value: `request_type:${r.id}`, class:""})
                })
            },
            setFilter(item) {
                this.filter = item.value;
                this.getRequestList();
            },
            clearSearch() {
                this.filter = null;
                this.getRequestList();
            },
            clear(item) {
                switch (item) {
                    case "description":
                        this.hasError = false;
                        this.errmsg.description = false;
                        break;
                    case "agent":
                        this.hasError = false;
                        this.errmsg.agent = false;
                        break;
                    case null:
                        this.hasError = false;
                        this.errmsg.agent = false;
                        this.errmsg.description = false;
                        break;
                }
            },
            updateListByDeleting() {
                this.getRequestList();
            },
            searchTimeOut() {
                if (this.timer) {
                    clearTimeout(this.timer);
                    this.timer = null;
                }
                this.timer = setTimeout(() => {
                    this.getRequestList();
                }, 500);
            },
            resetActionModal() {
            },
            getRequestList() {
                this.propertyPage.filter = {searchText: this.filter};
                this.propertyPage.pageNumber = this.currentPage - 1;
                this.showLoading(true);
                let that = this;
                this.$gate.request
                    .page(this.propertyPage)
                    .then(res => {
                        that.total = res.body.total;
                        that.capitanItems = res.body.items;
                        that.capitanItems.forEach(element => {
                            const requestType = that.requestTypes.filter(
                                i => i.id == element.requestTypeId
                            )[0];
                            element["type"] = requestType.name;
                            element.canAddProperty = requestType.canAddProperty;
                            element["fullName"] =
                                element.user.firstName + " " + element.user.lastName;
                            element["phone"] = element.user.phone01;
                            element["email"] = element.user.email;
                            element["actionBadges"] = element.actions.reduce((total, item) => {
                                return item.actionIsSuccess ? total++ : total + 1;
                            }, 0);
                            if (element.isAssigned) {
                                let agent = that.agents.filter(i => i.id == element.agentId)[0];
                                element["agent"] = agent.firstName + " " + agent.lastName;
                            } else {
                                element["agent"] = "Not assigned";
                            }
                        });
                        that.$forceUpdate();
                    })
                    .catch(err => this.$handleError(err))
                    .finally(() => this.showLoading(false));
            },
            infoMyAgent(item, index) {
                this.infoModal.content.type = item.type;
                this.infoModal.content.phone = item.phone;
                this.infoModal.content.fullName = item.fullName;
                this.infoModal.content.email = item.email;
                this.infoModal.content.description = item.description;
                this.infoModal.content.itemId = item.id;
                this.infoModal.content.title = item.title;
                this.$root.$emit("bv::show::modal", this.infoModal.id);
            },
            changeOrAssignMyAgent(item, index) {
                this.assignToAgentModal.content.itemId = item.id;
                this.assignToAgentModal.content.agent = this.agents.filter(
                    i => i.id == item.agentId
                )[0];
                this.assignToAgentModal.content.title = item.title;
                this.$root.$emit("bv::show::modal", this.assignToAgentModal.id);
            },
            addActions(id) {
                this.actionModal.content.requestId = id;
                this.$root.$emit("bv::show::modal", this.actionModal.id);
            },
            resetInfoModal() {
                this.show = false;
                this.infoModal.title = "";
                this.infoModal.content.type = "";
                this.infoModal.content.phone = "";
                this.infoModal.content.fullName = "";
                this.infoModal.content.email = "";
                this.infoModal.content.itemId = "";
                this.infoModal.content.title = "";

                this.infoModal.content.agent = "";
                this.infoModal.content.description = "";
                this.clear(null);
            },
            addReminder(id) {
                this.$router.pushLang("/agent/reminder/" + id);
            },
            history(id) {
                this.$router.pushLang("/agent/history/" + id);
            },
            goToProperty(property) {
                this.$router.pushLang("/estates/estate/" + property.id);
            },
            createProperty(requestId) {
                this.$router.push({
                    path: "/" + this.$route.params.lang + "/estates/estate/new/description",
                    query: {requestId: requestId}
                });
            },
            checkValidation() {
                if (this.infoModal.content.agent == "") {
                    this.errmsg.agent = true;
                    this.hasError = true;
                }
                if (this.infoModal.content.agentDescription == "") {
                    this.errmsg.description = true;
                    this.hasError = true;
                }
            },
            updateItem(id) {
                this.checkValidation();
                if (!this.hasError) {
                    const data = {
                        newAgentId: this.infoModal.content.agent.agentId,
                        requestId: id,
                        description: this.infoModal.content.agentDescription
                    };
                    this.$gate.request
                        .changeRequestAgent(data)
                        .then(res => {
                            this.$toasted.success("Request assigned to agent successfully.");
                        })
                        .catch(err => this.$handleError(err))
                        .finally();
                }
            },
            handleChangeRoute() {
                if (this.$route.query && this.$route.query.requestId)
                    this.filter = `id:${this.$route.query.requestId}`;
                window.scrollTo(0, 1000)
                this.getRequestList();
            },
            changePage(value) {
                this.propertyPage.pageSize = value;
                this.currentPage = 1;
                this.getRequestList();
            },
            changePageNumber(page) {
                this.currentPage = page;
                this.getRequestList();
            },
            addActionCount(reqId) {
                this.getRequestList();
            },
            ...mapMutations({
                showLoading: statics.mutations.loading
            })
        },
        watch: {
            currentPage(val) {
                this.changePageNumber(val);
            },
            capitanItems() {
                this.$forceUpdate();
            },
            '$route.fullPath': {
                handler: function () {
                    this.handleChangeRoute()
                },
                deep: true,
                immediate: true
            },
            filter() {
                if (this.$route.fullPath !== 'en/agent/requests' && this.filter === "")
                    this.$router.pushLang('/agent/requests')
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

<style>
  .vuelidate-invalid-feedback {
    width: 100%;
    margin-top: 0.25rem;
    font-size: 80%;
    color: #f86c6b;
  }

  .titleInfo {
    font-size: 16px;
    font-weight: bold;
  }

  .icon-bell:before {
    content: "\E027";
  }

  .action-badge {
    position: relative;
    min-width: 50px;
    margin: 0;
    text-align: center;
  }

  .action-badge-item {
    position: absolute;
    top: 50%;
    left: 50%;
    margin-top: -16px;
    margin-left: 0;
  }
</style>
