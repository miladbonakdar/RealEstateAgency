export default {
    name: "propertyFloorPlan",
    actions: [
      {type: "get", url: "/getPropertyFloorPlans/:propertyId", name: "getPropertyFloorPlans"},
      {type: "patch", url: "/setPropertyFloorPlanImage", name: "setPropertyFloorPlanImage",
      headers: {'Content-Type': 'multipart/form-data'}}
    ]
  }
  
