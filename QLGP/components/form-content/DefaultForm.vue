<template>
  <!-- <b-modal
    v-if="model.visible"
    v-model="model.visible"
    centered
    :size="model.size || 'lg'"
    :title="model.title"
    no-close-on-esc
    no-close-on-backdrop
    header-class="form-header-c"
  >
    <template v-slot:modal-header="{ close }">
      <span>{{ model.title }}</span>

      <i class="fa fa-times" @click="close()"></i>
    </template>
    <div v-focusNextOnEnter v-focusFirstOnLoad class="form-c">
      <slot name="content"></slot>
    </div>
    <template v-slot:modal-footer="{ cancel }">
      <el-button
        v-if="model.type != 'dialog'"
        type="primary"
        @click="$emit('actionOK')"
      >
        {{ model.OKtext || "Lưu" }}
      </el-button>
      <el-button v-if="model.type != 'dialog'" type="default" @click="cancel()">
        {{ model.Canceltext || "Thoát" }}
      </el-button>
    </template>
    </b-modal>-->
  <div v-if="model.visible" :is-fullscreen="model.fullscreen">
    <el-dialog
      title="Tips"
      :visible="true"
      :width="width"
      :custom-class="model.class"
      :show-close="false"
      :modal-append-to-body="model.modalAppendToBody === false ? false : true"
      :append-to-body="model.appendtobody || false"
      :fullscreen="model.fullscreen"
      :top="'auto'"
    >
      <span slot="title" class="form-header-c">
        <span v-html="model.title"></span>
        <i class="fa fa-times" @click="model.visible = false"></i>
      </span>
      <!-- v-focusFirstOnLoad -->
      <div v-focusNextOnEnter class="form-c">
        <slot name="content">
          <FormContainer
            v-if="model.control"
            :model="model.control(model)"
          ></FormContainer>
        </slot>
      </div>
      <span slot="footer" class="dialog-footer" v-if="model.type != 'dialog'">
        <template v-if="model.type != 'dialog'">
          <el-button
            v-for="item in model.btns"
            :id="item.id"
            :type="item.type || 'primary'"
            v-bind:key="item.id"
            @click="$emit(item.action)"
            >{{ item.text }}</el-button
          >
        </template>
        <el-button
          v-if=" !model.noSave && model.type != 'dialog'"
            type="primary"
          @click="$emit('actionOK')"
          >{{ model.OKtext || "Lưu" }}</el-button
        >
        <el-button
          v-if="model.type != 'dialog'"
          type="default"
          @click="model.visible = false"
          >{{ model.Canceltext || "Thoát" }}</el-button
        >
      </span>
    </el-dialog>
  </div>
</template>

<script>
export default {
  props: {
    model: { type: Object },
  },
  methods: {},
  computed: {

    width: function () {
      let defaultFormWidth = "500px";
      if (typeof this.model.width == "function") {
        return this.model.width() || defaultFormWidth;
      }
      return this.model.width || defaultFormWidth;
    },
  },
  watch: {
    "model.visible": function () {
      //   //console.log(this, this.model.visible);
    },
  },
  mounted() {
    // console.log('this.model',this.model);
  },
};
</script>

<style lang="scss" scoped>
@import "@/assets/css/variable.scss";
[is-fullscreen] {
  .el-dialog__wrapper {
    padding: 0;
  }
}
::v-deep  .el-dialog {
  border-top-left-radius: 4px;
  border-top-right-radius: 4px;
  &.is-fullscreen {
    display: flex;
    flex-direction: column;
    .el-dialog__body {
      flex: 1;
      overflow: hidden;
      .form-c {
        height: 100%;
        > div {
          height: 100%;
        }
      }
    }
  }
  .el-dialog__header {
    padding: 0;
    .form-header-c {
      background: $swatch_1;
      color: white;
      padding: 0;
      display: flex;
      justify-content: space-between;
      border-top-left-radius: 4px;
      border-top-right-radius: 4px;
      span {
        align-self: center;
        padding: 10px 10px;
      }
      i {
        cursor: pointer;
        align-self: center;
        padding: 20px;
        display: flex;
        align-items: center;
        justify-content: center;
        position: relative;
      }
    }
  }
  .el-dialog__body {
    padding: 10px;
  }
  .el-dialog__footer {
    padding: 10px;
    padding-top: 0px;
    .dialog-footer {
      display: flex;
      justify-content: flex-end;
    }
  }
}

::v-deep  .modal-body {
  padding: 5px;
  //   padding-bottom: 0;
}
::v-deep  .modal-footer {
  padding: 0px;
  border: 0px;
}
.el-button + .el-button {
  /* margin-left: 10px; */
  margin-left: 0.25rem;
  padding: 7px 15px;
  font-size: 13px;
}
::v-deep  .el-col {
  //   margin-bottom: -5px;
  > div:not(.el-row):not(.el-col) {
    &:not(:empty) {
      margin-bottom: 5px;
    }
    &:last-child {
      margin-bottom: 0;
    }
  }
}
.form-c {
  //   margin-bottom: -5px;

  ::v-deep  .el-row {
    &:not(:empty) {
      margin-bottom: 5px;
    }
    &:last-child {
      margin-bottom: 0;
    }
  }
}
.a:not([disabled]) {
  color: red;
}
.el-dialog__wrapper {
  padding: 0 5px;
  ::v-deep  .el-dialog {
    max-width: 100%;
  }
}

::v-deep  .el-input__inner {
  padding: 0 5px;
}
</style>
