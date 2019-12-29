<template>
  <div v-if="summary">
    <b-card header="information">

      <b-row>
        <div class="col-md-4 my-1">
          <i class="fa fa-home"></i>
          Title: {{summary.title}}
        </div>
        <div class="col-md-4 my-1" v-if="summary.propertyTypeId != ''">
          <i class="fa fa-building"></i>

          Property Type: {{summary.propertyTypeId}}
        </div>
        <div class="col-md-4 my-1" v-if="summary.propertyStatusId != ''">
          <i class="fa fa-bolt"></i>

          Property Status: {{summary.propertyStatusId}}
        </div>
        <div class="col-md-4 my-1" v-if="summary.propertyLabelId != ''">
          <i class="fa fa-handshake-o"></i>

          Property Label: {{summary.propertyLabelId}}
        </div>
        <div class="col-md-4 my-1" v-if="summary.regionId != ''">
          <i class="fa fa-map-marker"></i>

          Property Region: {{summary.regionId}}
        </div>
        <div class="col-md-4 my-1" v-if="summary.cityId != ''">
          <i class="fa fa-map-o"></i>

          Property City: {{summary.cityId}}
        </div>
        <div class="col-md-4 my-1" v-if="summary.currencyId != ''">
          <i class="fa fa-money"></i>

          Price: {{summary.price | currency}} {{summary.priceScaleUnitId}} {{summary.currencyId}}
        </div>
        <div class="col-md-4 my-1" v-if="summary.size != 0">
          Size: {{summary.size}} m²
        </div>
        <div class="col-md-4 my-1" v-if="summary.bedrooms != 0">
          <i class="fa fa-bed"></i>

          Bedrooms: {{summary.bedrooms}}
        </div>
        <div class="col-md-4 my-1" v-if="summary.bathrooms != 0">
          <i class="fa fa-shower"></i>

          Bathrooms: {{summary.bathrooms}}
        </div>
        <div class="col-md-4 my-1" v-if="summary.garages != 0">
          <i class="fa fa-car"></i>

          Garages: {{summary.garages}}
        </div>
        <div class="col-md-4 my-1" v-if="summary.rooms != 0">
          <i class="fa fa-briefcase"></i>


          Rooms: {{summary.rooms}}
        </div>
      </b-row>
    </b-card>
    <b-card header="Media">
      <b-row v-if="summary.imagesUrl.length>0">
        <div class="col-lg-2 col-sm-6 col-md-4" v-for="image in summary.imagesUrl" :key="image.id">
          <b-card
            :img-src="image"
            img-top
          >
            <b-card-text>{{image.imageCaption}}</b-card-text>

          </b-card>
        </div>
      </b-row>
      <b-row v-else>
        <div class="col-md-12">
          <div class="text-center p-5" style="align-self:center">
            <h4>No images</h4>
          </div>
        </div>
      </b-row>
    </b-card>
  </div>


</template>

<script>
    import {mapGetters, mapMutations} from 'vuex';
    import {statics} from '../../../store/types';

    export default {
        data() {
            return {
                summary: {
                    title: '',
                    propertyTypeId: '',
                    propertyStatusId: '',
                    propertyLabelId: '',
                    regionId: '',
                    cityId: '',
                    currencyId: '',
                    priceScaleUnitId: '',
                    price: 0,
                    size: 0,
                    bedrooms: 0,
                    bathrooms: 0,
                    garages: 0,
                    rooms: 0,
                    imagesUrl: []
                }
            }
        },
        methods: {
            ...mapMutations({
                showLoading: statics.mutations.loading
            }),
        },
        computed: {
            ...mapGetters({
                propertyCountries: statics.getters.country,
                cities: statics.getters.city,
                regions: statics.getters.region,
                propertyTypes: statics.getters.propertyType,
                propertyStatuses: statics.getters.propertyStatus,
                propertyLabels: statics.getters.propertyLabel,
                currencies: statics.getters.currency,
                priceScales: statics.getters.priceScale,



            }),
        },
        created() {
            let that = this;
            this.showLoading(true);
            this.$gate.property.getPropertySummery(this.$route.params.id).then(res => {
                that.summary.title = res.body.title;

                let propertyType = that.propertyTypes.filter(i => i.id === res.body.propertyTypeId)[0];
                that.summary.propertyTypeId = propertyType ? propertyType['name'] : '';

                let propertyStatus = that.propertyStatuses.filter(i => i.id === res.body.propertyStatusId)[0];
                that.summary.propertyStatusId = propertyStatus ? propertyStatus['name'] : '';

                let propertyLabel = that.propertyLabels.filter(i => i.id === res.body.propertyLabelId)[0];
                that.summary.propertyLabelId = propertyLabel ? propertyLabel['name'] : '';

                let propertyRegion = that.regions.filter(i => i.id === res.body.regionId)[0];
                that.summary.regionId = propertyRegion ? propertyRegion['name'] : '';

                let propertyCity = that.cities.filter(i => i.id === res.body.cityId)[0];
                that.summary.cityId = propertyCity ? propertyCity['name'] : '';

                let propertyCurrency = that.currencies.filter(i => i.id === res.body.currencyId)[0];
                that.summary.currencyId = propertyCurrency ? propertyCurrency['name'] : '';

                let propertyPriceScale = that.priceScales.filter(i => i.id === res.body.priceScaleUnitId)[0];
                that.summary.priceScaleUnitId = propertyPriceScale ? propertyPriceScale['name'] : '';
                that.summary.size = res.body.size;
                that.summary.price = res.body.price;

                that.summary.bedrooms = res.body.bedrooms;
                that.summary.bathrooms = res.body.bathrooms;
                that.summary.garages = res.body.garages;
                that.summary.rooms = res.body.rooms;
                that.summary.imagesUrl = res.body.imagesUrl;

            }).catch(err => this.$handleError(err))
                .finally(() => that.showLoading(false))
        }

    }
</script>

<style>

</style>
