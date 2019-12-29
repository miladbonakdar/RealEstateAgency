<template>
  <div>
    <template v-if="isLoading">
      <div class="loading-container">
      </div>
      <CubeShadow background="#20a8d8" class="loading"></CubeShadow>
    </template>
    <router-view></router-view>
  </div>
</template>

<script>
  import {CubeShadow} from 'vue-loading-spinner'
  import {mapGetters , mapActions} from 'vuex'
  import {statics} from './store/types'

  export default {
    name: 'app',
    components: {
      CubeShadow
    },
    created() {
        window.onerror = this.onerror;
        this.loadSiteInfo(this.$gate,)
    },
    methods: {
        ...mapActions({
            loadSiteInfo : statics.actions.loadSiteInfo
        }),
        onerror(message, source, lineno, colno, error){
            console.log(message, source, lineno, colno, error);
        }
    },
    computed: {
      ...mapGetters({
          isLoading: statics.getters.loading
      })
    }
  }
</script>

<style src="vue-multiselect/dist/vue-multiselect.min.css"></style>
<style lang="scss">
  // CoreUI Icons Set
  @import '~@coreui/icons/css/coreui-icons.min.css';
  /* Import Font Awesome Icons Set */
  $fa-font-path: '~font-awesome/fonts/';
  @import '~font-awesome/scss/font-awesome.scss';
  @import "~animate.css/animate.min.css";
  @import "~hover.css/css/hover-min.css";
  @import '~vue-datetime/dist/vue-datetime.min.css';
  /* Import Simple Line Icons Set */
  $simple-line-font-path: '~simple-line-icons/fonts/';
  @import '~simple-line-icons/scss/simple-line-icons.scss';
  /* Import Flag Icons Set */
  @import '~flag-icon-css/css/flag-icon.min.css';
  /* Import Bootstrap Vue Styles */
  @import '~bootstrap-vue/dist/bootstrap-vue.css';
  // Import Main styles for this application
  @import 'assets/scss/style';

  .loading {
    position: absolute;
    left: calc(50% - 20px);
    top: calc(50% - 20px);
    z-index: 10000;
  }

  .loading-container {
    width: 100%;
    height: 100%;
    position: fixed;
    z-index: 9999;
    background-color: whitesmoke;
    opacity: 0.6;
  }

  .datepicker input {
    border: none;
    width: 100%;
  }

  div.multiselect__tags {
    min-height: 35px;
    display: block;
    padding: 5px 40px 0 8px;
    border-radius: 5px;
    border: 1px solid #e8e8e8;
    background: #fff;
    font-size: 14px;
  }

  span.multiselect__placeholder {
    margin-bottom: 0px;
  }
  .pointer {
    cursor: pointer !important;
  }

  .vdatetime-popup__header,
  .vdatetime-calendar__month__day--selected > span > span,
  .vdatetime-calendar__month__day--selected:hover > span > span {
    background: #20a8d8;
  }

  .vdatetime-year-picker__item--selected,
  .vdatetime-time-picker__item--selected,
  .vdatetime-popup__actions__button {
    color: #20a8d8;
  }
  .highlighted {
    background-color: yellow;
  }
</style>
