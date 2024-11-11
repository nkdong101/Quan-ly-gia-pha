<template>
  <div class="form-info" v-if="data" :label-top="model.labelTop">
    <!-- <el-row :gutter="5" style="display: flex;" v-for="item in elements" v-bind:key="item.id">
            <FormElement :ref="item.id" :model="item" v-if="item" :formInfo="model">
                <slot :slot="slot" :name="slot" v-for="slot in item.getAllSlotChild()"></slot>
            </FormElement>
        </el-row>-->
    <div class="form-info-c">
      <el-row
        :gutter="5"
        style="display: flex"
        :class="item.isVisible ? '' : item.class ? item.class : 'hidden'"
        v-for="item in elements"
        v-bind:key="item.id"
      >
        <FormElement
          :ref="item.id"
          :model="item"
          v-if="item"
          :formInfo="data"
          :disabledformInfo="disabledformInfo"
        >
          <!-- <slot :slot="item.id" :name="item.id"> </slot> -->
          <slot
            :slot="slot"
            :name="slot"
            v-for="slot in item.getAllSlotChild()"
          ></slot>
          <!-- <template v-for="(_, slot) in $slots">
          <template :slot="slot">
            <slot :name="slot"></slot>
          </template>
                </template>-->
        </FormElement>
      </el-row>
    </div>
  </div>
</template>

<script>
import { MessageType, ShowMessage } from "~/assets/scripts/Functions";
function getRef(instance, ref) {
  try {
    if (instance.$refs) {
      if (instance.$refs[ref]) {
        if (Array.isArray(instance.$refs[ref])) {
          return instance.$refs[ref][0]; //.$refs.entry;
        } else {
          return instance.$refs[ref]; //.$refs.entry;
        }
      } else {
        let keys = Object.keys(instance.$refs);
        if (keys.length) {
          for (let i = 0; i < keys.length; i++) {
            const element = keys[i];
            let _ref = instance.$refs[element];
            if (Array.isArray(_ref)) _ref = _ref[0];
            let find = getRef(_ref, ref);
            if (find) return find;
          }
        }
      }
    }
  } catch (error) {
    return null;
  }
}
export default {
  components: {},
  props: {
    model: Object,
    disabled: {
      typeof: Boolean,
    },
  },
  watch: {
    disabled(newValue, oldValue) {
      this.disabledformInfo = newValue;
    },
  },
  computed: {
    elements: function () {
      let elements = [];
      switch (typeof this.model.elements) {
        case "function":
          elements = this.model.elements();
          break;
        case "object":
          elements = this.model.elements;
          break;
        default:
          break;
      }

      return elements.filter((p) => p);
    },
  },
  data() {
    return {
      slots: [],
      data: null,
      needValidate: true,
      disabledformInfo: null,
    };
  },
  methods: {
    getElement(id) {
      return getRef(this, id);
    },
    getEntry(id) {
      if (getRef(this, id) && getRef(this, id).$refs)
        return getRef(this, id).$refs.entry;
    },
    getValidate() {
      this.needValidate = true;
      //   console.log(this);
      return new Promise((rs, rj) => {
        var errP = this.elements
          .reduce((a, b) => {
            a = a.concat(b.getAllSlotChild());
            return a;
          }, [])
          .map((p) => {
            return new Promise((rs) => {
              var element = this.getElement(p);
              if (element) {
                if (element.model.type == "FormInfo") {
                  element.validateFormInfo().then((re) => {
                    rs(!re ? "Lỗi" : "");
                  });
                } else {
                  rs(element.validate());
                }
              } else {
                rs("");
              }
            });
          });
        Promise.all(errP).then((re) => {
          let err = (re || []).filter((p) => p);
          if (err.length) {
            rs(false);
          } else rs(true);
        });
      });
    },
    onChange() {
      if (this.model.formData.toJSON) this.model.formData.toJSON();
      this.$emit("onChanged");
    },
    // tmp(id) {
    //   return getRef(this, id);
    // }
  },
  mounted() {
    // console.log("disabled", this.disabled);
    // console.log(this.model.data);
  },
  beforeMount() {
    this.data = this.model;
    this.disabledformInfo = this.disabled;
    this.data.vm = this;
  },
};
</script>
<style lang="scss" scoped>
.form-info {
  font-size: 13px;
  .form-info-c,
  ::v-deep  .form-element-child-vertical {
    margin-bottom: -5px;
    & > *:empty {
      display: none !important;
    }

    & > *:not(:empty) {
      margin-bottom: 5px;
      // &:last-child {
      //   margin-bottom: 0px;
      // }
    }
  }
}
[label-top] {
  ::v-deep  .input-c {
    display: flex;
    flex-direction: column;
    .el-input-group__prepend {
      background: none;
      border: 0;
      width: 100%;
      padding: 5px 0px;
      box-shadow: none;
      > span {
        width: 100% !important;
        font-weight: bold;
      }
    }
    .el-input__inner {
      border-top-left-radius: 4px;
      border-bottom-left-radius: 4px;
    }
  }
}
</style>
