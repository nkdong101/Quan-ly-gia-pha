<template>
  <el-tooltip
    v-if="isVisible"
    :disabled="!validate()"
    :is-not-valid="!!validate()"
    :message="validate()"
    placement="top-start"
    effect="light"
    popper-class="iValid-class"
    :enterable="false"
    :tabindex="-1"
  >
    <div slot="content" v-html="validate()"></div>
    <el-col
      :id="'div_' + model.id"
      :span="model.col || 24"
  
      :xs="model.colsm || 24"
    
      v-if="!model.child && isLoaded && isVisible"
      :class="'form-element-type-' + model.type + ' ' + model.class"
      style="position: relative"
    >
      <slot :name="model.id" class="has-border" :disabled="disabled">
        <InputFile
          v-if="IsFile()"
          :label="model.label"
          :labelWidth="model.labelWidth || formInfo.labelWidth"
          :model="options"
          :id="model.id"
          ref="entry"
          v-model.lazy="elementValue"
          :disabled="disabled"
          v-on="model.events"
        />
        <div v-else-if="isLabel()" class="form-title" :style="model.style">
          {{ model.label }}
        </div>
        <component
          v-else-if="isCustom()"
          v-bind:is="model.type"
          v-bind="model.attr"
          :style="model.style"
          :label="model.label"
          :labelWidth="model.labelWidth || formInfo.labelWidth"
          :model="options"
          :id="model.id"
          ref="entry"
          @onChange="componentsValueChange"

          v-model.lazy="elementValue"
          :disabled="disabled"
        ></component>
        <quill-editor
          ref="entry"
          v-else-if="isQuillEditor()"
          v-model.lazy="elementValue"
          :options="model.attr"
          class="quill-container"
        />
        <!-- model.model+' - '+ -->
        <InputContainer
          v-else
          :label="model.label"
          :labelWidth="model.labelWidth || formInfo.labelWidth"
          :required="model.required"
        >
          <InputSelect
            v-if="IsSelect()"
            :model="options"
            :id="model.id"
            ref="entry"
            v-model.lazy="elementValue"
            :disabled="disabled"
            v-bind="model.attr"
          />
          <InputDate
            v-else-if="IsDate()"
            :id="model.id"
            ref="entry"
            v-model.lazy="elementValue"
            :disabled="disabled"
          />
          <el-checkbox
            v-else-if="IsCheckbox()"
            class="form-checkbox"
            v-model.lazy="elementValue"
            :disabled="disabled"
          ></el-checkbox>
          <InputNumber
            v-else-if="IsNumber()"
            :id="model.id"
            ref="entry"
            v-model.lazy="elementValue"
            :disabled="disabled"
          />

          <el-date-picker
            v-else-if="IsDatePicker()"
            :id="model.id"
            ref="entry"
            v-model.lazy="elementValue"
            :disabled="disabled"
            type="date"
            :format="model.attr.format || 'dd/MM/yyyy'"
            :picker-options="options"
            :placeholder="disabled ? '' : model.attr.format || 'dd/MM/yyyy'"
          ></el-date-picker>

          <el-date-picker
            v-else-if="IsDateTimePicker()"
            :id="model.id"
            ref="entry"
            v-model.lazy="elementValue"
            :disabled="disabled"
            type="datetime"
            format="dd/MM/yyyy HH:mm"
            :picker-options="options"
            placeholder="DD/MM/YYYY HH:mm"
          ></el-date-picker>

          <el-input
            v-else
            :id="model.id"
            ref="entry"
            v-bind="attr"
            v-model.lazy="elementValue"
            :disabled="disabled"
            v-on="model.events"
             @change="change(model, formInfo)"
              @blur="blur(model, formInfo)"
          />
        </InputContainer>
        <!-- @blur="$emit('blur', model, formInfo)" -->
        <!-- <div>{{ model.model }}</div> -->
      </slot>
    </el-col>
    <el-col
      :id="'div_' + model.id"
      :span="model.col || 24"
      v-else-if="isInline() && isVisible"
      :class="'form-element-child-horizontal ' + model.parent_class"
      style="display: flex; padding: 0px"
    >
      <template v-if="model.label">
        <div style="width: 100%; padding: 0 2.5px" :class="model.class">
          <ContentWithTitle :title="model.label">
            <el-row :gutter="5">
              <FormElement
                v-for="item in model.child.filter((p) => p)"
                v-bind:key="item.id"
                :ref="item.id"
                :model="item"
                :formInfo="formInfo"
                :disabledformInfo="disabledformInfo"
                v-on="item.events"
              >
                <!-- <slot :slot="item.id" :name="item.id"> </slot> -->
                <slot
                  :slot="slot"
                  :name="slot"
                  class="has-border"
                  v-for="slot in item.getAllSlotChild()"
                ></slot>
              </FormElement>
            </el-row>
          </ContentWithTitle>
        </div>
      </template>
      <template v-else>
        <FormElement
          v-for="item in model.child.filter((p) => p)"
          v-bind:key="item.id"
          :ref="item.id"
          :model="item"
          :formInfo="formInfo"
          :disabledformInfo="disabledformInfo"
          v-on="item.events"
          :class="model.class"
        >
          <!-- <slot :slot="item.id" :name="item.id"> </slot> -->
          <slot
            :slot="slot"
            :name="slot"
            class="has-border"
            v-for="slot in item.getAllSlotChild()"
          ></slot>
        </FormElement>
      </template>
    </el-col>
    <el-col
      :id="'div_' + model.id"
      :span="model.col || 24"
      v-else-if="isVisible"
      :class="'form-element-child-vertical '"
      style="display: flex; flex-direction: column"
    >
      <!-- <FormElement
      v-for="item in model.child"
      v-bind:key="item.id"
      :ref="item.id"
      :model="item"
      :formInfo="formInfo"
    >
      <slot :slot="item.id" :name="item.id"> </slot>
        </FormElement>-->
      <template v-if="model.label">
        <el-row :gutter="5" :class="model.class">
          <el-col :span="24">
            <ContentWithTitle :title="model.label">
              <el-row
                :gutter="5"
                style="display: flex"
                v-for="item in model.child.filter((p) => p)"
                v-bind:key="item.id"
              >
                <FormElement
                  :ref="item.id"
                  :model="item"
                  :formInfo="formInfo"
                  v-on="item.events"
                  v-if="item"
                  :disabledformInfo="disabledformInfo"
                >
                  <!-- <slot :slot="item.id" :name="item.id"> </slot> -->
                  <slot
                    :slot="slot"
                    :name="slot"
                    class="has-border"
                    v-for="slot in item.getAllSlotChild()"
                  ></slot>
                </FormElement>
              </el-row>
            </ContentWithTitle>
          </el-col>
        </el-row>
      </template>
      <template v-else>
        <el-row
          :gutter="5"
          style="display: flex"
          v-for="item in model.child.filter((p) => p)"
          v-bind:key="item.id"
          :class="model.class"
        >
          <FormElement
            :ref="item.id"
            :model="item"
            :formInfo="formInfo"
            v-on="item.events"
            v-if="item"
            :disabledformInfo="disabledformInfo"
          >
            <!-- <slot :slot="item.id" :name="item.id"> </slot> -->
            <slot
              :slot="slot"
              :name="slot"
              class="has-border"
              v-for="slot in item.getAllSlotChild()"
            ></slot>
          </FormElement>
        </el-row>
      </template>
    </el-col>
  </el-tooltip>

  <!-- </el-row> -->
</template>
<script>
import moment from "moment";

import DatePickerOption from "~/assets/scripts/base/DatePickerOption";
import {
  FormInfo,
  FormElement,
  FormElementType,
  FormDirectionType,
} from "~/assets/scripts/base/FormInfo";
import {
  GetTimeNow,
  IsAfterDate,
  resolve,
  resolveSet,
} from "~/assets/scripts/Functions";

export default {
  props: {
    model: Object,
    formInfo: Object,
    disabledformInfo: {},
  },
  data() {
    return {
      elementValue: "",
      isLoaded: true,
      validateStr: "",
      checkTime: GetTimeNow(),
    };
  },
  methods: {
    isCustom() {
      let typeObj = FormElementType;
      return !typeObj[this.model.type];
    },
    change(model, formInfo) {
      // console.log(model,formInfo);
      this.$nextTick(() => {
        // alert(this.GetModelValue());
        if (model.events.change) {
          model.events.change(formInfo.formData, this.GetModelValue(), this);
        }
      });
    },
    blur(model, formInfo) {
      // console.log(model,formInfo);
      this.$nextTick(() => {
        if (model.events.blur) {
          model.events.blur(formInfo.formData, this.GetModelValue(), this);
        }
      });
    },
    isLabel() {
      return this.model.type == FormElementType.label;
    },
    isQuillEditor() {
      return this.model.type == FormElementType.quillEditor;
    },
    isInline() {
      return this.model.direction == FormDirectionType.horizontal;
    },
    IsDatePicker() {
      return this.model.type == FormElementType.datePicker;
    },
    IsDateTimePicker() {
      return this.model.type == FormElementType.dateTimePicker;
    },
    IsFile() {
      return this.model.type == FormElementType.file;
    },
    IsNumber() {
      return this.model.type == FormElementType.number;
    },
    IsCheckbox() {
      return this.model.type == FormElementType.checkbox;
    },
    IsSelect() {
      return this.model.type == FormElementType.select && this.options;
    },
    IsDate() {
      return this.model.type == FormElementType.date;
    },
    GetModelValue() {
      var value = resolve(this.formInfo.formData, this.model.model);
      if (this.IsDatePicker() || this.IsDateTimePicker()) {
        var checkDate = moment(value);
        if (checkDate.isValid() && checkDate.toDate().getFullYear() > 1900) {
        } else {
          return "";
        }
      }

      return value;
    },
    SetModelValue(value) {
      if (value instanceof Date) {
        value = value.toJSON();
      }
      let result = resolveSet(this.formInfo.formData, this.model.model, value);
      // this.validate();
      return result;
    },
    getElement(id) {
      return this.formInfo.vm.getElement(id).model;
    },
    getEntry(id) {
      return this.formInfo.vm.getEntry(id);
    },

    componentsValueChange(model, newValue, oldValue) {
      // console.log(model, newValue, oldValue);
      if (this.model.watch) {
        this.model.watch(
          this.formInfo.formData,
          newValue,
          oldValue,
          this,
          false,
          model
        );
      }
    },

    onChange() {
      return this.formInfo.vm.onChange();
    },
    validate() {
      let a = this.checkTime;
      if (this.formInfo.vm && !this.formInfo.vm.needValidate) {
        return "";
      }

      if (!this.isVisible || this.disabled) return "";
      if (this.model.required) {
        let message = "is required";
        switch (this.model.type) {
          case FormElementType.select:
            if (!this.GetModelValue() && this.GetModelValue() !== false)
              return `<b>${
                this.model.label || "This field"
              }</b> has not picked`;
            break;
          case FormElementType.groupCheck:
            if (this.options.multiple) {
              if (!this.GetModelValue().length)
                return `<b>${
                  this.model.label || "This field"
                }</b> has not picked`;
            } else {
              if (!this.GetModelValue())
                return `<b>${
                  this.model.label || "This field"
                }</b> has not picked`;
            }

            break;

          default:
            if (!this.GetModelValue())
              return `<b>${this.model.label || "This field"}</b> is required`;
            break;
        }
      }

      if (this.IsDatePicker()) {
        // if (IsAfterDate(this.elementValue, new Date())) {
        //   return "Do not enter more than the current date.";
        // }
      }

      if (this.model.validate) {
        return this.model.validate(this.formInfo.formData, this);
      }
    },
    validateFormInfo() {
      if (this.model.type == "FormInfo") {
        if (this.formInfo && this.formInfo.vm.getEntry(this.model.id)) {
          let v = this.formInfo.vm.getEntry(this.model.id).getValidate();
          return v;
        }
      }
    },
  },

  computed: {
    formMobileResponsiveClass() {
      return "";
    },
    modelProcess() {},
    disabled() {
      if (this.model.forceDisabled) {
        switch (typeof this.model.forceDisabled) {
          case "function":
            if (this.model.forceDisabled(this.formInfo.formData, this) === true)
              return true;
            if (
              this.model.forceDisabled(this.formInfo.formData, this) === false
            )
              return false;
            break;
          default:
            if (this.model.forceDisabled === true) return true;
            if (this.model.forceDisabled === false) return false;
            break;
        }
      }
      if (this.disabledformInfo === true) {
        return true;
      }

      if (this.model.disabled) {
        switch (typeof this.model.disabled) {
          case "function":
            if (this.model.disabled(this.formInfo.formData, this) === true)
              return true;
            break;
          default:
            if (this.model.disabled === true) return true;
            break;
        }
      }

      return false;
    },
    /**
     * @return {Boolean} description
     */
    isVisible() {
      switch (typeof this.model.isVisible) {
        case "function":
          return this.model.isVisible(this.formInfo.formData, this);
          break;
        default:
          return this.model.isVisible !== false;
          break;
      }
      //       if (this.model.isVisible) {
      // }
      return true;
    },
    /**
     * @return {any} description
     */
    options() {
      if (this.model.options) {
        this.elementValue = this.GetModelValue();
        switch (typeof this.model.options) {
          case "function":
            return this.model.options(this.formInfo.formData);
            break;
          case "object":
            return this.model.options;
            break;
          default:
            break;
        }
      }

      if (this.IsDatePicker() || this.IsDateTimePicker()) {
        return new DatePickerOption();
      }
      return null;
    },
    /**
     * @return {any} description
     */
    attr() {
      if (this.model.attr) {
        this.elementValue = this.GetModelValue();
        switch (typeof this.model.attr) {
          case "function":
            return this.model.attr(this.formInfo.formData);
            break;
          case "object":
            return this.model.attr;
            break;
          default:
            break;
        }
      }
      return null;
    },
  },
  mounted() {
    // console.log(this.model.label, this.disabledformInfo);
    // console.log("render " + JSON.stringify(this.model));
    if (this.IsDatePicker()) {
      // console.log(this, this.model.id);
      if (this.GetModelValue() == "") this.SetModelValue("");
      //console.log("xxx");
    }
    //   this.model.
    //   this.formInfo.
    if (this.model.model) {
      this.elementValue = this.GetModelValue();
      this.$watch(
        "formInfo.formData." + this.model.model,
        function (newVal, oldVal) {
          // do something+
          this.$nextTick(() => {
            if (this.model.watch) {
              this.model.watch(
                this.formInfo.formData,
                newVal,
                oldVal,
                this,
                false
              );
            }
          });
          if (this.elementValue != newVal) {
            this.elementValue = newVal;
          }
        }
      );
    }
    this.isLoaded = true;
    setTimeout(() => {
      this.$nextTick(() => {
        if (this.model.watch) {
          this.model.watch(
            this.formInfo.formData,
            this.elementValue,
            "",
            this,
            true
          );
        }
        this.checkTime = GetTimeNow();
      });
    }, 500);
  },
  watch: {
    elementValue: {
      handler(val, oldVal) {
        // if (this.model.type == FormElementType.date) {
        //   //console.log(this);
        // }
        if (
          this.IsDatePicker() &&
          new Date(this.elementValue).getFullYear() < 1100
        ) {
          this.elementValue = this.GetModelValue();
          //console.log("xxx");
        }

        if (this.GetModelValue() != this.elementValue) {
          // this.elementValue = this.GetModelValue();
          this.SetModelValue(this.elementValue);
        }
      },
      deep: true,
    },
  },
};
</script>
<style lang="scss" scoped>
::v-deep  .has-border {
  border: 1px solid #ddd;
  border-radius: 4px;
}
.form-checkbox {
  height: 25px;
  border: 1px solid #ddd;
  border-top-right-radius: 4px;
  border-bottom-right-radius: 4px;
  display: flex;
  align-items: center;
  padding-left: 5px;
  width: 100%;
  .el-checkbox__input {
    display: flex;
  }
  //   padding-top: 2px;
}

.form-element-type-file {
  display: flex;
}

.quill-container {
  ::v-deep  .ql-editor {
    height: 400px;
    word-break: break-word;
  }
}
::v-deep  .form-title {
  border: 0;
  background: white;
}
</style>
