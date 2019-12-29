<template>
  <div>
    <b-card header="Add important place in English language">
      <b-row>
        <div class="col-lg-6 col-md-6 col-sm-6">
          <b-form-group class="col-sm-12">
        <label for="name">ImportantPlace Name</label>
        <b-form-input
          v-model="propertyImportantPlace.name"
          type="text"
          placeholder="Enter ImportantPlace Name"
          @focus="clear"
        ></b-form-input>

        <div
          v-if="errmsg.required == true"
          class="error vuelidate-invalid-feedback"
        >Property ImportantPlace name is required.</div>
        <div
          v-if="errmsg.repetetive == true"
          class="error vuelidate-invalid-feedback"
        >Property ImportantPlace name is exists.</div>
      </b-form-group>

        </div>

        <div class="col-md-3 col-sm-6 col-lg-3">
          <b-form-group>
            <label for="name">Icon Image</label>
            <b-form-file
              v-model="file"
              placeholder="Choose a file..."
              drop-placeholder="Drop file here..."
            ></b-form-file>
            <div
          v-if="errmsg.file == true"
          class="error vuelidate-invalid-feedback"
        >Icon is required.</div>
          </b-form-group>

        </div>
        <div class="col-md-3 col-sm-6 col-lg-3 mt-4">
          <span class="feature-language">Language:</span>
          <b-badge variant="success px-3 py-2">
            <span class="language-size">en-US</span>
          </b-badge>
        </div>
      </b-row>

      <submit-group v-on:onCancel="onCancel" v-on:onSubmit="onSubmit" />
    </b-card>
    <b-card v-if="isEditMode()" header="Add translate for this important place in other languages">
      <b-row>
        <b-form-group class="col-sm-12 col-lg-6 col-md-6">
          <label for="name">Important Place Translate Name</label>
          <b-form-input
            type="text"
            placeholder="Enter ImportantPlace Name"
            v-model="propertyImportantPlaceTranslateItem.name"
          ></b-form-input>
          <div
          v-if="errmsg.translate == true"
          class="error vuelidate-invalid-feedback"
        >this important place with this language exists.</div>
        </b-form-group>
        <div class="col-md-6 col-sm-12 col-lg-6">
          <b-form-group>
            <label for="name">Language</label>
            <multi-select
    class="pointer"

             v-model="languageId" track-by="id" :options="otherLanguages" label="code"></multi-select>
          </b-form-group>
        </div>
      </b-row>
      <submit-group v-on:onCancel="onCancelTranslate" v-on:onSubmit="onSubmitTranslate" />
      <div class="col-sm-12 py-4">
        <label for="name">ImportantPlace list</label>
        <b-table
          hover
          show-empty
          empty-html='<h6>There are no important place to show!</h6>'
          :items="propertyImportantPlaceTranslate"
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
      updateTranslateMode: 0,
      otherLanguages: [],
      errmsg:{
        required: false,
        repetetive: false,
        file: false,
        translate: false
      },
      propertyImportantPlace: {
        name: ""
      },
      importantPlaceList: [],
      file: null,
      languageId: 0,
      lastName: "",
      propertyImportantPlaceTranslateItem: {
        name: "",
        importantPlaceTypeId: this.$route.params.id,
        languageId: 1
      },
      propertyImportantPlaceTranslate: [],
      fields: [
        { key: "name", label: "Name", sortable: true }, 
        { key: "actions", label: "Actions" },
        
        ]
    };
  },
  created() {
    this.getList()
    this.otherLanguages = this.languages.filter(
                i => i.id !== 1 || i.code !== "en-US"
            );
    if (this.isEditMode()) {
      this.getImportantPlaceList();
      this.getImportantPlaceTranslate();
    }
  },
  computed: {
    ...mapGetters({
      languages: statics.getters.language
    })
  },
  methods: {
    ...mapMutations({
      showLoading: statics.mutations.loading
    }),
    getList(){
      this.$gate.importantPlaceType.getAll().then(res => {
        this.importantPlaceList = res.body;

      }).catch(err => this.$handleError(err))
    },
    clear() {
      this.errmsg.required = false;
      this.errmsg.repetetive = false;
      this.errmsg.file = false;
      this.errmsg.translate =false;
    },
    getImportantPlaceList() {
      this.showLoading(true);
      this.$gate.importantPlaceType
        .get(this.$route.params.id)
        .then(res => {
          this.propertyImportantPlace = res.body;
          this.lastName = this.propertyImportantPlace.name;
        })
        .catch(err => this.$handleError(err))
        .finally(() => {
          this.showLoading(false);
        });
    },
    getImportantPlaceTranslate() {
      this.showLoading(true);

      this.$gate.importantPlaceTypeTranslate
        .importantPlaceTypeTranslates(this.$route.params.id)
        .then(res => {
          this.propertyImportantPlaceTranslate = res.body;
        })
        .catch(err => this.$handleError(err))
        .finally(() => {
          this.showLoading(false);
        });
    },
    onSubmit() {

      if (this.propertyImportantPlace.name == "") {
        this.errmsg.required = true;
      } else {
        let that =this;
        let importantantPlace = this.importantPlaceList.find(function(el){
          if(that.propertyImportantPlace.name == el.name){
            return el;
          }
        })

        if(importantantPlace){
          this.errmsg.repetetive = true;
        }
        else{
          this.isEditMode
          ? this.createImportantPlace()
          : this.updateImportantPlace();
        }
      }
    },
     onCancel(){
            this.$router.pushLang('/importantPlace/list')
        },
    onSubmitTranslate() {
      let that = this;

      if (this.propertyImportantPlaceTranslate.length > 0) {
        let languageExist = this.propertyImportantPlaceTranslate.find(function(
          element
        ) {
          if (
            element.name == that.propertyImportantPlaceTranslateItem.name &&
            element.languageId == that.languageId.id
          ) {
            return element;
          }
        });
        if (!languageExist) {
          this.updateTranslateMode == 0
            ? this.submitTranslateOnDb()
            : this.updateonDb();

        } else {
          this.errmsg.translate = true;
        }
      }
    },
    submitTranslateOnDb() {

      this.propertyImportantPlaceTranslateItem.importantPlaceTypeId = this.$route.params.id;
      this.propertyImportantPlaceTranslateItem.languageId = this.languageId.id;
      this.$gate.importantPlaceTypeTranslate
        .create(this.propertyImportantPlaceTranslateItem)
        .then(res => {

          this.getImportantPlaceTranslate();
          this.onCancelTranslate();
          this.$toasted.success("Property ImportantPlace Translate Added.");
        })
        .catch(err => this.$handleError(err));
    },
    onCancelTranslate() {
      this.propertyImportantPlaceTranslateItem.name = null;
    },
     onUpdateItem(item) {
      this.updateTranslateMode = 1;
      this.languageId = this.languages.filter(i => i.id === item.languageId)[0];
      this.propertyImportantPlaceTranslateItem.name = item.name;
      this.propertyImportantPlaceTranslateItem.propertyImportantPlaceId =
        item.propertyImportantPlaceId;
      this.propertyImportantPlaceTranslateItem.id = item.id;
      this.scrollToTop();
    },
    scrollToTop() {
      window.scrollTo(0, 50);
    },
    updateonDb() {
      const data = {
        languageId: this.languageId.id,
        name: this.propertyImportantPlaceTranslateItem.name,
        propertyImportantPlaceId: this.propertyImportantPlaceTranslateItem.propertyImportantPlaceId,
        id: this.propertyImportantPlaceTranslateItem.id
      };
      this.$gate.importantPlaceTypeTranslate
        .update(data)
        .then(res => {
          this.$toasted.success("property ImportantPlace translate updated.");
          this.onCancelTranslate();
          this.getImportantPlaceTranslate();
          this.updateTranslateMode = 0;
        })
        .catch(err => this.$handleError(err));
    },
    onCancelTranslate() {
      this.propertyImportantPlaceTranslateItem.name = null;
      this.propertyImportantPlaceTranslateItem.languageId = null;
      this.languageId = null;
      this.errmsg.translate = false;
      this.errmsg.required = false;
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
      this.$gate.importantPlaceTypeTranslate
        .delete(id)
        .then(res => {
          this.$toasted.success("property ImportantPlace translate deleted.");
          this.getImportantPlaceTranslate();
        })
        .catch(err => this.$handleError(err));
  },
    updateImportantPlace() {
      const data = {
        name: this.propertyImportantPlace.name,
        propertyImportantPlaceId: this.$route.params.id
      };
      this.$gate.importantPlaceType
        .update(data)
        .then(res => {
          this.$toasted.success("Property ImportantPlace updated.");
        })
        .catch(err => this.$handleError(err));
    },
    createImportantPlace() {
      if(this.file){


          let formData = new FormData();
          formData.append('file', this.file);
          formData.append('name', this.propertyImportantPlace.name);
          this.$gate.importantPlaceType
          .uploadAndCreate(formData)
          .then(res => {

            this.$toasted.success("Property ImportantPlace created.");
            this.$router.replaceLang(`/importantPlace/details/${res.body.id}`);
            this.propertyImportantPlaceTranslateItem.propertyImportantPlaceId =
              res.body.id;

          })
        .catch(err => this.$handleError(err));
        }
        else{
          this.errmsg.file = true;
        }
    },
    isEditMode() {
      return this.$route.params.id !== "new";
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
