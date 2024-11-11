<template>
  <div class="el-input el-input--mini">
    <el-tooltip
      effect="dark"
      :content="model.errorText"
      placement="top-start"
      :disabled="!showError"
    >
      <masked-input
        :disabled="disabled"
        ref="maskInput"
        class="el-input__inner"
        :placeholder="model.placeholder"
        :mask="model.mask"
        :pipe="model.pipe"
        :keepCharPositions="model.keepCharPositions"
        v-model.lazy="maskedValue"
        @blur="handleBlur"
        @focus="handleFocus"
        @keydown.native.enter="handleEnter"
        :isValid="showError"
      />
    </el-tooltip>
  </div>
</template>

<script>
export default {
  props: {
    value: String | Number,
    model: Object,
    disabled: Boolean | String,
  },
  data() {
    return {
      maskedValue: "",
      lastCommitValue: "",
      validMask: true,
      showError: false,
    };
  },
  methods: {
    handleEnter() {
      if (this.model.handleEnter)
        this.model.handleEnter(this.maskedValue, (newValue) => {
          this.maskedValue = newValue;
        });
    },
    handleBlur() {
      this.showError = !this.validMask;
    },
    handleFocus(evt) {
      // evt.target.select();
    },
    checkFormat: function () {
      if (!this.maskedValue) return true;
      var patten = this.$refs.maskInput.mask(this.maskedValue);
      if (patten.length != this.maskedValue.length) return false;
      let values = this.maskedValue.split("");
      for (let i = 0; i < values.length; i++) {
        const p = values[i];
        let check = new RegExp(patten[i]).test(p);
        if (!check) return false;
      }
      if (this.model.checkFormat)
        return this.model.checkFormat(this.maskedValue);
      return true;
    },
  },
  watch: {
    maskedValue() {
      if (this.checkFormat()) {
        this.lastCommitValue = this.maskedValue;
        this.validMask = true;
      } else {
        this.validMask = false;
        this.lastCommitValue = "";
      }
      this.handleBlur();
      this.$emit("input", this.lastCommitValue);
    },
    value() {
      if (this.lastCommitValue != this.value) {
        this.maskedValue = this.value;
        this.$refs.maskInput.updateValue(this.value);
      }
    },
  },
  mounted() {
    this.maskedValue = this.value;
    // console.log(this);
  },
};
</script>

<style lang="scss" scoped>
input[isValid] {
  border-color: #f56c6c;
}

input:disabled {
  background-color: #f5f7fa;
  border-color: #e4e7ed;
  color: #c0c4cc;
  cursor: not-allowed;
}
</style>
