<template>
  <div>
    <b-card header="Add label in English language">
      <b-row>
        <b-form-group class="col-md-6 col-sm-12 col-lg-6 mt-4">
          <label for="name">Label Name</label>
          <b-form-input
            v-model="propertyLabelItem.name"
            type="text"
            placeholder="Enter Label Name"
            @focus="clear"
          ></b-form-input>
          <div
            v-if="errmsg.required == true"
            class="error vuelidate-invalid-feedback"
          >Property Label name is required.</div>
          <div
            v-if="errmsg.repetetive == true"
            class="error vuelidate-invalid-feedback"
          >Property Label name exists.</div>
        </b-form-group>
        <div class="col-md-6 col-sm-12 col-lg-6 mt-5">
          <span class="feature-language">Language: </span>
          <b-badge variant="success px-3 py-2">
            <span class="language-size">en-US</span>
          </b-badge>
        </div>
      </b-row>
      <submit-group v-on:onCancel="onCancel" v-on:onSubmit="onSubmit" />
    </b-card>
    <b-card v-if="isEditMode" header="Add translate for this label in other languages">
      <b-row>
        <div class="col-md-6 col-sm-12 col-lg-6">
          <b-form-group class="col-sm-12">
            <label for="name">Label Translate Name</label>
            <b-form-input
              type="text"
              placeholder="Enter Label Name"
              v-model="propertyLabelTranslateItem.name"
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
          >This translate for this Label is exist,please try another translate.</div>
        </div>
        <div class="col-md-6 col-sm-12 col-lg-6">
          <b-form-group>
            <label for="name">Label Language</label>
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
        <label for="name">Label list</label>
        <b-table
          hover
          :items="propertyLabelTranslate"
          :fields="fields"
          show-empty
          empty-html='<h6>There are no label to show!</h6>'>
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
        translateRequired: false,
      },
      propertyLabelItem: {
        name: ""
      },
      lastName: "",
      propertyLabelTranslateItem: {
        name: "",
        propertyLabelId: this.$route.params.id,
        languageId: 1,
        id: 0
      },
      languageId: null,
      propertyLabels: [],
      propertyLabelTranslate: [],
      fields: [
        { key: "name", label: "Name", sortable: true },
        { key: "language", label: "Language", sortable: true },
        { key: "actions", label: "Actions" }
      ]
    };
  },
  created() {
    this.getList();
    this.otherLanguages = this.languages.filter(
        i => i.id !== 1 || i.code !== "en-US"
      );
    if (this.isEditMode) {
      this.getLabelList();
      this.getLabelTranslate();
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
    getLabelList() {
      this.showLoading(true);
      this.$gate.propertyLabel
        .get(this.$route.params.id)
        .then(res => {
          this.propertyLabelItem = res.body;
          this.lastName = this.propertyLabelItem.name;
        })
        .catch(err => this.$handleError(err))
        .finally(() => {
          this.showLoading(false);
        });
    },
    getLabelTranslate() {
      this.showLoading(true);

      this.$gate.propertyLabelTranslate
        .getPropertyLabelTranslates(this.$route.params.id)
        .then(res => {
          this.propertyLabelTranslate = res.body;
          this.propertyLabelTranslate.forEach(element => {
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
      this.$router.pushLang("/Label/list");
    },
    getList() {
      this.showLoading(true);

      this.$gate.propertyLabel
        .getAll()
        .then(res => {
          this.propertyLabels = res.body;
        })
        .catch(err => this.$handleError(err))
        .finally(() => {
          this.showLoading(false);
        });
    },
    checkingSubmit() {
      if (this.propertyLabelItem.name == "") {
        this.errmsg.required = true;
      } else {
        let that = this;

        let LabelIsExist = this.propertyLabels.find(function(element) {
          if (element.name == that.propertyLabelItem.name) return element;
        });

        if (LabelIsExist != null) {
          this.errmsg.repetetive = true;
        } else {
          this.isEditMode ? this.updateLabel() : this.createLabel();
        }
      }
    },

    onSubmitTranslate() {
      if(this.propertyLabelTranslateItem.name == '' || this.languageId == null){
        this.errmsg.translateRequired = true;
      }

      else{

      let that = this;
      let translateIsExist = this.propertyLabelTranslate.find(function(el) {
        if (
          that.languageId.id == el.languageId &&
          that.propertyLabelTranslateItem.name == el.name
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
        name: this.propertyLabelTranslateItem.name,
        propertyLabelId: this.$route.params.id
      };
      this.$gate.propertyLabelTranslate
        .create(data)
        .then(res => {
          this.getLabelTranslate();
          this.onCancelTranslate();
          this.$toasted.success("Property Label Translate Added.");
        })
        .catch(err => this.$handleError(err));
    },
    onCancelTranslate() {
      this.propertyLabelTranslateItem.name = null;
      this.propertyLabelTranslateItem.languageId = null;
      this.languageId = null;
      this.errmsg.translateError = false;
      this.errmsg.translateRequired = false;


    },
    onUpdateItem(item) {
      this.updateTranslateMode = 1;
      this.languageId = this.languages.filter(i => i.id === item.languageId)[0];
      this.propertyLabelTranslateItem.name = item.name;
      this.propertyLabelTranslateItem.propertyLabelId =
        item.propertyLabelId;
      this.propertyLabelTranslateItem.id = item.id;
      this.scrollToTop();
    },
    scrollToTop() {
      window.scrollTo(0, 50);
    },
    updateonDb() {
      const data = {
        languageId: this.languageId.id,
        name: this.propertyLabelTranslateItem.name,
        propertyLabelId: this.propertyLabelTranslateItem.propertyLabelId,
        id: this.propertyLabelTranslateItem.id
      };
      this.$gate.propertyLabelTranslate
        .update(data)
        .then(res => {
          this.$toasted.success("property Label translate updated.");
          this.onCancelTranslate();
          this.getLabelTranslate();
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
      this.$gate.propertyLabelTranslate
        .delete(id)
        .then(res => {
          this.$toasted.success("property Label translate deleted.");
          this.getLabelTranslate();
        })
        .catch(err => this.$handleError(err));
    },
    updateLabel() {

      this.showLoading(true)
      const data = {
        name: this.propertyLabelItem.name,
        id: this.$route.params.id,
      };
      this.$gate.propertyLabel
        .update(data)
        .then(res => {
          this.$toasted.success("Property Label updated.");
        })
        .catch(err => this.$handleError(err))
        .finally(()=> this.showLoading(false));
    },
    createLabel() {
      this.showLoading(true)

      this.$gate.propertyLabel
        .create(this.propertyLabelItem)
        .then(res => {
          this.$toasted.success("Property Label created.");
          this.$router.replaceLang(`/Label/details/${res.body.id}`);
          this.propertyLabelTranslateItem.propertyLabelId = res.body.id;
        })
        .catch(err => this.$handleError(err))
        .finally(()=> this.showLoading(false));
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
