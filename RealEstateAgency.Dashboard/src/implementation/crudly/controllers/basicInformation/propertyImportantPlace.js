export default{
    name: "importantPlaceType",
    actions: [
      {type: "patch", url: "/uploadAndCreate", name: "uploadAndCreate",
      headers: {'Content-Type': 'multipart/form-data'}}
    ]
  }
