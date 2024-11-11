<template>
  <div style="font-family: Inter, Arial, sans-serif;  scroll-behavior: smooth !important;letter-spacing: 0.03em;" >
    <TopMenu @login="loginClick" ref="header" />
    <Nuxt />
    <Footer ></Footer>


  </div>
</template>

<script>
import StoreManager from "~/assets/scripts/StoreManager";

export default {
  data() {
    return {
      isAdd: null,

    };
  },
  methods: {
    // onWheel(e) {
    //   console.log(e, this);
    // },
    loginClick(isAdd){

      this.isAdd = isAdd;

    }
  },
  mounted() {
    const header = this.$refs.header;
    window.addEventListener("scroll", () => {
      this.$nextTick(() => {
        // console.log(header)
        if (window.scrollY > 50) {
          // Adjust the scroll value as needed
          header.$el.classList.add("scrolled");
        } else {
          header.$el.classList.remove("scrolled");
        }
      });
      // console.log(window.scrollY);
    });
  },
};
</script>

<style lang="scss" scoped>


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


