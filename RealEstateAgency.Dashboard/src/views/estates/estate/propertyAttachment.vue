<template>
  <div>
    <b-row>
      <div class="col-md-12 col-lg-12 col-sm-12">
        <b-card>
          <div slot="header" class="clearfix">
            <span>Property Attachment</span>
          </div>
          <div class="upload">
            <ul v-if="attachments.length">
              <li v-for="(file, index) in attachments" :key="file.id">
                <span>{{file.name}}</span> -
                <span>{{file.size }}</span> -
                <span
                  @click="deleteFileItem(file.id)"
                  class="text-danger mx-1 action-item pointer"
                  v-b-tooltip.hover
                  title="Delete this item?"
                >
                  <i class="fa fa-trash-o"></i>
                </span>

                <span v-if="file.success">success</span>
                <span v-else-if="file.active">active</span>
                <span v-else></span>
              </li>
            </ul>

            <div>
              <div class="text-center p-5" style="align-self:center">
                <h4>
                  Drop files anywhere to upload
                  <br />or
                </h4>
                <label for="file" class="btn btn-primary mx-1 pointer">
                  <i class="fa fa-file-o" aria-hidden="true"></i> Select Files
                </label>
                <label
                  v-if="!$refs.upload || !$refs.upload.active"
                  @click.prevent="uploadAttachments($event)"
                  for="file"
                  class="btn btn-success mx-1 pointer"
                >
                  <i class="fa fa-arrow-up animated infinite flipInX" aria-hidden="true"></i> Start Upload
                </label>
                <label
                  v-else
                  @click.prevent="$refs.upload.active = false"
                  class="btn btn-danger mx-1 pointer"
                >
                  <i class="fa fa-stop" aria-hidden="true"></i> Stop Upload
                </label>
                <h6>Valid file formats: (doc , docx , odt , txt , pdf , ppt , pptx , csv , xlsx , xls)</h6>
              </div>
            </div>

            <div class="example-btn text-center">
              <file-upload
                accept=".doc, .docx, .odt, .txt, .pdf, .ppt, .pptx, .csv, .xlsx, .xls"
                :multiple="true"
                :drop="true"
                :drop-directory="true"
                v-model="attachments"
                @input-filter="inputFilter"
                ref="upload"
              ></file-upload>
            </div>
          </div>
          <div class="mt-5">
            <b-table
              show-empty
              empty-html="<h6>There are no attachments to show!</h6>"
              hover
              :items="fileList"
              :fields="fields"
            >
              <template slot="name" slot-scope="row">{{row.item.fileCaption }}</template>
              <template slot="fileSize" slot-scope="row">{{row.item.fileSize | prettyBytes(1)}}</template>
              <template slot="actions" slot-scope="row">
                <span
                  @click="deleteItem(row.item.id)"
                  class="text-danger mx-1 action-item pointer"
                  v-b-tooltip.hover
                  title="Delete this item?"
                >
                  <i class="fa fa-trash-o"></i>
                </span>
              </template>
            </b-table>
          </div>
        </b-card>
      </div>
    </b-row>
  </div>
</template>

<script>
import FileUpload from "vue-upload-component";
import { mapGetters, mapMutations } from "vuex";
import { statics } from "../../../store/types";

export default {
  name: "MediaAttachment",
  components: {
    FileUpload
  },
  data() {
    return {
      attachments: [],
      fileList: [],
      fields: [
        { key: "fileCaption", label: "Name", sortable: true },
        { key: "fileSize", label: "Size", sortable: true },
        {
          key: "actions",
          label: "Actions"
        }
      ]
    };
  },
  created() {
    this.getAttachments();
  },
  methods: {
    ...mapMutations({
      showLoading: statics.mutations.loading
    }),
    uploadAttachments(event) {
      this.showLoading(true);

      this.$refs.upload.active = true;
      if (this.attachments.length > 0) {
        let formData = new FormData();

        formData.append("propertyId", this.$route.params.id);
        formData.append("fileCaption", "x");
        this.attachments.forEach(element => {
          formData.append("file[]", element.file);
        });

        this.$gate.propertyAttachment
          .create(formData)
          .then(resUpload => {
            this.$toasted.success("your file uploaded");
            this.attachments = [];
            this.getAttachments();
          })
          .catch(err => this.$logError(err))
          .finally(() => this.showLoading(false));
      } else {
        this.$toasted.global.warn("There isn't any files to upload");
        this.showLoading(false);
      }
    },
    inputFilter(newFile, oldFile, prevent) {
      if (newFile && !oldFile) {
        if (
          !/\.(xls|xlsx|csv|pptx|ppt|pdf|txt|odt|docx|doc)$/i.test(newFile.name)
        ) {
          this.$toasted.global.warn("File format is not valid");
          return prevent();
        }
      }
    },
    getAttachments() {
      this.showLoading(true);
      this.$gate.propertyAttachment
        .getAll(this.$route.params.id)
        .then(res => {
          this.fileList = res.body;
        })
        .catch(err => this.$handleError(err))
        .finally(() => this.showLoading(false));
    },
    deleteItem(id) {
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
      this.showLoading(true);
      this.$gate.propertyAttachment
        .delete(id)
        .then(res => {
          this.getAttachments();
          this.$toasted.global.deleted();
        })
        .catch(err => this.$handleError(err))
        .finally(() => this.showLoading(false));
    },
    deleteFileItem(id) {
      this.attachments = this.attachments.filter(function(value, index, arr) {
        return value.id != id;
      });
    }
  }
};
</script>

<style>
</style>
