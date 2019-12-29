<!--<template>-->
<!--  <div>-->
<!--    <b-input-group class="address-picker-container" v-b-modal.my-modal>-->
<!--      <b-input-group-prepend>-->
<!--        <b-input-group-text><i class="fa fa-location-arrow"></i></b-input-group-text>-->
<!--      </b-input-group-prepend>-->
<!--      <b-form-input disabled type="text" placeholder="address" :value="stringValue"></b-form-input>-->
<!--    </b-input-group>-->
<!--    <b-modal ref="my-modal" hide-footer title="Using Component Methods">-->
<!--      <div class="d-block text-center">-->
<!--        <h3>Hello From My Modal!</h3>-->
<!--      </div>-->
<!--    </b-modal>-->
<!--  </div>-->
<!--</template>-->
<!--<script>-->
<!--  export default {-->
<!--    props: ['value'],-->
<!--    name: 'address-picker',-->
<!--    methods: {-->
<!--      onCancel() {-->
<!--        this.$emit("onCancel");-->
<!--      },-->
<!--      onSubmit() {-->
<!--        this.$emit("onSubmit");-->
<!--      },-->
<!--      openAddressModal(){-->

<!--      }-->
<!--    },-->
<!--    computed: {-->
<!--      stringValue() {-->
<!--        if (!this.value) return null;-->
<!--        return "salam";-->
<!--      }-->
<!--    }-->
<!--  }-->
<!--</script>-->
<!--<style  scoped>-->
<!--  .address-picker-container{-->
<!--    cursor: pointer;-->
<!--  }-->
<!--</style>-->
