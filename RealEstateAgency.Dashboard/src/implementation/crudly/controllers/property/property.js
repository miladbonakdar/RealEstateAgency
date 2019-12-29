export default {
  name: "property",
  actions: [
    {
      type: "get",
      url: "/getPropertySummery/:propertyId",
      name: "getPropertySummery"
    },
    { type: "put", url: "/setReadyForPublish", name: "setReadyForPublish" },
    {
      type: "put",
      url: "/setNotReadyForPublish",
      name: "setNotReadyForPublish"
    },
    { type: "put", url: "/publishProperty", name: "publishProperty" },
    { type: "put", url: "/unPublishProperty", name: "unPublishProperty" }
  ]
};
