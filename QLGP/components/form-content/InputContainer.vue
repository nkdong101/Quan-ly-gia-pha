<template>
  <div
    :class="
      'el-input el-input--mini el-input-group input-c ' +
      (label ? 'el-input-group--prepend' : '')
    "
  >
    <div class="el-input-group__prepend" v-if="label">
      <slot name="label">
        <span
          :style="{
            display: 'block',
            width: labelWidth - 20 + 'px ',
     
          }"
          v-html="label"
        ></span>
      </slot>
    </div>
    <slot></slot>
  </div>
</template>

<script>
//  + (required ? ' <span style=\'color: red;\'>*</span>' : '')
export default {
  props: {
    label: {
      type: String,
      default: "label",
    },
    labelWidth: {
      type: Number,
      default: 125,
    },
    required: {
      type: Boolean,
      default: false,
    },
  },
};
</script>

<style lang="scss" scoped>

@media only screen and (max-width: 500px) {
  span{
    font-size:10px;
  }

}
.input-c {
  .el-input-group__append,
  .el-input-group__prepend {
    padding: 5px 10px;
    color: black;
    text-align: left;
    line-height: 13px;
    white-space: normal;
    word-break: break-word;
    vertical-align: top;
  }

  ::v-deep  .el-input__prefix {
    display: none;
  }
  ::v-deep  .el-date-editor.el-input,
  ::v-deep  .el-date-editor.el-input__inner {
    width: 100%;
    display: flex;
    align-items: center;
    ::v-deep  .el-range-input {
      width: 115px;
    }
    ::v-deep  .el-range-separator {
      padding: 0;
      width: 25px;
      display: flex;
      align-items: center;
    }
    ::v-deep  .el-input__icon {
      display: none;
    }
  }
  .el-date-editor.el-input,
  .el-date-editor.el-input__inner,
  .el-input-number {
    width: 100%;
    display: flex;
    align-items: center;
    ::v-deep  .el-range-input {
      width: 115px;
    }
    ::v-deep  .el-range-separator {
      width: 35px;
    }
    ::v-deep  .el-input__icon {
      display: none;
    }
  }
  ::v-deep  .el-textarea__inner {
    border-top-left-radius: 0;
    border-bottom-left-radius: 0;
  }
  ::v-deep  .el-select {
    width: 100%;
  }
  ::v-deep  .el-input {
    // &:not(.el-date-editor) {
    input {
      padding: 0 5px;
    }
    // }
  }
  ::v-deep .el-input.is-disabled .el-input__inner {
    color: #000000;
  }
  ::v-deep .el-textarea__inner[disabled] {
    color: #000000;
  }
  ::v-deep  .el-textarea {
    textarea {
      padding: 5px 5px;
    }
  }
  ::v-deep  .el-checkbox-group,
  ::v-deep  .el-radio-group {
    border: 1px solid #dcdfe6;
    border-top-right-radius: 4px;
    border-bottom-right-radius: 4px;
    display: flex;
    align-items: center;
    flex-wrap: wrap;
    padding: 0 5px;
    min-height: 28px;

    label {
      margin: 0px;
      margin-right: 5px;
      line-height: 28px;
      display: flex;
      align-items: center;
      :last-of-type {
        margin-right: 0px;
      }
      .el-checkbox__label,
      .el-radio__label {
        font-size: 12px;
        padding-left: 5px;
      }
    }
  }
}
</style>
