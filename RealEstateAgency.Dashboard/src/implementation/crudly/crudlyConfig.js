import property from './controllers/property/property';
import location from './controllers/property/propertyLocation';
import price from './controllers/property/propertyPrice';
import details from './controllers/property/propertyDetail';
import floorPlan from './controllers/property/propertyFloorPlan';
import additionalDetails from './controllers/property/propertyAdditionalDetail';
import propertyImage from './controllers/property/propertyImage';
import propertyAttachment from './controllers/property/propertyAttachment';

import propertyLabel from './controllers/basicInformation/propertyLabel';
import propertyLabelTranslate from './controllers/basicInformation/propertyLabelTranslate';
import propertyType from './controllers/basicInformation/propertyType';
import propertyTypeTranslate from './controllers/basicInformation/propertyTypeTranslate';
import propertyStatus from './controllers/basicInformation/propertyStatus';
import propertyStatusTranslate from './controllers/basicInformation/propertyStatusTranslate';
import propertyFeature from './controllers/basicInformation/propertyFeature';
import propertyFeatureTranslate from './controllers/basicInformation/propertyFeatureTranslate';
import propertyImportantPlace from './controllers/basicInformation/propertyImportantPlace';
import propertyImportantPlaceTranslate from './controllers/basicInformation/propertyImaportantPlaceTranslate';
import requests from './controllers/projects/requests';
import requestAction from './controllers/projects/requestAction';
import requestActionFollowUp from './controllers/projects/requestActionFollowUp'
import userAccount from './controllers/users/userAccount'
import agent from './controllers/users/agent'


import statics from './controllers/statics';
import {apiRoute} from "../routeProvider";


export default {
  root: apiRoute,
  defaultActions: [
    {type: "get", name: "getAll"},
    {type: "put"},
    {type: "post"},
    {type: "get", name: "get", url: "/:id"},
    {type: "delete", url: "/:id"},
    {type: "post", url: "/page", name: "page"},
    {type: "get", url: "/page/:pageSize/:pageNumber", name: "getPage"}
  ],
  controllers: [
    ...statics,
    property,
    location,
    price,
    details,
    floorPlan,
    additionalDetails,
    propertyLabel,
    propertyType,
    propertyStatus,
    propertyLabelTranslate,
    propertyTypeTranslate,
    propertyStatusTranslate,
    propertyImage,
    propertyAttachment,
    propertyFeatureTranslate,
    propertyFeature,
    propertyImportantPlace,
    propertyImportantPlaceTranslate,
    requests,
    requestAction,
    requestActionFollowUp,
    userAccount,
    agent,
    {
      name: "auth",
      loadDefaults: false,
      actions: [
        {type: "post", name: "login", url: "/login"},
        // { type: "post", name: "registerAgent", url:"/registerAgent"},
        // { type: "post", name: "checkUser", url:"/checkUser"},
        {type: "get", name: "getUserRealEstate", url: "/getUserRealEstate"},
        {type: "get", name: "get"},
      ]
    },
    
    {
      name: "public",
      loadDefaults: false,
      actions: [
        {type: "get", url: "/siteInfo", name: "siteInfo"},
      ]
    }
  ]
};
