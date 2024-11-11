<template>
  <el-input v-model.lazy="data"
            :placeholder="placeholder"
            :disabled="disabled"
            @blur="blur()"
            @focus="focus()"
            :is-focus="isFocus" />
</template>

<script>
import ConvertStr from "~/assets/scripts/ConvertStr";
export default {
  // props: {
  //   /** @type {Number|String} - description */
  //   value: {
  //     type: Number | String,
  //     def
  //   },
  //   /** @type {Boolean} - description */
  //   disabled: {
  //     type: Boolean,
  //   },
  //   /** @type {String} - description */
  //   placeholder: {
  //     type: String,
  //   },
  // },
  props: {
    value: {},
    disabled: {
      type: Boolean,
    },
    decimal_alway: {
      type: Boolean,
    },
    placeholder: {
      type: String,
    },
    negative_allowed: {
      type: Boolean,
      default: true
    },
  },
  data() {
    return { data: "", isFocus: false };
  },
  watch: {
    data(nv, ov) {
      this.data = this.getNumberFormat(nv);
      if (this.data == "-") {
        // this.$emit("input", ConvertStr.ToNumber(this.data || 0));
      } else {
        this.$emit("input", ConvertStr.ToNumber(this.data || 0));
        this.$emit("change");
      }

      // this.setData(+(ConvertStr.ToNumber(number) + "." + decimal));

      // if (this.data.length == 1 && this.data == "-") {
      //   this.$emit("input", 0);
      // } else if (this.data.length == 2 && this.data == "-0") {
      // } else if (
      //   this.data.split(".").length == 2 &&
      //   this.data[this.data.length - 1] == "."
      // ) {
      // } else {
      //   this.setData(ConvertStr.ToNumber(this.data));
      //   this.$emit("input", ConvertStr.ToNumber(this.data) || 0);
      // }
    },
    value(val, oldVal) {
      if (this.data != val) {
        this.setData(val, this.decimal_alway);
      }
    },
  },
  methods: {
    getNumberFormat(val, decimal_alway) {
      // console.log(nv);
      // console.log(ConvertStr.ToMoneyStr(nv));
      if (val || val === 0) val = val + "";

      if (val != "0") val = val || "";
      // val = val + "";
      let so_am = false;
      if (val[0] == "-" && this.negative_allowed !== false) {
        so_am = true;
        val = val.replace(/-/g, "");
      }
      // nv = nv.replace(/[^0-9.]/g, "");
      if (val == "") {
        if (so_am) {
          return "-";
        } else {
          return "";
        }
      }

      let splt = val.split(".");
      let userDecima = false;
      if (val[val.length - 1] == ".") {
        if (splt.length == 2) {
          userDecima = true;
        } else {
          // val = ov;
          return this.data;
        }
      }
      // if (this.data.length == 1 && this.data == "0") {
      //   this.$emit("input", 0);
      //   return;
      // }
      // this.data = ConvertStr.ToMoneyStr(nv.replace(/[^0-9]/g, ""));
      // console.log(this.data);
      let number = splt[0].replace(/[^0-9]/g, "");
      let decimal = (splt[1] || "").replace(/[^0-9]/g, "") || "";
      number = ConvertStr.ToMoneyStr(number) || 0;
      if (userDecima || decimal) {
        if (!decimal_alway) number += "." + decimal;
        else number += "." + decimal.substring(0, 2).padEnd(2, 0);
      } else if (decimal_alway && !this.isFocus) {
        number += ".00";
      }

      if (so_am) {
        number = "-" + number;
      }
      return number + "";
    },
    setData(val, decimal_alway) {
      this.data = this.getNumberFormat(val, decimal_alway);
      // ConvertStr.ToMoneyStr(val);
    },
    focus() {
      this.isFocus = true;
    },
    blur() {
      this.isFocus = false;
      if (this.value && this.decimal_alway) {
        this.data = this.getNumberFormat(this.value, this.decimal_alway);
      }
      this.$emit("blur");
    },
  },
  mounted() {
    this.setData(this.value, this.decimal_alway);

    // console.log(this);
  },
};
</script>


<style lang="scss" scoped>
::v-deep  input {
  &:not([is-focus]) {
    text-align: right !important;
    padding-right: 5px !important;
  }
}
</style>
