export default {
    name: "request",
    actions: [
      {type: "post", url: "/getOpenRequests", name: "getOpenRequests"},
      {type: "post", url: "/changeRequestAgent", name: "changeRequestAgent"},
    ]
  }
