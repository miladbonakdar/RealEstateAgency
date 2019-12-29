<template>
  <b-card>
    <div slot="header" class="clearfix">
      <b-row>
        <div class="col-md-4">
            <span class="align-middle">
              <i class="fa fa-list"></i> Agent List
            </span>
        </div>
        <div class="col-md-4"></div>

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
    <b-button variant="primary"
              class="float-lg-right newButton"
              @click='$router.pushLang("/agents/details/new")'>
      <i class="fa fa-plus px-1"></i>Add New Agent
    </b-button>
    <b-table
      hover fixed
      :items="agents"
      :fields="fields"
      show-empty
      empty-html='<h6>There are no agent to show!</h6>'
    >
    <template slot-scope="row" slot="firstName">
      <span v-html="$options.filters.highlight(row.item.userAccount.firstName, filter)">{{row.item.userAccount.firstName}}</span>
    </template>
    <template slot-scope="row" slot="lastName">
      <span v-html="$options.filters.highlight(row.item.userAccount.lastName, filter)">{{row.item.userAccount.lastName}}</span>
        {{row.item.userAccount.lastName}}
    </template>
    <template slot-scope="row" slot="userName">
      <span v-html="$options.filters.highlight(row.item.userAccount.userName, filter)">{{row.item.userAccount.userName}}</span>
    </template>
    <template slot-scope="row" slot="email">
      <span v-html="$options.filters.highlight(row.item.userAccount.firstName, filter)">{{row.item.userAccount.email | sub}}</span>
    </template>
    <template slot-scope="row" slot="phone01">
        <b-badge variant="warning" v-if="row.item.userAccount.phone01===null || row.item.userAccount.phone01===''">Empty</b-badge>
        <span v-else>
          {{row.item.userAccount.phone01}}
        </span>
    </template>
    <template slot-scope="row" slot="address01">
        <b-badge variant="warning" v-if="row.item.userAccount.address01===null || row.item.userAccount.address01==='' ">Empty</b-badge>
        <span v-else>
          {{row.item.userAccount.address01 | sub}}
        </span>
    </template>

      <template slot="actions" slot-scope="row">
        <crud-actions
          v-on:deleteItem="deleteItem(row.item.id, row.index)"
          v-on:showDetails="showDetails(row.item.id, row.index)"/>
      </template>
    </b-table>
    <b-row class="px-3">
      <b-pagination
        v-model="currentPage"
        :total-rows="total"
        :per-page="agentPage.pageSize"
        class="my-1 float-left"
      ></b-pagination>
      <b-form-group class="sortComp">
        <b-form-select v-model="agentPage.pageSize" :options="pageOptions" @change="changePage"></b-form-select>
      </b-form-group>
    </b-row>
  </b-card>
</template>

<script>
  import {mapMutations} from 'vuex';
  import {statics} from '../../store/types';

  export default {
    components: {},
    data() {
      return {
        perPage: 5,
        currentPage: 1,
        agentPage: {
          pageNumber: 0,
          pageSize: 5,
          filter: {}
        },
        filter: null,
        total: 10,
        agents: [],
        fields: [
          {key: 'userName', label: 'Username'},
          {key: 'firstName', label: 'First name'},
          {key: 'lastName', label: 'Last Name'},
          {key: 'email', label: 'Email'},
          {key: 'phone01', label: 'Phone'},
          {key: 'address01', label: 'Address'},
          {key: 'actions', label: 'Actions'}
        ],
        pageOptions: [5, 10, 15],


      }
    },
    created() {

      this.getList();
    },
    computed: {
      sortOptions() {
        // Create an options list from our fields
        return this.fields
          .filter(f => f.sortable)
          .map(f => {
            return {text: f.label, value: f.key}
          })
      },
    },
    watch: {
      currentPage(val) {
        this.changePageNumber(val);
      },
      items() {
        this.$forceUpdate();
      }
    },
    methods: {
      ...mapMutations({
        showLoading: statics.mutations.loading
      }),
      setFilter(item) {
                this.filter = item.value;
                this.getList();
            },
            clearSearch() {
                this.filter = null;
                this.getList();
            },
      getList() {
        this.showLoading(true);
        this.agentPage.filter = {searchText: this.filter};
        this.agentPage.pageNumber = this.currentPage - 1;
        this.$gate.agent.page(this.agentPage)
          .then(res => {
            this.agents = res.body.items;
            this.total = res.body.total;
          }).catch(err => this.$handleError(err)).finally(() => {
          this.showLoading(false);
        });
      },
      searchTimeOut() {
                if (this.timer) {
                    clearTimeout(this.timer);
                    this.timer = null;
                }
                this.timer = setTimeout(() => {
                    this.getList();
                }, 500);
            },
      deleteItem(id) {
        this.$toasted.show('Are you sure you want to delete this item?', {
          action: [
            {
              text: 'Yes',
              onClick: (e, toastObject) => {
                this.deleteItemFromDb(id);
                toastObject.goAway(0);
              },
            },
            {
              text: 'No',
              onClick: (e, toastObject) => {
                toastObject.goAway(0);
              },
            }
          ]
        });
      },
      showDetails(id, index) {
        const route = `/agents/details/${id}`;
        this.$router.pushLang(route);
      },
      deleteItemFromDb(id) {

        this.$gate.agent.delete(id)
          .then(res => {
            this.$toasted.global.deleted();
            this.getList();
          })
          .catch(err => this.$handleError(err))
      },
      changePage(value) {
        this.agentPage.pageSize = value;
        this.currentPage = 1;
        this.getList();
      },
      changePageNumber(page) {
        this.currentPage = page;
        this.getList();
      }
    },


  }
</script>

<style>
  .sortComp {
    width: 100px;
    padding-left: 10px;
    padding-top: 4px
  }

  .newButton {
    margin-bottom: 10px;
  }
</style>
