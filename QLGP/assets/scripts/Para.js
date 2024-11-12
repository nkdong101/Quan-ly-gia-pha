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
  Donvihanhchinh: new SelectOption({
    data: API.DonviHanhchinh_Get_Tinh,
    // method:"post",
    multiple: true,
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
  Disposal_Attachment: new InputFileModel({
    baseUrl: '/Images/Disposal/Attachment/'
  }),
  PR_Attachment: new InputFileModel({
    baseUrl: '/Images/Purchase/Attachment/',
  }),
  Comment_Attachment: new InputFileModel({
    baseUrl: '/Images/Comment/Attachment/',
  }),

  PR_Procurment_Attachment: new InputFileModel({
    baseUrl: '/Images/Purchase/Procurment_Attachment/',
  }),

  FixedAssetsImage: new InputFileModel({
    baseUrl: '/Images/FixedAssets/Attachment/',
    autoUpload: true
  }),

  PR_Procurment_Contract_Attachment: new InputFileModel({
    baseUrl: '/Images/Purchase/Contract_Attachment/',
    limit: 1,
    multiple: false
  }),
  GroupPermission: new SelectOption({
    data: []
  }),
  Para_Office: new SelectOption({
    data: [],
    IsItemDisabled: item => {
      if (item.Use == 2)
        return true;
    },
  }),
  Para_Category: new SelectOption({
    data: []
  }),
  Para_VehicleType: new SelectOption({
    data: []
  }),
  Para_Currency: new SelectOption({
    data: [],
    label: "Currency_Code",
    value: "Currency_Code",
  }),
  Para_Currency_now: new SelectOption({
    data: API.Get_Currency,
    label: "Currency_Code",
    value: "Currency_Code",
  }),
  Para_Practice: new SelectOption({
    data: [],
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
