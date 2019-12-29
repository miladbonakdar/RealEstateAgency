<template>
  <div class="animated fadeIn">
    <b-card header-tag="header">
      <div slot="header" class="clearfix">
        <b-row>
          <div class="col-md-4">
            <span class="align-middle"
              ><i class="fa fa-list"></i> List of Estates</span
            >
          </div>

          <div class="col-md-4">
            <multi-select
              class="pointer"
              :internal-search="false"
              :allow-empty="false"
              track-by="value"
              label="name"
              :options="propertyCategories"
              v-model="selectedCategory"
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
                    filter
                      ? 'Clear'
                      : `<span class=&quot;fa fa-search&quot;> </span>`
                  "
                >
                </b-button>
              </b-input-group-append>
            </b-input-group>
          </div>
        </b-row>
      </div>
      <b-link
        class="py-3"
        :to="{ path: `/${$route.params.lang}/estates/estate/new/description` }"
      >
        <b-button variant="primary" class="float-right" size="sm">
          <i class="fa fa-plus"></i> New Estate
        </b-button>
      </b-link>
      <!-- Main table element -->
      <b-table
        show-empty
        empty-html="<h6>There are no estates to show!</h6>"
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
        <template slot="title" slot-scope="row">
          <span v-html="$options.filters.highlight(row.item.title, filter)">{{
            row.item.title
          }}</span>
        </template>
        <template slot="propertyTypeName" slot-scope="row">
          <span
            v-html="
              $options.filters.highlight(row.item.propertyTypeName, filter)
            "
            >{{ row.item.propertyTypeName }}</span
          >
        </template>
        <template slot="propertyStatusName" slot-scope="row">
          <span
            v-html="
              $options.filters.highlight(row.item.propertyStatusName, filter)
            "
            >{{ row.item.propertyStatusName }}</span
          >
        </template>
        <template slot="propertyLabelName" slot-scope="row">
          <span
            v-html="
              $options.filters.highlight(row.item.propertyLabelName, filter)
            "
            >{{ row.item.propertyLabelName }}</span
          >
        </template>

        <template slot="price" slot-scope="row">
          {{ row.value | currency }}
        </template>

        <template slot="isActive" slot-scope="row">
          {{ row.value ? "Yes :)" : "No :(" }}
        </template>
        <template slot="status" slot-scope="row">
          <b-badge variant="warning" v-if="row.item.isPublished"
            >Published</b-badge
          >
          <b-badge variant="success" v-else-if="row.item.readyForPublish"
            >Ready for publish</b-badge
          >
          <b-badge variant="danger" v-else>Not ready</b-badge>
        </template>

        <template slot="actions" slot-scope="row">
          <div class="col-sm-12 col-lg-4 col-md-4" style="min-width: 150px;">
            <span
              @click="showDetails(row.item, row.index)"
              class="text-primary mx-1 action-item"
              v-b-tooltip.hover
              title="More info and edit!"
            >
              <i class="fa fa-pencil-square-o"></i>
            </span>
            <span
              @click="deleteItem(row.item, row.index)"
              class="text-danger mx-1 action-item"
              v-b-tooltip.hover
              title="Delete this item?"
            >
              <i class="fa fa-trash-o"></i>
            </span>
            <span
              v-if="
                user.hasPublishingAuthorization &&
                  row.item.readyForPublish &&
                  !row.item.isPublished
              "
              @click="publishProperty(row.item)"
              class="text-success mx-1 action-item"
              v-b-tooltip.hover
              title="Publish this property?"
            >
              <i class="fa fa-check-square-o"></i>
            </span>
            <span
              v-if="user.hasPublishingAuthorization && row.item.isPublished"
              @click="unPublishProperty(row.item)"
              class="text-danger mx-1 action-item"
              v-b-tooltip.hover
              title="UnPublish this property?"
            >
              <i class="fa fa-times-rectangle-o"></i>
            </span>
          </div>
        </template>

        <template slot="row-details" slot-scope="row">
          <b-card>
            <ul>
              <li v-for="(value, key) in row.item" :key="key">
                {{ key }}: {{ value }}
              </li>
            </ul>
          </b-card>
        </template>
      </b-table>

      <b-row class="clearfix" v-if="items.length > 0">
        <b-pagination
          v-model="currentPage"
          :total-rows="total"
          :per-page="propertyPage.pageSize"
          class="my-1  ml-3 float-left"
        ></b-pagination>
        <b-form-group label-cols-sm="1" class="my-1 perPage">
          <b-form-select
            v-model="propertyPage.pageSize"
            :options="pageOptions"
            @change="changePage"
          ></b-form-select>
        </b-form-group>
      </b-row>

      <!-- Info modal -->
      <b-modal
        :id="infoModal.id"
        :title="infoModal.title"
        ok-only
        @hide="resetInfoModal"
      >
        <pre>{{ infoModal.content }}</pre>
      </b-modal>
    </b-card>
  </div>
</template>

<script>
import { mapMutations, mapGetters } from "vuex";
import { statics } from "../../store/types";

export default {
  components: {},
  data() {
    return {
      selectedCategory: {},
      propertyCategories: [
        { name: "All properties", value: "" },
        { name: "Published", value: "is:published" },
        { name: "Not published", value: "is:not_published" },
        { name: "Ready for publish", value: "is:publish_ready" },
        { name: "Not ready for publish", value: "is:not_ready" }
      ],
      content: null,
      editorOption: {
        theme: "snow"
      },
      items: [],
      fields: [
        { key: "title", label: "Title", class: "text-center", sortable: true },
        { key: "price", label: "Price", sortable: true },
        {
          key: "propertyTypeName",
          label: "Property Type",
          class: "text-center",
          sortable: true
        },
        {
          key: "propertyStatusName",
          label: "Property Status",
          class: "text-center",
          sortable: true
        },
        {
          key: "propertyLabelName",
          label: "Property Label",
          class: "text-center",
          sortable: true
        },
        { key: "status", label: "Status" },
        { key: "actions", label: "Actions" }
      ],
      totalRows: 1,
      currentPage: 1,
      total: 0,
      propertyPage: {
        pageNumber: 0,
        pageSize: 5,
        filter: {}
      },
      totalBody: 0,
      pageOptions: [5, 10, 15],
      sortBy: null,
      sortDesc: false,
      sortDirection: "asc",
      filter: null,
      infoModal: {
        id: "info-modal",
        title: "",
        content: ""
      }
    };
  },
  created() {
    this.getList();
    this.completeRequestCategories();
  },
  computed: {
    ...mapGetters({
      user: statics.getters.user,
      propertyLabels: statics.getters.propertyLabel,
      propertyTypes: statics.getters.propertyType,
      propertyStatuses: statics.getters.propertyStatus
    }),
    sortOptions() {
      // Create an options list from our fields
      return this.fields
        .filter(f => f.sortable)
        .map(f => {
          return { text: f.label, value: f.key };
        });
    }
  },
  mounted() {
    // Set the initial number of items
    this.totalRows = this.items.length;
  },
  methods: {
    ...mapMutations({
      showLoading: statics.mutations.loading
    }),
    completeRequestCategories() {
      this.propertyStatuses.forEach(r => {
        this.propertyCategories.push({
          name: `status: ${r.name}`,
          value: `property_status:${r.id}`,
          class: ""
        });
      });

      this.propertyLabels.forEach(r => {
        this.propertyCategories.push({
          name: `label: ${r.name}`,
          value: `property_label:${r.id}`,
          class: ""
        });
      });

      this.propertyTypes.forEach(r => {
        this.propertyCategories.push({
          name: `type: ${r.name}`,
          value: `property_type:${r.id}`,
          class: ""
        });
      });
      this.selectedCategory = this.propertyCategories[0];
    },
    setFilter(item) {
      this.filter = item.value;
      this.getList();
    },
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
      this.$root.$emit("bv::show::modal", this.infoModal.id, button);
    },
    resetInfoModal() {
      this.infoModal.title = "";
      this.infoModal.content = "";
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
    getList() {
      this.propertyPage.pageNumber = this.currentPage - 1;
      this.propertyPage.filter = { searchText: this.filter };
      this.showLoading(true);
      this.$gate.property
        .page(this.propertyPage)
        .then(res => {
          this.items = res.body.items;
          this.total = res.body.total;
          this.totalBody = res.body.total;
        })
        .catch(err => this.$handleError(err))
        .finally(() => {
          this.showLoading(false);
        });
    },

    deleteItem(item, index) {
      this.$toasted.show("Are you sure you want to delete this item?", {
        action: [
          {
            text: "Yes",
            onClick: (e, toastObject) => {
              this.deleteItemFromDb(item.id);
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
    publishProperty(property) {
      this.$gate.property
        .publishProperty(property)
        .then(res => {
          this.$toasted.success("Property has been published");
          this.getList();
        })
        .catch(err => this.$handleError(err));
    },
    unPublishProperty(property) {
      this.$gate.property
        .unPublishProperty(property)
        .then(res => {
          this.$toasted.global.warn("Property has been unpublished");
          this.getList();
        })
        .catch(err => this.$handleError(err));
    },
    showDetails(item, index) {
      const route = `/estates/estate/${item.id}/summary`;
      this.$router.pushLang(route);
    },
    deleteItemFromDb(id) {
      this.$gate.property
        .delete(id)
        .then(res => {
          this.$toasted.global.deleted();
          this.getList();
        })
        .catch(err => this.$handleError(err));
    },
    clearSearch() {
      this.filter = null;
      this.selectedCategory = this.propertyCategories[0];
      this.getList();
    }
  },
  watch: {
    currentPage(val) {
      this.changePageNumber(val);
    },
    items() {
      this.$forceUpdate();
    }
  }
};
</script>

<style scoped>
.table {
  margin: 3rem 0 !important;
}

.perPage {
  width: 100px;
}

.action-item {
  margin-right: 2px;
  cursor: pointer !important;
}
</style>
