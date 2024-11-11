<template>
  <tr :role="item.State">
    <td>{{ index + 1 }}</td>
    <td>
      <ValidatePR :baseValue="item.Name" ref="validateSerial">
        <el-input :id="item.IdTmp" v-model="item.Name" />
      </ValidatePR>
    </td>
    <td>
      <el-input v-model="item.Serial" />
    </td>
    <td>
      <el-input v-model="item.ContactName" />
    </td>
    <td>
      <el-input v-model="item.ContactEmail" />
    </td>
    <td>
      <el-input v-model="item.ContactPhone" />
    </td>
    <td style="vertical-align: middle; text-align: center">
      <div
        v-if="!filePicked && !item.Quote_Url"
        class="vendor-attach"
        @click="pickFile"
      >
        <i class="fa fa-upload"></i>
        <input
          ref="file"
          type="file"
          style="display: none"
          @change="handleFileChange"
        />
      </div>
      <div
        v-else
        class="vendor-attach"
        style="display: flex; justify-content: space-evenly"
      >
        <a
          :href="filePickedUrl || pathUpload + getFilePath(item.Quote_Url)"
          :download="getFileDownload(item.Quote_Url)"
          ><i class="fa fa-download"></i
        ></a>
        <i
          class="fa fa-trash"
          style="color: red; font-size: 16px"
          @click="removeFile"
        ></i>
      </div>
    </td>

    <td>
      <InputSelect
        :model="vendor_state_tmp"
        v-model="item.State"
        style="width: 100%"
      />
    </td>
    <td
      style="text-align: center; color: red; cursor: pointer"
      @click="vendorRemove(item)"
    >
      <i class="fa fa-times"></i>
    </td>
  </tr>
</template>

<script>
import APIHelper from "~/assets/scripts/API/APIHelper";
import { readFileInput } from "~/assets/scripts/Functions";
export default {
  props: {
    index: {},
    item: {},
    vendor_state_tmp: {},
  },
  data() {
    return {
      filePicked: null,
      filePickedUrl: null,
      pathUpload: "/Images/Purchase/Vendor_Attachment/",
    };
  },
  methods: {
    vendorRemove(item) {
      this.$emit("vendorRemove", item);
    },
    pickFile() {
      this.$refs.file.click();
    },
    removeFile() {
      this.filePicked = null;
      this.filePickedUrl = null;
      this.item.Quote_Url = null;
    },
    handleFileChange() {
      if (this.$refs.file.files.length) {
        console.log(this.$refs.file.files);
        this.filePicked = this.$refs.file.files[0];
        readFileInput(this.filePicked).then((re) => {
          this.filePickedUrl = re;
        });
      }
    },
    getValidate() {
      return new Promise((rs) => {
        Promise.all([this.$refs.validateSerial.getValidate()]).then((re) => {
          rs(re.every((p) => p));
        });
      });
    },
    submitUpload() {
      return new Promise((rs) => {
        if (this.filePicked)
          APIHelper.util
            .UploadFile({
              file: this.filePicked,
              path: this.pathUpload,
            })
            .then((re) => {
              this.item.Quote_Url = re;
              this.filePicked = null;
              this.filePickedUrl = null;
              rs();
            });
        else rs();
      });
    },
  },
};
</script>

<style lang="scss" scoped>
.vendor-attach {
  cursor: pointer;
}
</style>
