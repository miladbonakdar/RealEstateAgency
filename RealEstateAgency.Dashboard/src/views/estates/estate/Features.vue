<template>
  <div class="animated fadeIn">
    <b-row class="checkBoxRows">
      <ul>
        <li class="col-md-4 col-sm-6 col-lg-3 featureItem"
            v-for="feature in features"
            :key="feature.id"
        >
          <div class="custom-control custom-checkbox">
            <input type="checkbox" class="custom-control-input" :id="'checkbox_'+feature.id" :value="feature.id"
                   v-model="featureList">
            <label class="custom-control-label" :for="'checkbox_'+feature.id">{{feature.name}}</label>
          </div>
        </li>
      </ul>
    </b-row>
    <submit-group v-on:onCancel="onCancel" v-on:onSubmit="onSubmit"/>

  </div>
</template>

<script>

    import {mapGetters, mapMutations} from 'vuex';
    import {statics} from '../../../store/types';

    export default {
        components: {},
        data: function () {
            return {
                featureList: []
            }
        },
        created() {
            if (this.isEditMode) {
                this.setFeatures();
            }
        },
        methods: {
            ...mapMutations({
                showLoading: statics.mutations.loading
            }),
            onCancel() {
                this.$router.replaceLang('/estates/list');
            },
            onSubmit() {
                this.showLoading(true);
                const data = {
                    propertyFeatureIds: [...this.featureList],
                    propertyId: this.$route.params.id
                };
                this.$gate.propertyFeature.setPropertyFeatures(data).then(res => {
                    this.$toasted.success('Features Added.')
                }).catch(err => this.$handleError(err)
                ).finally(() => this.showLoading(false));
            },
            setFeatures() {
                this.getFeatures();
            },
            getFeatures() {
                this.showLoading(true);
                this.$gate.propertyFeature.getPropertyFeatures(this.$route.params.id)
                    .then(res => {
                        this.featureList = res.body.map(i => i.id);
                    })
                    .catch(err => this.$logError(err))
                    .finally(() => {
                        this.showLoading(false);
                    });
            },
            isEditMode() {
                if (this.$route.params.id !== 'new') {
                    return true;
                }
            }
        },
        computed: {
            ...mapGetters({
                features: statics.getters.propertyFeature,
            })
        }
    }
</script>

<style>
  .checkBoxRows {
    padding: 10px
  }

  .featureItem {
    display: inline-block;
    list-style: none;
  }

  .container {
    display: block;
    position: relative;
    padding-left: 35px;
    margin-bottom: 12px;
    cursor: pointer;
    font-size: 18px;
    -webkit-user-select: none;
    -moz-user-select: none;
    -ms-user-select: none;
    user-select: none;
  }

  /* Hide the browser's default checkbox */
  .container input {
    position: absolute;
    opacity: 0;
    cursor: pointer;
    height: 0;
    width: 0;
  }

  .checkmark {
    position: absolute;
    top: 0;
    left: 0;
    height: 25px;
    width: 25px;
    background-color: #eee;
  }

  /* On mouse-over, add a grey background color */
  .container:hover input ~ .checkmark {
    background-color: #ccc;
  }

  /* When the checkbox is checked, add a blue background */
  .container input:checked ~ .checkmark {
    background-color: #2196F3;
  }

  /* Create the checkmark/indicator (hidden when not checked) */
  .checkmark:after {
    content: "";
    position: absolute;
    display: none;
  }

  /* Show the checkmark when checked */
  .container input:checked ~ .checkmark:after {
    display: block;
  }

  /* Style the checkmark/indicator */
  .container .checkmark:after {
    left: 9px;
    top: 5px;
    width: 5px;
    height: 10px;
    border: solid white;
    border-width: 0 3px 3px 0;
    -webkit-transform: rotate(45deg);
    -ms-transform: rotate(45deg);
    transform: rotate(45deg);
  }
</style>
