import moment from "moment";

export default class UserAuth {
  /**
   * @type {string} type - description
  */
  email;
  /**
   * @type {*} type - description
  */
  userInfo;
  /**
   * @type {string} type - description
  */
  refresh_token;
  /**
   * @type {Array<UserAuthToken>} type - description
  */
  tokens = [];



  /**
  *
  * @param {UserAuth} obj
  */
  constructor(obj) {
    this.update(obj);
  }
  /**
   *
   * @param {UserAuth} obj
   */
  update(obj) {
    Object.assign(this, obj);
  }
}

export class UserAuthToken {
  /**
   * @type {string} type - description
  */
  access_token;
  /**
   * @type {string} type - Code khi login
  */
  code;
  /**
   * @type {number} type - description
  */
  expiry_date;
  /**
   * @type {string} type - Code khi login
  */
  expiry_date_str;


  /**
  *
  * @param {UserAuthToken} obj
  */
  constructor(obj) {
    this.update(obj);
  }
  /**
   *
   * @param {UserAuthToken} obj
   */
  update(obj) {
    Object.assign(this, obj);
    this.expiry_date_str = moment(this.expiry_date).format('DD/MM/YYYY hh:mm:ss');
  }
}
