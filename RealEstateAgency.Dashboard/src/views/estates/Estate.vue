<template>
  <div class="animated fadeIn">
    <b-card header-tag="header">
      <div slot="header" class="clearfix">
        <i class="fa fa-home"></i>
        <strong>Estate details</strong>&nbsp;
        <small>{{ $route.name }}</small>
        <span
          v-if="estate && estate.readyForPublish"
          @click="setNotReadyForPublish"
          class="float-right text-warning mx-1 pointer hvr-pulse-grow"
          v-b-tooltip.hover
          title="Not ready for publish?"
        >
          <i class="fa fa-times-rectangle-o ml-2"></i>
        </span>
        <span
          v-if="estate && !estate.readyForPublish"
          @click="setReadyForPublish"
          class="float-right text-primary mx-1 pointer hvr-pulse-grow"
          v-b-tooltip.hover
          title="Ready for publish?"
        >
          <i class="fa fa-check-square-o ml-2"></i>
        </span>
        <span
          @mouseenter="deleteHover = true"
          @mouseleave="deleteHover = false"
          v-if="estate"
          @click="deleteProperty"
          class="float-right text-danger mx-1 action-item pointer"
          v-b-tooltip.hover
          title="Delete this property?"
        >
          <i
            class="fa fa-trash-o ml-2 animated"
            :class="{ 'infinity tada': deleteHover }"
          ></i>
        </span>
        <strong v-if="estate" class="float-right">
          <i class="fa fa-bookmark-o mx-1"></i>
          {{ estate.title }}
        </strong>
      </div>
      <div v-if="editMode">
        <b-nav tabs>
          <b-nav-item
            :key="child.path"
            v-for="child in children"
            :active="child.name === selectedItem"
          >
            <b-link :to="{ path: child.path }">{{ child.name }}</b-link>
            <i v-if="child.completed" class="fa fa-check text-success pl-1"></i>
          </b-nav-item>
        </b-nav>
      </div>
      <div class="mt-3">
        <router-view></router-view>
      </div>
    </b-card>
  </div>
</template>

<script>
export default {
  components: {},
  data: function() {
    return {
      estate: null,
      deleteHover: true
    };
  },
  methods: {
    setReadyForPublish() {
      this.$gate.property
        .setReadyForPublish(this.estate)
        .then(res => {
          this.$toasted.success("This property is ready for publish now");
            this.getProperty();
        })
        .catch(err => this.$handleError(err));
    },
    setNotReadyForPublish() {
      this.$gate.property
        .setNotReadyForPublish(this.estate)
        .then(res => {
          this.$toasted.global.warn("Property is not ready for publish");
            this.getProperty();
        })
        .catch(err => this.$handleError(err));
    },
    deleteProperty() {
      this.$toasted.show("Are you sure you want to delete this property?", {
        action: [
          {
            text: "Yes",
            onClick: (e, toastObject) => {
              this.deleteItemFromDb(this.estate.id);
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
      this.$gate.property
        .delete(id)
        .then(res => {
          this.$toasted.success("Property has been deleted!");
          this.$router.replaceLang("/estates");
        })
        .catch(err => this.$handleError(err));
    },
    getProperty() {
      this.$gate.property
        .get(this.$route.params.id)
        .then(res => (this.estate = res.body))
        .catch(err => {
          if (err.status === 404) return this.$router.replaceLang("/404");
          this.$handleError(err);
        });
    }
  },
  computed: {
    children() {
      return [
        { completed: false, path: `summary`, name: "Summary" },
        { completed: false, path: `description`, name: "Description" },
        { completed: false, path: `location`, name: "Location" },
        { completed: false, path: `price`, name: "Price" },
        { completed: false, path: `features`, name: "Features" },
        { completed: false, path: `details`, name: "Details" },
        {
          completed: false,
          path: `additionaldetails`,
          name: "Additional Details"
        },
        { completed: false, path: `media`, name: "Media" },
        { completed: false, path: `floors`, name: "Floors" }
        // {completed: false, path: `contacts`, name: 'Contacts'},

        // {
        //   completed: false,
        //   id: 9,
        //   path: `/estates/estate/${this.$route.params.id}/private_notes`,
        //   name: 'Private notes'
        // },
      ];
    },
    editMode: function() {
      return this.$route.params.id !== "new";
    },
    selectedItem: function() {
      let children = this.children.filter(
        i => this.$route.name.indexOf(i.name.toLowerCase()) >= 0
      );
      const child = children[children.length - 1];
      return child.name;
    }
  },
  created() {
    if (this.editMode) {
      this.getProperty();
    }
  }
};
</script>

<style></style>
