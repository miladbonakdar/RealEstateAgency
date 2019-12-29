<template>
  <div class="col-sm-12 col-lg-4 col-md-4" style="min-width: 100px;">
      <span @click="showDetails" class="text-primary mx-1 action-item" v-b-tooltip.hover title="More info and edit!">
        <i class="fa fa-pencil-square-o" ></i>
      </span>
    <span @click="deleteItem" class="text-danger mx-1 action-item" v-b-tooltip.hover title="Delete this item?">
        <i class="fa fa-trash-o mx-1"></i>
      </span>
  </div>
</template>
<script>
  export default {
    name: 'crud-actions',
    methods: {
      deleteItem() {
        this.$emit("deleteItem");
      },
      showDetails() {
        this.$emit("showDetails");
      }
    }
  }
</script>

<style scoped>
  .action-item {
    margin-right: 2px;
    cursor: pointer !important;
  }
</style>
