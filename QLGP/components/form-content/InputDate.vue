<template>
  <div class="input-date-c">
    <InputMask
      v-if="!isShowDatePicker"
      :model="MaskCfg.dateMask"
      v-model="dataMask"
      :disabled="disabled"
      @keydown.native.enter="ValidateInput()"
      class="input-date"
    />
    <el-button
      class="pick-date"
      @click="ShowDatePicker(true)"
      v-if="!isShowDatePicker && !disabled"
      ><i class="fa fa-calendar"></i
    ></el-button>
    <el-date-picker
      ref="entry"
      v-if="isShowDatePicker"
      v-model.lazy="data_picker"
      :disabled="disabled"
      type="date"
      format="dd/MM/yyyy"
      :picker-options="options"
      @blur="ShowDatePicker(false)"
      @change="SetDatePicker()"
    ></el-date-picker>
  </div>
</template>

<script>
import moment from "moment";
import ConvertStr from "~/assets/scripts/ConvertStr";
import DatePickerOption from "~/assets/scripts/base/DatePickerOption";
export default {
  props: {
    value: {
      type: String | Number,
    },
    disabled: {
      type: Boolean | String,
    },
    format: {
      type: String,
      default: "DD/MM/YYYY",
    },
  },
  data() {
    return {
      data_picker: "",
      isShowDatePicker: false,
      dataMask: "",
      defaultValue: null,
      options: new DatePickerOption(),
    };
  },
  methods: {
    ValidateInput() {},
    focus() {
      this.$el.querySelector("input").focus();
    },
    SetDatePicker() {
      this.$emit("input", this.data_picker);
    },
    ShowDatePicker(isShow) {
      this.isShowDatePicker = isShow;
      this.$nextTick(() => {
        if (isShow && this.$refs.entry) this.$refs.entry.showPicker();
      });
    },
    ProcessDataMask() {
      if (this.dataMask) {
        let date = moment(this.defaultValue);
        this.$emit(
          "input",
          moment(this.dataMask, this.format)
            .hour(0)
            .minute(0)
            .second(0)
            .toJSON()
        );
      } else {
        this.$emit("input", null);
      }
    },
  },
  watch: {
    dataMask: function () {
      this.ProcessDataMask();
    },
    value: function (ov, nv) {
      if (ov != nv) {
        // if (this.dataMask != ConvertStr.ToDateStr(this.value, this.format)) {
        this.defaultValue = this.value;
        this.dataMask = ConvertStr.ToDateStr(this.value, this.format);
        this.ProcessDataMask();
        // }
        if (ConvertStr.ToDateStr(this.value)) this.data_picker = this.value;
      }
    },
  },
  mounted: function () {
    if (this.value) {
      this.defaultValue = this.value;
      this.dataMask = ConvertStr.ToDateStr(this.value, this.format);
    }
    if (ConvertStr.ToDateStr(this.value)) this.data_picker = this.value;
  },
};
</script>

<style lang="scss" scoped>
.input-date-c {
  display: flex;
  flex: 1;
  // background: red;
  .input-date {
    ::v-deep  input {
      border-top-right-radius: 0;
      border-bottom-right-radius: 0;
    }
  }
  .pick-date {
    padding: 0;
    width: 24px;
    border-top-left-radius: 0;
    border-bottom-left-radius: 0;
    display: flex;
    align-items: center;
    justify-content: center;
    border-left: 0;
    outline: none;
    border-color: #dcdfe6 !important;
  }
}
</style>
