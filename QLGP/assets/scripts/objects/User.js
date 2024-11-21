import { FormElement, FormElementType, FormInfo } from "~/assets/scripts/base/FormInfo";
import API from "../API";
import { SelectOption } from "../base/SelectOption";
import { Para } from "../Para";
export default class User {
    /** @type {string} - description */
    Id;
    /** @type {string} - description */
    FullName;
    /** @type {string} - description */
    Email;
    /** @type {string} - description */
    Address;
    /** @type {string} - description */
    CMND;
    /** @type {string} - description */
    userLevel;
    /** @type {string} - description */
    BirthDay;
    /** @type {string} - description */
    Phone;
    /** @type {Number} - description */
    Gender;
    /** @type {string} - description */
    Phone;
    /** @type {string} - description */
    Images;
    /** @type {object} - description */
    Buttons;


    _formElements = {
        FullName: new FormElement({
            label: "Họ tên",
            model: "FullName",
            type: FormElementType.text,
        }),
        Email: new FormElement({
            label: "Email",
            model: "Email",
            type: FormElementType.text,
        }),
        BirthDay: new FormElement({
            label: "Ngày sinh",
            model: "BirthDay",
            type: FormElementType.datePicker,
        }),
        Gender: new FormElement({
            label: "Giới tính",
            model: "Gender",
            type: FormElementType.select,
            options: Para.Gender,
        }),
        Address: new FormElement({
            label: "Địa chỉ",
            model: "Address",
            type: FormElementType.text,
            // options: Para.GroupPermission,
        }),
        userLevel: new FormElement({
            label: "Loại tài khoản",
            model: "userLevel",
            type: FormElementType.select,

            options: Para.GroupPermission,
        }),
        UserStatus: new FormElement({
            label: "Status",
            model: "UserStatus",
            type: FormElementType.select,
            options: Para.Para_Use,
        }),
        Images: new FormElement({
            label: "Ảnh đại diện",
            model: "Images",
            type: FormElementType.text,
        }),
        Phone: new FormElement({
            label: "Số điện thoại",
            model: "Phone",
            type: FormElementType.text,
        }),
        CMND: new FormElement({
            label: "CMND/CCCD",
            model: "CMND",
            type: FormElementType.text,
        }),

    };

    /**
     *
     * @param {User} obj
     */
    constructor(obj) {
        this.update(obj);
    }
    /**
     *
     * @param {User} obj
     */
    update(obj) {
        Object.assign(this, obj);
    }
    toJSON() {
        return {
            ...this,
            _formElements: undefined,
        }
    }
    form() {
        return new FormInfo({
            formData: this,
            elements: [
                this._formElements.FullName,
                this._formElements.BirthDay,
                this._formElements.Phone,
                this._formElements.Gender,
                this._formElements.Email,
                this._formElements.CMND,
                this._formElements.Address,
                this._formElements.userLevel,
                // this._formElements.Country_id,
                // this._formElements.UserStatus,
                // this._formElements.Name_Excel,

            ]
        });
    }
}