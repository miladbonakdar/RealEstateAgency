<template>
  <div>
    <b-card header="Upload Images">
      <b-row class="px-1">
        <div class="col-md-6 col-sm-12 col-lg-4">
          <label for="name">Image File <span
            style="font-size:12px">(First image you upload is cover image)</span></label>

          <b-form-file
            v-model="file"
            placeholder="Choose a file..."
            accept=".jpeg , .jpg , .png , .tiff , .tif"
            drop-placeholder="Drop file here..."
            @change="setImage"
          ></b-form-file>
          <small
            class="text-primary vuelidate-invalid-feedback"
          >Valid formats: ( jpeg , jpg , png , tiff , tif )
          </small>
        </div>
         <div class="col-md-6 col-sm-12 col-lg-2">
            <b-form-checkbox
              v-if="imgSrc"
              class="checkBox py-1"
              v-model="is360View"
            >Is 360 view ?</b-form-checkbox>
          </div>
        <div class="col-md-6 col-sm-12 col-lg-4">
          <b-form-group>
            <label for="name">Image Caption</label>
            <b-form-input
              v-model="imageCaption"
              type="text"
              placeholder="Enter Image Caption"
            ></b-form-input>
          </b-form-group>
        </div>

        <div class="col-md-6 col-sm-12 col-lg-2">
          <b-form-group>
            <label for="name">Image priority</label>
            <b-form-input
              v-model="priority"
              type="number"
              placeholder="Enter Image Caption"
            ></b-form-input>
            <small
              class="text-primary vuelidate-invalid-feedback"
            >will sort from lowest to highest
            </small>
          </b-form-group>
        </div>


        <div class="col-12 mx-auto align-content-center">
          <div
            v-if="imgSrc"
            style="width: 400px; height:300px; border: 1px solid gray; display: inline-block;"
          >
          <img style="width: 400px; height: 300px" v-if="is360View" :src="imgSrc"/>
            <vue-cropper v-else
              ref="cropper"
              :guides="true"
              :view-mode="2"
              drag-mode="crop"
              :auto-crop-area="0.5"
              :min-container-width="250"
              :min-container-height="250"
              :background="true"
              :rotatable="true"
              :src="imgSrc"
              :aspectRatio="16/9"
              :initialAspectRatio="16/9"
              :minCropBoxWidth="100"
              :minCropBoxHeight="100"
              alt="Source Image"
              :img-style="{ 'width': '400px', 'height': '300px' }"
            ></vue-cropper>
          </div>
          <img
            v-if="cropImg!=='' && imgSrc!==''"
            :src="cropImg"
            style="width: 200px; height: 150px; border: 1px solid gray"
            alt="Cropped Image"
          />
          <br/>
          <br/>
          <div class="clearfix py-2">
            <b-button
              @click="cropImages"
              v-if="imgSrc!=='' && !is360View"
              variant="primary"
              class="float-left uploadButtons"
            >
              <i v-if="cropImg == ''" class="fa fa-crop animated infinite heartBeat slow"></i >
              <i v-else class="fa fa-crop"></i> Crop
            </b-button>
            <b-button
              @click="rotate"
              v-if="imgSrc!=='' && !is360View"
              variant="primary"
              class="float-left uploadButtons"
            >
              <i class="fa fa-rotate-right"></i> Rotate
            </b-button>
            <b-button
              @click="uploadImage($event)"
              v-if="cropImg!=='' || is360View"
              variant="primary"
              class="float-left uploadButtons"
            >
            <i class="fa fa-upload animated infinite heartBeat slow"></i> Upload
            </b-button>
          </div>
        </div>
      </b-row>
      <b-row>
        <div class="col-lg-3 col-md-4 col-sm-12" v-for="image in images" :key="image.id">
          <b-card
            :img-src="image.imageUrl"
            :img-alt="image.imageCaption"
            img-top>
            <b-card-text>{{image.imageCaption}}</b-card-text>
            <div slot="footer">
              <b-button block class="flex-nowrap" variant="danger" @click="deleteImage(image.id)">Delete</b-button>
            </div>
          </b-card>
        </div>
      </b-row>
    </b-card>

    <MediaAttachment v-if="imagesLoaded"/>
  </div>
</template>

<script>
    import {mapMutations} from "vuex";
    import {statics} from "../../../store/types";

    import VueCropper from "vue-cropperjs";
    import "cropperjs/dist/cropper.css";
    import MediaAttachment from "./propertyAttachment";

    export default {
        components: {
            VueCropper,
            MediaAttachment
        },
        data() {
            return {
                imgSrc: "",
                cropImg: "",
                imageCaption: "",
                images: [],
                file: null,
                attachments: [],
                imagesLoaded: false,
                is360View: false,
                priority : 0
            };
        },
        created() {
            this.showLoading(true);
            this.getImageList();
        },
        methods: {
            ...mapMutations({
                showLoading: statics.mutations.loading
            }),
            setImage(e) {
                const file = e.target.files[0];

                if (!file.type.includes("image/")) {
                    this.$toasted.global.warn("Please select an image file");
                    return;
                }
                if (typeof FileReader === "function") {
                    const reader = new FileReader();
                    reader.onload = event => {
                        this.imgSrc = event.target.result;
                        // rebuild cropperjs with the updated source
                        this.$refs.cropper.replace(event.target.result);
                    };
                    reader.readAsDataURL(file);
                } else {
                    alert("Sorry, FileReader API not supported");
                }
            },
            cropImages() {
                // get image data for post processing, e.g. upload or setting image src
                this.cropImg = this.$refs.cropper.getCroppedCanvas().toDataURL();
            },
            rotate() {
                // guess what this does :)
                this.$refs.cropper.rotate(90);
            },
            uploadImage(event) {
                if (this.cropImg != "") {
                    let arr = this.cropImg.split(","),
                        mime = arr[0].match(/:(.*?);/)[1],
                        bstr = atob(arr[1]),
                        n = bstr.length,
                        u8arr = new Uint8Array(n);
                    while (n--) {
                        u8arr[n] = bstr.charCodeAt(n);
                    }
                    this.file = new File([u8arr], "temp.jpg", {type: mime});
                } else {
                    let arr = this.imgSrc.split(","),
                        mime = arr[0].match(/:(.*?);/)[1],
                        bstr = atob(arr[1]),
                        n = bstr.length,
                        u8arr = new Uint8Array(n);
                    while (n--) {
                        u8arr[n] = bstr.charCodeAt(n);
                    }
                    this.file = new File([u8arr], "temp.jpg", {type: mime});
                }

                if (this.file) {
                    let id = this.$route.params.id;
                    let formData = new FormData();
                    formData.append("files", this.file);
                    formData.append("propertyId", id);
                    formData.append("imageCaption", this.imageCaption);
                    formData.append("is360View", this.is360View);
                    formData.append("priority", this.priority);
                    this.showLoading(true);

                    this.$gate.propertyImage
                        .create(formData)
                        .then(resUpload => {
                            event.target.files = [];
                            this.$toasted.success("your photo uploaded");
                            this.is360View = false;
                            this.imgSrc = "";
                            this.cropImg = "";
                            this.imageCaption = "";
                            this.file = null;
                        })
                        .catch(err => this.$logError(err))
                        .finally(() => {
                            this.showLoading(false);
                            this.getImageList();
                        });
                }
            },
            deleteImage(id) {
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
                this.$gate.propertyImage
                    .delete(id)
                    .then(res => {
                        this.$toasted.success("selected photo deleted");
                    })
                    .catch(err => this.$handleError(err)).finally(() => {
                    this.showLoading(false);
                    this.getImageList();
                });
            },
            getImageList() {
                this.showLoading(true);
                this.$gate.propertyImage
                    .getAll(1, this.$route.params.id)
                    .then(res => {
                        this.images = res.body;
                    })
                    .catch(err => this.$logError(err))
                    .finally(() => {
                        this.showLoading(false);
                        this.imagesLoaded = true;
                    });
            },
            // inputFilter(newFile, oldFile) {
            //   if (newFile && !oldFile) {
            //     if (!/\.(xls|xlsx|csv|pptx|ppt|pdf|txt|odt|docx|doc)$/i.test(newFile.name)) {
            //       this.$toasted.error("Your file is not valid");
            //     }
            //   }
            // }
        }
    };
</script>

<style>
  .uploadButtons {
    margin: 5px;
  }

  .example-drag label.btn {
    margin-bottom: 0;
    margin-right: 1rem;
  }

  .example-drag .drop-active {
    top: 0;
    bottom: 0;
    right: 0;
    left: 0;
    position: fixed;
    z-index: 9999;
    opacity: 0.6;
    text-align: center;
    background: #000;
  }

  .example-drag .drop-active h3 {
    margin: -0.5em 0 0;
    position: absolute;
    top: 50%;
    left: 0;
    right: 0;
    -webkit-transform: translateY(-50%);
    -ms-transform: translateY(-50%);
    transform: translateY(-50%);
    font-size: 40px;
    color: #fff;
    padding: 0;
  }
</style>
