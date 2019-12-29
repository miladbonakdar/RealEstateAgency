<template>
  <div class="animated fadeIn">
    <b-row>
      <div class="col-md-6 col-sm-12">
        <b-form-group>
          <label for="name">Title</label>
          <b-form-input
            v-model.trim="$v.title.$model"
            type="text"
            @focus="clear('title')"
            placeholder="Enter your Title"
          ></b-form-input>
          <div
            v-if="errmsg.title && errorStatus"
            class="error vuelidate-invalid-feedback"
          >Title is required.
          </div>
        </b-form-group>
      </div>
      <div class="col-md-6 col-sm-12">
        <b-form-group>
          <label for="name">Value</label>
          <b-form-input
            v-model.trim="$v.value.$model"
            @focus="clear('value')"
            type="text"
            placeholder="Your Value"
          ></b-form-input>
          <div
            v-if="errmsg.value && errorStatus"
            class="error vuelidate-invalid-feedback"
          >Value field is required.
          </div>
        </b-form-group>
      </div>
    </b-row>
    <submit-group v-on:onCancel="onCancel" v-on:onSubmit="createAdditionalDetails"/>

    <b-card header="Additional Details List" class="my-4">
      <b-row>
        <div class="col-md-12 col-sm-12 col-lg-12">
          <table style="width: 100%">
            <tr>
              <th>Title</th>
              <th>Value</th>
              <th>Actions</th>
            </tr>
            <tr v-for="item in addtionalDetailList" :key="item.id">
              <td>
                <b-form-group>
                  <b-form-input v-model="item.title" type="text" placeholder="Your Title"></b-form-input>
                </b-form-group>
              </td>
              <td>
                <b-form-group>
                  <b-form-input v-model="item.value" type="text" placeholder="Your Value"></b-form-input>
                </b-form-group>
              </td>
              <td>
                <update-actions
                  v-on:deleteItem="deleteAdditionalDetail(item.id)"
                  v-on:showDetails="updateAdditionalDetail(item)"
                />
              </td>
            </tr>
          </table>
        </div>
      </b-row>
    </b-card>
  </div>
</template>

<script>
    import {validationMixin} from "vuelidate";
    import {required} from "vuelidate/lib/validators";
    import {mapMutations} from "vuex";
    import {statics} from "../../../store/types";

    export default {
        mixins: [validationMixin],
        data: function () {
            return {
                title: "",
                value: "",
                addtionalDetailList: [],
                errmsg: {
                    title: false,
                    value: false
                },
                errorStatus: false
            };
        },
        validations: {
            title: {
                required,
            },
            value: {
                required,
            }
        },
        created() {
            this.getAddtionalListDetails();
        },

        methods: {
            ...mapMutations({
                showLoading: statics.mutations.loading
            }),
            createAdditionalDetails() {
                this.checkValidations();

                if (!this.errorStatus) {
                    this.showLoading(true);
                    const data = {
                        title: this.title,
                        value: this.value,
                        propertyId: this.$route.params.id
                    };
                    this.$gate.propertyAdditionalDetail.create(data).then(res => {
                        this.$toasted.success("additional details added");
                        this.title = "";
                        this.value = "";
                        this.errmsg.title = false;
                        this.errmsg.value = false;
                    }).finally(() => {
                            this.showLoading(false);
                            this.getAddtionalListDetails();
                        }
                    );
                }
            },
            onCancel() {
                this.$router.pushLang("/estates/list");
            },
            checkValidations() {
              debugger;
                if (!this.$v.value.required) {
                    this.errmsg.value = true;
                    this.errorStatus = true;
                }
                if (!this.$v.title.required) {
                    this.errmsg.title = true;
                    this.errorStatus = true;
                }
            },
            clear(name) {
                name == "value"
                    ? (this.errmsg.value = false)
                    : (this.errmsg.title = false);
                this.errorStatus = false;
            },
            getAddtionalListDetails() {
                this.showLoading(true);
                this.$gate.propertyAdditionalDetail
                    .getPropertyAdditionalDetails(this.$route.params.id)
                    .then(res => {
                        this.addtionalDetailList = res.body;
                    })
                    .finally(() => this.showLoading(false));
            },
            updateAdditionalDetail(item) {
                this.$toasted.show("Are you sure you want to update this item?", {
                    action: [
                        {
                            text: "Yes",
                            onClick: (e, toastObject) => {
                                this.updateOnDb(item);
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
            updateOnDb(item) {
                this.showLoading(true);
                this.$gate.propertyAdditionalDetail
                    .update(item)
                    .then(res => this.$toasted.success("additional detail updated."))
                    .catch(err => this.$handleError(err))
                    .finally(() => this.showLoading(false));
            },
            deleteAdditionalDetail(id) {
                this.$toasted.show("Are you sure you want to delete this item?", {
                    action: [
                        {
                            text: "Yes",
                            onClick: (e, toastObject) => {
                                this.deleteItemFromDb(id);
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
                this.$gate.propertyAdditionalDetail
                    .delete(id)
                    .then(res => {
                        this.$toasted.global.deleted();
                        this.getAddtionalListDetails();
                    })
                    .catch(err => this.$handleError(err));
            }
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

