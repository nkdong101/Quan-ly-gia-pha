<template lang="">
  <div>
    <div id="hero-section" class="hero-section">
      <div class="hero-section-logo">
        <img src="/images/vanhoaviet.png" />
        <dowload_link></dowload_link>
      </div>
      <div v-if="!isAdd" class="hero-section-content">
        <h3>Tiếp nối giá trị truyền thống bằng công nghệ</h3>
        <p style="padding-top: 20px">
          Văn hóa là nguồn cội, là bản sắc, là giá trị tinh thần được cha ông ta
          tạo dựng từ nhiều đời nay. Ngày nay công nghệ phát triển giúp cho cuộc
          sống diễn ra nhanh hơn, sôi động hơn đồng thời cũng là cơ hội để giúp
          chúng ta có những cách thức, phương tiện mới để lưu giữ và phát huy
          những giá trị tốt đẹp của văn hóa Việt. Chúng tôi rất vui và tự hào
          khi được đồng hành cùng các bạn thực hiện công việc ý nghĩa đó!
        </p>
      </div>

      <Login_form v-else></Login_form>
    </div>

    <div class="intro">
      <div class="intro-border">
        <div class="intro-logo">
          <img src="/images/caudoi_1.png" />
        </div>
        <div class="intro-content">
          <el-row :gutter="24">
            <el-col :span="8">
              <el-card shadow="always">
                <h4>Số lượng dòng họ</h4>
                <p>13</p>
              </el-card>
            </el-col>
            <el-col :span="8">
              <el-card shadow="always">
                <h4>Số lượng người</h4>
                <p>132</p>
              </el-card>
            </el-col>
            <el-col :span="8">
              <el-card shadow="always">
                <h4>Số lượng tỉnh có mặt</h4>
                <p>12</p>
              </el-card>
            </el-col>
          </el-row>
        </div>
      </div>
    </div>
    <div id="reigter" class="reigter">
      <div class="regiter-wrapper">
        <h4>Khởi tạo thông tin dòng họ</h4>
        <div style="width: 100%; text-align: center; padding: 0 0 10px 0">
          <i
            >Hãy để người thân, con cháu của bạn hiểu hơn về dòng họ của
            mình!</i
          >
        </div>
        <component :is="'Register'" />
      </div>
    </div>
    <Comment id="Comment" />
  </div>
</template>
<script>
export default {
  layout: "blank",
  props:{
    isAdd: {},
  },
  data() {
    return {
     // isAdd: null,
    };
  },
  methods: {
    // onWheel(e) {
    //   console.log(e, this);
    // },

  },
  mounted(){
    console.log(this)
  }
};
</script>
<style lang="scss" scoped>
.reigter {
  display: flex;
  align-items: center;
  justify-content: center;
  // padding-top: 40px;
  .regiter-wrapper {
    padding: 20px 40px;

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
  padding-top: 120px;
  // align-items: center;
  // justify-content: center;
  .hero-section-logo {
    height: 250px;
    margin-right: 20px;
    width: fit-content;
    img {
      height: 180px;
      // width: 300px;
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
