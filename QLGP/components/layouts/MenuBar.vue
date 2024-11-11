<template>
  <div style="display: flex; flex-direction: column">
    <div class="menu-bar">
      <div class="menu-controls">
        <el-breadcrumb separator-class="el-icon-arrow-right"
                       class="menu-site-name"
                       style="font-size: 16px;
}">
          <el-breadcrumb-item :to="{ path: '/' }">
            <span style="color: #464feb; font-size: 16px; font-weight: 600">
              Quản lý gia phả
              </span>
          </el-breadcrumb-item>
          <el-breadcrumb-item v-for="(item, index) in breadcrumb.filter((p) => p.path)"
                              :key="item.index"
                              :to="item.path && index != breadcrumb.filter((p) => p.path).length - 1
                                ? { path: item.path }
                                : null
                                ">
            {{ index != breadcrumb.filter((p) => p.path).length - 1 ? item.title : title_breadcrumb || item.title }}
          </el-breadcrumb-item>
          <!--
          <el-breadcrumb-item>promotion list</el-breadcrumb-item>
          <el-breadcrumb-item>promotion detail</el-breadcrumb-item> -->
        </el-breadcrumb>
      </div>
      <div class="menu-action">
        <!-- <ButtonPermission /> -->
      </div>
      <div class="menu-profiles">

        <Avatar />
      </div>
    </div>
  </div>
</template>

<script>
import API, { linkPurchase } from "~/assets/scripts/API";
import DefaultForm from "~/assets/scripts/base/DefaultForm";
import { EventBus } from "~/assets/scripts/EventBus";
import {
  SearchAllTree,
  SearchTree,
  ShowMessage,
} from "~/assets/scripts/Functions";
import GetDataAPI from "~/assets/scripts/GetDataAPI";
import { LoginResult } from "~/assets/scripts/objects/LoginResult";
import MenuItem from "~/assets/scripts/objects/MenuItem";
import StoreManager from "~/assets/scripts/StoreManager";
export default {
  data() {
    return {
      title_breadcrumb: '',
      menu_year_picked: localStorage.year || "2021",
      menu_year: [
        new MenuItem({
          index: "1",
          title: "FY21",
          visible: () => {
            let allow_year = ["Form-Summary", "Form-Detail", "Form-type"];
            return allow_year.some((p) => p == this.$route.name);
          },
          formatter: () => {
            return this.menu_year_picked_item.title;
          },
          children: [
            new MenuItem({ index: "2021", title: "FY21" }),
            new MenuItem({ index: "2022", title: "FY22" }),
          ],
        }),
      ],
      menu_active: "",
    };
  },
  computed: {
    menu() {
      return this.$store.state.menu;
    },
    BannerTitle() {
      return StoreManager.GetBannerTitle();
    },
    menu_year_picked_item() {
      let find = SearchTree(this.menu_year, this.menu_year_picked, "index");
      if (find) {
        localStorage.year = find.index;
      }
      return find;
    },

    menu_picked_item() {
      let find = SearchTree(this.menu, this.menu_active, "index");
      return find;
    },

    breadcrumb() {
      let data = this.getMenu(this.$route.path);
      if (!data.length) {
        data = this.getMenu($nuxt.$route.matched[0].path);
        data.splice(data.length - 1, 1);
        data.push(
          new MenuItem({
            title: Object.keys($nuxt.$route.params)
              .map((p) => $nuxt.$route.params[p])
              .join("_"),
            path: Object.keys($nuxt.$route.params)
              .map((p) => $nuxt.$route.params[p])
              .join("_"),
          })
        );
      }
      console.log(data);
      return data;
    },
  },
  methods: {
    Setbreadcrumb(newTitle) {
      this.title_breadcrumb = newTitle;
    },
    OpenFormCreate() {
      this.$router.push("/Order/Form");
      // window.open(linkPurchase + "/Form/Create", "_blank");
    },
    getMenu(path) {
      return SearchAllTree(
        this.menu,
        (p) => {
          return p.path == path;
        },
        "children"
      );
    },
    handleSelect(key, keyPath) {
      if (this.menu_active != key) {
        this.menu_active = key;
        StoreManager.SetBannerTitle(this.menu_picked_item.banner);
      }
    },
    onYearPicked(key, keyPath) {
      this.menu_year_picked = key;
    },
  },
  mounted() {
    // console.log(this);
    let find_router = SearchTree(this.menu, this.$route.path, "path");
    if (find_router) {
      this.menu_active = find_router.index;
      StoreManager.SetBannerTitle(find_router.banner);
    }
  },
  beforeMount() {
    EventBus.$on("Setbreadcrumb", this.Setbreadcrumb);
  },

  beforeDestroy() {
    EventBus.$off("Setbreadcrumb", this.Setbreadcrumb);
  },
};
</script>

<style lang="scss" scoped>
@import "@/assets/css/variable.scss";

.menu-bar {
  background: $menu_color;
  overflow: hidden;
  display: flex;
  height: 45px;

  // border-bottom: 1px solid #ddd;
  ::v-deep  .el-menu {
    >div>.el-menu-item {
      height: 45px;
      line-height: 45px;
    }

    .el-submenu {
      .el-submenu__title {
        height: 45px;
        line-height: 45px;
        font-size: 13px;
      }
    }
  }

  .menu-Logo {
    color: #fff;
    font-size: 24px;
    -webkit-text-shadow: none;
    text-shadow: none;
    line-height: 1.5em;
    font-weight: 200;

    img {
      height: 45px;
      width: 45px;
    }
  }

  .menu-controls {
    display: flex;
    // border-left: 1px solid white;
  }

  .menu-action {
    // border-left: 1px solid white;
    flex: 1;
    display: flex;
    align-items: center;
    padding-left: 10px;

    ::v-deep  .el-button-group {
      white-space: nowrap;
      display: flex;
    }

    button {}
  }

  .menu-profiles {
    display: flex;
    align-items: center;
  }
}

.menu-site-name {
  font-size: 24px;
  margin-left: 10px;
  display: flex;
  align-items: center;
}

::v-deep  {
  .el-breadcrumb__inner a,
  .el-breadcrumb__inner.is-link {
    color: rgb(70, 79, 235);
    font-size: 16px;
    font-weight: 600;
  }

  .el-breadcrumb__item:last-child .el-breadcrumb__inner {
    color: black;
    font-weight: 600;
  }
}
</style>
