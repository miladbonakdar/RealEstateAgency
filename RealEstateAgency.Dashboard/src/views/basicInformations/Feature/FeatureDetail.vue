<template>
  <div>
    <b-card header="Add feature in English language">
      <b-row>
        <b-form-group class="col-md-6 col-sm-12 col-lg-6">
          <label for="name">Feature Name</label>
          <b-form-input
            v-model="propertyFeatureItem.name"
            type="text"
            placeholder="Enter feature name"
            @focus="clear"
          ></b-form-input>
          <div
            v-if="errmsg.required == true"
            class="error vuelidate-invalid-feedback"
          >Property feature name is required.</div>
          <div
            v-if="errmsg.repetetive == true"
            class="error vuelidate-invalid-feedback"
          >Property feature name exists.</div>
        </b-form-group>
        <div class="col-md-6 col-sm-12 col-lg-6 mt-4">
          <span class="feature-language">Language: </span>
          <b-badge variant="success px-3 py-2">
            <span class="language-size">en-US</span>
          </b-badge>
        </div>
      </b-row>
      <submit-group v-on:onCancel="onCancel" v-on:onSubmit="onSubmit" />
    </b-card>
    <b-card v-if="isEditMode" header="Add translate for this feature in other languages">
      <b-row>
        <div class="col-md-6 col-sm-12 col-lg-6">
          <b-form-group class="col-sm-12">
            <label for="name">Feature Translate Name</label>
            <b-form-input
              type="text"
              placeholder="Enter feature name"
              v-model="propertyFeatureTranslateItem.name"
              @focus="clear"
            ></b-form-input>
            <div
              v-if="errmsg.translateRequired == true"
              class="error vuelidate-invalid-feedback"
            >All fields are required.</div>
          </b-form-group>
          <div
            v-if="errmsg.translateError == true"
            class="error vuelidate-invalid-feedback"
          >This translate for this feature is exist,please try another translate.</div>
        </div>
        <div class="col-md-6 col-sm-12 col-lg-6">
          <b-form-group>
            <label for="name">Feature Language</label>
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
      <submit-group v-on:onCancel="onCancelTranslate" v-on:onSubmit="onSubmitTranslate" />
      <div class="col-sm-12 py-4">
        <label for="name">Feature translate list</label>
        <b-table
          hover
          show-empty
          empty-html='<h6>There are no translate for this feature!</h6>'
          :items="propertyFeatureTranslate"
          :fields="fields"
        >
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
import { mapGetters, mapMutations } from "vuex";
import { statics } from "../../../store/types";
export default {
  data() {
    return {
      otherLanguages: [],
      updateTranslateMode: 0,
      errmsg: {
        repetetive: false,
        required: false,
        translateError: false,
        translateRequired: false
      },
      propertyFeatureItem: {
        name: ""
      },
      lastName: "",
      propertyFeatureTranslateItem: {
        name: "",
        propertyFeatureId: this.$route.params.id,
        languageId: 1,
        id: 0
      },
      languageId: null,
      propertyFeatures: [],
      propertyFeatureTranslate: [],
      fields: [
        { key: "name", label: "Name", sortable: true },
        { key: "language", label: "Language", sortable: true },
        { key: "actions", label: "Actions", sortable: true }
      ]
    };
  },
  created() {
    this.getList();
    this.otherLanguages = this.languages.filter(
      i => i.id !== 1 || i.code !== "en-US"
    );
    if (this.isEditMode) {
      this.getFeatureList();
      this.getFeatureTranslate();
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
    getFeatureList() {
      this.showLoading(true);
      this.$gate.propertyFeature
        .get(this.$route.params.id)
        .then(res => {
          this.propertyFeatureItem = res.body;
          this.lastName = this.propertyFeatureItem.name;
        })
        .catch(err => this.$handleError(err))
        .finally(() => {
          this.showLoading(false);
        });
    },
    getFeatureTranslate() {
      this.showLoading(true);

      this.$gate.propertyFeatureTranslate
        .getPropertyFeatureTranslates(this.$route.params.id)
        .then(res => {
          this.propertyFeatureTranslate = res.body;
          this.propertyFeatureTranslate.forEach(element => {
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
      this.$router.pushLang("/feature/list");
    },
    getList() {
      this.showLoading(true);

      this.$gate.propertyFeature
        .getAll()
        .then(res => {
          this.propertyFeatures = res.body;
        })
        .catch(err => this.$handleError(err))
        .finally(() => {
          this.showLoading(false);
        });
    },
    checkingSubmit() {
      if (this.propertyFeatureItem.name == "") {
        this.errmsg.required = true;
      } else {
        let that = this;

        let featureIsExist = this.propertyFeatures.find(function(element) {
          if (element.name == that.propertyFeatureItem.name) return element;
        });

        if (featureIsExist != null) {
          this.errmsg.repetetive = true;
        } else {
          this.isEditMode ? this.updateFeature() : this.createFeature();
        }
      }
    },

    onSubmitTranslate() {
      if (
        this.propertyFeatureTranslateItem.name == "" ||
        this.languageId == null
      ) {
        this.errmsg.translateRequired = true;
      } else {
        let that = this;
        let translateIsExist = this.propertyFeatureTranslate.find(function(el) {
          if (
            that.languageId.id == el.languageId &&
            that.propertyFeatureTranslateItem.name == el.name
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
        name: this.propertyFeatureTranslateItem.name,
        propertyFeatureId: this.$route.params.id
      };
      this.$gate.propertyFeatureTranslate
        .create(data)
        .then(res => {
          this.getFeatureTranslate();
          this.onCancelTranslate();
          this.$toasted.success("Property Feature Translate Added.");
        })
        .catch(err => this.$handleError(err));
    },
    onCancelTranslate() {
      this.propertyFeatureTranslateItem.name = null;
      this.propertyFeatureTranslateItem.languageId = null;
      this.languageId = null;
      this.errmsg.translateError = false;
      this.errmsg.translateRequired = false;
    },
    onUpdateItem(item) {
      this.updateTranslateMode = 1;
      this.languageId = this.languages.filter(i => i.id === item.languageId)[0];
      this.propertyFeatureTranslateItem.name = item.name;
      this.propertyFeatureTranslateItem.propertyFeatureId =
        item.propertyFeatureId;
      this.propertyFeatureTranslateItem.id = item.id;
      this.scrollToTop();
    },
    scrollToTop() {
      window.scrollTo(0, 50);
    },
    updateonDb() {
      const data = {
        languageId: this.languageId.id,
        name: this.propertyFeatureTranslateItem.name,
        propertyFeatureId: this.propertyFeatureTranslateItem.propertyFeatureId,
        id: this.propertyFeatureTranslateItem.id
      };
      this.$gate.propertyFeatureTranslate
        .update(data)
        .then(res => {
          this.$toasted.success("property feature translate updated.");
          this.onCancelTranslate();
          this.getFeatureTranslate();
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
      this.$gate.propertyFeatureTranslate
        .delete(id)
        .then(res => {
          this.$toasted.success("property feature translate deleted.");
          this.getFeatureTranslate();
        })
        .catch(err => this.$handleError(err));
    },
    updateFeature() {
      this.showLoading(true);
      const data = {
        name: this.propertyFeatureItem.name,
        id: this.$route.params.id
      };
      this.$gate.propertyFeature
        .update(data)
        .then(res => {
          this.$toasted.success("Property feature updated.");
        })
        .catch(err => this.$handleError(err))
        .finally(() => this.showLoading(false));
    },
    createFeature() {
      this.showLoading(true);

      this.$gate.propertyFeature
        .create(this.propertyFeatureItem)
        .then(res => {
          this.$toasted.success("Property feature created.");
          this.$router.replaceLang(`/feature/details/${res.body.id}`);
          this.propertyFeatureTranslateItem.propertyFeatureId = res.body.id;
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
