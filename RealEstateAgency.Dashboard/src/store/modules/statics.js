import { statics } from "../types";
import loadStaticsAction from "./partials/loadStaticsAction.partial";
import loadPromise from "./partials/loadPromise.partial";

const state = {
  loading: false,
  statics: {
    //
    propertyFeatures: [],
    propertyLabels: [],
    propertyStatuses: [],
    propertyTypes: [],
    importantPlaces: [],
    agents: [],
    //
    cities: [],
    countries: [],
    currencies: [],
    languages: [],
    priceScales: [],
    regions: [],
    actionTypes: [],
    requestTypes: [],
    //
    user: null,
    dateFormat: "YYYY-MM-DD",
    currency: "$",
    userLanguage: null,
    realEstate: null,
    selectedLanguage: {},
    //
    siteInfo: {}
  }
};

const getters = {
  [statics.getters.loading]: state => state.loading,
  //
  [statics.getters.propertyFeature]: state => state.statics.propertyFeatures,
  [statics.getters.propertyLabel]: state => state.statics.propertyLabels,
  [statics.getters.propertyStatus]: state => state.statics.propertyStatuses,
  [statics.getters.propertyType]: state => state.statics.propertyTypes,
  [statics.getters.importantPlace]: state => state.statics.importantPlaces,
  [statics.getters.agent]: state => state.statics.agents,
  //
  [statics.getters.city]: state => state.statics.cities,
  [statics.getters.country]: state => state.statics.countries,
  [statics.getters.currency]: state => state.statics.currencies,
  [statics.getters.language]: state => state.statics.languages,
  [statics.getters.priceScale]: state => state.statics.priceScales,
  [statics.getters.region]: state => state.statics.regions,
  [statics.getters.actionType]: state => state.statics.actionTypes,
  [statics.getters.requestType]: state => state.statics.requestTypes,
  //
  [statics.getters.user]: state => state.statics.user,
  [statics.getters.userLanguage]: state => state.statics.userLanguage,
  [statics.getters.dateFormat]: state => state.statics.dateFormat,
  [statics.getters.realEstate]: state => state.statics.realEstate,
  [statics.getters.userCurrency]: state => state.statics.currency,
  [statics.getters.selectedLanguage]: state => state.statics.selectedLanguage,
  //
  [statics.getters.siteInfo]: state => state.statics.siteInfo
};

const mutations = {
  [statics.mutations.loading]: (state, payload) => (state.loading = payload),
  [statics.mutations.propertyFeature]: (state, payload) =>
    (state.statics.propertyFeatures = payload),
  [statics.mutations.propertyLabel]: (state, payload) =>
    (state.statics.propertyLabels = payload),
  [statics.mutations.propertyStatus]: (state, payload) =>
    (state.statics.propertyStatuses = payload),
  [statics.mutations.propertyType]: (state, payload) =>
    (state.statics.propertyTypes = payload),
  [statics.mutations.importantPlace]: (state, payload) =>
    (state.statics.importantPlaces = payload),
  [statics.mutations.agent]: (state, payload) =>
    (state.statics.agents = payload),
  //
  [statics.mutations.city]: (state, payload) =>
    (state.statics.cities = payload),
  [statics.mutations.country]: (state, payload) =>
    (state.statics.countries = payload),
  [statics.mutations.currency]: (state, payload) =>
    (state.statics.currencies = payload),
  [statics.mutations.language]: (state, payload) =>
    (state.statics.languages = payload),
  [statics.mutations.priceScale]: (state, payload) =>
    (state.statics.priceScales = payload),
  [statics.mutations.region]: (state, payload) =>
    (state.statics.regions = payload),
  [statics.mutations.actionType]: (state, payload) =>
    (state.statics.actionTypes = payload),
  [statics.mutations.requestType]: (state, payload) =>
    (state.statics.requestTypes = payload),
  //
  [statics.mutations.user]: (state, payload) => (state.statics.user = payload),
  [statics.mutations.userLanguage]: (state, payload) =>
    (state.statics.userLanguage = payload),
  [statics.mutations.dateFormat]: (state, payload) => {
    state.statics.dateFormat = payload;
    window.dateFormat = payload;
  },
  [statics.mutations.realEstate]: (state, payload) =>
    (state.realEstate = payload),
  [statics.mutations.userCurrency]: (state, payload) => {
    state.statics.currency = payload;
    window.currency = payload;
  },
  [statics.mutations.selectedLanguage]: (state, payload) => {
    const langId = state.realEstate.languageIdDefault;
    const Language = state.statics.languages.filter(
      i => i.urlCode === payload
    )[0];
    if(Language.id == langId)
      state.statics.selectedLanguage =Language;
    else
    {
      const validLanguage = state.statics.languages.filter(
        i => i.id === langId
      )[0];
      const newRoute = `${location.origin}/${validLanguage.urlCode}${location.pathname.substr(3)}`;
      location.replace(newRoute);
    }
  },
  [statics.mutations.siteInfo]: (state, payload) =>
    (state.statics.siteInfo = payload)
};

let actions = {
  [statics.actions.loadStatics]: loadStaticsAction,
  [statics.actions.loadPropertyFeatures]: ({ commit }, $gate) => {
    loadPromise($gate.propertyFeature.getAll(), items =>
      commit(statics.mutations.propertyFeature, items)
    );
  },
  [statics.actions.loadPropertyLabels]: ({ commit }, $gate) => {
    loadPromise($gate.propertyLabel.getAll(), items =>
      commit(statics.mutations.propertyLabel, items)
    );
  },
  [statics.actions.loadPropertyStatuses]: ({ commit }, $gate) => {
    loadPromise($gate.propertyStatus.getAll(), items =>
      commit(statics.mutations.propertyStatus, items)
    );
  },
  [statics.actions.loadPropertyTypes]: ({ commit }, $gate) => {
    loadPromise($gate.propertyType.getAll(), items =>
      commit(statics.mutations.propertyType, items)
    );
  },
  [statics.actions.loadImportantPlaces]: ({ commit }, $gate) => {
    loadPromise($gate.importantPlace.getAll(), items =>
      commit(statics.mutations.importantPlace, items)
    );
  },
  [statics.actions.loadAgents]: ({ commit }, $gate) => {
    loadPromise($gate.agent.getAgents(), items =>
      commit(statics.mutations.agent, items)
    );
  },
  [statics.actions.loadUser]: ({ commit }, $gate, callBack) => {
    loadPromise($gate.auth.get(), item => {
      commit(statics.mutations.user, item);
      callBack();
    });
  },
  [statics.actions.loadSiteInfo]: ({ commit }, $gate) => {
    loadPromise($gate.public.siteInfo(), info =>
      commit(statics.mutations.siteInfo, info)
    );
  }
};

export default {
  state,
  getters,
  mutations,
  actions
};
