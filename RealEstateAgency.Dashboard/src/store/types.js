const propertyPrefix = "Property/";
export const property = {
  getters: {
    getAll: `${propertyPrefix}getAll`
  },
  mutations: {},
  actions: {}
};

const staticsPrefix = 'appStatics/';
export const statics = {
  getters: {
    //in app memory
    loading: `${staticsPrefix}get/loading`,
    //will get at the page load. also they have actions for set the value
    propertyFeature: `${staticsPrefix}get/propertyFeature`,
    importantPlace: `${staticsPrefix}get/importantPlace`,
    propertyLabel: `${staticsPrefix}get/propertyLabel`,
    propertyStatus: `${staticsPrefix}get/propertyStatus`,
    propertyType: `${staticsPrefix}get/propertyType`,
    agent: `${staticsPrefix}get/agent`,
    //these values will set only at the page load
    city: `${staticsPrefix}get/city`,
    actionType: `${staticsPrefix}get/actionType`,
    requestType: `${staticsPrefix}get/requestType`,
    country: `${staticsPrefix}get/country`,
    currency: `${staticsPrefix}get/currency`,
    language: `${staticsPrefix}get/language`,
    priceScale: `${staticsPrefix}get/priceScale`,
    region: `${staticsPrefix}get/region`,
    //
    user: `${staticsPrefix}get/user`,
    userLanguage: `${staticsPrefix}get/userLanguage`,
    dateFormat: `${staticsPrefix}get/dateFormat`,
    userCurrency: `${staticsPrefix}get/userCurrency`,
    realEstate: `${staticsPrefix}get/realEstate`,
    selectedLanguage: `${staticsPrefix}get/selectedLanguage`,
    //
    siteInfo: `${staticsPrefix}get/siteInfo`,
  },
  mutations: {
    loading: `${staticsPrefix}mut/loading`,
    propertyFeature: `${staticsPrefix}mut/propertyFeature`,
    importantPlace: `${staticsPrefix}mut/importantPlace`,
    propertyLabel: `${staticsPrefix}mut/propertyLabel`,
    propertyStatus: `${staticsPrefix}mut/propertyStatus`,
    propertyType: `${staticsPrefix}mut/propertyType`,
    agent: `${staticsPrefix}mut/agent`,
    //these values will set only at the page load
    city: `${staticsPrefix}mut/city`,
    actionType: `${staticsPrefix}mut/actionType`,
    requestType: `${staticsPrefix}mut/requestType`,
    country: `${staticsPrefix}mut/country`,
    currency: `${staticsPrefix}mut/currency`,
    language: `${staticsPrefix}mut/language`,
    priceScale: `${staticsPrefix}mut/priceScale`,
    region: `${staticsPrefix}mut/region`,
    selectedLanguage: `${staticsPrefix}mut/selectedLanguage`,
    //
    user: `${staticsPrefix}mut/user`,
    userLanguage: `${staticsPrefix}mut/userLanguage`,
    dateFormat: `${staticsPrefix}mut/dateFormat`,
    userCurrency: `${staticsPrefix}mut/userCurrency`,
    realEstate: `${staticsPrefix}mut/realEstate`,
    //
    siteInfo: `${staticsPrefix}mut/siteInfo`,
  },
  actions: {
    loadStatics: `${staticsPrefix}action/loadStatics`,
    //
    loadPropertyFeatures: `${staticsPrefix}action/propertyFeature`,
    loadImportantPlaces: `${staticsPrefix}action/importantPlace`,
    loadPropertyLabels: `${staticsPrefix}action/propertyLabel`,
    loadPropertyStatuses: `${staticsPrefix}action/propertyStatus`,
    loadPropertyTypes: `${staticsPrefix}action/propertyType`,
    loadAgents: `${staticsPrefix}action/agent`,
    loadUser: `${staticsPrefix}action/user`,

    //
    loadSiteInfo: `${staticsPrefix}action/siteInfo`,
  }
};

const userEventsPrefix = 'userEvents/';
export const userEvents = {
  getters: {
    userEvents: `${userEventsPrefix}get/userEvents`,
    userActions: `${userEventsPrefix}get/userActions`,
    notCompletedEventCount: `${userEventsPrefix}get/notCompletedEventCount`,
    notCompletedActionCount: `${userEventsPrefix}get/notCompletedActionCount`,
  },
  mutations: {
    userEvents: `${userEventsPrefix}mut/userEvents`,
    userActions: `${userEventsPrefix}mut/userActions`,
  },
  actions: {
    loadAll: `${userEventsPrefix}action/loadAll`,
    loadEvents: `${userEventsPrefix}action/loadEvents`,
    loadActions: `${userEventsPrefix}action/loadActions`,
  }
};
