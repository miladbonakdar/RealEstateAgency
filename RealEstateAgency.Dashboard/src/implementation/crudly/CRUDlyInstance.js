import CRUDly from 'crudly';
import config from './crudlyConfig'

let myGate = new CRUDly(config);

myGate.afterEach(response => {
  if (!response.ok)
    console.error(response);
  if (response.status == 401) {
    localStorage.removeItem("token");
    localStorage.removeItem("user");
    localStorage.removeItem("loggedOn");
    document.location.href = "/";
  }
  return response;
});

myGate.beforeEach(request => {
  let accessToken = localStorage.getItem("token");
  if (accessToken) request.setProperty("headers",
    {
      Authorization: "bearer " + accessToken
    });
});

export default myGate;
