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
    Title;
    /** @type {number} - description */
    Office_id;
    /** @type {string} - description */
    UserLevel_id;
    /** @type {number} - description */
    Country_id;
    /** @type {number} - description */
    UserStatus;
    /** @type {string} - description */
    SignatureImage;
    /** @type {object} - description */
    Buttons;


    _formElements = {
        FullName: new FormElement({
            label: "Full Name",
            model: "FullName",
            type: FormElementType.text,
        }),
        Email: new FormElement({
            label: "Email",
            model: "Email",
            type: FormElementType.text,
        }),
        Title: new FormElement({
            label: "Title",
            model: "Title",
            type: FormElementType.text,
        }),
        Office_id: new FormElement({
            label: "Office",
            model: "Office_id",
            type: FormElementType.select,
            options: Para.Para_Office,
        }),
        UserLevel_id: new FormElement({
            label: "Level",
            model: "UserLevel_id",
            type: FormElementType.select,
            options: Para.GroupPermission,
        }),
        Country_id: new FormElement({
            label: "Country",
            model: "Country_id",
            type: FormElementType.text,
        }),
        UserStatus: new FormElement({
            label: "Status",
            model: "UserStatus",
            type: FormElementType.select,
            options: Para.Para_Use,
        }),
        SignatureImage: new FormElement({
            label: "SignatureImage",
            model: "SignatureImage",
            type: FormElementType.text,
        }),
        Buttons: new FormElement({
            label: "Buttons",
            model: "Buttons",
            type: FormElementType.text,
        }),
        Name_Excel: new FormElement({
            label: "Name Excel",
            model: "Name_Excel",
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
                this._formElements.Email,
                this._formElements.Title,
                this._formElements.Office_id,
                this._formElements.UserLevel_id,
                this._formElements.Country_id,
                this._formElements.UserStatus,
                this._formElements.Name_Excel,

            ]
        });
    }
}