<template>
  <div class="animated fadeIn">
    <b-row>
      <div class="col-md-6 col-sm-12 col-lg-4">
        <b-form-group>
          <label for="name">Title</label>
          <b-form-input
            v-model="request.title"
            type="text"
            placeholder="Title"
          ></b-form-input>
        </b-form-group>
      </div>
      <div class="col-md-8 col-sm-12 col-lg-8">
        <b-form-group>
          <label for="name">Description</label>
          <b-form-input
            v-model="request.description"
            type="text"
            placeholder="Description"
          ></b-form-input>
        </b-form-group>
      </div>
      <div class="col-md-6 col-sm-12 col-lg-4">
        <b-form-group>
          <label for="name">Request type</label>
          <multi-select
            class="pointer"
            v-model="request.requestType"
            :options="requestTypes" track-by="id" label="name">
            <template slot="singleLabel" slot-scope="{ option }">{{ option.name }}</template>
          </multi-select>
        </b-form-group>
      </div>
    </b-row>
    <submit-group v-on:onCancel="onCancel" v-on:onSubmit="onSubmit"/>
  </div>
</template>

<script>
    import {mapGetters} from "vuex";
    import {statics} from "../../store/types";

    export default {
        data: function () {
            return {
                request: {}
            };
        },
        created() {
        },
        methods: {
            onCancel() {
                this.$router.replaceLang("/agent/requests");
            },
            onSubmit() {
                if(!this.request.title) return  alert("please fill the title section");
                if(!this.request.description) this.request.description = "description";
                if(!this.request.requestType) return  alert("please select a requestType");
                this.request.requestTypeId = this.request.requestType.id;
                this.$gate.request.create(this.request).then(res => {
                    this.request = {};
                }).catch(err => this.$handleError(err));
            },
        },
        computed: {
            ...mapGetters({
                requestTypes: statics.getters.requestType,
            })
        }
    };
</script>

<style>
  .vuelidate-invalid-feedback {
    width: 100%;
    margin-top: 0.25rem;
    font-size: 80%;
    color: #f86c6b;
  }
</style>
