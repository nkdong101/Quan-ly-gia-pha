import { $auth } from "~/plugins/auth";
import { LoginResult } from "./objects/LoginResult";

export default {
  store: null,
  events: {},
  Init: function (store) {
    this.store = store;
    // alert('123
  },
  ShowLoading: function (isShow) {
    var _app = this;
    // //console.log(state, mutations);
    // mutations.showLoading(state(),isShow);
    if (isShow === false) {
      // setTimeout(function () {
      _app.store.commit("showLoading", isShow);
      // }, 5000);
    } else this.store.commit("showLoading", isShow);
  },
  IsShowLoading: function () {
    return this.store.state.loadingCount > 0;
  },
  GetBannerTitle() {
    return this.store.state.banner_title;
  },
  SetBannerTitle(title) {
    this.store.commit("SetBannerTitle", title);
  },


  GetUserLevel() {
    return this.store.state.user.UserLevel;
  },
  GetHeaders() {
    return this.store.state.headers;
  },
  SetHeaders(headers) {
    this.store.commit("SetHeaders", headers);
    localStorage.access_token = $auth.access_token;
    localStorage.identity = $auth.identity;
  },
  /**
   * @return {LoginResult} description
   */
  GetUser() {
    return this.store.state.user
  },
  SetUser(user) {
    this.store.commit("SetUser", user);
  },
  InitMenu(menu) {
    this.store.commit("InitMenu", menu);
  },

  GetMenu(){
    return this.store.state.menu;
  },


  //pr_form
  Getcurrency_code() {
    return this.store.state.pr_form.currency_code;
  },
  Setcurrency_code(currency_code) {
    this.store.commit("pr_form/Setcurrency_code", currency_code);
  },
  GetMenuButton() {
    return this.store.state.pr_form.btns;
  },
  SetMenuButton(btns) {
    this.store.commit("pr_form/SetMenuButton", btns);
  },
}
