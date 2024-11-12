import { FormDirectionType, FormElement, FormElementType, FormInfo } from "~/assets/scripts/base/FormInfo";
import API from "../API";
import { SelectOption } from "../base/SelectOption";
import { Para } from "../Para";
export default class user_infor {
  /** @type {string} - description */
  UserName;
  /** @type {string} - description */
  Password;
  /** @type {string} - description */
  FullName;
  /** @type {string} - description */
  Images;
  /** @type {string} - description */
  Address;
  /** @type {string} - description */
  Address;
  /** @type {string} - description */
  CMND;
  /** @type {string} - description */
  BirthDay;
  /** @type {string} - description */
  Phone;
  /** @type {string} - description */
  Gender;
  /** @type {string} - description */
  Email;
  /** @type {number} - description */
  Dongho_id;
  /** @type {string} - description */
  Dongho_Name;
  /** @type {string} - description */
  Buttons;
  GroupPermission_Id;

  _formElements = {

    // Project_Code: new FormElement({
    //   label: "Project Code",
    //   model: "Project_Code",
    //   type: FormElementType.select,
    //   options: new SelectOption({
    //     data: API.user_infor_Get_Para,
    //     allowCreate: true
    //   })
    // }),
    UserName: new FormElement({
      label: "Tên đăng nhập",
      model: "UserName",
      type: FormElementType.text,
      required: true,

    }),
    Email: new FormElement({
      label: "Email",
      model: "Email",
      type: FormElementType.text,
      required: true,

    }),
    Password: new FormElement({
      label: "Mật khẩu",
      model: "Password",
      type: FormElementType.Password,
      required: true,

    }),
    FullName: new FormElement({
      label: "Tên đầy đủ",
      model: "FullName",
      type: FormElementType.text,
      required: true,

    }),
    Address: new FormElement({
      label: "Địa chỉ",
      model: "Address",
      type: FormElementType.text,
    }),
    BirthDay: new FormElement({
      label: "Ngày sinh",
      model: "BirthDay",
      type: FormElementType.datePicker,

    }),
    Gender: new FormElement({
      label: "Dongho_id",
      model: "Gender",
      type: FormElementType.select,
      options: Para.Gender,
      required: true,
    }),
    Dongho_id: new FormElement({
      label: "Dòng họ",
      model: "Dongho_id",
      type: FormElementType.select,
      options: Para.dong_ho,
      // required: true,
    }),
   
  };

  /**
   *
   * @param {user_infor} obj
   */
  constructor(obj) {
    this.update(obj);
  }
  /**
   *
   * @param {user_infor} obj
   */
  update(obj) {
    Object.assign(this, obj);
  }
  toJSON() {
    return {
      ...this,
      _formElements: undefined,
      Details: undefined,
    }
  }
  form() {
    return new FormInfo({
      formData: this,
      elements: [
        // this._formElements.DateCreate,
        this._formElements.UserName,
        this._formElements.Password,,
        this._formElements.FullName,,
        this._formElements.Address,,
        this._formElements.Dongho_id,,
        this._formElements.Gender,,

      
      ]
    });
  }
}
