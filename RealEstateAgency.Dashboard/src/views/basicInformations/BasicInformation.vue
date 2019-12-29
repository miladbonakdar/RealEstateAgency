<template>
  <div class="animated fadeIn">
    <b-card
      header-tag="header">
      <div slot="header">
        <i class="fa fa-home"></i><strong> Basic Infromation Details </strong>
        <small>{{$route.name}}</small>
      </div>
      <div v-if="editMode">
        <b-nav tabs>
          <b-nav-item :key="child.path" v-for="child in children" :active="child.name === selectedItem">
            <b-link :to="{path : child.path}"> {{child.name}}</b-link>
            <i v-if="child.completed" class="fa fa-check text-success pl-1"></i>
          </b-nav-item>
        </b-nav>
      </div>
      <div class="mt-3">
        <router-view></router-view>
      </div>
<!--      <input type="file" id="file" ref="file" v-on:change="handleFileUpload()"/>-->
<!--      <button v-on:click="submitFile()">Submit</button>-->
    </b-card>

  </div>
</template>

<script>

  export default {
    components: {},
    data: function () {
      return {
        file: ''
      }
    },
    methods: {
      // handleFileUpload() {
      //   this.file = this.$refs.file.files[0];
      // },
      // submitFile() {
      //   let formData = new FormData();
      //   formData.append('file', this.file);
      //   formData.append('propertyFloorPlanId', "1");
      //   this.$gate.propertyFloorPlan.setPropertyFloorPlanImage(formData)
      //     .then(res => console.log(res)).catch(err => console.log(err))
      // },
    },
    computed: {
      children() {
        return [
          {completed: false, path: `/estates/estate/${this.$route.params.id}/description`, name: 'Description'},
          {completed: false, path: `/estates/estate/${this.$route.params.id}/location`, name: 'Location'},
          {completed: false, path: `/estates/estate/${this.$route.params.id}/price`, name: 'Price'},
          {completed: false, path: `/estates/estate/${this.$route.params.id}/features`, name: 'Features'},
          {completed: false, path: `/estates/estate/${this.$route.params.id}/details`, name: 'Details'},
          {completed: false, path: `/estates/estate/${this.$route.params.id}/media`, name: 'Media'},
          {completed: false, path: `/estates/estate/${this.$route.params.id}/floors`, name: 'Floors'},
          {completed: false, path: `/estates/estate/${this.$route.params.id}/contacts`, name: 'Contacts'},
          {
            completed: false,
            id: 9,
            path: `/estates/estate/${this.$route.params.id}/private_notes`,
            name: 'Private notes'
          }
        ]
      },
      editMode: function () {
        return this.$route.params.id !== 'new';
      },
      selectedItem: function () {
        let child = this.children.find(i => this.$route.name.indexOf(i.name.toLowerCase()) >= 0);
        return child.name;
      }
    },
    created() {
    }
  }
</script>

<style>

</style>
