<template>
  <div
    :class="'has-border file-manage input-file-type-' + model.type"
    :disabled="isDisabled()"
  >
    <!-- <InputContainer
      :label="label"
      :labelWidth="labelWidth"
    >
        el
    </InputContainer>-->
    <div v-if="label" class="form-title" style="">
      <span v-html="label"></span>
    </div>
    <div
      v-if="isShowAvatar() && uploadFileList.length && !isDisabled()"
      class="avatar-remove"
      title="Xóa"
      @click="handleCustomRemove(uploadFileList[0])"
    >
      <i class="fa fa-times"></i>
    </div>
    <el-upload
      class="uploader"
      :has-title="!!label"
      :has-limit="isMaxinum"
      :view-only="model.isViewOnly"
      ref="upload"
      action="/API/SaveFileToServer"
      :accept="model.accept"
      :headers="headers"
      :file-list="fileList"
      :auto-upload="model.autoUpload"
      :on-preview="handlePreview"
      :on-remove="handleRemove"
      :before-remove="beforeRemove"
      :on-success="handleSuccess"
      :before-upload="beforeUpload"
      :on-change="handleChange"
      :on-error="handleError"
      :on-progress="handleprogress"
      :limit="model.limit"
      :multiple="model.multiple !== false ? true : false"
      :on-exceed="handleExceed"
      :show-file-list="isShowDefault()"
      :list-type="model.accept == InputFileAccept.image ? 'picture-card' : ''"
      :disabled="isDisabled()"
    >
      <div slot="trigger">
        <!-- <i slot="default" class="el-icon-plus"></i> -->

        <InputContainer
          v-if="isShowChoose()"
          label="Đính kèm tệp"
          :labelWidth="labelWidth"
        >
          <el-input :value="placeholder" disabled></el-input>
        </InputContainer>
        <template v-else-if="model.type == 'avatar'">
          <div
            v-if="uploadFileList.length > 0"
            class="image-preview"
            :style="GetFileUrl(0)"
          ></div>
          <div v-else class="el-upload el-upload-avatar">
            <i class="el-icon-plus"></i>
          </div>
        </template>
        <div v-else></div>
      </div>

      <div
        v-if="model.accept != InputFileAccept.image"
        slot="file"
        slot-scope="{ file }"
        class="file-list-c"
      >
        <i class="fa fa-file-text-o" aria-hidden="true" style=""></i>
        <span class="file-list-title" @click="handlePreview(file)">
          {{ file.getNameDownload ? file.getNameDownload() : file.name }}
        </span>
        <span
          v-if="!isDisabled()"
          class="file-list-delete"
          @click="handleCustomRemove(file)"
          ><i class="fa fa-times"></i
        ></span>
      </div>

      <div v-else slot="file" slot-scope="{ file }">
        <div
          v-if="isImg(file.name)"
          :style="{
            position: 'absolute',
            width: '100%',
            height: '100%',
            'background-color': '#d4d4d4',
            'background-image': 'url(\'' + file.url + '\')',
            'background-size': 'cover',
          }"
        ></div>
        <i
          v-else-if="isVideo(file.name)"
          class="fa fa-file-video-o file-input-preview-ic"
          aria-hidden="true"
          style=""
        ></i>
        <i
          v-else
          class="fa fa-file-text-o file-input-preview-ic"
          aria-hidden="true"
          style=""
        ></i>
        <span class="el-upload-list__item-actions" style="z-index: 1">
          <span
            class="el-upload-list__item-preview"
            style="
              position: absolute;
              top: 0;
              left: 0;
              width: 100%;
              height: 100%;
              display: flex;
              align-items: center;
              justify-content: center;
            "
            @click="handlePreview(file)"
          >
            <i class="el-icon-zoom-in"></i>
          </span>

          <span
            v-if="!model.isViewOnly && !isDisabled()"
            class="el-upload-list__item-delete"
            style="
              position: absolute;
              margin: 0px;
              top: -6px;
              right: -16px;
              width: 46px;
              height: 26px;
              background: #f56c6c;
              transform: rotate(45deg);
              color: white;
              display: flex;
              align-items: center;
              justify-content: center;
              z-index: 2;
            "
            @click="handleCustomRemove(file)"
            ><i
              data-v-767fd051=""
              class="el-icon-upload-success el-icon-delete"
              style="
                transform: rotate(-45deg);
                font-size: 13px;
                margin-top: 8px;
                display: block;
                position: relative;
                color: white;
              "
            ></i
          ></span>
        </span>
        <span
          v-if="file.status == 'success'"
          style="
            position: absolute;
            margin: 0px;
            top: -6px;
            right: -16px;
            width: 46px;
            height: 26px;
            background: #13ce66;
            transform: rotate(45deg);
            color: white;
            display: flex;
            align-items: center;
            justify-content: center;
          "
          ><i
            data-v-767fd051=""
            class="el-icon-upload-success el-icon-check"
            style="transform: rotate(-45deg); font-size: 13px; margin-top: 13px"
          ></i
        ></span>
        <el-progress
          v-if="file.status == 'uploading'"
          type="circle"
          :width="78"
          style="width: 78px; background: white"
          :percentage="Math.floor(file.percentage || 0)"
        ></el-progress>
      </div>
    </el-upload>
    <!-- <DefaultForm :model="formPreview"> -->
    <div class="preview-c" v-if="formPreview.visible">
      <span
        @click="formPreview.visible = false"
        class="el-image-viewer__btn el-image-viewer__close"
        ><i class="el-icon-circle-close"></i
      ></span>

      <!-- <div class="el-image-viewer__btn el-image-viewer__actions">
        <div class="el-image-viewer__actions__inner"><i class="el-icon-zoom-out"></i><i class="el-icon-zoom-in"></i><i class="el-image-viewer__actions__divider"></i><i class="el-icon-full-screen"></i><i class="el-image-viewer__actions__divider"></i><i class="el-icon-refresh-left"></i><i class="el-icon-refresh-right"></i></div>
      </div> -->
      <el-carousel
        height="100vh"
        style="margin-bottom: 5px"
        :autoplay="false"
        :initial-index="previewFileIndex"
        @change="handlerPreviewChange"
      >
        <el-carousel-item v-for="item in previewFiles" :key="item.href">
          <div style="height: 100%; position: relative">
            <!-- {{ item.href }} -->
            <img
              v-if="isImg(item.getNameDownload())"
              :src="item.href"
              :alt="item.getNameDownload()"
              style="
                position: absolute;
                top: 50%;
                left: 50%;
                transform: translate(-50%, -50%);
                max-width: 100%;
                max-height: 100%;
              "
              srcset=""
            />
            <iframe
              v-else-if="isPDF(item.getNameDownload())"
              :src="item.href"
              :alt="item.getNameDownload()"
              style="
                position: absolute;
                top: 50%;
                left: 50%;
                transform: translate(-50%, -50%);
                width: 100vw;
                height: 100vh;
              "
              srcset=""
            />
            <!-- <iframe
              v-else-if="
                isDOC(item.getNameDownload()) && previewFileIndex == index
              "
              :src="GetGoogleEmbeddedLink(item)"
              :alt="item.getNameDownload()"
              style="
                position: absolute;
                top: 50%;
                left: 50%;
                transform: translate(-50%, -50%);
                width: 100vw;
                height: 100vh;
              "
              srcset=""
            /> -->
            <div
              v-else-if="isVideo(item.getNameDownload())"
              style="
                height: calc(100vh - 62px);
                width: 100%;
                position: absolute;
                top: 50%;
                left: 50%;
                transform: translate(-50%, -50%);
                text-align: center;
              "
            >
              <video height="100%" controls>
                <source :src="item.href" />
                Your browser does not support the video tag.
              </video>
            </div>
            <div
              v-else
              style="
                display: flex;
                align-items: center;
                justify-content: center;
                height: 100%;
                font-size: 100px;
                flex-direction: column;
              "
            >
              <span
                style="
                  font-size: 30px;
                  text-align: center;
                  color: white;
                  font-weight: bold;
                  margin-bottom: 32px;
                "
                >{{ GetFileName(item.getNameDownload()) }}</span
              >
              <div style="display: flex">
                <a
                  :href="item.href"
                  style="
                    display: flex;
                    flex-direction: column;
                    align-items: center;
                    text-decoration: none;
                    color: white;
                  "
                  :download="GetFileName(item.getNameDownload())"
                >
                  <i
                    class="fa fa-download"
                    style="width: 100px; height: 100px"
                  ></i>

                  <span
                    style="
                      font-size: 24px;
                      text-align: center;
                      font-weight: bold;
                    "
                    >Download</span
                  >
                </a>
                <a
                  :href="GetGoogleEmbeddedLink(item)"
                  target="_blank"
                  style="
                    display: flex;
                    flex-direction: column;
                    align-items: center;
                    text-decoration: none;
                    color: white;
                    margin-left: 80px;
                  "
                >
                  <i class="fa fa-eye" style="width: 100px; height: 100px"></i>

                  <span
                    style="
                      font-size: 24px;
                      text-align: center;
                      font-weight: bold;
                    "
                    >View</span
                  >
                </a>
              </div>
            </div>
          </div>
        </el-carousel-item>
      </el-carousel>
    </div>
    <!-- </DefaultForm> -->
  </div>
</template>
<style lang="scss" scoped>
.preview-c {
  position: fixed;
  height: 100vh;
  width: 100vw;
  top: 0px;
  left: 0px;
  background: #00000080;
  z-index: 9999999;
  .el-image-viewer__close {
    color: white;
    z-index: 99999999999999999999;
    background-color: red;
  }
  .el-image-viewer__actions {
    color: white;
    z-index: 99999999999999999999;
  }
}

.file-manage {
  flex: 1;

  &[disabled] {
    .uploader {
      // background: #ddd;
    }
  }
  .form-title {
    border-bottom-left-radius: 0;
    border-bottom-right-radius: 0;
  }
  .avatar-remove {
    position: absolute;
    right: 8px;
    top: 32px;
    width: 20px;
    height: 20px;
    color: white;
    background: red;
    border-radius: 100%;
    display: flex;
    align-items: center;
    justify-content: center;
    cursor: pointer;
  }
  .uploader {
    padding: 5px;
    // border: 1px solid #dcdfe6;
    // border-style: dashed;
    border-radius: 4px;
    // border: 1px solid #ddd;
    ::v-deep  .el-upload--text {
      margin: 0;
    }
    &[has-limit] {
      ::v-deep  .el-upload-list__item:first-child {
        margin-top: 0;
      }
      ::v-deep  .el-upload--text {
        margin: 0;
      }
    }
    &[has-title] {
      border: 0;

      border-top-left-radius: 0px;
      border-top-right-radius: 0px;
    }
    &:not([has-title]) {
      ::v-deep  .el-upload {
        margin: 0;
      }
    }
    &[view-only] {
      ::v-deep  .el-upload {
        display: none;
        margin-bottom: 0px;
      }
    }
    ::v-deep  .el-upload-list--picture-card {
      display: flex;
      flex-wrap: wrap;
      margin-bottom: -8px;
    }
    ::v-deep  .el-input--mini input {
      padding: 0 5px;
    }
  }
  ::v-deep  .el-icon-close-tip {
    display: none !important;
  }
  ::v-deep  .el-upload-list__item:focus {
    .el-icon-close {
      display: inline-block;
    }
  }
  ::v-deep  .el-upload {
    width: 100%;

    height: inherit;
    line-height: initial;
    margin-top: 8px;
    border: none;
    .el-input-group__prepend {
      background: #409eff;
      color: white;
      border-color: #409eff;
    }
    .el-input__inner {
      cursor: pointer;
      background: white;
      border-color: #409eff;
      &:hover {
        background: #f5f7fa;
      }
    }
  }
  ::v-deep  .el-upload-list.is-disabled .el-upload-list__item {
    margin-top: 0;
  }
  ::v-deep  .el-upload-list--picture-card .el-upload-list__item {
    width: 80px;
    height: 80px;
  }
  ::v-deep  .el-upload-list__item-name {
    cursor: pointer;
  }
}
::v-deep  .el-carousel__button {
  background: #66b1ff;
}
.input-file-type-avatar {
  display: flex;
  flex-direction: column;
  .form-title {
    border: 0;
    border-bottom: 1px solid #ddd;
  }
  .uploader {
    border: 0;
  }
  &:hover {
    border-color: #409eff;
    .form-title {
      //   border-color: #409eff;
      // border-color: #409eff;
      border-bottom-color: #409eff;
      border-bottom-style: dashed;
    }
    // .uploader {
    //   border-color: #409eff;
    // }
  }
  .uploader {
    flex: 1;
    display: flex;
    min-height: 120px;
    // border: 1px solid #ddd;
  }
  ::v-deep  .el-upload {
    display: flex;
    height: auto;
    width: 100%;
    line-height: normal;
    line-height: initial;
    justify-content: center;
    margin-top: 0px;
    align-items: center;

    & > div {
      display: flex;
      flex: 1;
      height: 100%;
    }
    .image-preview {
      //   display: flex;
      //   align-items: center;
      //   justify-content: center;
      flex: 1;
      background-size: contain;
      background-repeat: no-repeat;
      background-position: center;
      background-color: #fbfdff;
    }
  }

  ::v-deep  .el-upload--text {
    display: flex;

    i {
      align-self: center;
      font-size: 28px;
      color: #8c939d;
    }
  }
}
.input-file-type-default {
  .uploader {
    display: flex;
    flex-direction: column;
  }

  &[disabled] {
    ::v-deep  .el-upload {
      display: none;
    }
  }
}
::v-deep  .file-input-preview-ic {
  position: absolute;
  top: 50%;
  left: 50%;
  transform: translate(-50%, -50%);
  font-size: 35px;
  height: 35px;
  width: auto;
  color: #9c9c9c;
  box-shadow: 5px 5px 10px #ddd;
}

.file-list-c {
  display: flex;
  align-items: baseline;
  cursor: pointer;
  font-size: 13px;
  &:hover {
    color: #409eff;
  }
  .file-list-title {
    flex: 1;
    padding-left: 5px;
    word-break: break-all;
  }
  .file-list-delete {
    flex: 0 0 24px;
    height: 100%;
    text-align: center;
    &:hover {
      color: red;
    }
    // color: black;
  }
}
</style>

<script>
import { ShowMessage } from "~/assets/scripts/Functions";
import InputFileModel, { InputFileType } from "./InputFIleModel";
export default {
  props: {
    model: InputFileModel,
    value: String | Array,
    label: String,
    labelWidth: Number,
    disabled: Boolean,
  },
  data() {
    return {
      formPreview: {
        type: "dialog",
        visible: false,
        size: "lg",
        title: "Xem trước",
      },
      previewFileIndex: 0,
      previewFiles: [],
      fileList: [],
      totalsFile: 0,

      headers: {
        path: this.model.baseUrl,
      },

      counter: 0,
      uploadFileList: [],
    };
  },
  computed: {
    isMaxinum() {
      if (this.model.limit) {
        if (this.totalsFile >= this.model.limit) return true;
      }
      return false;
    },
    placeholder: function () {
      let str;
      if (this.model.type == InputFileType.OneFile) {
        str = ` ${this.totalsFile} tệp ${
          this.uploadFileList[0]
            ? this.uploadFileList[0].getNameDownload
              ? this.uploadFileList[0].getNameDownload()
              : this.uploadFileList[0].name_download ||
                this.uploadFileList[0].name
            : this.fileList[0]
            ? this.fileList[0].getNameDownload()
            : ""
        }`;
      } else {
        str = ` ${this.totalsFile} tệp`;
      }

      if (this.model.limit && this.model.limit > 1)
        str += ` (maximum ${this.model.limit} files)`;

      return str;
    },
  },
  watch: {
    value(nv, ov) {
      this.loadData();
    },
  },
  methods: {
    handlerPreviewChange(index) {
      this.previewFileIndex = index;
    },
    isDisabled() {
      // if (this.isMaxinum) return true;
      return this.model.isViewOnly || this.disabled;
    },
    handleCustomRemove(file) {
      this.$refs.upload.handleRemove(null, file);
    },
    GetFileName(str) {
      var splt = str.split("/");
      return splt[splt.length - 1];
    },
    GetGoogleEmbeddedLink(item) {
      return `https://view.officeapps.live.com/op/view.aspx?src=${
        location.origin + item.href
      }`;
    },
    isDOC(item) {
      var type = [".DOC", ".DOCX", ".XLS", ".XLSX"];
      var splt = item.split(".");
      var ext = splt[splt.length - 1].toUpperCase();
      return type.some((p) => p == "." + ext);
    },
    isPDF(item) {
      var type = [".PDF"];
      var splt = item.split(".");
      var ext = splt[splt.length - 1].toUpperCase();
      return type.some((p) => p == "." + ext);
    },
    isImg(item) {
      var type = [".JPG", ".JPE", ".BMP", ".GIF", ".PNG", ".WEBP"];
      var splt = item.split(".");
      var ext = splt[splt.length - 1].toUpperCase();
      return type.some((p) => p == "." + ext);
    },
    isVideo(item) {
      var type = [
        ".PNG",
        ".JPG",
        ".JPEG",
        ".BMP",
        ".GIF", //etc
        ".WAV",
        ".MID",
        ".MIDI",
        ".WMA",
        ".MP3",
        ".OGG",
        ".RMA", //etc
        ".AVI",
        ".MP4",
        ".DIVX",
        ".WMV",
      ];
      var splt = item.split(".");
      var ext = splt[splt.length - 1].toUpperCase();
      return type.some((p) => p == "." + ext);
    },
    tmp() {
      // console.log(this.uploadFileList);
    },
    isShowDefault() {
      return (
        this.model.type == InputFileType.default ||
        this.model.type == InputFileType.images
      );
    },
    isShowChoose() {
      if (this.isMaxinum) return false;
      return (
        this.model.type == InputFileType.default ||
        this.model.type == InputFileType.OneFile
      );
    },
    isShowAvatar() {
      return this.model.type == InputFileType.avatar;
    },
    GetFileUrl: function (index) {
      if (this.uploadFileList.length > 0) {
        let file = this.uploadFileList[0];
        let href = "";
        if (file.status == "success") {
          href = this.model.baseUrl + file.name;
        } else {
          var blob = new Blob([file.raw]);
          href = URL.createObjectURL(blob);
        }
        return `background-image: url('${href}');`;
      }
    },
    enabelPicker: function () {
      return true;
    },
    submitUpload() {
      let _app = this;
      if (_app.uploadFileList.length) {
        _app.$refs.upload.submit();
        return new Promise(function (rs) {
          const itv = setInterval(function () {
            if (_app.counter == 0) {
              clearInterval(itv);
              let _filelist = _app.$refs.upload.$refs["upload-inner"].fileList;
              rs(_filelist.map((p) => `${p.name}|${p.name_download}`));
            }
          }, 250);
        });
      } else {
        return new Promise(function (rs) {
          rs([]);
        });
      }
    },
    beforeUpload(file) {
      this.counter += 1;
    },
    handleError(err, file, fileList) {
      ShowMessage(err);
      this.counter -= 1;
      if (this.counter == 0) this.UploadDone(fileList);
    },
    handleSuccess(response, file, fileList) {
      if (response.Status == "OK") {
        let fileUploaded = response.Data[0];
        file.name = fileUploaded;
      } else {
        ShowMessage(response.Data);
      }

      this.counter -= 1;
      if (this.counter == 0) this.UploadDone(fileList);

      // console.log(response, file, fileList);
    },
    UploadDone(fileList) {
      this.$emit(
        "input",
        fileList
          .map((p) => {
            if (p.name_download) return p.name + "|" + p.name_download;
            return p.name;
          })
          .join(",")
      );
    },
    handleRemove(file, fileList) {
      this.totalsFile = fileList.length;
      //   console.log(file, fileList);
    },
    handlePreviewOld(file) {
      //   console.log(file);
      let a = document.createElement("a");
      if (file.status == "success") {
        a.href = this.model.baseUrl + file.name;
      } else {
        var blob = new Blob([file.raw]);
        a.href = URL.createObjectURL(blob);
      }
      a.download = file.name;
      document.body.appendChild(a);
      a.click();
      document.body.removeChild(a);
    },
    handlePreview(fileDangChon) {
      // console.log(fileDangChon);

      let _app = this;
      this.previewFileIndex = this.uploadFileList.indexOf(fileDangChon);
      this.previewFiles = this.uploadFileList.map(function (file) {
        var href = "";
        if (file.status == "success") {
          href = _app.model.baseUrl + file.name;
        } else {
          var blob = new Blob([file.raw]);
          href = URL.createObjectURL(blob);
        }

        return {
          name: file.name,
          name_download: file.name_download || file.name,
          href: href.replace(/\?/g, ","),
          getNameDownload() {
            return this.name_download.replace(/\?/g, ",");
          },
        };
      });
      if (this.model.onPreview)
        this.model.onPreview({
          previewFileIndex: this.previewFileIndex,
          previewFiles: this.previewFiles,
          entry: this,
        });
      else this.formPreview.visible = true;
      //   console.log(this.uploadFileList);
    },
    handleprogress(event, file, fileList) {
      // console.log(file.status, file.percentage);
    },
    handleChange(file, fileList) {
      let findFileMax = fileList.filter((p) => {
        if (p.size) {
          if (p.size > 20 * 1024 * 1024) {
            return true;
          }
        }
        return false;
      });
      if (findFileMax.length) {
        ShowMessage("Upload file exceeds 20 Mb", "error");

        findFileMax.forEach((p) => {
          fileList.splice(fileList.indexOf(p), 1);
        });
      }

      this.uploadFileList = fileList;
      this.totalsFile = fileList.length;
      console.log(fileList);
      if (this.model.type == InputFileType.avatar) {
        if (fileList.length > 1) fileList.splice(0, 1);
      } else if (this.model.type == InputFileType.OneFile) {
        if (fileList.length > 1) {
          fileList.splice(0, 1);
          this.totalsFile = fileList.length;
        }
      }
      // if (this.model.autoUpload) this.submitUpload();
    },
    handleExceed(files, fileList) {
      if (this.model.type == InputFileType.avatar) {
        // console.log(files);
        // console.log(fileList);
        // console.log(this);
      } else
        this.$message.warning(
          `You can only select up to ${
            this.model.limit
          } files, you have selected ${files.length} files, up to a total of ${
            files.length + fileList.length
          } files`
        );
    },
    beforeRemove(file, fileList) {
      if (file == null) return true;
      if (file.status == "ready") return true;
      return this.$confirm(`Confirm to delete file ${file.name} ?`);
    },
    loadData() {
      let defaultFile = [];
      if (typeof this.value == "string") {
        defaultFile = this.value.split(",");
      } else {
        defaultFile = this.value || [];
      }

      if (this.model.type == InputFileType.avatar) this.model.limit = 2;
      this.fileList = defaultFile
        .filter((p) => p)
        .map((p) => {
          let splt = p.split("|");

          return {
            name: splt[0],
            name_download: splt[1] || splt[0],
            url: this.model.baseUrl + splt[0],
            getNameDownload() {
              return this.name_download.replace(/\?/g, ",");
            },
          };
        });
      this.totalsFile = this.fileList.length;
      // console.log(this);

      this.$nextTick(() => {
        this.uploadFileList = this.$refs.upload.$refs["upload-inner"].fileList;
      });
    },
  },
  mounted() {
    this.loadData();
    // console.log(this);
    // this.uploader = this.$refs.upload.$refs["upload-inner"];
  },
};
</script>
