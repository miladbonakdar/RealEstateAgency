<template>
  <div class="col-sm-12 col-lg-4 col-md-4" style="min-width: 100px;">
      <span @click="showDetails" class="text-primary px-1" v-b-tooltip.hover title="More info and edit!">
        <i class="fa fa-floppy-o" ></i>
      </span>
    <span @click="deleteItem" class="text-danger px-1" v-b-tooltip.hover title="Delete this item?">
        <i class="fa fa-trash-o"></i>
      </span>
  </div>
</template>
<script>
  export default {
    name: 'update-actions',
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
  .action-item-delete {
    font-size: large;
    padding-right: 6px;
    padding-left: 6px;
    margin-right: 2px;
    cursor: pointer;
  }
  .action-item-delete {
    font-size: large;
    padding-right: 6px;
    padding-left: 6px;
    margin-right: 2px;
    cursor: pointer;
  }
  .iconColor {
    color: rgb(35, 31, 32,90 )
  }
</style>
