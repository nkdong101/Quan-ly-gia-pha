import Vue from 'vue';
import API from '~/assets/scripts/API';
import GetDataAPI from '~/assets/scripts/GetDataAPI';
import StoreManager from '~/assets/scripts/StoreManager';

export const authAPI = {
  Callback: '/Auth/Callback',
  GetUserInfo: '/Auth/GetUserInfo',
  Logout: '/Auth/Logout',
}

export const $auth = {
  dongho_watching: null,
  user: null,
  access_token: null,
  identity: null,
  init(user) {
    this.access_token = localStorage.access_token;
    this.identity = localStorage.identity;
    this.dongho_watching = localStorage.dongho_watching;

    StoreManager.SetHeaders({
      Authorization: `Bearer ${this.access_token}`,
      Identity: this.identity,
      SourceAuth: location.origin,
    });
    let _user = JSON.parse(user);
    StoreManager.SetUser(_user)

    // if (!this.user)
    //   return this.userinfo();
    if (_user) {
      this.user = _user;
    } else {
      return this.userinfo();
    }

  },
  // processCallback(code) {
  //   return new Promise(rs => {
  //     GetDataAPI({
  //       method: 'post',
  //       url: authAPI.Callback,
  //       params: {
  //         code: code,
  //       },
  //       action: (access_token) => {
  //         this.access_token = access_token;
  //         this.userinfo().then((result) => {
  //           rs();
  //         });
  //       },
  //     });

  //   })
  // },
  userinfo() {
    return new Promise(rs => {
      GetDataAPI({
        method: 'post',
        url: API.GetUserInfo,
        action: re => {
          this.user = re;
          if (re.access_token)
            this.access_token = re.access_token;
          StoreManager.SetHeaders({
            Authorization: `Bearer ${re.access_token || this.access_token}`,
            Identity: this.identity,
            SourceAuth: location.origin
          });
          rs(re);
        }
      })
    });
  },
  logout() {
    return new Promise(rs => {
      GetDataAPI({
        method: 'post',
        params: {
          code: this.access_token
        },
        url: API.Logout,
        action: re => {
          localStorage.clear();
          this.access_token = '';
          this.identity = '';
          this.user = null;
          rs();
        },
        error() {
          localStorage.clear();
          this.access_token = '';
          this.identity = '';
          this.user = null;
          rs();
        }
      })

    })
  }
}

Vue.prototype.$auth = $auth;
