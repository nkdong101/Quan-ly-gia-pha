<template>
  <div class="main">
    <!-- <div class="head-title">
        Hệ thống quản lý chương trình Dự phòng và Tư vấn xét nghiệm HIV/AIDS
      </div> -->

    <div class="backdrop">
      <transition name="slide-fade" mode="out-in">
        <!-- <Login_form v-if="!isRegit" @regiter="handleRegister"></Login_form>
        <Register @regiter="handleRegister" v-else></Register> -->

        <component @regiter="handleRegister" :is="isRegit ? 'Register' : 'Login_form'" />
      </transition>
    </div>
  </div>
</template>

<script>

export default {
  layout: "blank",
  // middleware: "Auth",
  data() {
    return {
      isRegit: false,
    };
  },
  methods: {
    handleRegister(value) {
      this.isRegit = value;
      // console.log(value);
    },
  },
};
</script>

<style lang="scss" scoped>
.slide-fade-enter-active, .slide-fade-leave-active {
  transition: all 0.3s ease-in-out;
}
.slide-fade-enter /* .slide-fade-leave-active in <2.1.8 */ {
 
  transform:translateX(25%);
  opacity: 0;
}

.slide-fade-leave-to{
  transform: translateX(-25%);
  opacity: 0;
}



.main {
  background: #f8f9fd;

  display: flex;
  // // flex-direction: column;
  align-items: center;
  justify-content: center;
  // align-items: center;
  color: #1c75bc;
  overflow: auto;

  .head-title {
    color: black;
    max-width: 100vw;
    text-align: center;
    width: 440px;
    padding: 0 20px;
    padding-bottom: 25px;
    font-size: 22px;
  }

  .footer-title {
    display: flex;
    flex-direction: column;
    align-items: center;
    padding-bottom: 10px;
    max-width: 100vw;

    position: fixed;
    bottom: 0;

    .image-c {
      overflow: hidden;
      display: flex;
      margin-top: 10px;
      max-width: 100vw;

      img {
        height: 10vw;
        max-height: 50px;
      }
    }

    .title-c {
      color: #fff;
      font-size: 12px;
      padding-top: 10px;
      max-width: 100vw;
      text-align: center;
      width: 300px;
    }
  }

  .backdrop {
    // position: absolute;
   
    // top: 50%;
    // left: 50%;
    // transform: translate(-50%, -50%);
    display: flex;
    align-items: center;
    // &:after,
    // &:before {
    //   content: "";
    //   background: #f56c6c;
    //   height: calc(100% - 80px);
    //   width: 20px;
    //   position: absolute;
    //   top: 40px;
    // }

    // &::after {
    //   left: -20px;
    //   border-top-left-radius: 20px;
    //   border-bottom-left-radius: 20px;
    // }

    // &::before {
    //   border-top-right-radius: 20px;
    //   border-bottom-right-radius: 20px;
    //   left: 100%;
    // }
  }
}
</style>
