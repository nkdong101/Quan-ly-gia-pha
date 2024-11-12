

import { FormElement, FormElementType, FormInfo, } from "../../base/FormInfo";


export default function (obj) {
  obj = JSON.parse(JSON.stringify(obj || {}));
  const formElements = {

    Id: new FormElement({
      id: "txtId",
      label: "Id",
      model: "Id",
      type: FormElementType.text
    }),


    Name: new FormElement({
      id: "txtName",
      label: "Account type name",
      model: "Name",
      type: FormElementType.text
    }),

    Description: new FormElement({
      id: "txtDescription",
      label: "Description",
      model: "Description",
      type: FormElementType.text
    }),

    Permission: new FormElement({
      id: "txtPermission",
      label: "Permission",
      model: "Permission",
      type: FormElementType.text
    }),

    SetPermission: new FormElement({
      id: "txtSetPermission",
      label: "Permission",
      model: "Permission",
      type: 'TreeMenu',
      attr: {
        isConfig: true,
      }
    }),

    UserCreate: new FormElement({
      id: "txtUserCreate",
      label: "UserCreate",
      model: "UserCreate",
      type: FormElementType.text
    }),

    UserUpdate: new FormElement({
      id: "txtUserUpdate",
      label: "UserUpdate",
      model: "UserUpdate",
      type: FormElementType.text
    }),

    DateUpdate: new FormElement({
      id: "txtDateUpdate",
      label: "DateUpdate",
      model: "DateUpdate",
      type: FormElementType.text
    }),

    DateCreate: new FormElement({
      id: "txtDateCreate",
      label: "DateCreate",
      model: "DateCreate",
      type: FormElementType.text
    })
  };
  return {
    Id: obj.Id,
    Name: obj.Name,
    Description: obj.Description,
    Permission: obj.Permission || [],
    SetPermission: [],
    form: function () {
      return new FormInfo({
        formData: this,
        labelWidth:160,
        elements: [
          formElements.Name,
          formElements.Description,
          formElements.SetPermission

        ]
      });
    }
  };
};

