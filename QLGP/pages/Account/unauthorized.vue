<template>
  <div style="background-color: #fb6e52; color: #fff; overflow: auto">
    <div
      class="error-header"
      style="background: #fff; height: 280px; background-color: white"
    ></div>
    <div
      class="container"
      style="
        padding-right: 15px;
        padding-left: 15px;
        margin-right: auto;
        margin-left: auto;
        width: 1200px;
        max-width: 100%;
      "
    >
      <section
        class="error-container text-center"
        style="background: rgba(0, 0, 0, 0.05); text-align: center"
      >
        <h1
          style="
            font-size: 110px;
            font-weight: 300;
            margin: -118px 0 0 0;
            text-align: center;
            color: #fb6e52;
          "
        >
          Error 401
        </h1>
        <div class="error-divider" style="margin-top: -27px; padding: 30px 0">
          <h2
            style="
              font-size: 58px;
              font-weight: 300;
              margin: 0;
              text-transform: uppercase;
            "
          >
            Unauthorized Error
          </h2>
          <p
            style="
              line-height: 1.5em;
              color: #e5e5e5;
              font-size: 40px;
              margin: 0;
            "
            class="description"
            v-html="err_message"
          ></p>
        </div>
        <a
          href="/"
          class="return-btn"
          style="
            border: 1px solid rgba(255, 255, 255, 0.3);
            padding: 10px 15px;
            margin-top: 100px;
            margin-bottom: 30px;
            display: inline-block;
            -webkit-border-radius: 5px;
            -webkit-background-clip: padding-box;
            -moz-border-radius: 5px;
            -moz-background-clip: padding;
            border-radius: 5px;
            background-clip: padding-box;
            color: #fff;
            font-size: 16px;
            font-weight: 300;
          "
        >
          <i class="fa fa-home" style="margin-right: 5px"></i>
          Dashboard
        </a>
      </section>
    </div>
  </div>
</template>

<script>
import { $auth } from "~/plugins/auth";
export default {
  layout: "blank",
  data() {
    return {
      err_message: localStorage.err_message,
    };
  },
  mounted() {
    // debugger;
    this.err_message = localStorage.err_message;
    if (localStorage.err_message) {
      $auth.logout();
    } else {
      this.$router.push("/");
    }
  },
};
</script>

<style lang="scss" scoped>
</style>
