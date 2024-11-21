import { FormElement, FormElementType, FormInfo } from "~/assets/scripts/base/FormInfo";
import { Para } from "../Para";
import { SelectOption } from "../base/SelectOption";
import API from "../API";
export default class Ho_VN {
    /** @type {string} - description */
    Title;
    /** @type {string} - description */
    Content;
    /** @type {string} - description */
    DateBegin;
    /** @type {string} - description */
    DateEnd;
    /** @type {string} - description */
    UserCreate;
    /** @type {string} - description */
    UserUpdate;
    
    Files = [];


    _formElements = {
        Content: new FormElement({
            label: "Nội dung tổ chức",
            model: "Content",
            type: FormElementType.text,
            attr:{
                type: 'textarea',
            }
            // disabled: true,
            // options: Para.Level,
        }),
        Title: new FormElement({
            label: "Tên sự kiện",
            model: "Title",
            type: FormElementType.text,
            // disabled: true,
        }),

        DateBegin: new FormElement({
            label: "Ngày bắt đầu",
            model: "DateBegin",
            type: FormElementType.datePicker,



            // labelWidth: 140,
        }),
        DateEnd: new FormElement({
            label: "Ngày kết thúc",
            model: "DateEnd",
            type: FormElementType.datePicker,
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

                this._formElements.Title,
                this._formElements.Content,
                this._formElements.DateBegin,
                this._formElements.DateEnd,

            ]
        });
    }
}