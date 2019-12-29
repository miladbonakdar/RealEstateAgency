import Vue from "vue";
import Vuex from "vuex";
import statics from "./modules/statics";
import userEvents from "./modules/userEvents";

Vue.use(Vuex);

export const store = new Vuex.Store({
  modules: {
    statics,
    events :userEvents
  }
});
