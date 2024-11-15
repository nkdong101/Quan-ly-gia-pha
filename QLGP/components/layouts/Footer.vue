<template lang="">
  <div class="footer-w">
    <h4>Phát triển bởi <b>Ngô Kim Đông</b> gnodong2002@gmail.com</h4>
  </div>
</template>
<script>
export default {};
</script>
<style lang="scss" scoped>
.footer-w {
  padding: 10px;
  background: #044085;
  color: #fff;
  display: flex;
  align-items: center;
  justify-content: center;

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
          a {
            width: 100%;
            img {
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
