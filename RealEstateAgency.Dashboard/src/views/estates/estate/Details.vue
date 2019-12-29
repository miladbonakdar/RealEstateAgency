<template>
  <div class="animated fadeIn">
    <b-row>
      <div class="col-md-6 col-sm-12 col-lg-4">
        <b-form-group class="mb-0">
          <label for="name">Size</label>
          <b-input-group>
            <b-form-input v-model="details.size" type="number" placeholder="Enter Size"></b-form-input>
            <b-input-group-append>
              <b-button :disabled="disable">m²</b-button>
            </b-input-group-append>
          </b-input-group>
        </b-form-group>
      </div>
      <div class="col-md-6 col-sm-12 col-lg-4">
        <b-form-group>
          <label for="name">LandArea</label>
          <b-form-input v-model="details.landArea" type="number" placeholder="Enter your Land Area"></b-form-input>
        </b-form-group>
      </div>
      <div class="col-md-6 col-sm-12 col-lg-4">
        <b-form-group>
          <label for="name">Rooms</label>
          <b-form-input v-model="details.rooms" type="number" placeholder="Enter Rooms' Number "></b-form-input>
        </b-form-group>
      </div>
    </b-row>
    <b-row>
      <div class="col-md-6 col-sm-12 col-lg-4">
        <b-form-group>
          <label for="name">Bedrooms</label>
          <b-form-input
            v-model="details.bedrooms"
            type="number"
            placeholder="Enter Bedrooms' Number"
          ></b-form-input>
        </b-form-group>
      </div>
      <div class="col-md-6 col-sm-12 col-lg-4">
        <b-form-group>
          <label for="name">Bathrooms</label>
          <b-form-input
            v-model="details.bathrooms"
            type="number"
            placeholder="Enter Bathrooms' Number"
          ></b-form-input>
        </b-form-group>
      </div>
      <div class="col-md-6 col-sm-12 col-lg-4">
        <b-form-group>
          <label for="name">Garages</label>
          <b-form-input v-model="details.garages" type="number" placeholder="Enter Garages' Number"></b-form-input>
        </b-form-group>
      </div>
    </b-row>
    <b-row>
      <div class="col-md-6 col-sm-12 col-lg-4">
        <b-form-group>
          <label for="name">Garage Size</label>
          <b-input-group>
            <b-form-input
              v-model="details.garagesSize"
              type="number"
              placeholder="Enter garage size"
            ></b-form-input>
            <b-input-group-append>
              <b-button :disabled="disable">m²</b-button>
            </b-input-group-append>
          </b-input-group>
        </b-form-group>
      </div>
      <div class="col-md-6 col-sm-12 col-lg-4">
        <b-form-group>
          <label for="name">year build</label>
          <b-form-input v-model="details.yearBuild" type="number" placeholder="Enter Year Build"></b-form-input>
        </b-form-group>
      </div>
    </b-row>
    <submit-group v-on:onCancel="onCancel" v-on:onSubmit="onSubmit" />
  </div>
</template>

<script>
    import {mapMutations} from "vuex";
    import {statics} from "../../../store/types";

    export default {
        components: {},
        data: function () {
            return {
                disable: true,
                details: {
                    size: null,
                    landArea: null,
                    rooms: null,
                    bedrooms: null,
                    bathrooms: null,
                    garages: null,
                    garagesSize: null,
                    yearBuild: null,
                    propertyId: this.$route.params.id
                },
                detailNumber: 1,
                editMode: false,
            }
        },
        created() {
            this.setPropertyDetails();
        },
        methods: {
            ...mapMutations({
                showLoading: statics.mutations.loading
            }),
            onCancel() {
                this.$router.replaceLang('/estates/list');
            },
            onSubmit() {
                if (this.editMode)
                    this.updatePropertyDetails();
                else
                    this.createPropertyDetails();
            },
            setPropertyDetails() {
                let that = this;
                this.showLoading(true);
                this.$gate.propertyDetail.get(this.$route.params.id).then(res => {
                    that.details = res.body;
                    that.editMode = true;
                }).catch(err => this.$handleError(err))
                    .finally(() => that.showLoading(false))
            },
            checkNumbers(){
              if(this.details.size <0 || this.details.landArea <0 || this.details.rooms <0  || this.details.bedrooms <0 || this.details.bathrooms <0 || this.details.garagesSize < 0 || this.details.yearBuild < 0){
                
                return false;
              }else{
                return true;
              }

            },
            createPropertyDetails() {
              if(!this.checkNumbers()){
                this.$toasted.global.warn("Numbers can't be negative.")
              }
              else{
                this.showLoading(true);
                this.$gate.propertyDetail.create(this.details).then(() => {
                    this.$toasted.success('Details created');
                    this.editMode = true;
                }).catch(err => this.$handleError(err))
                    .finally(() => this.showLoading(false));
              }
              this.showLoading(false)
            },
            updatePropertyDetails() {
                this.showLoading(true);
                if(!this.checkNumbers()){
                this.$toasted.global.warn("Numbers can't be negative.")
              }
              else{
                this.$gate.propertyDetail.update(this.details).then(() => {
                    this.$toasted.success('Details updated');
                }).catch(err => this.$handleError(err))
                    .finally(() => this.showLoading(false));
              }
              this.showLoading(false)
                
              }
              
                
            },
    }
</script>

<style>
th,
td {
  padding: 5px;
  text-align: left;
}

table#t01 tr:nth-child(even) {
  background-color: #eee;
}

table#t01 tr:nth-child(odd) {
  background-color: #fff;
}

table#t01 th {
  background-color: gray;
  color: white;
}

.addDetail {
  border: 1px solid #d5d5d5;
  border-radius: 10px;
}

td {
  width: 41%;
}
</style>
