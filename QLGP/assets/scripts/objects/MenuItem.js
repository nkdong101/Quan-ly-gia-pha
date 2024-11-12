export default class MenuItem {
  /**
   * @type {string} type - description
  */
  index;
  /**
   * @type {string} type - description
  */
  title;
  /**
   * @type {string} type - description
  */
  icon;
  image;
  note;
  type_id;
  /**
   * @type {string} type - description
  */
  path;
  /**
   * @type {string} type - description
  */
  banner;
  /**
   * @type {()=>string)} type - description
  */
  formatter;
  /**
   * @type {()=>bool)} type - description
  */
  visible;
  /**
   * @type {()=>void)} type - description
  */
  action;
  /**
   * @type {Array<MenuItem>} type - description
  */
  children = [];
  /**
   * @type {Array<MenuItem>} type - description
  */
  permission = [];



  /**
   * @param {MenuItem} obj - description
  */
  constructor(obj) {
    this.update(obj);
  }

  /**
    * @param {MenuItem} obj - description
   */
  update(obj) {
    Object.assign(this, obj);
  }
}
