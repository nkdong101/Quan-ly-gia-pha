import Vue from 'vue'
import { getFileObj, resolve, SearchTree } from '~/assets/scripts/Functions'
import Mask from '~/assets/scripts/Mask'
import { Para } from '~/assets/scripts/Para'
import StoreManager from '~/assets/scripts/StoreManager'
const requireComponent = require.context('@/components', true, /\.vue$/)

import MaskedInput from "vue-text-mask";
import ConvertStr from '~/assets/scripts/ConvertStr'
import { LoginResult } from '~/assets/scripts/objects/LoginResult'
import { InputFileAccept } from '~/components/form-content/InputFile/InputFIleModel'
// import { linkPurchase } from '~/assets/scripts/API'
Vue.component("masked-input", MaskedInput);

requireComponent.keys().forEach(fileName => {

  const componentConfig = requireComponent(fileName)

  const componentName = fileName
    .replace(/^\.\//, '')
    .replace(/\.\w+$/, '')
    .replace(/\//g, '-')
  // .toLowerCase()
  var componentNames = componentName.split('-');

  Vue.component(componentNames[componentNames.length - 1], componentConfig.default || componentConfig)
})

Vue.directive("focusNextOnEnter", {
  inserted(el, binding, vnode) {
    el.addEventListener("keydown", ev => {
      let queryNotHidden =
        ':not([style*="display:none"]):not([style*="display: none"]):not([disabled])';
      let allInput = [
        ...vnode.elm.querySelectorAll(
          `input${queryNotHidden}:not([type=file]),select${queryNotHidden},textarea${queryNotHidden}`
        )
      ];
      if (!ev.target.hasAttribute("nonFocusNextOnEnter")) {
        if (ev.keyCode === 13 && !ev.shiftKey) {
          let find = allInput.indexOf(ev.target);
          if (allInput[find] && allInput[find].parentElement.parentElement.querySelector('.el-select-dropdown')) {
            if (ev.path[2].__vue__ && ev.path[2].__vue__.blur) {
              ev.path[2].__vue__.blur();
            }
          }
          if (find < allInput.length - 1 && find > -1) {
            allInput[find + 1].focus();
            allInput[find + 1].select();
          }
        } else {
          if (ev.shiftKey && ev.target.tagName == "TEXTAREA") {
            //   ev.target.value += "\n";
            //   vnode.children[0].context.obj.Info += "\n";
          }
        }
      }
    });
  }
});
Vue.directive("focusFirstOnLoad", {
  inserted(el, binding, vnode) {
    let queryNotHidden =
      ':not([style*="display:none"]):not([style*="display: none"]):not([disabled])';
    let allInput = [
      ...vnode.elm.querySelectorAll(
        `div:not([nonFocusFirstOnLoad]) > input${queryNotHidden},div:not([nonFocusFirstOnLoad]) > select${queryNotHidden},div:not([nonFocusFirstOnLoad]) > textarea${queryNotHidden}`
      )
    ];
    if (allInput[0]) allInput[0].focus();
  }
});


Vue.mixin({
  data() {
    return {
      MaskCfg: Mask,
      Para: Para,
      ConvertStr: ConvertStr,
      InputFileAccept: InputFileAccept,
      Dongho_watching: '',
      // linkPurchase: linkPurchase,
    };
  },
  computed: {
    /**
     * @return {LoginResult} description
     */
    user() {
      return StoreManager.GetUser();
    },
    
    currentMenu() {
      let feature = this.$route.path
      let find = SearchTree(StoreManager.store.state.rawMenu, feature, "path");
      return find || {};
    },
    pagePermission() {
      let per = (this.AllPermission() || []).reduce((a, b) => {
        var key = b.banner;
        switch (key) {
          case 'Xem':
            key = "view"
            break;
          case 'Add':
            key = "add"
            break;
          case 'Edit':
            key = "edit"
            break;
          case 'Delete':
            key = "delete"
            break;

          default:
            break;
        }
        a[key] = b;
        a[b.banner] = b;
        return a;
      }, {});
      // console.log(per);
      return per;
      // return {
      //     view: this.CheckPermission("Xem"),
      //     add: this.CheckPermission("Add"),
      //     edit: this.CheckPermission("Edit"),
      //     delete: this.CheckPermission("Delete"),
      // };
    },
  },
  methods: {
    // getPRFileAttach(filePath, isPrLink) {
    //   if (isPrLink)
    //     return linkPurchase + filePath;
    //   return filePath;

    // },
    resolve: resolve,
    getFilePath(str) {
      str = str || '';
      return getFileObj(str).file
      // return (str.split('|')[0] || '').replace(/\?/g, ",");
    },
    getFileDownload(str) {
      str = str || '';
      return getFileObj(str).fileDownload
      // return (str.split('|')[0] || '').replace(/\?/g, ",");
    },
    CheckPermission(action, feature) {
      feature = feature || this.$route.path
      let find = SearchTree(StoreManager.store.state.rawMenu, feature, "path");
      if (find) {
        return (find.permission || []).filter(p => p.Action == action)[0];
      }

      return null;
      //   //console.log(StoreManager.store.state.menu);
    },
    AllPermission(feature) {
      feature = feature || this.$route.path
      let find = SearchTree(StoreManager.store.state.rawMenu, feature, "path");
      if (find) {
        return find.permission || [];
      }

      return [];
    }
  },

});



Object.equals = function (x, y) {
  if (x === y) return true;
  // if both x and y are null or undefined and exactly the same

  if (!(x instanceof Object) || !(y instanceof Object)) return false;
  // if they are not strictly equal, they both need to be Objects

  if (x.constructor !== y.constructor) return false;
  // they must have the exact same prototype chain, the closest we can do is
  // test there constructor.

  for (var p in x) {
    if (!x.hasOwnProperty(p)) continue;
    // other properties were tested using x.constructor === y.constructor

    if (!y.hasOwnProperty(p)) return false;
    // allows to compare x[ p ] and y[ p ] when set to undefined

    if (x[p] === y[p]) continue;
    // if they have the same strict value or identity then they are equal

    if (typeof x[p] !== "object") return false;
    // Numbers, Strings, Functions, Booleans must be strictly equal

    if (!Object.equals(x[p], y[p])) return false;
    // Objects and Arrays must be tested recursively
  }

  for (p in y) {
    if (y.hasOwnProperty(p) && !x.hasOwnProperty(p)) return false;
    // allows x[ p ] to be set to undefined
  }
  return true;
};
