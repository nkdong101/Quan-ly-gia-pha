import {
  Global
} from "../Functions";
import BaseWidget, { BaseWidgetDefaultParams } from "./BaseWidget";


export class FormInfo {
  constructor({
    labelTop = false,
    formData = null,
    labelWidth = undefined,
    elements = [],

  }) {
    this.labelTop = labelTop;
    this.formData = formData;
    this.labelWidth = labelWidth;
    this.elements = elements;
  }
  /**
   * @param {FormElement} element - description.
   */
  getAllSlotByElement(element) {
    let slots = [element.id];
    if (element.child && element.child.length)
      slots = slots.concat(element.child.reduce(getAllChild, []));
    return slots;
    //   return this.elements
  }
}

var defaultParams = function () {
  return {
    label: "",
    class: "",
    type: FormElementType.text,
    model: "",
    child: null,
    col: 24,
    labelWidth: undefined,
    disabled: null,
    forceDisabled: null,
    isVisible: null,
    required: false,
    validate: function (data, $this = {
      getElement: function (id) { return new FormElement() },
      getEntry: function (id) { return null; },
      formInfo: {
        vm: {
          getElement: function (id) { return { model: new FormElement() } },
          getEntry: function (id) { },
        }
      }
    }) {

      return '';
    },
    watch: function (data, newValue, oldValue, $this = {
      getElement: function (id) { return new FormElement() },
      getEntry: function (id) { return null; },
      onChange: function () { },
      formInfo: {
        vm: {
          getElement: function (id) { return { model: new FormElement() } },
          getEntry: function (id) { },
        }
      }
    }, isFirst) { },
    direction: FormDirectionType.horizontal,
    options: null,
    ...BaseWidgetDefaultParams()
  };
};

export var FormElementType = {
  text: "text", // Default. Defines a single-line text field
  checkbox: "checkbox", // checkbox
  select: "select", // select from combobox
  number: "number", // input số
  file: "file", // input số
  date: "date", // input date
  label: "label", // nhãn form
  datePicker: "datePicker", // input date
  dateTimePicker: "dateTimePicker", // input date time
  //   color: "color", // Defines a color picker
  //   file: "file", // Defines a file-select field and a "Browse" button (for file uploads)
  //   number: "number", // Defines a field for entering a number
  //   password: "password" // Defines a password field
  quillEditor: "QEditor", // Quill editor (html editor)
};
export var FormDirectionType = {
  vertical: "vertical",
  horizontal: "horizontal"
};
export class FormElement extends BaseWidget {
  /** @type {String} - nhãn input */
  label;
  /** @type {String} - nhãn input */
  class;




  /** @type {String} - loại input */
  type;

  /** @type {String} - v-model */
  model;

  /** @type {Number} - độ rộng input (1-24) */
  col;

  /** @type {Number} - độ rộng label (1-24) */
  labelWidth;

  /** @type {Object} - tùy biến disabled input */
  disabled;
  forceDisabled;

  /** @type {Object} - ẩn hiện input */
  isVisible;
  /** @type {Boolean} - bắt nhập */
  required;

  /** @type {Function} - method watch element function(data,val,oldVal, vueModel) */
  validate;
  /** @type {Function} - method watch element function(data,val,oldVal, vueModel) */
  watch;

  /** @type {Object} - tùy biến */
  options;

  // Multi form

  /** @type {Array} - input con dùng khi muốn gộp nhiều input vào làm 1 */
  child;

  /** @type {String} - hướng render input con (dọc/ngang)*/
  direction;

  constructor(obj = defaultParams()) {
    super(obj);
    Object.assign(this, defaultParams(), obj);

  }

  set(setFunction = function (e = defaultParams()) { }) {
    let data = new FormElement(this);
    setFunction(data);
    return data;
  }
  getAllSlotChild() {
    let results = [this.id];
    if (this.child && this.child.length) {
      results = results.concat(this.child.reduce(getAllChild, []));
    }
    // //console.log(this.id, results);
    return results;
  }
}

function getAllChild(results, element) {
  if (element != null) {

    results.push(element.id);
    if (element.child && element.child.length) {
      element.child.forEach(function (p) {
        getAllChild(results, p);
      });
    }
  }
  return results;
}
