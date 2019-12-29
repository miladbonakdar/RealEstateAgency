export default {
  install: function (Vue, myGate, name = "$gate") {
    Object.defineProperty(Vue.prototype, name, {value: myGate});
  }
};
