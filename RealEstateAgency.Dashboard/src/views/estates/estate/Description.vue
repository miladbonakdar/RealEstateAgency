<template>
  <div class="animated fadeIn">
    <!-- <b-form validated> -->

    <b-row>
      <div class="col-md-6 col-sm-12 col-lg-4">
        <b-form-group>
          <label for="name">Property Title</label>
          <b-form-input type="text" placeholder="Enter your Title" v-model="title"></b-form-input>
        </b-form-group>
      </div>
      <div class="col-md-6 col-sm-12 col-lg-4">
        <b-form-group>
          <label for="name">Property uniq id</label>
          <b-form-input v-model="propertyUniqId" type="text" placeholder="Enter your Unique Id"></b-form-input>
        </b-form-group>
      </div>
      <!-- <div class="col-md-6 col-sm-12 col-lg-4">
        <b-form-group>
          <label for="name">Property video link</label>
          <b-form-input v-model="videoUrl" type="text" id="name" placeholder="Enter your Video Url"></b-form-input>
        </b-form-group>
      </div>-->

      <div class="col-md-6 col-sm-12 col-lg-4">
        <b-form-group>
          <label for="name">Property type *</label>
          <multi-select
            class="pointer"
            @search-change="customTypeSearch($event)"
            :internal-search="false"
            :allow-empty="false"
            v-model="propertyType"
            track-by="id"
            label="name"
            @select="clear('type')"
            :options="types"
          ></multi-select>
          <div
            v-if="errmsg.propertyType && errorStatus"
            class="error vuelidate-invalid-feedback"
          >Property type field is required.</div>
        </b-form-group>
      </div>
      <div class="col-md-6 col-sm-12 col-lg-4">
        <b-form-group>
          <label for="name">Property status *</label>
          <multi-select
            class="pointer"
            :internal-search="false"
            :allow-empty="false"
            v-model="propertyStatus"
            track-by="id"
            label="name"
            :options="propertyStatuses"
            @select="clear('status')"
          ></multi-select>
          <div
            v-if="errmsg.propertyStatus && errorStatus"
            class="error vuelidate-invalid-feedback"
          >Property status field is required.</div>
        </b-form-group>
      </div>
      <div class="col-md-6 col-sm-12 col-lg-4">
        <b-form-group>
          <label for="name">Property label *</label>
          <multi-select
            class="pointer"
            :internal-search="false"
            :allow-empty="false"
            v-model="propertyLabel"
            track-by="id"
            label="name"
            :options="propertyLabels"
            @select="clear('label')"
          ></multi-select>
          <div
            v-if="errmsg.propertyLabel && errorStatus"
            class="error vuelidate-invalid-feedback"
          >Property label field is required.</div>
        </b-form-group>
      </div>
      <div class="col-md-12 col-sm-12 col-lg-12">
        <b-form-group>
          <label for="name">Description *</label>
          <quill-editor
            class="editor-example bubble"
            :options="editorOptions"
            v-model="description"
            @change="clear('description')"
          ></quill-editor>
          <div
            v-if="errmsg.description.required && errorStatus"
            class="error vuelidate-invalid-feedback"
          >Description field is required.</div>
          <div
            v-if="errmsg.description.minLength && errorStatus"
            class="error vuelidate-invalid-feedback"
          >Description must have at least 20 letters.</div>
        </b-form-group>
      </div>
    </b-row>
    <submit-group v-on:onCancel="onCancel" v-on:onSubmit="onSubmit" />
  </div>
</template>

<script>
import { mapGetters, mapMutations } from "vuex";
import { statics } from "../../../store/types";
import { validationMixin } from "vuelidate";
import { required, minLength } from "vuelidate/lib/validators";

export default {
  mixins: [validationMixin],
  components: {},
  data: function() {
    return {
      types: [],
      editorOptions: {
        placeholder: "Enter description here ...",
        modules: {
          toolbar: [
            ["bold", "italic", "underline", "strike"],
            ["blockquote", "code-block"],
            [{ header: 1 }, { header: 2 }],
            [{ list: "ordered" }, { list: "bullet" }],
            [{ script: "sub" }, { script: "super" }],
            [{ indent: "-1" }, { indent: "+1" }],
            [{ direction: "rtl" }],
            [{ size: ["small", false, "large", "huge"] }],
            [{ header: [1, 2, 3, 4, 5, 6, false] }],
            [{ color: [] }],
            [{ align: [] }],
            ["clean"]
          ]
        }
      },
      title: "",
      propertyUniqId: "",
      description: "",
      videoUrl: "",
      propertyTypeId: null,
      propertyStatusId: null,
      propertyLabelId: null,
      propertyType: {
        id: 0,
        name: ""
      },
      propertyStatus: {
        id: 0,
        name: ""
      },
      propertyLabel: {
        id: 0,
        name: ""
      },
      requestId: null,
      errmsg: {
        description: {
          required: false,
          minLength: false
        },
        propertyType: false,
        propertyStatus: false,
        propertyLabel: false
      },
      errorStatus: false
    };
  },
  created() {
    this.types = this.propertyTypes;
    if (this.isEditMode) this.setProperty();
    else if (this.$route.query && this.$route.query.requestId)
      this.requestId = this.$route.query.requestId;
  },
  methods: {
    ...mapMutations({
      showLoading: statics.mutations.loading
    }),
    customTypeSearch(e) {
      let typeList = [];
      this.propertyTypes.forEach(element => {
        if (element.name.toLowerCase().startsWith(e.toLowerCase())) {
          typeList.push(element);
        }
      });
      this.types = typeList;
    },
    onCancel() {
      this.$route.query.requestId
        ? this.$router.replaceLang("/agent/requests")
        : this.$router.replaceLang("/estates/list");
    },
    onSubmit() {
      debugger;
      this.checkValidations();
      if (!this.errorStatus) {
        this.propertyTypeId = this.propertyType.id;
        this.propertyStatusId = this.propertyStatus.id;
        this.propertyLabelId = this.propertyLabel.id;
        if (this.isEditMode) {
          this.updateProperty();
        } else {
          this.createProperty();
        }
      }
    },
    checkValidations() {
      debugger;
      if (this.description == "") {
        this.errmsg.description.required = true;
        this.errorStatus = true;
      }
      if (this.description.length <= 20) {
        this.errmsg.description.minLength = true;
        this.errorStatus = true;
      }
      if (this.propertyStatus.id === 0 && this.propertyStatus.name === "") {
        this.errmsg.propertyStatus = true;
        this.errorStatus = true;
      }
      if (this.propertyType.id === 0 && this.propertyType.name === "") {
        this.errmsg.propertyType = true;
        this.errorStatus = true;
      }
      if (this.propertyLabel.id === 0 && this.propertyLabel.name === "") {
        this.errmsg.propertyLabel = true;
        this.errorStatus = true;
      }
    },
    createProperty() {
      if (!this.errorStatus) this.setPropertyOnDb();
    },
    setPropertyOnDb() {
      this.title = !this.title ? this.createCustomTitle() : this.title;
      const data = {
        title: this.title,
        propertyUniqId: this.propertyUniqId,
        description: this.description,
        videoUrl: this.videoUrl,
        propertyTypeId: this.propertyTypeId,
        propertyStatusId: this.propertyStatusId,
        propertyLabelId: this.propertyLabelId,
        requestId: this.requestId
      };
      this.$gate.property
        .create(data)
        .then(res => {
          this.$route.params.id = res.body.id;
          this.$toasted.success("Property created.");
          const route = "/estates/estate/" + res.body.id + "/description";
          this.$router.replaceLang(route);
        })
        .catch(err => this.$handleError(err));
    },
    createCustomTitle() {
      let customTitle = this.propertyTypes.filter(
        i => i.id == this.propertyTypeId
      )[0].name;
      customTitle += `- ${
        this.propertyStatuses.filter(i => i.id == this.propertyStatusId)[0].name
      }`;
      customTitle += `- ${
        this.propertyLabels.filter(i => i.id == this.propertyLabelId)[0].name
      }`;
      return customTitle;
    },
    updateProperty() {
      this.title = !this.title ? this.createCustomTitle() : this.title;
      const data = {
        title: this.title,
        propertyUniqId: this.propertyUniqId,
        description: this.description,
        videoUrl: this.videoUrl,
        propertyTypeId: this.propertyTypeId,
        propertyStatusId: this.propertyStatusId,
        propertyLabelId: this.propertyLabelId,
        id: this.$route.params.id,
        requestId: this.requestId
      };

      this.showLoading(true);
      this.$gate.property
        .update(data)
        .then(res => {
          this.$toasted.success("Property updated.");
          console.info(res);
        })
        .catch(err => this.$handleError(err))
        .finally(() => this.showLoading(false));
    },
    setProperty() {
      this.showLoading(true);
      let that = this;
      this.$gate.property
        .get(this.$route.params.id)
        .then(res => {
          that.title = res.body.title;
          that.propertyUniqId = res.body.propertyUniqId;
          that.description = res.body.description;
          that.videoUrl = res.body.videoUrl;
          that.requestId = res.body.requestId;
          that.propertyType = that.propertyTypes.filter(
            i => i.id === res.body.propertyTypeId
          )[0];
          that.propertyStatus = that.propertyStatuses.filter(
            i => i.id === res.body.propertyStatusId
          )[0];
          that.propertyLabel = that.propertyLabels.filter(
            i => i.id === res.body.propertyLabelId
          )[0];
        })
        .catch(err => {
          if (err.status === 404) {
            that.$toasted.global.warn("cannot find the property");
            that.$router.replaceLang("/estates/estate/new/description");
          } else this.$handleError(err);
        })
        .finally(() => {
          this.showLoading(false);
        });
    },
    clear(name) {
      switch (name) {
        case "description":
          debugger;
          if (this.description !== "") {
            this.errmsg.description.required = false;
            this.errorStatus = false;
          }

          if (this.description.length >= 20) {
            this.errmsg.description.minLength = false;
            this.errorStatus = false;
          } else {
            this.errmsg.description.minLength = true;
            this.errorStatus = true;
          }

          break;
        case "type":
          this.errmsg.propertyType = false;
          this.errorStatus = false;
          break;
        case "status":
          this.errmsg.propertyStatus = false;
          this.errorStatus = false;
          break;
        case "label":
          this.errmsg.propertyLabel = false;
          this.errorStatus = false;
          break;
      }
    }
  },
  computed: {
    ...mapGetters({
      propertyTypes: statics.getters.propertyType,
      propertyStatuses: statics.getters.propertyStatus,
      propertyLabels: statics.getters.propertyLabel
    }),
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
</style>
