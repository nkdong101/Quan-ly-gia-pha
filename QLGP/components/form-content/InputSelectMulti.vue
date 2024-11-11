<template>
  <div>
    <InputContainer :disabled="disabled" :label="label">
      <div class="main-content-c">
        <InputSelect
          v-if="!disabled"
          :disabled="disabled"
          :model="model"
          v-model="selectedValue"
          ref="input"
        />
        <div class="main-content" :disabled="disabled">
          <div
            class="main-content-item"
            v-for="item in data"
            :key="item[model.value]"
          >
            <span v-html="GetLabel(item)"> </span>
            <i class="el-icon-close" @click="Del(item)"></i>
          </div>
        </div>
      </div>
    </InputContainer>
  </div>
</template>


<script>
export default {
  props: {
    value: Array,
    label: String,
    model: Object,
    disabled: {
      type: Boolean,
      default: false,
    },
  },
  data() {
    return {
      data: this.value || [],
      selectedValue: "",
      selectedValue2: [],
    };
  },
  watch: {
    value() {
      if (Array.isArray(this.value)) this.data = this.value;
    },
    selectedValue(newValue, oldValue) {
      if (newValue) {
        this.$nextTick(() => {
          this.data.push(this.$refs.input.selectedData);
          this.selectedValue = null;
        });
      }
    },
    data: {
      deep: true,
      handler() {
        this.model.exclude = this.data.map((p) => p[this.model.value]);
        this.$emit("input", this.data);
      },
    },
  },
  methods: {
    GetLabel(item) {
      if (this.model.labelFormat) {
        return this.model.labelFormat(item);
      } else
        switch (typeof this.model.label) {
          case "function":
            return this.model.label(item);
          case "string":
            return item[this.model.label];
          default:
            return item.Name;
        }
    },
    Del(item) {
      this.data.splice(this.data.indexOf(item), 1);
    },
  },
  mounted() {},
};
</script>

<style lang="scss" scoped>
@import "~@/assets/css/variable.scss";
.main-content-c {
  .main-content {
    &[disabled] {
      max-height: 81px;
    }
    &:not([disabled]) {
      height: 81px;
    }
    border: 1px solid #ddd;
    border-top: 0;
    // border-top-right-radius: 4px;
    border-bottom-right-radius: 4px;
    overflow: auto;
    &[disabled] {
      background: #f5f7fa;
      i {
        visibility: hidden;
      }
      color: blue;
    }
    .main-content-item {
      padding: 5px 5px;
      border-bottom: 1px solid #ddd;
      position: relative;
      text-transform: capitalize;
      padding-right: 26px;
      word-break: break-word;
      // &:last-child {
      //   border-bottom: 0;
      // }
      i {
        position: absolute;
        right: 0;
        height: 100%;
        top: 0;
        width: 26px;
        /* font-size: 10px; */
        display: flex;
        align-items: center;
        justify-content: center;
        color: red;
        cursor: pointer;
        &:hover {
          background: #f8931d;
          color: white;
        }
      }
    }
  }
  ::v-deep  input {
    border-bottom-right-radius: 0;
  }
}
</style>
