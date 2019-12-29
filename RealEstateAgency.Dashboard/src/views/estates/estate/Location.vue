<template>
  <div class="animated fadeIn">
    <b-row>
      <div class="col-md-6 col-sm-12 col-lg-4">
        <b-form-group>
          <label for="name">Address 1 *</label>
          <b-form-input
            @focus="clear"
            v-model.trim="$v.addressLine1.$model"
            type="text"
            placeholder="Enter your Address 1"
          ></b-form-input>
          <div
            v-if="($v.addressLine1.$anyError && !$v.addressLine1.required) || errmsg.addressLine1 ==true"
            class="error vuelidate-invalid-feedback"
          >Address Line 1 field is required.
          </div>
        </b-form-group>
      </div>
      <div class="col-md-6 col-sm-12 col-lg-4">
        <b-form-group>
          <label for="name">Address 2</label>
          <b-form-input
            v-model="location.addressLine2"
            type="text"
            placeholder="Enter your Address 2"
          ></b-form-input>
        </b-form-group>
      </div>
      <div class="col-md-6 col-sm-12 col-lg-4">
        <b-form-group>
          <label for="name">Country</label>
          <multi-select
    class="pointer"

           :internal-search="false" @search-change="customCountrySearch($event)" v-model="countryId"
                        :options="countries" track-by="id" label="name"></multi-select>
        </b-form-group>
      </div>
      <div class="col-md-6 col-sm-12 col-lg-4">
        <b-form-group>
          <label for="name">Province / State</label>
          <multi-select
    class="pointer"
          
           :internal-search="false" @search-change="customProvinceSearch($event)" v-model="regionId"
                        track-by="id" :options="regions" label="name"></multi-select>
        </b-form-group>
      </div>
      <div class="col-md-6 col-sm-12 col-lg-4">
        <b-form-group>
          <label for="name">City / Town</label>
          <multi-select
    class="pointer"
          
           :internal-search="false" @search-change="customCitySearch($event)" v-model="cityId"
                        :options="cities" track-by="id" label="name"></multi-select>
        </b-form-group>
      </div>
      <!-- <div class="col-md-6 col-sm-12 col-lg-4">
        <b-form-group>
          <label for="name">Neighborhood</label>
          <multi-select :options="cities" track-by="id" label="name"></multi-select>
        </b-form-group>
      </div>-->
      <div class="col-md-6 col-sm-12 col-lg-4">
        <b-form-group>
          <label for="name">Postcode / Zip</label>
          <b-form-input
            v-model="location.zipCode"
            type="text"
            placeholder="Enter your Postcode / Zip"
          ></b-form-input>
        </b-form-group>
      </div>
    </b-row>
    <!-- <div class="row">
      <div class="col-sm-12 col-lg-8">
        <b-form-group>
          <label for="name">Location</label>
          <div style="width: 100%;height: 100%;">

            <GmapMap style="width: auto; height: 300px;" :zoom="1" :center="{lat: 0, lng: 0}">
              <GmapMarker v-for="(marker, index) in markers"
                          :key="index"
                          :position="marker.position"
              />
              <GmapMarker
                v-if="this.place"
                label="★"
                :position="{
                    lat: 35.715298,
                    lng: 51.404343,
                  }"
              />
            </GmapMap>

          </div>
        </b-form-group>
      </div>
      <div class="col-sm-12 col-lg-4">
        <div class="row">
          <div class="col-sm-12">
            <b-form-group>
              <label for="name">Latitude</label>
              <b-form-input v-model="location.googleMapsLatitude" type="number" id="name"
                            placeholder="Enter your Location's Latitude"></b-form-input>
            </b-form-group>
          </div>
          <div class="col-sm-12">
            <b-form-group>
              <label for="name">Longitude</label>
              <b-form-input v-model="location.googleMapsLongitude" type="number" id="name"
                            placeholder="Enter your Location's Longitude"></b-form-input>
            </b-form-group>
          </div>
        </div>
      </div>
    </div>-->
    <submit-group v-on:onCancel="onCancel" v-on:onSubmit="onSubmit"/>
  </div>
</template>

<script>
    import {mapGetters, mapMutations} from "vuex";
    import {statics} from "../../../store/types";
    import {validationMixin} from "vuelidate";
    import {required, minLength} from "vuelidate/lib/validators";

    import * as VueGoogleMaps from "vue2-google-maps";

    export default {
        components: {},

        data: function () {
            return {
                editMode: false,
                location: {
                    countryId: "",
                    regionId: "",
                    cityId: "",
                    zipCode: "",
                    addressLine1: "",
                    addressLine2: "",
                    googleMapsLatitude: "",
                    googleMapsLongitude: "",
                    propertyId: this.$route.params.id
                },
                cities: [],
                regions: [],
                countries: [],
                value: null,
                markers: [],
                place: null,
                countryId: {},
                regionId: {},
                cityId: {},
                errmsg: {
                    addressLine1: false
                },
                addressLine1: "",
                errorStatus: false
            };
        },
        validations: {
            addressLine1: {
                required
            }
        },
        created() {
            this.countries = this.propertyCountries;
            this.cities = this.propertyCities;
            this.regions = this.propertyRegions;
            this.setProperty();
            this.markers.push({
                position: {
                    lat: 100,
                    lng: 200
                }
            });
        },
        methods: {
            ...mapMutations({
                showLoading: statics.mutations.loading
            }),
            customCountrySearch(e) {
                let countryList = [];
                this.propertyCountries.forEach(element => {
                    if (element.name.toLowerCase().startsWith(e.toLowerCase())) {
                        countryList.push(element)
                    }
                });
                this.countries = countryList;
            },
            customProvinceSearch(e) {
                let regionList = [];
                this.propertyRegions.forEach(element => {
                    if (element.name.toLowerCase().startsWith(e.toLowerCase())) {
                        regionList.push(element)
                    }
                });
                this.regions = regionList;
            },
            customCitySearch(e) {
                let cityList = []
                this.propertyCities.forEach(element => {
                    if (element.name.toLowerCase().startsWith(e.toLowerCase())) {
                        cityList.push(element)
                    }
                });
                this.cities = cityList;
            },
            onCancel() {
                this.$router.replaceLang("/estates/list");
            },
            onSubmit() {
                this.checkValidations();
                if (!this.errorStatus) {
                    this.location.countryId = this.countryId ? this.countryId.id : null;
                    this.location.cityId = this.cityId ? this.cityId.id : null;
                    this.location.regionId = this.regionId ? this.regionId.id : null;
                    this.editMode ? this.updateProperty() : this.createProperty();
                }
            },
            checkValidations() {
                if (!this.$v.addressLine1.required) {
                    this.errmsg.addressLine1 = true;
                    this.errorStatus = true;
                }
            },
            clear() {
                this.errmsg.addressLine1 = false;
                this.errorStatus = false;
            },
            setProperty() {
                this.showLoading(true);
                let that = this;
                this.$gate.propertyLocation
                    .get(this.$route.params.id)
                    .then(res => {
                        that.location = res.body;
                        this.addressLine1 = res.body.addressLine1;
                        that.countryId = that.propertyCountries.filter(
                            i => i.id === that.location.countryId
                        )[0];
                        that.cityId = that.propertyCities.filter(
                            i => i.id === that.location.cityId
                        )[0];
                        that.regionId = that.propertyRegions.filter(
                            i => i.id === that.location.regionId
                        )[0];
                        this.editMode = true;
                    })
                    .catch(err => this.$handleError(err))
                    .finally(() => {
                        this.showLoading(false);
                    });
            },
            createProperty() {
                this.location.addressLine1 = this.addressLine1;
                this.showLoading(true);
                this.$gate.propertyLocation
                    .create(this.location)
                    .then(res => {
                        this.$toasted.success("Location added.");
                        this.editMode = true;
                    })
                    .catch(err => this.$handleError(err)).finally(() => this.showLoading(false));
            },
            updateProperty() {
                this.location.addressLine1 = this.addressLine1;
                this.showLoading(true);
                this.$gate.propertyLocation
                    .update(this.location)
                    .then(res => {
                        this.$toasted.success("Location updated.");
                        let route = "/estates/estate/" + this.$route.params.id + "/location";
                        this.$router.replaceLang(route);
                    })
                    .catch(err => this.$handleError(err)).finally(() => this.showLoading(false));
            }
        },
        computed: {
            ...mapGetters({
                propertyCountries: statics.getters.country,
                propertyCities: statics.getters.city,
                propertyRegions: statics.getters.region
            }),
            isEditMode() {
                if (this.$route.params.id !== "new") {
                    return true;
                }
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
