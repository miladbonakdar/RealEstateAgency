export default{
    name: "propertyImage",
    loadDefaults: false,
    actions: [
      { type: "post", headers: { 'Content-Type': 'multipart/form-data' } },
      { type: "get", name: "getAll", url: "/:imageType/:propertyId" },
      { type: "get", name: "get", url: "/:imageType/:propertyId/:id" },
      { type: "delete", url: "/:id" },
    ]
}