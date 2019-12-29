export default{
    name: "propertyFeature",
    actions: [
      {type: "patch", url: "/setPropertyFeatures", name: "setPropertyFeatures"},
      {type: "get", url: "/getPropertyFeatures/:propertyId", name: "getPropertyFeatures"}
    ]
  }