<template>
  <b-card header="Property Status List">
    <b-button variant="primary"
              class="float-lg-right newButton"
              @click='$router.pushLang("/Status/details/new")'>
      <i class="fa fa-plus px-1"></i>Add New Status
    </b-button>
    <b-table
      hover fixed
      :items="propertyStatuss"
      :fields="fields"
      show-empty
      empty-html='<h6>There are no status to show!</h6>'
    >
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
        :per-page="StatusPage.pageSize"
        class="my-1 float-left"
      ></b-pagination>
      <b-form-group class="sortComp">
        <b-form-select v-model="StatusPage.pageSize" :options="pageOptions" @change="changePage"></b-form-select>
      </b-form-group>
    </b-row>
  </b-card>
</template>

<script>
  import {mapMutations} from 'vuex';
  import {statics} from '../../../store/types';

  export default {
    components: {},
    data() {
      return {
        perPage: 5,
        currentPage: 1,
        StatusPage: {
          pageNumber: 0,
          pageSize: 5,
        },
        total: 10,
        propertyStatuss: [],
        fields: [
          {key: 'name', label: 'Name', sortable: true},
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
      getList() {
        this.showLoading(true);
        this.StatusPage.pageNumber = this.currentPage - 1;
        this.$gate.propertyStatus.getPage(this.StatusPage.pageSize, this.StatusPage.pageNumber)
          .then(res => {
            this.propertyStatuss = res.body.items;
            this.total = res.body.total;
          }).catch(err => this.$handleError(err)).finally(() => {
          this.showLoading(false);
        });
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
        const route = `/Status/details/${id}`;
        this.$router.pushLang(route);
      },
      deleteItemFromDb(id) {
        this.$gate.propertyStatus.delete(id)
          .then(res => {
            this.$toasted.global.deleted();
            this.getList();
          })
          .catch(err => this.$handleError(err))
      },
      changePage(value) {
        this.StatusPage.pageSize = value;
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
