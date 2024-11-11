<template>
  <el-select
    v-if="loaded"
    ref="input"
    v-model="valueSelect"
    filterable
    clearable
    :allow-create="model.allowCreate"
    :default-first-option="true"
    :popper-append-to-body="true"
    :popper-class="customClass"
    :placeholder="placeholder"
    :disabled="disabled"
    :multiple="model.multiple"
    :reserve-keyword="model.multiple"
    :remote="model.remote"
    :remote-method="model.remote ? remoteMethod : undefined"
    :filter-method="onFilterMethod"
    :loading="loading"
    @clear="onClear"
    @change="abc"
    @blur="handerBlur"
    style="min-width: none"
    :data-length="getOptionsProcess.length"
  >
    <el-option
      v-for="item in getOptionsProcess"
      :key="resolve(item, selectKey)"
      :label="
        selectLabel
          ? GetLabel(item)
          : labelFormat
          ? labelFormat(item)
          : item.Name
      "
      :value="resolve(item, selectValue)"
      :disabled="IsDisabled(item)"
    >
      <span v-html="labelFormat ? labelFormat(item) : GetLabel(item)"></span>
      <span
        style="float: right; color: #8492a6; font-size: 13px; padding-left: 5px"
      >
        <i
          v-if="valueSelect == resolve(item, selectValue)"
          class="fa fa-check"
          style="color: black"
        ></i>
      </span>
    </el-option>
    <el-option
      v-if="getOptionsProcess.length < options.length"
      :disabled="true"
      :value="null"
      label="Nhập thêm để tìm kiếm"
    >
    </el-option>
  </el-select>
</template>

<script>
import GetDataAPI from "@/assets/scripts/GetDataAPI";
import { Uni2None } from "~/assets/scripts/Functions";
export default {
  props: {
    value: String | Number | Array,
    model: Object,
    showAll: {},
    disabled: {
      type: Boolean,
      default: false,
    },
    nonFocusNextOnEnter: {
      type: Boolean,
      default: false,
    },
  },
  data() {
    return {
      valueSelect: "",
      options: [],
      optionsProcess: [],
      selectKey: this.model.key || this.model.value,
      selectLabel: this.model.label,
      selectValue: this.model.value,
      labelFormat: this.model.labelFormat,
      firstLoad: true,
      placeholder: this.model.placeholder,
      loading: false,
      loaded: true,
      customClass: "",
      inputvalue: "",
    };
  },
  methods: {
    handerBlur() {
      setTimeout(() => {
        if (this.model.allowCreate) {
          this.valueSelect = this.inputvalue;
          this.abc();
        } else this.onFilterMethod("");
      }, 500);
    },
    IsDisabled(item) {
      if (this.model.IsItemDisabled) return this.model.IsItemDisabled(item);

      return item.disabled;
    },
    abc() {
      // console.log(this.valueSelect);
      this.$emit("input", this.valueSelect);
      this.$nextTick(() => {
        this.$emit("change");
      });
    },
    onClear() {
      this.onFilterMethod("");
    },
    GetLabel: function (item) {
      switch (typeof this.model.label) {
        case "function":
          return this.model.label(item);
          break;
        case "string":
          return this.resolve(item, this.model.label);
          break;
        default:
          return item.Name;
          break;
      }
    },
    onFilterMethod(input) {
      this.inputvalue = input;
      let count = 0;
      let data = [];
      for (let i = 0; i < this.options.length; i++) {
        const p = this.options[i];
        if (count > 100) break;
        let value =
          this.GetLabel(p) +
          " - " +
          (typeof this.model.labelFormat == "function"
            ? this.model.labelFormat(p)
            : p[this.model.labelFormat]);
        let check =
          Uni2None(value.toLowerCase()).indexOf(
            Uni2None((input || "").toLowerCase())
          ) > -1 || this.model.remote;
        if (check) {
          data.push(p);
          count += 1;
        }
      }
      this.optionsProcess = data;
      // console.log("filter");
      // this.optionsProcess = this.options.filter((p) => {
      //   if (count > 100) return false;
      //   let check =
      //     Uni2None((p[this.model.label] || "").toLowerCase()).indexOf(
      //       Uni2None((input || "").toLowerCase())
      //     ) > -1;
      //   if (check) count += 1;
      //   return check;
      // });
      return [];
    },
    focus() {
      this.$nextTick(function () {
        this.$refs.input.focus();
      });
    },
    LoadData(callback) {
      var _app = this;
      this.firstLoad = false;
      if (this.model.validate) {
        var validate = this.model.validate({ ...this.model.params });
        if (validate) {
          this.placeholder = validate;
          return;
        }
      } else {
        this.placeholder = this.model.placeholder;
      }
      _app.loading = true;
      GetDataAPI({
        isWait: false,
        url: this.model.data,
        params: { ...this.model.params },
        action(re) {
          _app.InitData(re);
          _app.$nextTick(function () {
            if (callback) callback();
            _app.loading = false;
          });
        },
      });
    },
    remoteMethod: function (query) {
      if (query || true) {
        if (this.model.remoteParamsHandle)
          this.model.remoteParamsHandle(query, this.model).then(() => {
            this.LoadData();
          });
        else {
          this.model.params.iSearchInfo = query;
          this.model.params.iRecordOfPage = 100;
          this.model.params.iPage = 0;
          this.LoadData();
        }
      } else {
        this.InitData([]);
      }
    },
    ProcessData() {
      let length = this.optionsProcess.length;
      // console.log("ProcessData1");
      // console.log(this.optionsProcess.length);
      if (length > 100) {
        if (this.value) {
          if (Array.isArray(this.value)) {
            this.optionsProcess = this.optionsProcess
              .filter(
                (p) =>
                  this.value.some(
                    (p1) => p[this.model.value] == p1[this.model.value]
                  ) || this.model.remote
              )
              .slice(0, 100);
          } else {
            this.optionsProcess = this.optionsProcess
              .filter(
                (p) => this.value == p[this.model.value] || this.model.remote
              )
              .slice(0, 100);
          }
        } else this.optionsProcess = this.optionsProcess.slice(0, 100);

        // console.log("ProcessData");
        // console.log(this.optionsProcess.length);
        // if (this.optionsProcess.length < length) {
        //   let extObj = {};
        //   extObj[this.model.label || "Name"] = "Nhập thêm để tìm kiếm";
        //   extObj[this.model.value] = "Nhập thêm để tìm kiếm";
        //   extObj.disabled = true;
        //   this.optionsProcess.push(extObj);
        // }
      }
    },
    InitData(data) {
      if (Array.isArray(data)) this.options = data;
      else if (typeof data == "object") this.options = data.Data;
      if (this.model.dataExt) {
        this.options = [...this.model.dataExt, ...this.options];
      }

      this.optionsProcess = this.options;
      // console.log("InitData", this.options);
      this.ProcessData();

      if (
        this.options.length == 1 &&
        !this.value &&
        !this.model.remote &&
        !this.disabled
      ) {
        this.valueSelect = this.options[0][this.model.value];
        this.$emit("input", this.valueSelect);
      }
    },
    Init() {
      let _app = this;
      return new Promise(function (rs) {
        if (!_app.model.remote) {
          if (Array.isArray(_app.model.data)) {
            _app.InitData(_app.model.data);
            rs();
          } else if (typeof _app.model.data == "string") {
            _app.LoadData(function () {
              rs();
            });
          }
        } else {
          _app.InitData(_app.model.remoteValue);
          rs();
        }
      });
    },
    Loaded() {
      let _app = this;
      let _value = this.value || "";
      this.customClass = this.$el.id;
      // console.log(this);
      this.Init().then(() => {
        _app.$nextTick(() => {
          if (_app.$refs.input) {
            if (
              !Array.isArray(_value) &&
              !_app.options.some(
                (p) => this.resolve(p, _app.selectValue) == _value
              )
            )
              _value = "";
            if (_app.$refs.input.$el.querySelector(".is-focus")) {
              _app.$refs.input.blur();

              _app.valueSelect = _value;
              _app.focus();
            } else {
              _app.valueSelect = _value;
            }

            if (!_value) {
              if (_app.optionsProcess.length) {
                if (this.model.selectFirst && _app.optionsProcess.length >= 1) {
                  if (!this.disabled) {
                    let find = _app.optionsProcess[0];
                    _value = this.resolve(find, _app.selectValue);
                    _app.$emit("input", _value);
                  }
                } else {
                  if (_app.optionsProcess.length == 1) {
                    if (!this.disabled) {
                      let find = _app.optionsProcess[0];
                      _value = this.resolve(find, _app.selectValue);
                      _app.$emit("input", _value);
                    }
                  }
                }
              }
            }
          }
        });
      });
      if (this.nonFocusNextOnEnter) {
        this.$el
          .querySelector("input")
          .setAttribute("nonFocusNextOnEnter", true);
      }
    },
    LoadselectFirst() {
      let _app = this;
      _app.valueSelect = this.value;
      this.Init().then(() => {
        let _value = _app.valueSelect;
        if (!_value) {
          if (_app.optionsProcess.length) {
            if (this.model.selectFirst && _app.optionsProcess.length >= 1) {
              let find = _app.optionsProcess[0];
              _value = this.resolve(find, _app.selectValue);
              _app.$emit("input", _value);
            }
          }
        }

        // console.log("this", this.optionsProcess);
      });
    },
  },
  computed: {
    getOptionsProcess() {
      let data = [];
      if (!this.model.remote)
        data = this.optionsProcess.filter((p) => {
          if (!p) return false;
          // console.log(this.showAll);
          if (p.Use == 2 && !this.showAll) return false;

          return true;
        });
      else data = this.optionsProcess;

      // console.log(this, data);
      return data;
    },
    selectedData: function () {
      return this.options.filter(
        (p) => this.resolve(p, this.selectValue) == this.value
      )[0];
    },
  },
  mounted() {
    // let _app = this;
    // _app.valueSelect = this.value;
    this.Loaded();
    if (this.model.remote) {
      this.remoteMethod("");
    } else {
    }
    // this.LoadselectFirst();
    // this.Init().then(() => {
    //   let _value = _app.valueSelect;
    //   if (!_value) {
    //     if (_app.optionsProcess.length) {
    //       if (this.model.selectFirst && _app.optionsProcess.length == 1) {
    //         let find = _app.optionsProcess[0];
    //         _value = find[_app.selectValue];
    //         _app.$emit("input", _value);
    //       }
    //     }
    //   }

    // console.log("this", this.optionsProcess);
    // });
    // console.log("this", this);
  },
  watch: {
    "model.data": {
      deep: true,
      handler(val, oldVal) {
        if (!this.model.remote) {
          if (!Object.equals(val, oldVal)) {
            if (!this.firstLoad) this.$emit("input", "");
            // if (oldVal[0]) {
              this.Init();
            // }
          }
        }
      },
    },
    "model.params": {
      deep: true,
      handler(val, oldVal) {
        if (!this.model.remote) {
          if (!Object.equals(val, oldVal)) {
            if (!this.firstLoad) this.$emit("input", "");
            if (Array.isArray(this.model.data)) {
            } else {
              this.LoadData();
            }
          }
        }
      },
    },
    model() {
      // console.log(this);
      if (this.model) {
        this.loaded = false;
        this.$nextTick(() => {
          this.loaded = true;
          this.$nextTick(() => {
            if (this.model.remote) {
              this.Loaded();
            }
          });
        });
      }
    },
    value: {
      handler(val, oldVal) {
        if (this.valueSelect !== val) {
          this.valueSelect = val || "";
          // console.log("select_change", val, oldVal);
        }
        // if (!val) {
        this.onFilterMethod("");
        // }
      },
    },
    optionsProcess() {
      this.$nextTick(() => {
        setTimeout(() => {
          // console.log(this);
          try {
            if (this.$refs) {
              this.$refs.input.$refs.popper.updatePopper();
            }
          } catch (error) {}
        }, 50);
      });
    },
  },
};
</script>


<style lang="scss" scoped>
.el-select {
  min-width: 0 !important;
}
::v-deep  .is-disabled {
  .el-input__suffix {
    display: none;
  }
}

::v-deep  .el-input__inner {
  padding-right: 30px !important;
}
</style>
