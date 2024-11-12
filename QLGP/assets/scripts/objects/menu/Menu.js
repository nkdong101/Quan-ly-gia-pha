import {
  FormElement,
  FormInfo,
  FormDirectionType,
  FormElementType
} from "~/assets/scripts/base/FormInfo";
import { Para } from "../../Para";
export const formElements = {
  Id: new FormElement({
    id: "txtId",
    label: "Id",
    model: "Id",
    disabled: true,
    type: FormElementType.text
  }),

  Icon: new FormElement({
    id: "txtIcon",
    label: "Icon",
    model: "Icon",
    type: FormElementType.text
  }),

  Image: new FormElement({
    id: "txtImage",
    label: "Image",
    model: "Image",
    type: FormElementType.file,
    options: Para.MenuImage,
    // col: 8
  }),


  ParentId: new FormElement({
    id: "txtParentId",
    label: "ParentId",
    model: "ParentId",
    type: FormElementType.text
  }),

  isButton: new FormElement({
    id: "txtisButton",
    label: "isButton",
    model: "isButton",
    type: FormElementType.text
  }),

  isViewAll: new FormElement({
    id: "txtisViewAll",
    label: "isViewAll",
    model: "isViewAll",
    type: FormElementType.checkbox
  }),

  Title: new FormElement({
    id: "txtTitle",
    label: "Title",
    model: "Title",
    type: FormElementType.text
  }),

  Note: new FormElement({
    id: "txtNote",
    label: "Note",
    model: "Note",
    type: FormElementType.text,
    attr: {
      type: 'textarea'
    }
  }),

  Title_Class: new FormElement({
    id: "txtTitle_Class",
    label: "Title_Class",
    model: "Title_Class",
    type: FormElementType.text
  }),

  Tag: new FormElement({
    id: "txtTag",
    label: "Tag",
    model: "Tag",
    type: FormElementType.text,

  }),

  Other_Class: new FormElement({
    id: "txtOther_Class",
    label: "Other_Class",
    model: "Other_Class",
    type: FormElementType.text
  }),

  LiClass: new FormElement({
    id: "txtLiClass",
    label: "LiClass",
    model: "LiClass",
    type: FormElementType.text
  }),

  LiID: new FormElement({
    id: "txtLiID",
    label: "LiID",
    model: "LiID",
    type: FormElementType.text
  }),

  Href: new FormElement({
    id: "txtHref",
    label: "Href",
    model: "Href",
    type: FormElementType.text
  }),

  Action: new FormElement({
    id: "txtAction",
    label: "Action",
    model: "Action",
    type: FormElementType.text,
    attr: {
      autofocus: true
    }
  }),



  Expanded: new FormElement({
    id: "txtExpanded",
    label: "Expanded",
    model: "Expanded",
    type: FormElementType.text
  }),

  Stt: new FormElement({
    id: "txtStt",
    label: "Stt",
    model: "Stt",
    type: FormElementType.text
  }),

  Child: new FormElement({
    id: "txtChild",
    label: "Child",
    model: "Child",
    type: FormElementType.text
  }),

  ListButton: new FormElement({
    id: "txtListButton",
    label: "ListButton",
    model: "ListButton",
    type: FormElementType.text
  }),
  Type_id: new FormElement({
    id: "Type_id",
    label: "Type_id",
    model: "Type_id",
    type: FormElementType.text
  })
};
let defaultParams = function () {
  return {
    Id: '',
    Icon: '',
    ParentId: '',
    Type_id: '',
    isViewAll: '',
    Title: '',
    Title_Class: '',
    Tag: '',
    Other_Class: '',
    LiClass: '',
    LiID: '',
    Href: '',
    Action: '',
    Expanded: '',
    Stt: '',
    Note: '',
    Image: '',
    Child: [],
    ListButton: [],
  };
}
export default function (obj = defaultParams()) {
  obj = JSON.parse(JSON.stringify(obj || {}));

  return {
    Id: obj.Id,
    Icon: obj.Icon,
    ParentId: obj.ParentId,
    Type_id: obj.Type_id,
    isViewAll: obj.isViewAll,
    Title: obj.Title,
    Title_Class: obj.Title_Class,
    Tag: obj.Tag,
    Other_Class: obj.Other_Class,
    LiClass: obj.LiClass,
    LiID: obj.LiID,
    Href: obj.Href,
    Action: obj.Action,
    Expanded: obj.Expanded,
    Stt: obj.Stt,
    Child: obj.Child,
    ListButton: obj.ListButton,
    Image: obj.Image,
    Note: obj.Note,
    form: function () {
      if (this.Type_id == 0)
        return new FormInfo({
          formData: this,
          elements: [
            formElements.ParentId,
            formElements.Stt,
            formElements.Action,
            formElements.LiClass,
            formElements.Title,
            formElements.Icon
          ]
        });
      else
        return new FormInfo({
          formData: this,
          elements: [
            formElements.ParentId,
            formElements.Id,
            formElements.Stt,
            formElements.Action,
            formElements.Href,
            formElements.Title,
            formElements.LiClass.set(p => p.label = "Header"),
            formElements.Icon,
            formElements.Image,
            formElements.Note,
            formElements.isViewAll,
            formElements.Type_id,

          ]
        });
    },

  };
}
