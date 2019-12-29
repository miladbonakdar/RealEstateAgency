<template>
  <b-row>
    <div class="col-sm-12">
      <div class="py-2">
        <div class="text-center">
          <b-button @click="onCancel" type="reset" variant="secondary" style="color:#6e6c6c" class="mx-2">
            <i class="fa fa-ban"></i> {{$t("cancel")}}
          </b-button>

          <b-button @click="onSubmit" type="submit" variant="primary" class="mx-2">
            <i class="fa fa-dot-circle-o">
            </i> {{$t("save")}}
          </b-button>
        </div>

      </div>
    </div>

  </b-row>

</template>
<script>
  export default {
    name: 'submit-group',
    methods: {
      onCancel() {
        this.$emit("onCancel");
      },
      onSubmit() {
        this.$emit("onSubmit");
      }
    }
  }
</script>
