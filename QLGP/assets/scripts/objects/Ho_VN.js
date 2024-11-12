import { FormElement, FormElementType, FormInfo } from "~/assets/scripts/base/FormInfo";
import { Para } from "../Para";
import { SelectOption } from "../base/SelectOption";
import API from "../API";
export default class Ho_VN {
    /** @type {number} - description */
    Total;
    /** @type {string} - description */
    Name;
    /** @type {number} - description */
    DateUpdate;
    /** @type {number} - description */
    DateCreate;
    /** @type {number} - description */
    UserCreate;
    /** @type {number} - description */
    UserUpdate;
    DS_Tinh = ''


    _formElements = {
        Total: new FormElement({
            label: "Số lượng",
            model: "Total",
            type: FormElementType.number,
            disabled: true,
            // options: Para.Level,
        }),
        Name: new FormElement({
            label: "Tên họ",
            model: "Name",
            type: FormElementType.text,
            // disabled: true,
        }),

        DS_Tinh: new FormElement({
            label: "Tỉnh",
            model: "DS_Tinh",
            type: FormElementType.select,
            options: Para.Donvihanhchinh,
            disabled: true,


            // labelWidth: 140,
        }),
        Id: new FormElement({
            label: "Id",
            model: "Id",
            type: FormElementType.text,
        }),


    };

    /**
     *
     * @param {Ho_VN} obj
     */
    constructor(obj) {
        this.update(obj);
    }
    /**
     *
     * @param {Ho_VN} obj
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
            labelWidth: 100,
            formData: this,
            elements: [

                this._formElements.Name,
                this._formElements.Total,
                this._formElements.DS_Tinh,

            ]
        });
    }
}