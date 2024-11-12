import { GetGlobalId } from "../Functions";


/**
 * hàm xử lý các tác vụ bất đồng bộ có thể bị gọi đè nhau, đảm bảo không bị gọi đè trong thời gian nhất định
 */
export default class DelayCall {
  /** @type {number} - time delay mỗi khi bị gọi đè */
  timer = 250;
  /** @type {()=>void)} - hàm callback khi quá thời gian timer mà không bị gọi  */
  callback;

  /** @type {string} - Biến checking */
  _timeCheck;

  /**
   * Khởi tạo mặc định
   * @param {()=>void)} callback callback
   */
  constructor(callback) {
    this.callback = callback;
  }
  /**
  * Khởi tạo với timer
  * @param {number} timer thời gian delay (ms)
  * @param {()=>void)} callback callback
  */
  static WithTimer(timer, callback) {
    let _this = new DelayCall(callback);
    _this.timer = timer;
    return _this;
  }

  run() {
    let timeCheck = GetGlobalId();
    this._timeCheck = timeCheck;
    setTimeout(() => {
      if (this._timeCheck == timeCheck) {
        // console.log(timeCheck, this._timeCheck, this.timer)
        if (this.callback)
          this.callback();
      }
    }, this.timer);
  }
}
