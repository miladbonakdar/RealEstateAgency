import Vue from 'vue';
import SubmitGroup from "../components/SubmitGroup";
import MultiSelect from 'vue-multiselect'
import CrudActions from '../components/CrudActions';
import UpdateActions from '../components/UpdateActions';
import { Datetime } from 'vue-datetime';


Vue.component('datetime', Datetime);
Vue.component(SubmitGroup.name, SubmitGroup);

Vue.component(CrudActions.name, CrudActions);
Vue.component(UpdateActions.name, UpdateActions);



//https://vue-multiselect.js.org/#sub-getting-started
Vue.component('multiSelect', MultiSelect);
//https://www.npmjs.com/package/vuejs-datepicker

Date.prototype.toLocalISOString = function () {
  let tzOffset = (new Date()).getTimezoneOffset() * 60000; //offset in milliseconds
  return (new Date(Date.now() - tzOffset)).toISOString().slice(0, -1);
};
