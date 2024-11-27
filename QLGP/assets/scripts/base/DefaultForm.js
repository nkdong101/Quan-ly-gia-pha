// @ts-check
import BaseWidget, {
  BaseWidgetDefaultParams
} from "./BaseWidget";

var defaultParams = function () {
  return {
    obj: {},
    visible: false,
    fullscreen: false,
    modalAppendToBody: true, 
    appendtobody: true,
    noSave: false,
    width: '500px',
    OKtext: "Lưu",
    Canceltext: "Thoát",
    type: "",
    title: "",
    control: null,
    action: null,
    btns: [],
    ...BaseWidgetDefaultParams()
  };
};
export default class DefaultForm extends BaseWidget {
  /** @type {*} - obj reactive form */
  obj;
  /** @type {Boolean} - Trạng thái ẩn hiện form */
  visible;
  /** @type {Boolean} - Trạng thái ẩn hiện form */
  modalAppendToBody;
  /** @type {Boolean} - Trạng thái ẩn hiện form */
  appendtobody;
  /** @type {Boolean} - Trạng thái full màn hình */
  fullscreen;
    /** @type {Boolean} - Trạng thái full màn hình */
  noSave
  /** @type {String} - Độ rộng form */
  width;
  /** @type {String} - Text của nút action */
  OKtext;
  /** @type {String} - Text của nút hủy */
  Canceltext;
  /** @type {String} - Loại form (nếu là dialog thì ko hiển thị nút, còn không cứ để trống) */
  type;
  /** @type {String} - Tiêu đề form */
  title;
  /** @type {Function} - các trường trong form return FormContainer*/
  control;
  /** @type {Function} - action khi ấn nút */
  action;
  /** @type {Array} - adds more button */
  btns;

  constructor(obj = defaultParams()) {
    super(obj);
    // //console.log(Object.keys(obj));
    // //console.log('1.....' + JSON.stringify(this) + '.....' + JSON.stringify(obj));
    Object.assign(this, defaultParams(), obj);
    // //console.log('2.....' + JSON.stringify(this) + '.....' + JSON.stringify(obj));
    // if (this.action) {
    //   //console.log('action' + JSON.stringify(this));
    // }
    // if (this.action) {
    //   //console.log('action');
    // }
    this.events.actionOK = this.action;
    // //console.log('3.....' + JSON.stringify(this) + '.....' + JSON.stringify(obj));
  }
}
