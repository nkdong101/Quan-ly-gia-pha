<template>
  <div>
    <el-popover placement="bottom-end"
                width="320"
                trigger="click"
                v-model="profile_pop_visible">
      <el-button slot="reference"
                 type="text"
                 style="padding: 0px 10px; display: flex"
                 size="small">
        <div style="display: flex; align-items: center; color: white">
          <div class="circle-avatar">
            <!-- <img :src="user.Picture.split('|')[0] + '?t=' + new Date().getTime()"
                 :alt="user.Picture.split('|')[0] " /> -->
                 <i style="color: white" class="el-icon-user-solid"></i>
          </div>
        </div>
      </el-button>
      <div class="pop-main">
        <div class="profile-main">
          <div>
            <div class="circle-avatar"
                 style="height: 80px; width: 80px;">
              <img style="height: 100%;" :src="user.Picture ?  '/Images/avatar/' + user.Picture.split('|')[0]
          : '/Images/avatar/user.png' + '?t=' + new Date().getTime()"
                   :alt="user.Picture ?  '/Images/avatar/' + user.Picture.split('|')[0]
          : '/Images/avatar/user.png'"  />
            </div>
          </div>
          <div class="profile-info">
            <div>
              <b>{{ user.FullName }}</b>
            </div>
            <div>{{ user.Email }}</div>
            <div class="pop-main-action"
                 style="">
              <el-button type="text"
                         size="small"
                         style="padding: 0"
                         @click="profile()">
                Thông tin
              </el-button>
              <el-button type="text"
                         size="small"
                         style="padding: 0"
                         @click="logout()">
                Đăng xuất
              </el-button>
            </div>
          </div>
        </div>
      </div>
    </el-popover>
    <DefaultForm :model="form_profile"
                 @actionOK="SaveProfile()">
      <div slot="content">
        <FormInfo ref="user_profile"
                  :model="user_profile.form()" />
      </div>
    </DefaultForm>
  </div>
</template>

<script>
import API from "~/assets/scripts/API";
import DefaultForm from "~/assets/scripts/base/DefaultForm";
import GetDataAPI from "~/assets/scripts/GetDataAPI";
import { LoginResult } from "~/assets/scripts/objects/LoginResult";
import StoreManager from "~/assets/scripts/StoreManager";
export default {
  data() {

    return {
      // user: JSON.parse(localStorage.user),
      profile_pop_visible: false,
      user_profile: new LoginResult(),
      form_profile: new DefaultForm({
        title: "User profile",
        width: "400px",
      }),
    };
  },
  methods: {
    SaveProfile() {
      // alert("123");
      // console.log(this);
      this.$refs.user_profile.getValidate().then((re) => {
        if (re) {
          this.$refs.user_profile
            .getEntry("txtImage")
            .submitUpload()
            .then((results) => {
              this.user_profile.Image = results[0];
              GetDataAPI({
                method: "post",
                url: API.UpdateProfile,
                params: this.user_profile.toJSON(),
                action: (re) => {
                  StoreManager.SetUser(this.user_profile);
                  this.form_profile.visible = false;
                  // location.reload(true);
                },
              });
            });
        } else {
          ShowMessage("Please enter full information", "error");
        }
      });
    },

    profile() {
      this.profile_pop_visible = false;
      this.user_profile.update({
        ...this.user,
        Email: this.$auth.user.Email,
      });
      this.form_profile.visible = true;
    },
    logout() {
      localStorage.clear();
      StoreManager.SetUser(null)
          this.$router.push("/Account/Home");

    },
  },
  computed:{
    // user(){
    //   return
    // }
  },
  beforeMount(){
    // console.log('Avatar',this.user)
    // StoreManager.SetUser( JSON.parse(localStorage.user))
  },
  mounted(){

    // console.log('Avatar',this)
  }
};
</script>

<style lang="scss" scoped>
.circle-avatar {
  // height: 35px;
  // width: 35px;
  // border-radius: 100%;
  overflow: hidden;
  // border: 1px solid #fff;
  color: #fff

  img {
    height: 100%;
  }
}

.pop-main {
  padding: 5px 5px;

  .pop-main-action {
    display: flex;
    justify-content: space-between;
  }

  .profile-main {
    display: flex;
    align-items: center;

    .profile-info {
      padding-left: 20px;

      >div {
        height: 20px;
      }
    }
  }
}</style>
