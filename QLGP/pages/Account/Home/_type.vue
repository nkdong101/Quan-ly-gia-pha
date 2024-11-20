<template lang="">
  <div>
    <div id="trang-chu" class="hero-section">
      <div class="hero-section-logo">
        <img :style="{ width: '250px' }" src="/images/ngoicon.jpg" />
        <dowload_link></dowload_link>
      </div>
      <!-- <div>{{ type }}</div> -->
      <!-- <Login_form v-if="type == 'dang-nhap'"></Login_form> -->

      <div class="hero-section-content">
        <h4 style="font-size: 24pt; margin-bottom: 20px">
          Tiếp nối giá trị truyền thống bằng công nghệ
        </h4>
        <i style="padding-top: 30px">
          Văn hóa là nguồn cội, là bản sắc, là giá trị tinh thần được cha ông ta
          tạo dựng từ nhiều đời nay. Ngày nay công nghệ phát triển giúp cho cuộc
          sống diễn ra nhanh hơn, sôi động hơn đồng thời cũng là cơ hội để giúp
          chúng ta có những cách thức, phương tiện mới để lưu giữ và phát huy
          những giá trị tốt đẹp của văn hóa Việt. Chúng tôi rất vui và tự hào
          khi được đồng hành cùng các bạn thực hiện công việc ý nghĩa đó!
        </i>
      </div>
    </div>

    <div ref="intro" class="intro">
      <div class="intro-border">
        <div class="intro-logo">
          <img src="/images/caudoi_1.png" />
        </div>
        <div class="intro-content">
          <el-row :gutter="24">
            <el-col :span="12">
              <el-card shadow="always">
                <p style="font-size: 36pt">{{ countDongho  }}</p>
                <h4>Dòng họ</h4>
              </el-card>
            </el-col>
            <el-col :span="12">
              <el-card shadow="always">
                <p style="font-size: 36pt">{{ countGiapha  }}</p>
                <h4>Người sử dụng</h4>
              </el-card>
            </el-col>
            <!-- <el-col :span="8">
              <el-card shadow="always">
                <p style="font-size: 36pt">{{ countTinh  }}</p>
                <h4>Tỉnh có mặt</h4>
              </el-card>
            </el-col> -->
          </el-row>
        </div>
      </div>
    </div>
    <div id="dang-nhap" class="reigter">
      <div style="text-align: center" class="regiter-wrapper">
        <h4>{{ isRegit ? "Khởi tạo thông tin dòng họ" : "Đăng nhập" }}</h4>
        <div
          v-if="isRegit"
          style="width: 100%; text-align: center; padding: 0 0 10px 0"
        >
          <i
            >Hãy để người thân, con cháu của bạn hiểu hơn về dòng họ của
            mình!</i
          >
        </div>
        <transition name="slide-fade" mode="out-in">
          <component
            @regiter="handleRegister"
            :is="isRegit ? 'Register' : 'Login_form'"
          />
        </transition>
      </div>
    </div>
    <Comment id="gop-y" />
  </div>
</template>
<script>
import API, { ServerAPI } from "~/assets/scripts/API";
import DefaultForm from "~/assets/scripts/base/DefaultForm";
import { ShowMessage, validateEmail } from "~/assets/scripts/Functions";
import GetDataAPI from "~/assets/scripts/GetDataAPI";
export default {
  layout: "blank",
  props: {
    isAdd: {},
  },
  data() {
    return {
      // isAdd: null,
      home: {
        Dongho: 1,
        Giapha: 20,
       
      },
      countDongho: 0,
      countGiapha: 0,
      countTinh: 0,
      isRegit: false,
      hasStarted: false,
    };
  },

  watch: {
    // $route: {
    //   deep: true,
    //   handler() {
    //     // console.log(this.type);
    //   },
    // },
  },
  computed: {
    type() {
      return this.$route.params.type;
    },
  },
  methods: {
    handleRegister(value) {
      this.isRegit = false;
      // console.log(value);
    },
    checkScroll() {
      // console.log(this.hasStarted)
      let introPos = "";
      if (this.$refs.intro)
        introPos = this.$refs.intro.getBoundingClientRect().top;

      // console.log(introPos)
      const screenPosition = window.innerHeight;
      if (!this.hasStarted && introPos < screenPosition) {
        console.log("number increase");
        this.hasStarted = true;
        this.startCounting();
      }
    },
    startCounting() {
      this.animateValue("countDongho", 0, this.home.Dongho, 600);
      this.animateValue("countGiapha", 0, this.home.Giapha, 600);
      this.animateValue("countTinh", 0, this.home.Tinh, 600);
    },
    animateValue(ref, start, end, duration) {
      if (start == end) return;
      let startTime = null;

      const animate = (currentTime) => {
        if (!startTime) startTime = currentTime;
        const progress = Math.min((currentTime - startTime) / duration, 1);
        this[ref] = Math.floor(progress * (end - start) + start);

        if (progress < 1) {
          requestAnimationFrame(animate);
        }
      };

      requestAnimationFrame(animate);
    },
  },
  mounted() {
    // console.log(this);
    this.hasStarted = false;
    // console.log('mounted',this.hasStarted)
    window.addEventListener("scroll", this.checkScroll);

    // GetDataAPI({
    //   url: API.Reports_Home,
    //   action: (re) => {// console.log(re)
    //     // this.home = re;
    //   },
    // });
    this.$nextTick(() => {
      // console.log(this)
      console.log(document.getElementById(this.type));
      setTimeout(() => {
        document.getElementById(this.type).scrollIntoView();
      }, 0);
    });

  
  },
};
</script>
<style lang="scss" scoped>
.slide-fade-enter-active,
.slide-fade-leave-active {
  transition: all 0.3s ease-in-out;
}
.slide-fade-enter /* .slide-fade-leave-active in <2.1.8 */ {
  transform: translateX(25%);
  opacity: 0;
}

.slide-fade-leave-to {
  transform: translateX(-25%);
  opacity: 0;
}

.reigter {
 
  display: flex;
  align-items: center;
  justify-content: center;
  // padding-top: 40px;
  .regiter-wrapper {
    padding: 20px 40px;
    overflow: hidden;
    background-color: #fff;
    border-radius: 20px;
  }
  background: linear-gradient(
      180deg,
      rgba(48, 53, 58, 0.6) 0%,
      rgba(50, 48, 48, 0.2) 70%
    ),
    url("/images/3-cac-loai-hinh-van-hoa.jpg");
  background-position: center;
  background-size: cover;

  flex-direction: column;
  padding: 60px 0 40px 0;

  h4 {
    font-family: PlayfairDisplay;
    padding: 20px 0 10px 0;
    text-transform: uppercase;
    font-size: 24pt;
    color: #9e1c1e;
  }
}
.hero-section {
  height: 600px;

  // background:
  //   linear-gradient(180deg, rgba(90, 102, 114, 1) 0%, rgba(255, 255, 255, 1) 70%),
  //   url("/images/3-cac-loai-hinh-van-hoa.jpg");
  // background-position: 0px 0px, 50% 50%;
  // background-size: auto, cover;
  background-image: linear-gradient(
      180deg,
      rgba(48, 53, 58, 0.6) 0%,
      rgba(50, 48, 48, 0.2) 70%
    ),
    url("/images/banner1.jpg");
  background-position: 0px 0px, 50% 50%;
  background-size: auto, cover;
  padding: 0 20%;
  display: flex;
  justify-content: space-evenly;
  padding-top: 120px;
  // align-items: center;
  // justify-content: center;
  .hero-section-logo {
    height: 250px;
    margin-right: 20px;
    width: fit-content;
    display: flex;
    flex-direction: column;
    align-items: center;
    img {
      //   height: 180px;
      //   // width: 300px;
      //   width: 100%;
    }
  }

  .hero-section-content {
    color: #fff;
    font-size: 1.125rem;
    // line-height: 150%;
  }
}

.intro {
  background-color: #fff1dc;
  // background: url("/images/banner2.jpg");
  background-position: center;
  background-size: cover;
  // padding-top: 50px;
  padding: 50px 18%;

  .intro-border {
    display: flex;
    flex-direction: column;
    align-items: center;
    // border: 3px solid rgb(249, 204, 25);
    padding: 50px 20px;
    border-radius: 10px;
    // background-color: #fff1dc;
    .intro-logo {
      height: 150px;
      img {
        height: 150px;
      }
    }
    .intro-content {
      width: 100%;
      margin-top: 20px;
      ::v-deep .el-row {
        .el-col {
          .el-card {
            border: 2px solid #1e6248;
            border-radius: 10px;
            &:hover {
              transform: scale(1.1, 1.1);
            }
            .el-card__body {
              background-color: #fff1dc;
              padding: 10px;
              text-align: center;
              h4 {
                margin-bottom: 20px;
                color: rgb(246, 140, 0);
              }
            }
          }
        }
      }
    }
  }
}
</style>
