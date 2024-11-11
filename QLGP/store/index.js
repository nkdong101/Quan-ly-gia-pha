import { LoginResult } from "~/assets/scripts/objects/LoginResult";
import StoreManager from "~/assets/scripts/StoreManager";

function createStoreManager() {
  return store => {
    StoreManager.Init(store);
  };
}

export const plugins = [createStoreManager()];

export const state = () => ({
  count: 0,
  loadingCount: 0,
  preventClick: false,
  isFullScreen: false,

  banner_title: '',
  user: null,
  headers: null,
  menu: [],
  rawMenu: [],

});


export const mutations = {
  SetBannerTitle(state, title) {
    state.banner_title = title;
  },

  showLoading(state, isShow) {
    if (isShow == false) {
      state.loadingCount -= 1;
    } else {
      state.loadingCount += 1;
    }
  },
  SetUser(state, user) {
    state.user = user;
  },
  SetHeaders(state, headers) {
    state.headers = headers;
  },
  InitMenu(state, menu) {
    state.menu = menu;
  },
  InitRawMenu(state, menu) {
    state.rawMenu = menu;
  },
}
