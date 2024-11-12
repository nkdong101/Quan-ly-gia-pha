import { FormDirectionType, FormElement, FormElementType, FormInfo } from "../base/FormInfo";
import { Para } from "../Para";

export class LoginResult {
  /**
   * @type {string} type - description
  */
  UserSerial;
  /**
   * @type {string} type - description
  */
  Image;
  /**
   * @type {string} type - description
  */
  FullName;
  /**
   * @type {string} type - description
  */
  Email;
  /**
   * @type {string} type - description
  */
  Title;
  /**
   * @type {number} type - description
  */
  UserLevel;

  /**
   * @param {LoginResult} obj - description
  */
  constructor(obj) {
    this.update(obj);
  }

  /**
    * @param {LoginResult} obj - description
   */
  update(obj) {
    Object.assign(this, obj);
  }


  _formElements = {
    FullName: new FormElement({
      label: "Name",
      model: "FullName",
      disabled: true,
    }),
    Email: new FormElement({
      label: "Email",
      model: "Email",
      disabled: true,
    }),
    Title: new FormElement({
      label: "Title",
      model: "Title",
      required: true,
    }),
    Image: new FormElement({
      id: 'txtImage',
      label: "Signature image",
      model: "Image",
      type: FormElementType.file,
      options: Para.SignImage,
      validate(data, t) {
        //console.log('File_cg', t);
        if (t.getEntry('txtImage')) {
          if (!t.getEntry('txtImage').uploadFileList[0]) {
            return 'Signature image is required';
          }
        }
      },
    }),
  };

  toJSON() {
    return {
      ...this,
      _formElements: undefined,
    }
  }
  form() {
    return new FormInfo({
      labelTop: true,
      formData: this,
      elements: [
        new FormElement({
          child: [
            new FormElement({
              col: 14,
              direction: FormDirectionType.vertical,
              child: [
                this._formElements.FullName,
                this._formElements.Email,
                this._formElements.Title,
              ]
            }),
            new FormElement({
              col: 10,
              child: [
                this._formElements.Image,
              ]
            }),
          ]
        }),

      ]
    });
  }
}
