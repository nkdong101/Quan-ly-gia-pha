<template>
  <div :class="'file-manage input-file-type-' + model.type">
    <!-- <InputContainer
      :label="label"
      :labelWidth="labelWidth"
    >
        el
        </InputContainer>-->
    <div class="form-title" style>{{ label }}</div>
    <el-upload
      class="uploader"
      ref="upload"
      :action="APISaveFileToServer"
      :accept="model.accept"
      :headers="headers"
      :file-list="fileList"
      :auto-upload="false"
      :on-preview="handlePreview"
      :on-remove="handleRemove"
      :before-remove="beforeRemove"
      :on-success="handleSuccess"
      :before-upload="beforeUpload"
      :on-change="handleChange"
      :on-error="handleError"
      :limit="model.limit"
      :multiple="model.multiple !== false ? true : false"
      :on-exceed="handleExceed"
      :show-file-list="isShowDefault()"
    >
      <div slot="trigger">
        <!-- <i slot="default" class="el-icon-plus"></i> -->
        <InputContainer
          v-if="isShowDefault()"
          label="Chọn file"
          :labelWidth="labelWidth"
        >
          <el-input :value="placeholder" disabled></el-input>
        </InputContainer>
        <template v-else>
          <div
            v-if="uploadFileList.length > 0"
            class="image-preview"
            :style="GetFileUrl(0)"
          ></div>
          <div v-else class="el-upload el-upload-avatar">
            <i class="el-icon-plus"></i>
          </div>
        </template>
      </div>

      <!-- <el-button  size="small" type="primary">{{
        label
            }}</el-button>-->
      <!-- <el-button
        style="margin-left: 10px;"
        size="small"
        type="success"
        @click="submitUpload"
        >upload to server</el-button
            >-->
    </el-upload>
  </div>
</template>
<style lang="scss" scoped>
.file-manage {
  flex: 1;

  .form-title {
    border-bottom-left-radius: 0;
    border-bottom-right-radius: 0;
  }
  .uploader {
    padding: 5px;
    border: 1px solid #dcdfe6;
    border-style: dashed;
    border-top: 0;
    border-bottom-left-radius: 4px;
    border-bottom-right-radius: 4px;
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
  ::v-deep  .el-upload-list__item-name {
    cursor: pointer;
  }
}

.input-file-type-avatar {
  display: flex;
  flex-direction: column;
  &:hover {
    .form-title {
      //   border-color: #409eff;
      border-bottom-color: #409eff;
      border-bottom-style: dashed;
    }
    .uploader {
      border-color: #409eff;
    }
  }
  .uploader {
    flex: 1;
    display: flex;
    min-height: 120px;
  }
  .el-upload {
    display: flex;
    height: auto;
    width: 100%;
    line-height: initial;
    justify-content: center;
  }

  ::v-deep  .el-upload--text {
    display: flex;
    & > div {
      display: flex;
      flex: 1;
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
    i {
      align-self: center;
      font-size: 28px;
      color: #8c939d;
    }
  }
}
</style>

<script>
import { ServerAPI } from "~/assets/scripts/API";
import { ShowMessage } from "~/assets/scripts/Functions";
import { InputFileType } from "./InputFIleModel";
export default {
  props: {
    model: Object,
    value: String | Array,
    label: String,
    labelWidth: Number,
    disabled: Boolean,
  },
  data() {
    return {
      APISaveFileToServer: ServerAPI.APISaveFileToServer,
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
    placeholder: function () {
      let str = `Đã chọn ${this.totalsFile} files, click để chọn thêm`;
      if (this.model.limit) str += ` (tối đa ${this.model.limit} files)`;

      return str;
    },
  },
  watch: {},
  methods: {
    tmp() {
      //console.log(this.uploadFileList);
    },
    isShowDefault() {
      return this.model.type == InputFileType.default;
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
        return `background-image: url("${href}");`;
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
              rs(_filelist.map((p) => p.name));
            }
          }, 250);
        });
      } else {
        return new Promise(function (rs) {
          rs([_app.uploadFileList]);
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

      // //console.log(response, file, fileList);
    },
    UploadDone(fileList) {
      //   //console.log(this.fileList, fileList);
      this.$emit(
        "input",
        fileList.map((p) => p.name)
      );
    },
    handleRemove(file, fileList) {
      this.totalsFile = fileList.length;
      //   //console.log(file, fileList);
    },
    handlePreview(file) {
      //   //console.log(file);

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
    handleChange(file, fileList) {
      this.uploadFileList = fileList;
      this.totalsFile = fileList.length;
      if (this.model.type == InputFileType.avatar) {
        if (fileList.length > 1) fileList.splice(0, 1);
      }
    },
    handleExceed(files, fileList) {
      if (this.model.type == InputFileType.avatar) {
        // //console.log(files);
        // //console.log(fileList);
        // //console.log(this);
      } else
        this.$message.warning(
          `Chỉ được chọn tối đa ${this.model.limit}, bạn đã chọn ${
            files.length
          } file, lên đến tổng số ${files.length + fileList.length} file`
        );
    },
    beforeRemove(file, fileList) {
      return this.$confirm(`Xác nhận xóa file ${file.name} ?`);
    },
  },
  mounted() {
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
        return {
          name: p,
          url: this.model.baseUrl + p,
        };
      });
    this.totalsFile = this.fileList.length;

    this.$nextTick(function () {
      this.uploadFileList = this.$refs.upload.$refs["upload-inner"].fileList;
    });
    // //console.log(this);
    // this.uploader = this.$refs.upload.$refs["upload-inner"];
  },
};
</script>
