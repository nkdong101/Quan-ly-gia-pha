<template lang="">
  <div class="footer-w">
    <div class="img">
      <img class="logo" src="/images/Logo-Chu_V2.png" />
      <!-- <div class="dowload">
        <div style="padding-right: 10px;
    text-align: right;" class="ggPlay">
          <img src="/images/ggPlay.png" />
          <a href=""> <img src="/images/link_qrcode.png" /></a>
        </div>
        <div class="appStore">
          <img src="/images/appStore.png" />
          <a href=""> <img src="/images/link_qrcode.png" /></a>
        </div>
      </div> -->
    </div>
    <!-- <div class="ft-content">
            <div></div>
          
            <p>Số 19, Ngõ 48 Phố Hưng Thịnh, X2A Yên Sở, Hoàng Mai, Hà Nội</p>
            <p>buihai@viettech.info	0906 191 684</p>
        </div> -->
    <el-row :gutter="50" class="ft-content">
      <el-col :span="14">
        <!-- <h4>Thông tin chung</h4> -->
        <div>
          <i class="fa fa-building-o" aria-hidden="true"></i
          ><span>Công ty cổ phần giải pháp số hóa Viettech</span>
        </div>
        <div>
          <i class="el-icon-location"></i
          ><span
            >Số 19, Ngõ 48 Phố Hưng Thịnh, X2A Yên Sở, Hoàng Mai, Hà Nội</span
          >
        </div>
        <div>
          <i class="el-icon-user-solid"> </i> <span>buihai@viettech.info </span>
        </div>
        <div>
          <i class="el-icon-phone-outline"> </i>
          <a style="color: #fff; text-decoration: none" href="tel:+84906191684"
            >0906 191 684</a
          >
        </div>
      </el-col>
      <el-col :span="4">
        <!-- <h4>Chức năng</h4> -->
        <div><a href="#reigter">Đăng ký</a></div>
        <div><a href="#hero-section">Đi đến đăng nhập</a></div>
        <div><a href="#Comment">Góp ý</a></div>
      </el-col>
      <el-col :span="6">
        <div class="dowload">
          <div style="padding-right: 10px; text-align: right" class="ggPlay">
            <img src="/images/ggPlay.png" />
            <a href=""> <img src="/images/link_qrcode.png" /></a>
          </div>
          <div class="appStore">
            <img src="/images/appStore.png" />
            <a href=""> <img src="/images/link_qrcode.png" /></a>
          </div>
        </div>
      </el-col>
    </el-row>
  </div>
</template>
<script>
export default {};
</script>
<style lang="scss" scoped>
.footer-w {
  padding: 10px 0 0 0;
  background: #044085;
  color: #fff;
  display: flex;
  align-items: center;

  width: 100%;

  .img {
    display: flex;
    flex-wrap: wrap;
    // grid-template-columns: repeat(2, 1fr);
    // gap: 10px;
    // grid-auto-rows: minmax(100px, auto);
    width: 20%;
    // height: 100%;
    .logo {
      width: 100%;
    }
  }
  .ft-content {
    padding: 0 40px;
    width: 80%;
    .el-col {
      h4 {
        padding: 12px 0;
        text-transform: uppercase;
      }
      .dowload {
          display: flex;
          align-items: center;
          justify-content: center;
          div {
            padding: 0;
            a{
              width: 100%;
              img{
                width: 70px;
              }
            }
            img {
              width: 70px;
            }
          }
        }
      div {
        
        padding: 6px 0 10px 0;
        i {
          margin-right: 5px;
        }
        a {
          color: #fff;
          text-decoration: none;
          &:hover {
            text-decoration: underline;
          }
        }
      }
    }
  }
}
</style>
