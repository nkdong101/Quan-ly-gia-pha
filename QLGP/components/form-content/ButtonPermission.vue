<template>
  <div class="ButtonPermission" style="display: flex; flex-wrap: wrap">
    <el-button-group>
      <el-button
        v-for="item in data_btn"
        v-bind:key="item.Id"
        :disabled="!item.banner"
        :type="item.path || 'primary'"
        size="small"
        @click="ClickButton(item)"
      >
        <i v-if="item.icon" :class="item.icon" style="margin-right: 5px"></i>
        {{ item.title }}
      </el-button>
    </el-button-group>
  </div>
</template>

<script>
import { ShowMessage } from "~/assets/scripts/Functions";
import { EventBus } from "~/assets/scripts/EventBus.js";
import StoreManager from "~/assets/scripts/StoreManager";
export default {
  name: "ButtonPermission",
  props: {
    permissions: {
      type: Array,
      default: function () {
        return [];
      },
    },
  },
  data() {
    return {
      //   data: []
    };
  },
  computed: {
    data() {
      let defaultMenu = this.AllPermission();
      let data = (
        this.permissions.length > 0 ? this.permissions : defaultMenu
      ).filter(
        (p) => p.title && (p.path || "").toLowerCase().indexOf("hidden") == -1
      );
      //   this.ShowTip(data);
      return data;
    },
    data_btn() {
      return [...this.data, ...StoreManager.GetMenuButton()];
    },
  },
  methods: {
    ClickButton(item) {
      if (item.banner) {
        EventBus.$emit(item.banner);
        this.$emit(item.banner);
      }
    },
    ShowTip(data) {
      if (data.length > 0)
        ShowMessage(
          `<textarea style="width: 500px; height: 200px;">
      ${` <ButtonPermission
          ${data
            .map(
              (p) => `
          @${p.banner}="${p.banner}"`
            )
            .join("")}
        />`}
        </textarea>

    `
        );
    },
  },
  mounted() {
    // console.log(this.permissions);
    // if (this.permissions.length == 0) {
    //   this.data = this.AllPermission();
    // } else this.data = this.permissions;
    // this.data = this.data.filter(
    //   p => (p.LiClass || "").toLowerCase().indexOf("hidden") == -1
    // );
  },
};
</script>

<style lang="scss" scoped></style>
