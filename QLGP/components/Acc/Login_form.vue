<template>

      <div class="login-form">
        <!-- <div class="login-banner">Gia ph/ả</div> -->
        <!-- <div class="title">Đăng nhập hệ thống</div> -->
        <div class="login-form-input">
          <i class="el-icon-user-solid append"></i>
          <el-input
            size="medium"
            autofocus
            placeholder="Tên đăng nhập"
            v-model="iUserName"
            :is-error="iUserNameError != null"
          >
          </el-input>
          <div v-if="iUserNameError" class="error-text">
            {{ iUserNameError }}
          </div>
        </div>
        <div class="login-form-input">
          <i class="el-icon-lock append"></i>
          <el-input
            size="medium"
            placeholder="Mật khẩu"
            :type="isShowPassword ? 'text' : 'password'"
            v-model="iPassword"
            :is-error="iPasswordError != null"
            @keypress.enter.native="Login()"
          >
          </el-input>
          <i
            v-if="isShowPassword"
            class="fa fa-eye-slash prepend"
            @click="isShowPassword = !isShowPassword"
          ></i>
          <i
            v-else
            class="fa fa-eye prepend"
            @click="isShowPassword = !isShowPassword"
          ></i>
          <div v-if="iPasswordError" class="error-text">
            {{ iPasswordError }}
          </div>
        </div>
        <div
          style="
            display: flex;
            align-items: center;
            justify-content: space-between;
            padding: 5px;
          "
        >
          <!-- <div style="cursor:pointer" class="Register" @click="toRegiter()">Đăng ký</div> -->
          <!-- <div class="forget-pass" @click="forgetPass()">Quên mật khẩu</div> -->
        </div>

        <div class="button-action">
          <div>
            <el-button type="primary" size="medium" :loading="loading" @click="Login()"
              >Đăng nhập</el-button
            >
            <!-- <el-button type="primary" size="medium"
              >Đăng nhập bằng email</el-button
            > -->
          </div>
        </div>
        <div class="footer">
          <!-- <el-button class="register" type="primary" size="small"
                >Đăng ký</el-button
              > -->
        </div>
      </div>

</template>

<script>
import API, { ServerAPI } from "~/assets/scripts/API";
import DefaultForm from "~/assets/scripts/base/DefaultForm";
import { ShowMessage, validateEmail } from "~/assets/scripts/Functions";
import GetDataAPI from "~/assets/scripts/GetDataAPI";
import StoreManager from "~/assets/scripts/StoreManager";
import { $auth } from "~/plugins/auth";
export default {
  props: {
    Project_Info: {
      type: Object,
    },
  },
  data() {
    return {
    loading: false,
      iUserName: "",
      iUserNameError: "",
      iPassword: "",
      iPasswordError: "",
      isShowPassword: false,
      email: "",
      emailError: "",
      form: new DefaultForm({
        title: "Quên mật khẩu",
        // type: "dialog",
        width: "400px",
        OKtext: "Xác nhận",
      }),
    };
  },
  computed: {
    isValidate() {
      return !(this.iUserNameError || this.iPasswordError);
    },
  },
  methods: {
    isApp() {
      return window.isApp;
    },
    forgetPassProcess() {
      this.emailError = "";
      if (!this.email) this.emailError = "Chưa nhập email.";
      else if (!validateEmail(this.email))
        this.emailError = "email không đúng định dạng.";
      if (!this.emailError) {
        GetDataAPI({
          method: "post",
          url: API.ResetPassword,
          params: {
            iEmail: this.email,
          },
          action: (re) => {
            ShowMessage("Xin vui lòng check email để tiến hành đổi mật khẩu.");
            this.form.visible = false;
          },
        });
      }
    },
    forgetPass() {
      if (!this.iUserName) {
        ShowMessage(
          "Xin vui lòng nhập tên tài khoản để thực hiện tính năng này!"
        );
        return;
      }

      GetDataAPI({
        method: "post",
        url: API.ResetPassword,
        params: {
          iUserName: this.iUserName,
        },
        action: (re) => {
          ShowMessage("Xin vui lòng check email để tiến hành đổi mật khẩu.");
          this.form.visible = false;
        },
      });
      // this.form.visible = true;
      // this.$nextTick(()=>{
      //   document.getElementById('txtEmail').focus();
      // });
    },
    Login() {
      var _app = this;
      this.loading = true;
      this.Validate()
        .then((isValidate) => {
          GetDataAPI({
            url: API.Login,
            params: {
              iUsername: this.iUserName,
              iPassword: this.iPassword,
            },
            method: "post",
            action: (re) => {
              $auth.user = re;
              // Object.assign( $auth.user, re);
              $auth.user.Picture = re.ImgUrl;
              $auth.access_token = re.AccessToken;
              StoreManager.SetHeaders({
                Authorization: `Bearer ${re.AccessToken}`,
                Identity: "",
                SourceAuth: location.origin,
              });

              // console.log(StoreManager);
              localStorage.access_token = re.AccessToken;
              localStorage.user = {};
              localStorage.user = JSON.stringify(re);
              // this.Dongho_watching = user.Dongho_id;
              localStorage.Dongho_watching = re.Dongho_id;
              StoreManager.SetUser(JSON.parse(localStorage.user));
              // console.log(StoreManager);
              _app.$router.push("/");
              this.loading = false;

            },
            error: (re) => {
              ShowMessage(re,'error')
              setTimeout(()=>{this.loading = false;},600)
              

            }
          });
        })
        .catch((e) => {
          console.log(e);
          this.loading = false;
        });
    },
    toRegiter(){
      this.$emit("regiter",true);
    },
    Validate() {
      this.iUserNameError = "";
      this.iPasswordError = "";
      if (!this.iUserName)
        this.iUserNameError = "Xin vui lòng nhập đúng tên đăng nhập";
      if (!this.iPassword) this.iPasswordError = "Xin vui lòng nhập mật khẩu";
      return new Promise((rs, rj) => {
        if (!(this.iUserNameError || this.iPasswordError)) rs(true);
        else rj(false);
      });
    },
  },
  mounted() {
    // this.LoadData();
  },
};
</script>
<style lang="scss" scoped>
@import "~/assets/css/variable.scss";

.forgot-pass-label {
  font-weight: bold;
  padding-left: 1px;
  padding-bottom: 5px;
}

.error-text {
  font-size: 0.7em;
  left: 15px;
  bottom: 25px;
  color: #f56c6c;
}

.forget-pass {
  cursor: pointer;
  // text-align: right;
  color: #1c75bc;
  font-size: 13px;
  padding-top: 5px;
  font-style: italic;
}



.login-form {
      position: relative;
      display: flex;
      flex-direction: column;
      // box-shadow: 0px 10px 34px -15px rgba(0, 0, 0, 0.24);
      // background: #fff;
      width: 300px;
      border-radius: 20px;
      padding: 20px;
  height: fit-content;
      .backdrop-1 {
        position: absolute;
      }

      .error-text {
        position: absolute;
        font-size: 0.7em;
        left: 15px;
        bottom: 5px;
        color: #f56c6c;
      }

      & > div {
        padding-bottom: 10px;

        &:last-of-type {
          padding-bottom: 0;
        }
      }

      .login-banner {
        text-align: center;
        font-weight: bold;
        font-size: 2em;
        height: 50px;
        display: flex;
        align-items: center;
        justify-content: center;

        img {
          max-height: 160px;
          width: auto;
        }
      }

      .title {
        text-align: center;
        font-size: 1.5em;
      }

      .login-form-input {
        position: relative;

        i {
          color: #9d9d9d;
          position: absolute;
          z-index: 1;
          top: 9px;

          &.append {
            left: 12px;
          }

          &.prepend {
            right: 15px;
            top: 2px;
            cursor: pointer;
            padding: 8px;
          }
        }

        .el-input {
          display: flex;

          &[is-error] ::v-deep .el-input__inner {
            border-color: #f56c6c;
          }

          ::v-deep .el-input__inner {
            // border-radius: 20px;
            padding-left: 34px;
          }

          ::v-deep .el-input-group__prepend {
            border-top-left-radius: 20px;
            border-bottom-left-radius: 20px;
            flex: 0 0 50px;
            padding: 0;
            justify-content: center;
            align-items: center;
            display: flex;
            padding-left: 5px;
            background: white;
          }
        }
      }

      .button-action {
        div:first-child {
          display: flex;
          flex-direction: column;
          ::v-deep  button {
            transition: all .2s ease-in;
            &:hover {
              // box-shadow: #d1d1d1 1px 4px 12px;
              span {
                transform: scale(1.1, 1.1);
                transition: all .2s ease-in;
              }
            }
            width: 100%;
            border-radius: 5px;
            // box-shadow: 0 2px 12px 0 #f56c6c;
            // background: #1c75bc;
            margin: 2.5px 0;
            text-align: center;
            span {
              justify-content: center !important;
              // transition: all 0.5 ease-in;
            }
          }
        }
      }

      .footer {
        text-align: center;
        color: #1c75bc;
        opacity: 0.5;
      }
    }
</style>
