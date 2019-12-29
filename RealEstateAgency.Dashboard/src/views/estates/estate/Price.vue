<template>
  <div class="animated fadeIn">
    <b-row>
      <div class="col-md-6 col-sm-12 col-lg-4">
        <b-form-group>
          <label for="name">Price *</label>
          <b-form-input
            v-model="price.price"
            type="number"
            placeholder="Enter your price"
            @focus="clear"
          ></b-form-input>
          <div v-if="errmsg == true" class="error vuelidate-invalid-feedback">Price is required.</div>
        </b-form-group>
      </div>
      <div class="col-md-6 col-sm-12 col-lg-4">
        <b-form-group>
          <label for="name">Currency *</label>
          <multi-select
            class="pointer"
            :internal-search="false"
            @search-change="customCurrencySearch($event)"
            v-model="currencyId"
            :options="currencies"
            label="name"
            track-by="id"
          ></multi-select>
        </b-form-group>
      </div>
      <div class="col-md-6 col-sm-12 col-lg-4">
        <b-form-group>
          <label for="name">Unit *</label>
          <multi-select
            class="pointer"
            :internal-search="false"
            v-model="priceScaleUnitId"
            :options="priceScales"
            label="name"
            track-by="id"
          ></multi-select>
        </b-form-group>
      </div>
      <div class="col-md-6 col-sm-12 col-lg-4">
        <b-form-group>
          <label for="name">Before Price Label</label>
          <b-form-input v-model="price.beforePriceLabel" type="text" placeholder="Enter your price"></b-form-input>
        </b-form-group>
      </div>
      <div class="col-md-6 col-sm-12 col-lg-4">
        <b-form-group>
          <label for="name">After Price Label</label>
          <b-form-input v-model="price.afterPriceLabel" type="text" placeholder="Enter your price"></b-form-input>
        </b-form-group>
      </div>
      <div class="col-md-6 col-sm-12 col-lg-4">
        <b-form-checkbox class="checkBox" v-model="price.priceOnCall">Price On Call</b-form-checkbox>
      </div>
    </b-row>
    <submit-group v-on:onCancel="onCancel" v-on:onSubmit="onSubmit" />
  </div>
</template>

<script>
import { mapGetters, mapMutations } from "vuex";
import { statics } from "../../../store/types";

export default {
  components: {},
  data: function() {
    return {
      currencies: [],
      options: [1, 2, 3, 4, 5, 6, 7],
      price: {
        currencyId: "",
        priceScaleUnitId: "",
        price: "",
        beforePriceLabel: "",
        afterPriceLabel: "",
        priceOnCall: false,
        propertyId: this.$route.params.id
      },
      errmsg: false,
      priceScaleUnitId: null,
      currencyId: null,
      editMode: false
    };
  },
  created() {
    this.currencies = this.propertyCurrencies;
    this.setProperty();
  },
  methods: {
    ...mapMutations({
      showLoading: statics.mutations.loading
    }),
    onCancel() {
      this.$router.replaceLang("/estates/list");
    },
    clear() {
      this.errmsg = false;
    },
    customCurrencySearch(e) {
      let currencyList = [];
      this.propertyCurrencies.forEach(element => {
        if (element.name.toLowerCase().startsWith(e.toLowerCase())) {
          currencyList.push(element);
        }
      });
      this.currencies = currencyList;
    },
    onSubmit() {
      if (this.price.price == "") {
        this.errmsg = true;
      } else {
        if (this.currencyId == null || this.priceScaleUnitId == null) {
          this.$toasted.global.warn("Please fill required fields.");
          this.showLoading(false);
        } else {
          debugger
          if (this.editMode) this.updateProperty();
          else this.createProperty();
        }
      }
    },
    setProperty() {
      this.showLoading(true);
      let that = this;
      this.$gate.propertyPrice
        .get(this.$route.params.id)
        .then(res => {
          that.price = res.body;
          that.currencyId = that.propertyCurrencies.filter(
            i => i.id === that.price.currencyId
          )[0];
          that.priceScaleUnitId = that.priceScales.filter(
            i => i.id === that.price.priceScaleUnitId
          )[0];
          that.editMode = true;
        })
        .catch(err => this.$handleError(err))
        .finally(() => {
          this.showLoading(false);
        });
    },
    createProperty() {
      if (this.price.price <= 0) {
        this.$toasted.global.warn("Price can't be negative.");
      } else {
        this.showLoading(true);
        this.price.currencyId = this.currencyId.id;
        this.price.calculatedPriceUnit =
          this.priceScales.filter(i => i.id === this.priceScaleUnitId.id)[0]
            .scale * this.price.price;
        this.price.priceScaleUnitId = this.priceScaleUnitId.id;
        this.$gate.propertyPrice
          .create(this.price)
          .then(res => {
            this.editMode = true;
            this.$toasted.success("Property price created.");
          })
          .catch(err => this.$handleError(err))
          .finally(() => this.showLoading(false));
      }
    },
    updateProperty() {
      if (this.price.price <= 0) {
        this.$toasted.global.warn("Price can't be negative.");
      } else {
        this.price.currencyId = this.currencyId.id;
        this.price.priceScaleUnitId = this.priceScaleUnitId.id;
        this.price.calculatedPriceUnit =
          this.priceScales.filter(i => i.id === this.priceScaleUnitId.id)[0]
            .scale * this.price.price;
        this.showLoading(true);
        this.$gate.propertyPrice
          .update(this.price)
          .then(res => {
            this.$toasted.success("Property price updated.");
          })
          .catch(err => this.$handleError(err))
          .finally(() => this.showLoading(false));
      }
    }
  },
  computed: {
    ...mapGetters({
      propertyCurrencies: statics.getters.currency,
      priceScales: statics.getters.priceScale
    })
  }
};
</script>

<style>
.checkBox {
  margin-top: 35px;
}

.vuelidate-invalid-feedback {
  width: 100%;
  margin-top: 0.25rem;
  font-size: 80%;
  color: #f86c6b;
}
</style>
