<template>
  <div class="ll-main">
    <div class="menu-Logo">
      <img src="\images\icon\wwf-logo.jpg" alt="" />
    </div>
    <el-scrollbar wrap-class="scrollbar-wrapper" style="flex: 1">
      <NuxtLink
        v-for="item in menu"
        :key="item.index"
        :to="item.path ? item.path : '/Menu/' + item.index"
        class="menu-link"
      >
        <div class="menu-c">
          <div class="menu-c-i">
            <img v-if="item.image" :src="Para.MenuImage.getImage(item.image)" />
            <i v-else :class="item.icon || 'fa fa-server'"></i>
          </div>
          <div class="menu-c-t">{{ item.title }}</div>
        </div>
      </NuxtLink>
      <!-- <div>{{ menu }}</div> -->
    </el-scrollbar>
  </div>
</template>

<script>
export default {
  computed: {
    menu() {
      return this.$store.state.menu;
    },
  },
};
</script>

<style lang="scss" scoped>
@import "@/assets/css/variable.scss";
.ll-main {
  flex: 0 0 70px;
  overflow: hidden;
  background: $menu_color;
  // border-right: 1px solid #ddd;
  height: 100vh;
  display: flex;
  flex-direction: column;
  .menu-Logo {
    display: flex;
    justify-content: center;
    // border-bottom: 1px solid #ddd;
    height: 45px;
    img {
      height: 100%;
    }
  }
  ::v-deep  .scrollbar-wrapper {
    overflow-x: hidden;
  }
  .menu-link {
    text-decoration: unset;
    color: black;
    .menu-c {
      display: flex;
      justify-content: center;
      flex-direction: column;
      text-align: center;
      // height: 55px;
      padding: 10px 0px;
      cursor: pointer;
      &:hover {
        background: $bg_color;
      }

      .menu-c-i {
        padding-bottom: 5px;
        i {
          font-size: 20px;
          width: 20px;
          height: 20px;
        }
        img {
          width: 24px;
          height: 24px;
        }
      }
      .menu-c-t {
        font-size: 10px;
      }
    }
  }
}
</style>
