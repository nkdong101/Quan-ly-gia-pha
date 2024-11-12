import { ServerAPI } from "./API";
import { ShowMessage } from "./Functions";
import StoreManager from "./StoreManager";
const axios = require("axios");
const qs = require("qs");

// Format nested params correctly
axios.interceptors.request.use(config => {
  //   window.console.log(config);

  config.paramsSerializer = params => {
    // Qs is already included in the Axios package
    return qs.stringify(params, {
      arrayFormat: "index",
      encode: true
    });
  };

  return config;
});

/**
 * Call HttpRequest
 * @param {Object} options - Đầu vào.
 * @param {'get'|'post'|'put'|'delete'} options.method - Phương thức call default get.
 * @param {Object} options.params - Tham số truyền đi.
 * @param {Object} options.headers - Tham số truyền đi.
 * @param {string} options.url - Đường link call API.
 * @param {Function} options.action - callback khi call API thành công.
 * @param {Function} options.error - callback khi call API false.
 * @param {AbortController} options.loadDataController - callback khi call API false.
 * @param {Boolean} options.isWait - có show loading không default true.
 */
export default function ({
  method,
  params = {},
  url,
  action,
  error,
  loadDataController,
  headers,
  isWait = true
}) {
  try {
    if (isWait) StoreManager.ShowLoading();
    var option = {};
    let _headers = {};
    if (StoreManager.GetHeaders()) {
      _headers = StoreManager.GetHeaders()
    }
    Object.assign(_headers, headers)
    method = method || "get";
    if (params) {
      if (params.toJSON) {
        params = params.toJSON();
      }

      Object.keys(params).forEach(p => {
        if (p.indexOf("_") == 0) {
          params[p] = undefined;
        }
      });
    }
    if (method != "get") {
      option.data = params;
    } else {
      option.params = params;
    }
    if (loadDataController) {
      // option.signal = loadDataController.signal
      option.cancelToken = loadDataController.token
    }
    if (!url) throw "Link API Không tồn tại";
    // alert(url)
    return axios({
      url: url,
      method: method,
      headers: _headers,
      ...option
    })
      .then(function (re) {
        try {
          var data = re.data;
          // if (re.request.responseURL.indexOf(':54963/') > -1) {

          if (data.Code == 1 || (url == ServerAPI.APISaveFileToServer && data.Status == "OK")) {
            if (action) {
              action(data.Data);
            }
          } 
          else {
            if (data.Code == 3) {
              if (!window.isApp) {
                // localStorage.clear();
                localStorage.err_message = data.ErrMessage;
                location.href = "/Account/Home";
              }
              return;
            }
            if (error) {
              error(data.ErrMessage, data);
            } else {
              ShowMessage(data.ErrMessage, "error");
            }
          }
          // } else {
          //     if (action) {
          //         action(data);
          //     }
          // }
        } catch (error) {
          console.error(error);
        }
      })
      .catch(function (err) {
        if (err != "Cancel") {
          var data = (err.response || {}).data;
          var message = "";
          if (typeof data == "string") message = data;
          else message = JSON.stringify(data);
          if (error) {
            error(message);
          } else {
            ShowMessage(message, "error");
          }
        }
      })
      .then(function () {
        if (isWait) StoreManager.ShowLoading(false);
      });
  } catch (err) {
    ShowMessage(err, "error");
    if (isWait) StoreManager.ShowLoading(false);
    return null;
  }
}

