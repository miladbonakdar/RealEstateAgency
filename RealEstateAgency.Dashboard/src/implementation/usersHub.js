import {HubConnectionBuilder, LogLevel} from '@aspnet/signalr';
import {usersHubRoute} from "./routeProvider"
//https://www.dotnetcurry.com/aspnet-core/1480/aspnet-core-vuejs-signalr-app
//https://github.com/DaniJG/so-signalr
export default {
  install(Vue) {
    const usersHub = new Vue();
    Vue.prototype.$usersHub = usersHub;

    // Provide methods to connect/disconnect from the SignalR hub
    let connection = null;
    let startedPromise = null;
    let manuallyClosed = false;

    Vue.prototype.startSignalR = (jwtToken) => {
      connection = new HubConnectionBuilder()
        .withUrl(
          usersHubRoute,
          jwtToken ? {accessTokenFactory: () => jwtToken} : null
        )
        .configureLogging(LogLevel.Information)
        .build();

      // Forward hub events through the event, so we can listen for them in the Vue components
      connection.on('user_event', (event) => {
        usersHub.$emit('user_event', event);
      });

      connection.on('update_signal', (event) => {
        usersHub.$emit('update_signal', event);
        usersHub.$emit(event.entityCode + '_update_signal', event);
      });

      function start() {
        startedPromise = connection.start()
          .catch(err => {
            console.error('Failed to connect with hub', err);
            return new Promise((resolve, reject) => setTimeout(() => start().then(resolve).catch(reject), 5000))
          });
        return startedPromise
      }

      connection.onclose(() => {
        if (!manuallyClosed) start()
      });

      // Start everything
      manuallyClosed = false;
      start();
    };
    Vue.prototype.stopSignalR = () => {
      if (!startedPromise) return;

      manuallyClosed = true;
      return startedPromise
        .then(() => connection.stop())
        .then(() => {
          startedPromise = null
        })
    };

  }
}
