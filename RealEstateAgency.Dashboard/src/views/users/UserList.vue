<template>
  <div class="animated fadeIn">

    <b-card header-tag="header">
      <div slot="header" class="clearfix">
        <b-row>
          <div class="col-md-4">
            <span class="align-middle"><i class="fa fa-list"></i> List of Users</span>
          </div>
          <div class="col-md-4"></div>
          <div class="col-md-4">
            <b-input-group class="float-right">
              <b-form-input @keyup="searchTimeOut" v-model="filter" placeholder="Type to Search"></b-form-input>
              <b-input-group-append>
                <b-button class="pointer" :disabled="!filter" :class="{'bg-info': filter}" @click="clearSearch" v-html='filter ? "Clear" : `<span class="fa fa-search"> </span>`'> </b-button>
              </b-input-group-append>
            </b-input-group>
          </div>

        </b-row>
      </div>

      <!-- Main table element -->
      <b-table
        show-empty
        empty-html = '<h6>There are no estates to show!</h6>'
        stacked="md"
        hover
        responsive
        fixed
        :items="items"
        :fields="fields"
        :sort-by.sync="sortBy"
        :sort-desc.sync="sortDesc"
        :sort-direction="sortDirection"
      >
        <template slot="phone01" slot-scope="row">
          <b-badge v-if="row.item.phone01 === null" variant="warning">Empty</b-badge>
        </template>

        <template slot="address01" slot-scope="row">
          <b-badge v-if="row.item.phone01 === null" variant="warning">Empty</b-badge>
        </template>

        <template slot="actions" slot-scope="row">
         <div class="col-sm-12 col-lg-4 col-md-4" style="min-width: 100px;">
            <span
              @click="showDetails(row.item, row.index)"
              class="text-primary mx-1 action-item"
              v-b-tooltip.hover
              title="More info!"
            >
              <i class="fa fa-info"></i>
            </span>
            <span
              @click="deleteItem(row.item, row.index)"
              class="text-danger mx-1 action-item"
              v-b-tooltip.hover
              title="Delete this item?"
            >
              <i class="fa fa-trash-o mx-1"></i>
            </span>
          </div>
        </template>


      </b-table>

      <b-row class="clearfix" v-if="items.length > 0">
        <b-pagination
          v-model="currentPage"
          :total-rows="total"
          :per-page="userPage.pageSize"
          class="my-1  ml-3 float-left"
        ></b-pagination>
        <b-form-group label-cols-sm="1" class="my-1 per-page">
          <b-form-select v-model="userPage.pageSize" :options="pageOptions" @change="changePage"></b-form-select>
        </b-form-group>


      </b-row>

      <!-- Info modal -->
      <b-modal :id="infoModal.id" :title="infoModal.title" ok-only @hide="resetInfoModal">
        <span class="user-title"><i class="fa fa-user px-2 icon-color"></i>Full name:</span><span> {{infoModal.content.firstName}} {{infoModal.content.lastName}}</span>
        <br/>
        <span class="user-title"><i class="fa fa-user px-2 icon-color"></i>Username:</span><span> {{infoModal.content.userName}}</span>
        <br/>

        <span class="user-title"><i class="fa fa-envelope px-2 icon-color"></i>Email:</span><span> {{infoModal.content.email}}</span>
        <br/>

        <span class="user-title"><i class="fa fa-phone px-2 icon-color"></i>Phone 01:</span><b-badge class="mx-1" v-if="infoModal.content.phone01 === null" variant="warning">Empty</b-badge><span v-else> {{infoModal.content.phone01}}</span>

        <br/>
        <span class="user-title"><i class="fa fa-phone px-2 icon-color"></i>Phone 02:</span><b-badge class="mx-1" v-if="infoModal.content.phone02 === null" variant="warning">Empty</b-badge><span v-else> {{infoModal.content.phone02}}</span>

        <br/>
        <span class="user-title"><i class="fa fa-address-card-o px-2 icon-color"></i>Address 01:</span><b-badge class="mx-1" v-if="infoModal.content.address01 === null" variant="warning">Empty</b-badge><span v-else> {{infoModal.content.address01}}</span>
        <br/>

        <span class="user-title"><i class="fa fa-address-card-o px-2 icon-color"></i>Address 02:</span><b-badge class="mx-1" v-if="infoModal.content.address02 === null" variant="warning">Empty</b-badge><span v-else> {{infoModal.content.address02}}</span>

        <br/>
        <span class="user-title"><i class="fa fa-group px-2 icon-color"></i>Group name:</span><span> {{infoModal.content.userGroupName}}</span>

        <br/>
        <span class="user-title"><i class="fa fa-calendar px-2 icon-color"></i>Registration Date:</span><span>{{infoModal.content.registrationDate }}</span>-<b-badge class="mx-" variant="primary">{{infoModal.content.registrationDate | moment("from") }}</b-badge>

        <br/>
        <span class="user-title"><i class="fa fa-file-zip-o px-2 icon-color"></i>Zip Code:</span><b-badge class="mx-1" v-if="infoModal.content.zipCode === null" variant="warning">Empty</b-badge><span v-else> {{infoModal.content.zipCode}}</span>

        <br/>
        <span class="user-title"><i class="fa fa-check px-2 icon-color"></i>Is Active:</span>
        <b-badge variant="success" class="mx-2" v-if="infoModal.content.isActive"> yes</b-badge>
        <b-badge  variant="danger" v-else> no</b-badge>
      </b-modal>
    </b-card>
  </div>
</template>

<script>
    import {mapMutations} from 'vuex';
    import {statics} from '../../store/types';

    export default {
        components: {},
        data() {
            return {
                items: [],
                fields: [
                    {key: 'userName', label: 'Username', class: 'text-center', sortable: true},
                    {key: 'firstName', label: 'First name', class: 'text-center', sortable: true},
                    {key: 'lastName', label: 'Last name', class: 'text-center', sortable: true},
                    {key: 'email', label: 'Email', class: 'text-center', sortable: true},
                    {key: 'phone01', label: 'Phone number', class: 'text-center', sortable: true},
                    {key: 'address01', label: 'Address', class: 'text-center', sortable: true},
                    {key: 'userGroupName', label: 'Group name', class: 'text-center', sortable: true},
                    {key: 'actions', label: 'Actions'}
                ],
                totalRows: 1,
                currentPage: 1,
                total: 0,
                userPage: {
                    pageNumber: 0,
                    pageSize: 5,
                    filter: {},
                },
                totalBody: 0,
                pageOptions: [5, 10, 15],
                sortBy: null,
                sortDesc: false,
                sortDirection: 'asc',
                filter: null,
                infoModal: {
                    id: 'info-modal',
                    title: 'User Info',
                    content: {
                      userName: '',
                      firstName: '',
                      lastName: '',
                      isActive: false,
                      isConfirmed: false,
                      registrationDate: '',
                      middleName: '',
                      email: '',
                      phone01: '',
                      phone02: '',
                      address01: '',
                      address02: '',
                      zipCode: '',
                      userGroupName: ''
                    }
                },
            }

        },
        created() {
            this.getUserList();
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
        mounted() {
            // Set the initial number of items
            this.totalRows = this.items.length
        },
        methods: {
            ...mapMutations({
                showLoading: statics.mutations.loading
            }),
            changePage(value) {
                this.userPage.pageSize = value;
                this.currentPage = 1;
                this.getUserList();
            },
            changePageNumber(page) {
                this.currentPage = page;
                this.getUserList();
            },
            showDetails(content, index) {
                this.infoModal.content = content;
                this.$root.$emit('bv::show::modal', this.infoModal.id);
            },
            resetInfoModal() {
                this.infoModal.title = '';
                this.infoModal.content = '';
            },
            searchTimeOut() {
                if (this.timer) {
                    clearTimeout(this.timer);
                    this.timer = null;
                }
                this.timer = setTimeout(() => {
                    this.getUserList();
                }, 500);
            },
            getUserList() {
                this.userPage.pageNumber = this.currentPage - 1;
                this.userPage.filter = {searchText: this.filter};
                this.showLoading(true);
                this.$gate.userAccount.page(this.userPage).then(res => {
                    this.items = res.body.items;
                    this.total = res.body.total;
                    this.totalBody = res.body.total;

                }).catch(err => this.$handleError(err)).finally(() => {
                    this.showLoading(false);
                });
            },

            deleteItem(item, index) {
                this.$toasted.show('Are you sure you want to delete this user?', {
                    action: [
                        {
                            text: 'Yes',
                            onClick: (e, toastObject) => {
                                this.deleteUserFromDb(item.id);
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
            deleteUserFromDb(id) {
                this.$gate.userAccount.delete(id).then(res => {
                    this.$toasted.global.deleted();
                    this.getUserList();
                }).catch(err => this.$handleError(err));
            }
        }
        ,
        watch: {
            currentPage(val) {
                this.changePageNumber(val);
            },
            items() {
                this.$forceUpdate();
            }
        }
    }
</script>

<style>
  .table {
    margin: 3rem 0 !important;
  }

  .per-page {
    width: 100px;
  }
  .user-title {
    font-weight: bold;
    font-size: 16px;
  }
  .icon-color {
    color: #20a8d8;
  }
  .action-item {
    cursor: pointer;
  }
</style>
