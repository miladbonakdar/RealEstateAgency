export default{
    name: "agent",
    actions: [
      {type: "post", url: "/createAgent", name: "createAgent"},
      {type: "get", url: "/getAgents", name: "getAgents"},
      {type: "get", url: "/getAgentForUpdate/:agentId", name: "getAgentForUpdate"},
      {type: "put", url: "/updateAgent", name: "updateAgent"}
    ]
  }
  