<template>
  <div v-if="visibled()" class="noselect">
    <el-menu-item
      v-if="!model.children.length"
      :index="model.index"
      @click="process()"
    >
      {{ getLabel() }}
    </el-menu-item>
    <el-submenu
      v-else
      :index="model.index"
      :show-timeout="100"
      :hide-timeout="300"
    >
      <template slot="title">
        {{ getLabel() }}
      </template>
      <MenuItem
        v-for="item in model.children"
        :key="item.index"
        :model="item"
      />
    </el-submenu>
  </div>
</template>

<script>
import MenuItem from "~/assets/scripts/objects/MenuItem";
export default {
  props: {
    model: {
      type: MenuItem,
    },
  },
  methods: {
    visibled() {
      if (this.model.visible) return this.model.visible();
      return true;
    },
    process() {
      if (this.model.path) this.$router.push(this.model.path);
    },
    getLabel() {
      if (this.model.formatter) return this.model.formatter();
      else return this.model.title;
    },
  },
};
</script>

<style lang="scss" scoped>
.menu-item-c {
  padding-left: 10px;
}
</style>
