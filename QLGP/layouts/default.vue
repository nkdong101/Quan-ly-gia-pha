<template>
  <div class="main-content-g" v-if="mainLoad" v-loading="loading">
    <div class="main-content">
      <TopMenu />
      <div class="main-content-c">
        <Nuxt />
      </div>
      <!-- <Footer ></Footer> -->
    </div>

    <!-- <div style="font-family: Inter, Arial, sans-serif;  scroll-behavior: smooth !important;letter-spacing: 0.03em;display:flex; flex-direction: column;" >
    <TopMenu @login="loginClick" ref="header" />
    <Nuxt />
    <Footer ></Footer> -->
  </div>
</template>

<script>
import API from "~/assets/scripts/API";
import GetDataAPI from "~/assets/scripts/GetDataAPI";
import MenuItem from "~/assets/scripts/objects/MenuItem";
import { Para } from "~/assets/scripts/Para";
import StoreManager from "~/assets/scripts/StoreManager";
import LeftLayout from "~/components/layouts/LeftLayout.vue";
import MenuBar from "~/components/layouts/MenuBar.vue";

export default {
  middleware: "auth",
  components: {
    MenuBar,
    LeftLayout,
  },
  data() {
    return {
      mainLoad: false,
    };
  },

  computed: {
    loading: function () {
      return StoreManager.IsShowLoading();
    },
  },
  watch: {
    "$route.params.search": {
      handler: function (search) {
        StoreManager.Setcurrency_code(""); //Clear code
        StoreManager.SetMenuButton([]); //Clear button menu
      },
      deep: true,
      immediate: true,
    },
  },
  methods: {
    ToButton(p) {
      return new MenuItem({
        index: p.Id,
        title: !p.Stt ? p.Title : "",
        banner: p.Action,
        path: p.LiClass,
        icon: p.Icon,
      });
    },
    ToMenuItem(p) {
      return new MenuItem({
        index: p.Id,
        title: p.Title,
        path: p.Href,
        icon: p.Icon,
        image: p.Image,
        note: p.Note,
        type_id: p.Type_id,
        banner: p.LiClass || p.Title,
        children: (p.Child || []).map((p1) => this.ToMenuItem(p1)),
        permission: (p.ListButton || []).map((p1) => this.ToButton(p1)),
      });
    },
    InitMenu() {
      return new Promise((rs) => {
        GetDataAPI({
          url: API.Menu,
          action: (re) => {
            let menu = re.map((p) => this.ToMenuItem(p));
            StoreManager.store.commit("InitRawMenu", menu);
            StoreManager.InitMenu(menu);
            rs();
          },
        });
      });
    },
    InitPara() {
      return new Promise((rs) => {
        let GroupPermission = new Promise((rs) => {
          GetDataAPI({
            url: API.GroupPermission,
            action: (re) => {
              Para.GroupPermission.data = re;
              rs();
            },
          });
        });

        Promise.all([GroupPermission]).then((result) => {
          rs();
        });
      });
    },
    InitStuff() {
      return new Promise((rs) => {
        Promise.all([this.InitMenu(), this.InitPara()]).then((result) => {
          rs();
        });
        // GetDataAPI({
        //   method: "GET",
        //   url: API.CheckLogin,
        //   action: (re) => {
        //     Promise.all([this.InitMenu(), this.InitPara()]).then((result) => {
        //       rs();
        //     });
        //   },
        // });
      });
    },
  },
  mounted() {
    // console.log("default", this);
    if (location.href.indexOf("localhost") == -1) {
      console.log = () => {};
      console.error = () => {};
    }
    this.InitStuff().then(() => {
      this.mainLoad = true;
    });

    console.log(this);
  },
};
</script>

<style lang="scss" scoped>
@import "@/assets/css/variable.scss";

.main-content-g {
  display: flex;
  background-image: linear-gradient(
          180deg,
          rgba(48, 53, 58, 0.6) 0%,
          rgba(50, 48, 48, 0.2) 70%
        ),
        url("/images/banner1.jpg");
      background-position: center;
  > .main-content {
    flex: 1;
    overflow: hidden;
    height: calc(100vh);
    display: flex;
    flex-direction: column;

    .main-content-c {
      flex: 1;
      overflow: auto;
      padding: 10px;
      div{
        background: $bg_color;
      }
      // 
      // box-shadow: rgba(0, 0, 0, 0.12) 0px 0px 2px 0px,
      //   rgba(0, 0, 0, 0.14) 0px 2px 4px 0px;
      
     
    }
  }
}

.scrolled {
  // background: rgb(90,102,114);
  background: linear-gradient(
    180deg,
    rgba(64, 70, 77, 0.4) 0%,
    rgba(114, 114, 114, 0.6) 90%
  ) !important;

  animation: smScroll 0.6s forwards;
}

@keyframes smScroll {
  0% {
    transform: translateY(-50px);
  }
  100% {
    -webkit-transform: translateY(0);
    -moz-transform: translateY(0);
    transform: translateY(0);
  }
}
</style>
