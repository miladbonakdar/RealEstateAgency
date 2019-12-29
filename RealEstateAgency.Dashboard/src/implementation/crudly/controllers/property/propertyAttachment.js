export default{
    name: "propertyAttachment",
    loadDefaults: false,
    actions: [
      { type: "post", headers: { 'Content-Type': 'multipart/form-data' } },
      { type: "get", name: "getAll", url: "/:propertyId" },
      { type: "get", name: "get", url: "/:propertyId/:id" },
      { type: "delete", url: "/:id" },
    ]
}