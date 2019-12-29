<template>
  <div>
    <b-card header="Add type in English language">
      <b-row>
        <b-form-group class="col-md-6 col-sm-12 col-lg-6">
          <label for="name">Type Name</label>
          <b-form-input
            v-model="propertyTypeItem.name"
            type="text"
            placeholder="Enter Type Name"
            @focus="clear"
          ></b-form-input>
          <div
            v-if="errmsg.required == true"
            class="error vuelidate-invalid-feedback"
          >Property Type name is required.
          </div>
          <div
            v-if="errmsg.repetetive == true"
            class="error vuelidate-invalid-feedback"
          >Property Type name exists.
          </div>
        </b-form-group>
        
        <div class="col-md-6 col-sm-12 col-lg-6 mt-4">
          <span class="feature-language">Language: </span>
          <b-badge variant="success px-3 py-2">
            <span class="language-size">en-US</span>
          </b-badge>
        </div>

      </b-row>
      <submit-group v-on:onCancel="onCancel" v-on:onSubmit="onSubmit"/>
    </b-card>
    <b-card v-if="isEditMode" header="Add translate for this type in other languages">
      <b-row>
        <div class="col-md-6 col-sm-12 col-lg-6">
          <b-form-group class="col-sm-12">
            <label for="name">Type Translate Name</label>
            <b-form-input
              type="text"
              placeholder="Enter Type Name"
              v-model="propertyTypeTranslateItem.name"
              @focus="clear"

            ></b-form-input>
            <div
              v-if="errmsg.translateRequired == true"
              class="error vuelidate-invalid-feedback"
            >All fields are required.
            </div>
          </b-form-group>
          <div
            v-if="errmsg.translateError == true"
            class="error vuelidate-invalid-feedback"
          >This translate for this Type is exist,please try another translate.
          </div>
        </div>
        <div class="col-md-6 col-sm-12 col-lg-6">
          <b-form-group>
            <label for="name">Type Language</label>
            <multi-select
    class="pointer"

              placeholder="Select one"
              v-model="languageId"
              deselect-label="Can't remove this value"
              track-by="id"
              label="code"
              :options="otherLanguages"
              :searchable="false"
              :allow-empty="false"
              @select="clear"
            ></multi-select>
          </b-form-group>
        </div>
      </b-row>
      <submit-group v-on:onCancel="onCancelTranslate" v-on:onSubmit="onSubmitTranslate"/>
      <div class="col-sm-12 py-4">
        <label for="name">Type list</label>
        <b-table
        hover
        :items="propertyTypeTranslate"
        :fields="fields"
        show-empty
        empty-html='<h6>There are no type to show!</h6>'>
          <template slot="name" slot-scope="row">{{row.item.name}}</template>
          <template slot="language" slot-scope="row">{{row.item.language}}</template>
          <template slot="actions" slot-scope="row">
            <crud-actions
              v-on:deleteItem="onDeleteItem(row.item.id, row.index)"
              v-on:showDetails="onUpdateItem(row.item, row.index)"
            />
          </template>
        </b-table>
      </div>
    </b-card>
  </div>
</template>

<script>
    import {mapGetters, mapMutations} from "vuex";
    import {statics} from "../../../store/types";

    export default {
        data() {
            return {

                otherLanguages: [],
                updateTranslateMode: 0,
                errmsg: {
                    repetetive: false,
                    required: false,
                    translateError: false,
                    translateRequired: false,
                },
                propertyTypeItem: {
                    name: ""
                },
                propertyTypeTranslateItem: {
                    name: "",
                    propertyTypeId: this.$route.params.id,
                    languageId: 1,
                    id: 0
                },
                languageId: null,
                propertyTypes: [],
                propertyTypeTranslate: [],
                fields: [
                    {key: "name", label: "Name", sortable: true},
                    {key: "language", label: "Language", sortable: true},
                    {key: "actions", label: "Actions"}
                ]
            };
        },
        created() {
            this.getList();
            this.otherLanguages = this.languages.filter(
                i => i.id !== 1 || i.code !== "en-US"
            );
            if (this.isEditMode) {
                this.getTypeList();
                this.getTypeTranslate();
            }
        },
        computed: {
            ...mapGetters({
                languages: statics.getters.language
            }),
            isEditMode() {
                return this.$route.params.id !== "new";
            }
        },
        methods: {
            ...mapMutations({
                showLoading: statics.mutations.loading
            }),
            clear() {
                this.errmsg.repetetive = false;
                this.errmsg.required = false;
                this.errmsg.translateError = false;
                this.errmsg.translateRequired = false;
            },
            getTypeList() {
                this.showLoading(true);
                this.$gate.propertyType
                    .get(this.$route.params.id)
                    .then(res => {
                        this.propertyTypeItem = res.body;
                    })
                    .catch(err => this.$handleError(err))
                    .finally(() => {
                        this.showLoading(false);
                    });
            },
            getTypeTranslate() {
                this.showLoading(true);

                this.$gate.propertyTypeTranslate
                    .getPropertyTypeTranslates(this.$route.params.id)
                    .then(res => {
                        this.propertyTypeTranslate = res.body;
                        this.propertyTypeTranslate.forEach(element => {
                            element["language"] = this.languages.filter(
                                i => i.id === element.languageId
                            )[0].code;
                        });
                    })
                    .catch(err => this.$handleError(err))
                    .finally(() => {
                        this.showLoading(false);
                    });
            },
            onSubmit() {
                this.checkingSubmit();
            },
            onCancel() {
                this.$router.pushLang("/Type/list");
            },
            getList() {
                this.showLoading(true);

                this.$gate.propertyType
                    .getAll()
                    .then(res => {
                        this.propertyTypes = res.body;
                    })
                    .catch(err => this.$handleError(err))
                    .finally(() => {
                        this.showLoading(false);
                    });
            },
            checkingSubmit() {
                if (this.propertyTypeItem.name == "") {
                    this.errmsg.required = true;
                } else {
                    let that = this;

                    let TypeIsExist = this.propertyTypes.find(function (element) {
                        if (element.name == that.propertyTypeItem.name) return element;
                    });

                    if (TypeIsExist != null) {
                        this.errmsg.repetetive = true;
                    } else {
                        this.isEditMode ? this.updateType() : this.createType();
                    }
                }
            },

            onSubmitTranslate() {
                if (this.propertyTypeTranslateItem.name == '' || this.languageId == null) {
                    this.errmsg.translateRequired = true;
                } else {

                    let that = this;
                    let translateIsExist = this.propertyTypeTranslate.find(function (el) {
                        if (
                            that.languageId.id == el.languageId &&
                            that.propertyTypeTranslateItem.name == el.name
                        ) {
                            return el;
                        }
                    });
                    if (translateIsExist) {
                        this.errmsg.translateError = true;
                    } else {
                        this.updateTranslateMode == 0
                            ? this.submitTranslateOnDb()
                            : this.updateonDb();
                    }
                }
            },
            submitTranslateOnDb() {
                const data = {
                    languageId: this.languageId.id,
                    name: this.propertyTypeTranslateItem.name,
                    propertyTypeId: this.$route.params.id
                };
                this.$gate.propertyTypeTranslate
                    .create(data)
                    .then(res => {
                        this.getTypeTranslate();
                        this.onCancelTranslate();
                        this.$toasted.success("Property Type Translate Added.");
                    })
                    .catch(err => this.$handleError(err));
            },
            onCancelTranslate() {
                this.propertyTypeTranslateItem.name = null;
                this.propertyTypeTranslateItem.languageId = null;
                this.languageId = null;
                this.errmsg.translateError = false;
                this.errmsg.translateRequired = false;


            },
            onUpdateItem(item) {
                this.updateTranslateMode = 1;
                this.languageId = this.languages.filter(i => i.id === item.languageId)[0];
                this.propertyTypeTranslateItem.name = item.name;
                this.propertyTypeTranslateItem.propertyTypeId =
                    item.propertyTypeId;
                this.propertyTypeTranslateItem.id = item.id;
                this.scrollToTop();
            },
            scrollToTop() {
                window.scrollTo(0, 50);
            },
            updateonDb() {
                const data = {
                    languageId: this.languageId.id,
                    name: this.propertyTypeTranslateItem.name,
                    propertyTypeId: this.propertyTypeTranslateItem.propertyTypeId,
                    id: this.propertyTypeTranslateItem.id
                };
                this.$gate.propertyTypeTranslate
                    .update(data)
                    .then(res => {
                        this.$toasted.success("property Type translate updated.");
                        this.onCancelTranslate();
                        this.getTypeTranslate();
                        this.updateTranslateMode = 0;
                    })
                    .catch(err => this.$handleError(err));
            },
            onDeleteItem(id) {
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
                this.$gate.propertyTypeTranslate
                    .delete(id)
                    .then(res => {
                        this.$toasted.success("property Type translate deleted.");
                        this.getTypeTranslate();
                    })
                    .catch(err => this.$handleError(err));
            },
            updateType() {
                this.showLoading(true)
                const data = {
                    name: this.propertyTypeItem.name,
                    id: this.$route.params.id
                };
                this.$gate.propertyType
                    .update(data)
                    .then(res => {
                        this.$toasted.success("Property Type updated.");
                    })
                    .catch(err => this.$handleError(err))
                    .finally(() => this.showLoading(false));
            },
            createType() {
                this.showLoading(true)

                this.$gate.propertyType
                    .create(this.propertyTypeItem)
                    .then(res => {
                        this.$toasted.success("Property Type created.");
                        this.$router.replaceLang(`/Type/details/${res.body.id}`);
                        this.propertyTypeTranslateItem.propertyTypeId = res.body.id;
                    })
                    .catch(err => this.$handleError(err))
                    .finally(() => this.showLoading(false));
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
  .feature-language {
  font-size: 18px;
  font-weight: bold;
}
.language-size {
  font-size: 14px;
}
</style>
