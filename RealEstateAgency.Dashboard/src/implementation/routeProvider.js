export const [baseRoute, apiRoute, usersHubRoute] = [
  process.env.VUE_APP_SERVER_BASE_URI,
  process.env.VUE_APP_SERVER_BASE_URI + process.env.VUE_APP_API_URL,
  process.env.VUE_APP_SERVER_BASE_URI + process.env.VUE_APP_SIGNALR_URL
]
