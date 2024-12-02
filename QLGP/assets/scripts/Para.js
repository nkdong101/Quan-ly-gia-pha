// @ts-nocheck

import InputFileModel, { InputFileAccept, InputFileType } from "~/components/form-content/InputFile/InputFIleModel";
import API from "./API";
import { SelectOption } from "./base/SelectOption";

export var Para = {

  Level: new SelectOption({
    data: [
      { Id: 0, Name: "Tỉnh" },
      { Id: 1, Name: "Huyện" },
      { Id: 2, Name: "Xã" },

    ]
  }),
  State: new SelectOption({
    data: [
      { Id: 1, Name: "Đang sống" },
      { Id: 2, Name: "Đã mất" },
      { Id: 3, Name: "Hiện không có thông tin" },

    ]
  }),
  Giapha: new SelectOption({
    data: [],
    value: 'id',
    label: 'name',
    placeholder: 'Tìm kiếm'
  }),
  dong_ho: new SelectOption({
    data: API.HoVietNam,
    // method:"post",
    // multiple: true,
  }),



  SignImage: new InputFileModel({
    accept: InputFileAccept.image,
    baseUrl: '/images/signature/',
    limit: 1,
    type: InputFileType.avatar
  }),
  MenuImage: new InputFileModel({
    accept: InputFileAccept.image,
    baseUrl: '/images/menu/',
    limit: 1,
    // type: InputFileType.avatar
  }),
  Inventory_Attachment: new InputFileModel({
    baseUrl: '/Images/Inventory/Attachment/',
    accept: InputFileAccept.image,
  }),

  GroupPermission: new SelectOption({
    data: []
  }),
 
  Gender: new SelectOption({
    // placeholder: 'Select one',
    data: [
      {Id: 1, Name: "Nam"},
      {Id: 2, Name: "Nữ"},
    ],
  
  }),
  

  
  Para_Account_purchasing: new SelectOption({
    data: [],
    placeholder: 'Nothing selected',
    label: 'FullName'
  }),


};
