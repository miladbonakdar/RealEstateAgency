<template>
  <div class="animated fadeIn">
    <b-card header="Filters">
      <b-row>
        <b-col md="6" class="my-1">
          <b-form-group label-cols-sm="3" label="Filter" class="mb-0">
            <b-input-group>
              <b-form-input v-model="filter" placeholder="Type to Search"></b-form-input>
              <b-input-group-append>
                <b-button class="pointer" :disabled="!filter" :class="{'bg-info': filter}" v-html='filter ? "Clear" : `<span class="fa fa-search"> </span>`'> </b-button>
              </b-input-group-append>
            </b-input-group>
          </b-form-group>
        </b-col>

        <b-col md="6" class="my-1">
          <b-form-group label-cols-sm="3" label="Sort" class="mb-0">
            <b-input-group>
              <b-form-select v-model="sortBy" :options="sortOptions">
                <option slot="first" :value="null">-- none --</option>
              </b-form-select>
              <b-form-select v-model="sortDesc" :disabled="!sortBy" slot="append">
                <option :value="false">Asc</option>
                <option :value="true">Desc</option>
              </b-form-select>
            </b-input-group>
          </b-form-group>
        </b-col>

        <b-col md="6" class="my-1">
          <b-form-group label-cols-sm="3" label="Sort direction" class="mb-0">
            <b-form-select v-model="sortDirection">
              <option value="asc">Asc</option>
              <option value="desc">Desc</option>
              <option value="last">Last</option>
            </b-form-select>
          </b-form-group>
        </b-col>

        <b-col md="6" class="my-1">
          <b-form-group label-cols-sm="3" label="Per page" class="mb-0">
            <b-form-select v-model="propertyPage.pageSize" :options="pageOptions" @change="changePage"></b-form-select>
          </b-form-group>
        </b-col>
      </b-row>
    </b-card>
    <b-card header="List of Estates">
      <!-- Main table element -->
      <b-table
        show-empty
        empty-html='<h6>There are no basic information to show!</h6>'
        stacked="md"
        hover
        fixed
        :items="items"
        :fields="fields"
        :filter="filter"
        :sort-by.sync="sortBy"
        :sort-desc.sync="sortDesc"
        :sort-direction="sortDirection"
        @filtered="onFiltered"
        @row-clicked="showDetails"

      >
        <template slot="name" slot-scope="row">
          {{ row.value.first }} {{ row.value.last }}
        </template>

        <template slot="isActive" slot-scope="row">
          {{ row.value ? 'Yes :)' : 'No :(' }}
        </template>

        <template slot="actions" slot-scope="row">
          <crud-actions style="min-width: 100px;"
                        v-on:deleteItem="deleteItem(row.item, row.index)"
                        v-on:showDetails="showDetails(row.item, row.index)"/>
        </template>

        <template slot="row-details" slot-scope="row">
          <b-card>
            <ul>
              <li v-for="(value, key) in row.item" :key="key">{{ key }}: {{ value }}</li>
            </ul>
          </b-card>
        </template>
      </b-table>

      <div class="clearfix">
        <b-pagination
          v-model="currentPage"
          :total-rows="total"
          :per-page="propertyPage.pageSize"
          class="my-1 float-left"
        ></b-pagination>

        <b-link :to="{path : '/estates/estate/new/description'}">
          <b-button variant="primary" class="float-right">
            <i class="fa fa-plus"></i> New Estate
          </b-button>
        </b-link>
      </div>

      <!-- Info modal -->
      <b-modal :id="infoModal.id" :title="infoModal.title" ok-only @hide="resetInfoModal">
        <pre>{{ infoModal.content }}</pre>
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
                content: null,
                editorOption: {
                    theme: 'snow'
                },

                items: [],
                fields: [
                    {key: 'title', label: 'Title', sortable: true, class: 'text-center'},
                    {key: 'price', label: 'price', sortable: true, sortDirection: 'desc'},
                    {key: 'propertyTypeName', label: 'Property Type', sortable: true, class: 'text-center'},
                    {key: 'propertyStatusName', label: 'Property Status', sortable: true, class: 'text-center'},
                    {key: 'propertyLabelName', label: 'Property Label', sortable: true, class: 'text-center'},
                    {key: 'actions', label: 'Actions'}
                ],
                totalRows: 1,
                currentPage: 1,
                total: 0,
                propertyPage: {
                    pageNumber: 0,
                    pageSize: 5,
                    filter: {},
                },
                pageOptions: [5, 10, 15],
                sortBy: null,
                sortDesc: false,
                sortDirection: 'asc',
                filter: null,
                infoModal: {
                    id: 'info-modal',
                    title: '',
                    content: ''
                }
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
        mounted() {
            // Set the initial number of items
            this.totalRows = this.items.length
        },
        methods: {
            ...mapMutations({
                showLoading: statics.mutations.loading
            }),
            changePage(value) {
                this.propertyPage.pageSize = value;
                this.currentPage = 1;
                this.getList();
            },
            changePageNumber(page) {
                this.currentPage = page;
                this.getList();
            },
            info(item, index, button) {
                this.infoModal.title = `Row index: ${index}`;
                this.infoModal.content = JSON.stringify(item, null, 2);
                this.$root.$emit('bv::show::modal', this.infoModal.id, button);
            },
            resetInfoModal() {
                this.infoModal.title = '';
                this.infoModal.content = '';
            },
            onFiltered(filteredItems) {
                // Trigger pagination to update the number of buttons/pages due to filtering
                this.totalRows = filteredItems.length;
                this.currentPage = 1;
            },
            getList() {
                this.propertyPage.pageNumber = this.currentPage - 1;
                this.showLoading(true);
                this.$gate.property.getList(this.propertyPage).then(res => {
                    this.items = res.body.items;
                    this.total = res.body.total;
                }).catch(err => this.$handleError(err)
                ).finally(() => {
                    this.showLoading(false);
                });
            },

            deleteItem(item, index) {
                this.$toasted.show('Are you sure you want to delete this item?', {
                    action: [
                        {
                            text: 'Yes',
                            onClick: (e, toastObject) => {
                                this.deleteItemFromDb(item.id);
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
            showDetails(item, index) {
                const route = `/estates/estate/${item.id}/description`;
                this.$router.push(route);
            },
            deleteItemFromDb(id) {
                this.$gate.property.delete(id).then(res => {
                    this.$toasted.global.warn('Data has been deleted!!');
                    this.getList();
                }).catch(err => this.$handleError(err)
                );
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
</style>
