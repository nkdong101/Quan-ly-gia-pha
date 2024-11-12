import { FormDirectionType, FormElement, FormElementType, FormInfo } from "~/assets/scripts/base/FormInfo";
import { Para } from "../Para";
import { SelectOption } from "../base/SelectOption";
import API from "../API";
import DatePickerOption from "../base/DatePickerOption";
import InputFileModel, { InputFileAccept, InputFileType } from "~/components/form-content/InputFile/InputFIleModel";
export default class Giapha {
    /** @type {number} - description */
    Dongho_id;
    /** @type {number} - description */
    HoVietNam_id;
    /** @type {string} - description */
    Name = '';
    /** @type {string} - description */
    Other_Name = '';
    /** @type {string} - description */
    Place_of_birth = '';
    /** @type {string} - description */
    Address = '';
    /** @type {string} - description */
    Phone = '';
    /** @type {string} - description */
    CCCD = '';
    /** @type {string} - description */
    Birthday = '';
    /** @type {number} - description */
    Index = '';
    /** @type {string} - description */
    Year_Of_Birth = '';
    /** @type {string} - description */
    Avatar = '';
    Birthday = ''
    /** @type {number} - description */
    Gender = '';
    /** @type {number} - description */
    State = 1;

    /** @type {string} - description */
    Date_of_death = '';
    /** @type {string} - description */
    burial_ground = '';
    /** @type {string} - description */
    Burial_GPS = '';
    // /** @type {string} - description */
    Parent = '';

    Childs = []
    siblings = []
    Couple;
    /** @type {number} - description */
    DateUpdate;
    /** @type {number} - description */
    DateCreate;
    /** @type {number} - description */
    UserCreate;
    /** @type {number} - description */
    UserUpdate;
    URL = []
    year_die = ''

    _formElements = {

        Name: new FormElement({
            label: 'Họ và tên',
            model: 'Name',
            type: 'text',

        }),
        burial_ground
        : new FormElement({
            label: 'Nơi chôn cất',
            model: 'burial_ground',
            type: 'text',
            isVisible(data){
                return data.State == 2
            }
        }),
        Other_Name: new FormElement({
            label: 'Tên khác',
            model: 'Other_Name',
            type: 'text',

        }),
        Place_of_birth: new FormElement({
            label: 'Nơi sinh',
            model: 'Place_of_birth',
            type: 'text',

        }),
        Address: new FormElement({
            label: 'Địa chỉ',
            model: 'Address',
            type: 'text',

        }),
        Phone: new FormElement({
            label: 'Số điện thoại',
            model: 'Phone',
            type: 'text',

        }),
        Birthday: new FormElement({
            label: 'Ngày sinh',
            // col: 18,
            model: 'Birthday',
            type: 'datePicker',
            attr: {
                format: 'dd/MM'
            },
            watch(data, n, o, t, isFirst) {
                if (!isFirst) {
                    if (data.Birthday) {
                        let ngaysinh = new Date(data.Birthday)

                        if (data.Year_Of_Birth != ngaysinh.getFullYear()) {

                            data.Year_Of_Birth = ngaysinh.getFullYear()
                        }
                    }



                }
            },
            options(data) {
                return new DatePickerOption({
                    shortcuts: null,
                })
            },

        }),
        CCCD: new FormElement({
            label: 'CCCD',
            model: 'CCCD',
            type: 'text',
            // labelWidth: 120,

        }),
        Index: new FormElement({
            label: 'Thứ tự',
            model: 'Index',
            type: 'text',
            labelWidth: 80

        }),
        Year_Of_Birth: new FormElement({
            // label: 'Năm sinh',
            model: 'Year_Of_Birth',
            type: FormElementType.text,
            attr: {
                placeholder: 'Năm'
            },


            events: {
                blur(data, t) {
                    if (t) {
                        if (data.Birthday) {
                            let date = new Date(data.Birthday);
                            data.Birthday = new Date(data.Year_Of_Birth, date.getMonth(), date.getDate()).toJSON()
                        }

                    }
                }
            }

        }),
        Avatar: new FormElement({
            // label: 'Số điện thoại',
            id: "avatarUrl",
            model: 'Avatar',
            type: 'file',
            options: new InputFileModel({
                type: InputFileType.avatar,
                baseUrl: '/Images/avatar/',
                accept: InputFileAccept.image
                // limit: 1
              }),
        }),
        Gender: new FormElement({
            label: 'Giới tính',
            model: 'Gender',
            type: FormElementType.select,
            options: Para.Gender,
            labelWidth: 80,
            col: 6,
            required: true,
        }),
        State: new FormElement({
            label: 'Trạng thái',
            model: 'State',
            type: 'select',
            options: Para.State,
            // col: 18,
        }),
        Date_of_death: new FormElement({
            label: 'Ngày mất',
            model: 'Date_of_death',
            type: FormElementType.datePicker,
            attr: {
                format: 'dd/MM'
            },
            watch(data, n, o, t, isFirst) {
            //   if(!isFirst) {
                    if (data.Date_of_death) {
                        let ngaysinh = new Date(data.Date_of_death)

                        if (data.year_die != ngaysinh.getFullYear()) {

                            data.year_die = ngaysinh.getFullYear()
                        }
                    }
// 
                // }

                
            },
            options(data) {
                return new DatePickerOption({
                    shortcuts: null,
                })
            },
        }),

        death_year : new FormElement({
            label: '',
            model: 'year_die',
            // type: FormElementType.datePicker,
            attr: {
                placeholder: 'Năm'
            },
            col: 5,

            events: {
                blur(data, t) {
                    if (t) {
                        if (data.Date_of_death) {
                            let date = new Date(data.Date_of_death);
                            data.Date_of_death = new Date(data.year_die, date.getMonth(), date.getDate()).toJSON()
                        }

                    }
                }
            }
        }),





        Parent: new FormElement({
            col: 16,
            id: "father_form",
            type: "parent",
            attr: {
                obj: this,
                // type: 1,
            },
            // watch(data,n,o,t,){
            //     console.log('data,n,o,,',data,n,o,t)
            // }
            // options: Para.Level,
        }),
        Me: new FormElement({
            id: "me_form",
            type: "MeInfo",
            col: 8,
            attr: {
                obj: this,
            }
        }),

        Couple: new FormElement({
            id: "Couple_form",
            col: 12,
            type: "Couple",
            attr: {
                obj: this,
            }
        }),
        siblings: new FormElement({
            id: "siblings_form",

            type: "Siblings",
            attr: {
                obj: this,
            },
            col: 12,
        }),

        Id: new FormElement({
            label: "Id",
            model: "Id",
            type: FormElementType.text,
        }),


    };

    /**
     *
     * @param {Giapha} obj
     */
    constructor(obj) {
        this.update(obj);
    }
    /**
     *
     * @param {Giapha} obj
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
                this._formElements.Parent,
                this._formElements.Me,

                new FormElement({
                    parent_class:"align-flex-start",
                    direction: FormDirectionType.horizontal,
                    child: [
                        this._formElements.Couple,
                        this._formElements.siblings,
                    ]
                })

            ]
        });
    }
    form2() {
        return new FormInfo({
            labelWidth: 100,
            formData: this,
            elements: [
                this._formElements.Avatar,
                new FormElement({
                    direction: FormDirectionType.horizontal,
                    child: [

                        this._formElements.Index.set(p => p.col = 12),
                        this._formElements.Name,
                    ]
                }),
                this._formElements.Other_Name,
                new FormElement({
                    col: 24,
                    direction: FormDirectionType.horizontal,
                    child: [

                        this._formElements.Birthday.set(p => p.col = 10),
                        this._formElements.Year_Of_Birth.set(p => p.col = 5),
                        this._formElements.Gender.set(p => p.col = 10).set(p=>p.isVisible = this.type === 4 ),

                    ]
                }),

                this._formElements.CCCD,
                this._formElements.Phone,


                this._formElements.Place_of_birth.set(p => p.col = 24),
                this._formElements.Address.set(p => p.col = 24),
                this._formElements.State,
                // new FormElement({
                //     direction: FormDirectionType.horizontal,
                //     child: [

                //         this._formElements.Birthday,
                //         this._formElements.Year_Of_Birth,

                //     ]
                // }),

             
                new FormElement({
                    isVisible(data) {
                        return data.State == 2;
                    },
                    child: [

                        new FormElement({
                            label: '',
                            type: 'label',
                            child: [
                                this._formElements.Date_of_death.set(p => p.col = 10),
                                this._formElements.death_year
                                
                            ]
                        }),

                    ]
                }),
                this._formElements.burial_ground,


            ]
        });
    }
}