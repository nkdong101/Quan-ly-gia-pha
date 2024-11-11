import { getFileObj } from "~/assets/scripts/Functions";

// @ts-check
export default class InputFileModel {
  /** @type {Number} - limit upload file */
  limit;
  /** @type {Boolean} - Cho phép up nhiều file */
  multiple;
  /** @type {Boolean} - Cho phép up nhiều file */
  autoUpload;
  /** @type {String} - Base upload url */
  baseUrl;
  /** @type {String} - hình thức chọn file */
  type;
  /** @type {String} - cho phép chọn file các định dạng */
  accept;

  /** @type {({previewFileIndex: number,previewFiles: Array<{name,name_download,href,getNameDownload}>, entry}=>{}))} - cho phép chọn file các định dạng */
  onPreview;
  constructor({
    limit = 0,
    multiple = true,
    autoUpload = false,
    baseUrl = "",
    onPreview = undefined,
    type = InputFileType.default,
    accept = InputFileAccept.all
  }) {
    this.limit = limit;
    this.multiple = multiple;
    this.baseUrl = baseUrl;
    this.type = type;
    this.accept = accept;
    this.autoUpload = autoUpload;
    this.onPreview = onPreview;
  }

  getImage(file) {
    let fileO = getFileObj(file);
    return this.baseUrl + fileO.file
  }


  /**
   * @param {(obj:InputFileModel)=>{}} setFunction - description
  */
  set(setFunction) {
    let data = new InputFileModel(this);
    setFunction(data);
    return data;
  }

}

export const InputFileType = {
  default: "default",
  avatar: "avatar",
  OneFile: "OneFile",
};

export const InputFileAccept = {
  all: undefined,
  image: "image/*,video/*,audio/*"
};
