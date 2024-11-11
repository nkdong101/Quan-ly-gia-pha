<template lang="">
  <div
    :class="[
      'header',
      { hasUser: user && $route.path !== '/Account/Home/trang-chu' },
    ]"
  >
    <div class="header-logo"></div>
    <div class="header-menu">
      <i class="el-icon-s-fold slide-in-top" @click="toggleMenu"></i>
      <el-menu
        :default-active="$route.path"
        :class="['el-menu']"
        :mode="!this.menuVisible ? 'horizontal' : 'vertical'"
        active-text-color=" #fff"
        text-color="#f1f1f1"
        :router="true"
        ref="elMenu"
      >
        <el-menu-item @click="menuClick" index="/Account/Home/trang-chu">
          <!-- <a style="" href="#hero-section">Đăng nhập</a> -->
          <nuxt-link to="/Account/Home/trang-chu">Trang chủ</nuxt-link>
          <!-- Đăng nhập -->
        </el-menu-item>
        <el-menu-item v-if="user" index="/">
          <nuxt-link to="/"> Gia phả </nuxt-link></el-menu-item
        >
        <el-menu-item
          v-if="!user"
          @click="menuClick"
          index="/Account/Home/dang-ky"
        >
          <!-- <a href="#reigter">Đăng ký sử dụng</a> -->
          <nuxt-link to="/Account/Home/dang-ky">Đăng ký sử dụng</nuxt-link>
          <!-- Đăng ký sử dụng -->
        </el-menu-item>
        <el-menu-item
          v-if="!user"
          @click="menuClick"
          index="/Account/Home/gop-y"
        >
          <!-- <a href="#Comment">Góp ý</a> -->
          <nuxt-link to="/Account/Home/gop-y">Góp ý</nuxt-link>
        </el-menu-item>
        <el-submenu v-if="user" popper-class="submenu" index="#">
          <template slot="title">Mở rộng</template>

          <el-menu-item
            v-for="item in menu"
            :key="item.index"
            :index="item.path"
            >{{ item.banner }}</el-menu-item
          >
          <!-- <el-menu-item index="2-2">item two</el-menu-item>
          <el-menu-item index="2-3">item three</el-menu-item> -->
        </el-submenu>
        <el-menu-item
          style="display: flex; align-items: center"
          v-if="user"
          index="#"
        >
          <Avatar />
        </el-menu-item>
      </el-menu>
    </div>
  </div>
</template>
<script>
import StoreManager from "~/assets/scripts/StoreManager";

export default {
  data() {
    return {
      menuVisible: false,
    };
  },
  computed: {
    menu() {
      return StoreManager.GetMenu();
    },
  },
  methods: {
    // isMoble() {
    //  return window.innerWidth <= 610;
    // },
    menuClick(el) {},
    toggleMenu() {
      this.menuVisible = !this.menuVisible;
      if (this.menuVisible) {
        console.log(this.$refs.elMenu);
        this.$refs.elMenu.$el.style.display = "block";
      } else {
        this.$refs.elMenu.$el.style.display = "none";
      }
    },
  },
  mounted() {
    console.log("topmenu", this);
    console.log("topmenu", StoreManager.GetMenu());
  },
};
</script>
<style lang="scss" scoped>
// .el-menu--popup {
//
// }
.hasUser {
  position: static !important;
}
.header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 5px 10%;
  position: fixed;
  background: inherit;
  z-index: 1000;
  width: 100%;
  backdrop-filter: blur(2px);
  .header-logo {
    width: 50px;
    height: 50px;
    background-position: center;
    background-repeat: no-repeat;
    background-size: contain;
    // background-;
    background-image: url("/images/vanhoaviet.png");
  }

  .header-menu {
    .el-menu {
      border: 0;
      background-color: inherit !important;
      /deep/ .el-submenu {
        div {
          &:hover {
            background: transparent;
          }
          border: 0;
          line-height: 50px;
          height: 50px;
          i {
            color: white;
            font-size: 14px;
          }
        }
      }
      .el-menu-item {
        letter-spacing: 0.02em;
        position: relative;
        background-color: inherit !important;
        font-weight: 500;
        padding: 0;
        text-align: center;
        height: 50px;
        border: 0 !important;
        line-height: 50px;
        a {
          padding: 0 40px;
          // text-align: center
          text-decoration: none;
        }
        &::after {
          content: "";
          width: 100%;
          position: absolute;
          height: 2px;
          background-color: #f1f1f1;
          left: 50%;
          transform: translateX(-50%);
          bottom: 0;
          opacity: 0;
        }

        // &::before {
        //   content: "";
        //   width: 0;
        //   position: absolute;
        //   height: 3px;
        //   background-color: #000;
        //   left: 0;
        //   bottom: 0;
        //   // transform: translateX(-100%);
        //   // opacity: 0;

        // }
        &:hover {
          transform: translateY(-1px);

          transition: transform 0.2s ease;
          &::before {
          }
        }

        .el-submenu__title {
          height: 50px;
          line-height: 50px;
        }
      }
      .is-active {
        text-decoration: none !important;
        // font-weight: bold;
        // color: yellow;
        &::after {
          // top: 65%;
          width: calc(100% - 75px);
          transition: all 0.5s ease;
          opacity: 1;
          left: 50%;
          transform: translate(-50%, -600%);
        }
        &::before {
          display: none;
          // transition: 0s !important;
        }
      }
    }
  }
}
.el-menu--horizontal > .el-menu-item.is-active {
  // border: unset;
}

// .

.el-icon-s-fold {
  // visibility: hidden;
  // opacity: 0;
  display: none;
  transform: rotate(-90deg);
  font-size: 30px;
  color: white;
}

@media only screen and (max-width: 610px) {
  .header-menu {
    .el-icon-s-fold {
      display: block;
    }
    .el-menu {
      display: none;
    }
  }
}

.slide-in-top {
  -webkit-animation: slide-in-top 0.4s cubic-bezier(0.25, 0.46, 0.45, 0.94) both;
  animation: slide-in-top 0.4s cubic-bezier(0.25, 0.46, 0.45, 0.94) both;
}
@-webkit-keyframes slide-in-top {
  0% {
    -webkit-transform: translateY(-1000px);
    transform: translateY(-1000px);
    opacity: 0;
  }
  100% {
    -webkit-transform: translateY(0);
    transform: translateY(0);
    opacity: 1;
  }
}
@keyframes slide-in-top {
  0% {
    -webkit-transform: translateY(-1000px);
    transform: translateY(-1000px);
    opacity: 0;
  }
  100% {
    -webkit-transform: translateY(0);
    transform: translateY(0);
    opacity: 1;
  }
}
</style>
