import {statics} from "../../types";
import getLangFormat from "../../../utils/localeDateFormat";

export default (global, {
  $gate,
  done
}) => {
  $gate.all([
    $gate.propertyFeature.getAll(),
    $gate.propertyLabel.getAll(),
    $gate.propertyStatus.getAll(),
    $gate.propertyType.getAll(),
    $gate.city.getAll(),
    $gate.country.getAll(),
    $gate.currency.getAll(),
    $gate.language.getAll(),
    $gate.priceScale.getAll(),
    $gate.region.getAll(),
    $gate.importantPlaceType.getAll(),
    $gate.actionType.getAll(),
    $gate.requestType.getAll(),
    $gate.auth.get(),
    $gate.agent.getAgents(),
    $gate.auth.getUserRealEstate(),
  ])
    .then(([
             propertyFeatures,
             propertyLabels,
             propertyStatuses,
             propertyTypes,
             cities,
             countries,
             currencies,
             languages,
             priceScales,
             regions,
             importantPlaces,
             actionTypes,
             requestTypes,
             user,
             agent,
             realEstate
           ]) => {
      let commit = global.commit;
      commit(statics.mutations.propertyFeature, propertyFeatures.data);
      commit(statics.mutations.propertyLabel, propertyLabels.data);
      commit(statics.mutations.propertyStatus, propertyStatuses.data);
      commit(statics.mutations.propertyType, propertyTypes.data);
      commit(statics.mutations.city, cities.data);
      commit(statics.mutations.country, countries.data);
      commit(statics.mutations.currency, currencies.data);
      commit(statics.mutations.language, languages.data);
      commit(statics.mutations.priceScale, priceScales.data);
      commit(statics.mutations.region, regions.data);
      commit(statics.mutations.importantPlace, importantPlaces.data);
      commit(statics.mutations.actionType, actionTypes.data);
      commit(statics.mutations.requestType, requestTypes.data);
      commit(statics.mutations.user, user.data);
      commit(statics.mutations.agent, agent.data);
      commit(statics.mutations.realEstate, realEstate.data);

      let userLang = languages.data.filter(i => i.id === user.data.languageId)[0] || {};
      let userCurrency = currencies.data.filter(i => i.id === realEstate.data.currencyId)[0] || {};
      commit(statics.mutations.userLanguage, userLang);
      commit(statics.mutations.dateFormat, getLangFormat(userLang.code));
      commit(statics.mutations.userCurrency, userCurrency.symbol);
      done();
    }).catch(error => {
    console.log(error);
    done(error);
  });
}
