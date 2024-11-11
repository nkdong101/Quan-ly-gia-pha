<template>
  <div class="logging">
    <img src="~/assets/images/loading.svg" alt="Loading..." width="80px" />
    Logging in...
  </div>
</template>

<script>
import API from "~/assets/scripts/API";
import GetDataAPI from "~/assets/scripts/GetDataAPI";
import StoreManager from "~/assets/scripts/StoreManager";
import { $auth } from "~/plugins/auth";
export default {
  layout: "blank",
  mounted() {
    let code = new URLSearchParams(location.search).get("code");
    let error = new URLSearchParams(location.search).get("error");
    if (code) {
      console.log(code);
      StoreManager.SetHeaders({
        redirect_uri: location.origin + location.pathname,
        // Authorization: `Bearer ${re.access_token || this.access_token}`,
        // Identity: this.user.email,
        // SourceAuth: location.origin,
      });
      GetDataAPI({
        url: API.Login,
        method: "post",
        params: {
          code: code,
        },
        action: (re) => {
          $auth.access_token = re.access_token;
          $auth.identity = re.email;
          StoreManager.SetHeaders({
            Authorization: `Bearer ${re.access_token}`,
            Identity: re.email,
            SourceAuth: location.origin,
          });
          $auth.userinfo().then(() => {
            this.$router.push("/");
          });
        },
        error: (error) => {
          localStorage.err_message = error;
          this.$router.push("/Account/Unauthorized");
        },
      });
      // $auth.processCallback(code).then(() => {
      //   // this.$router.push("/");
      // });
    } else if (error) {
      localStorage.err_message = error;
      this.$router.push("/Account/Unauthorized");
    } else {
      this.$router.push("/");
    }
  },
};
</script>

<style lang="scss" scoped>
.logging {
  display: flex;
  align-items: center;
  justify-content: center;
  flex-direction: column;
}
</style>
