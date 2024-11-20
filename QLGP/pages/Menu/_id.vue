<template>
  <div v-if="data" class="menu-detail">
    <MenuCardItem :title="data.banner || data.title" :items="nonChildren" />
    <MenuCardItem
      style="padding-top: 30px"
      v-for="item in hasChildren"
      :key="item.index"
      :title="item.banner || item.title"
      :items="item.children"
    />
    <!-- <pre>

    {{ data }}
    </pre> -->
  </div>
</template>

<script>
import { SearchTree } from "~/assets/scripts/Functions";
export default {
  data() {
    return {
      data: null,
    };
  },
  computed: {
    nonChildren() {
      return this.data.children.filter(
        (p) =>
          !p.children ||
          !p.children.length ||
          !p.children.filter((p1) => p1.type_id != 2).length
      );
    },
    hasChildren() {
      return this.data.children.filter(
        (p) => p.children && p.children.filter((p1) => p1.type_id != 2).length
      );
    },
  },
  methods: {
    getMenu(id) {
      return "";
    },
  },
  mounted() {
    console.log(this);

    let find_router = SearchTree(
      this.$store.state.menu,
      "/Menu/" + this.$route.params.id,
      "path"
    );
    if (find_router) {
      this.data = find_router;
    }
  },
};
</script>

<style lang="scss" scoped>
.menu-detail {
  padding: 48px 64px;
  overflow: auto;
  height: 100%;
}
</style>
