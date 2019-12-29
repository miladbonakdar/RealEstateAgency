<template>
  <div class="animated fadeIn">

    <b-card>
      <b-row>
        <div class="col-lg-10"></div>
        <div class="col-lg-2">
          <button v-if="updateMode" type="button" class="close" aria-label="Close" v-on:click="makeNull">
            <span aria-hidden="true">&times;</span>
          </button>
        </div>
      </b-row>
      <b-row>

        <div class="col-md-6 col-sm-12 col-lg-4">
          <b-form-group>
            <label for="name">Floor Name *</label>
            <b-form-input
              @focus="clear"
              v-model="floorPlan.floorName" type="text"
              placeholder="Enter Floor Name"></b-form-input>
            <div
              v-if="errmsg == true"
              class="error vuelidate-invalid-feedback"
            >Floor Name is required.
            </div>
          </b-form-group>
        </div>
        <div class="col-md-6 col-sm-12 col-lg-4">
          <b-form-group>
            <label for="name">Floor Price(only digits)</label>
            <b-form-input  v-model="floorPlan.floorPrice" type="number"
                          placeholder="Enter Floor Price(only digits)"></b-form-input>

          </b-form-group>
        </div>
        <div class="col-md-6 col-sm-12 col-lg-4">
          <b-form-group>
            <label for="name">Price Postfix</label>
            <b-form-input type="text" v-model="floorPlan.pricePostfix"
                          placeholder="Enter Price Postfix"></b-form-input>
          </b-form-group>
        </div>
      </b-row>
      <b-row>

        <div class="col-md-6 col-sm-12 col-lg-4">
          <b-form-group>
            <label for="name">Floor Size(only digits)</label>
            <b-input-group>

              <b-form-input type="number" v-model="floorPlan.floorSize"
                            placeholder="Enter Floor Size(only digits)"></b-form-input>
              <b-input-group-append>
                <b-button :disabled="disable">m²</b-button>
              </b-input-group-append>
            </b-input-group>


          </b-form-group>
        </div>
        <div class="col-md-6 col-sm-12 col-lg-4">
          <b-form-group>
            <label for="name">Size Postfix</label>
            <b-form-input type="text" v-model="floorPlan.sizePostfix"
                          placeholder="EnterSize Postfix"></b-form-input>
          </b-form-group>
        </div>
        <div class="col-md-6 col-sm-12 col-lg-4">
          <b-form-group>
            <label for="name">Bedrooms</label>
            <b-form-input type="number" v-model="floorPlan.bedrooms"
                          placeholder="Enter Bedrooms"></b-form-input>

          </b-form-group>
        </div>
      </b-row>
      <b-row>

        <div class="col-md-6 col-sm-12 col-lg-4">
          <b-form-group>
            <label>Bathrooms</label>
            <b-form-input v-model="floorPlan.bathrooms" type="number"
                          placeholder="Enter Bathrooms"></b-form-input>

          </b-form-group>
        </div>
        <div class="col-md-6 col-sm-12 col-lg-4">
          <b-form-group>
            <label for="name">Floors' Image</label>
            <b-form-file
              v-model="file"
              placeholder="Choose a file..."
              drop-placeholder="Drop file here..."
            ></b-form-file>
          </b-form-group>

        </div>
        <div class="col-md-6 col-sm-12 col-lg-4">
          <b-form-group>
            <label for="name">Image Caption</label>
            <b-form-input
              v-model="floorPlan.imageCaption"
              placeholder="Enter Image Caption"
            ></b-form-input>
          </b-form-group>

        </div>
      </b-row>
      <b-row>
        <div class="col-md-12 col-sm-12 col-lg-12">
          <b-form-group>
            <label for="name">Plane Description</label>
            <quill-editor :options="editorOptions" v-model="floorPlan.planDescription">
            </quill-editor>
          </b-form-group>
        </div>
      </b-row>
      <b-row>
        <div class="col-md-12">
          <submit-group v-on:onCancel="makeNull" v-on:onSubmit="floorAction"/>
        </div>
      </b-row>
    </b-card>
    <b-card class="my-4" header="Floors List">
      <b-table
        hover
        small
        show-empty
        empty-html='<h6>There are no floors to show!</h6>'
        :items="items"
        :fields="fields"
        id="table-transition-floors"
        primary-key="a"
        :tbody-transition-props="transProps"
      >
        <template slot="actions" slot-scope="row">
          <crud-actions
            v-on:deleteItem="onDelete(row.item.id)"
            v-on:showDetails="reqForUpdate(row.item.id)"/>
        </template>
      </b-table>
    </b-card>
  </div>
</template>

<script>
    import {mapMutations} from 'vuex';
    import {statics} from '../../../store/types';

    export default {
        components: {},
        data: function () {
            return {
              editorOptions: {
          placeholder: "Enter description here ...",
          modules: {
            toolbar:  [
              ['bold', 'italic', 'underline', 'strike'],
              ['blockquote', 'code-block'],
              [{ 'header': 1 }, { 'header': 2 }],
              [{ 'list': 'ordered' }, { 'list': 'bullet' }],
              [{ 'script': 'sub' }, { 'script': 'super' }],
              [{ 'indent': '-1' }, { 'indent': '+1' }],
              [{ 'direction': 'rtl' }],
              [{ 'size': ['small', false, 'large', 'huge'] }],
              [{ 'header': [1, 2, 3, 4, 5, 6, false] }],
              [{ 'color': [] }],
              [{ 'align': [] }],
              ['clean'],
            ]
          }
        },
                disable: true,
                file: null,
                checked: false,
                updateMode: false,
                DetailNumber: 1,
                floorPlanLi: [],
                transProps: {
                    name: 'flip-list'
                },
                floorPlan: {
                    floorName: '',
                    floorPrice: '',
                    pricePostfix: '',
                    floorSize: '',
                    sizePostfix: '',
                    bedrooms: '',
                    bathrooms: '',
                    planDescription: '',
                    image: '',
                    imageCaption: '',
                    propertyId: this.$route.params.id
                },
                fields: [
                  {key: 'floorName', sortable: true},
                  {key: 'floorPrice', sortable: true}, {
                    key: 'floorSize',
                    sortable: true
                }, 'actions'],
                items: [
                    {
                        floorName: '',
                        floorPrice: '',
                        floorSize: ''
                    },
                ],
                errmsg: false
            }
        },
        created() {
            this.getFloorPlanList();
        },
        methods: {
            ...mapMutations({
                showLoading: statics.mutations.loading
            }),
            checkNumbers(){
              if(this.floorPlan.floorPrice <0 || this.floorPlan.floorSize <0 || this.floorPlan.bedrooms <0  || this.floorPlan.bathrooms <0 ){

                return false;
              }else{
                return true;
              }

            },
            onSubmit() {
                if(!this.checkNumbers()){
                this.$toasted.global.warn("Numbers can't be negative.")
                }

                else{
                  if (this.floorPlan.floorName == '') {
                    this.errmsg = true;
                } else {
                    this.$gate.propertyFloorPlan.create(this.floorPlan).then(res => {
                        this.$toasted.success('Floor plan added.');
                        this.uploadImage(res.body.id);
                        this.makeNull();
                        this.getFloorPlanList();
                    }).catch(err => this.$handleError(err))
                }
                }

            },
            clear() {
                this.errmsg = false;
            },
            uploadImage(propertyFloorId) {
                if (this.file) {
                    this.showLoading(true);
                    let formData = new FormData();
                    formData.append('file', this.file);
                    formData.append('propertyFloorPlanId', propertyFloorId);
                    this.$gate.propertyFloorPlan.setPropertyFloorPlanImage(formData).then(resUpload => {
                    }).catch(err => this.$logError(err)).finally(() => this.showLoading(false));
                }
            },
            getFloorPlanList() {
                this.showLoading(true);
                this.$gate.propertyFloorPlan.getPropertyFloorPlans(this.$route.params.id).then(res => {
                    this.items = res.body;
                }).catch(err => this.$handleError(err)
                ).finally(() => {
                    this.showLoading(false);
                });
            },
            onDelete(id) {
                this.$toasted.show('Are you sure you want to delete this item?', {
                    action: [
                        {
                            text: 'Yes',
                            onClick: (e, toastObject) => {
                                this.deleteItemFromDb(id);
                                toastObject.goAway(0);
                            },
                        },
                        {
                            text: 'No',
                            onClick: (e, toastObject) => {
                                toastObject.goAway(0);
                            },
                        }
                    ]
                });

            },
            reqForUpdate(id) {
                this.floorPlan = this.items.filter(i => i.id == id)[0];
                this.updateMode = true;
            },
            deleteItemFromDb(id) {
                this.$gate.propertyFloorPlan.delete(id).then(res => {
                    this.getFloorPlanList();
                    this.$toasted.global.deleted()
                }).catch(err => this.$handleError(err))
            },
            onUpdate() {

               if(!this.checkNumbers()){
                this.$toasted.global.warn("Numbers can't be negative.")
                }
              else{
                if (this.floorPlan.floorName == '') {
                    this.errmsg = true;
                } else {
                    this.$gate.propertyFloorPlan.update(this.floorPlan)
                    .then(res => {
                        this.$toasted.success('floor plan updated');
                        this.updateMode = false;
                        this.makeNull();
                    })
                    .catch(err => this.$handleError(err))
                }
              }

            },
            floorAction() {
                if (!this.updateMode) this.onSubmit();
                else this.onUpdate();
            },
            makeNull() {
                this.floorPlan = {
                    FloorName: '',
                    FloorPrice: '',
                    PricePostfix: '',
                    FloorSize: '',
                    SizePostfix: '',
                    Bedrooms: '',
                    Bathrooms: '',
                    PlanDescription: '',
                    Image: '',
                    ImageCaption: '',
                    propertyId: this.$route.params.id
                };
                this.file = null;
                this.updateMode = false;
            }
        }
    }
</script>

<style>
  table#table-transition-floors .flip-list-move {
    transition: transform 1s;
  }

  .vuelidate-invalid-feedback {
    width: 100%;
    margin-top: 0.25rem;
    font-size: 80%;
    color: #f86c6b;
  }
</style>
