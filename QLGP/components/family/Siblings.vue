<template lang="">
  <div class="sibling" style="display: flex">
    <div class="title">
      <span
        >Có
        {{
          obj.Siblings && obj.Siblings.length > 0
            ? `${obj.Siblings.length}`
            : "0"
        }}
        anh/chị/em
      </span>

      <el-button
        @click="form.ShowForm('Thêm anh/chị/em', {}, true)"
        type="text"
        style="background-color: #ffffff; padding: 5px 7px"
      >
        <i class="fa fa-plus" aria-hidden="true"></i>
      </el-button>
    </div>

    <div class="list_sib">
      <div v-for="item in obj.Siblings">
        <el-row class="sib_infor">
          <el-col :span="3">
            <el-avatar size="small" v-if="!item.Avatar" icon="el-icon-user-solid"></el-avatar>
            <el-avatar size="small" v-else :src="'/Images/avatar/' + item.Avatar.split('|')[0]" fit="fill"></el-avatar>
          </el-col>
          <el-col :span="6">
            <span>
              {{ item.Name }}
              {{ item.Other_Name ? `(${item.Other_Name})` : "" }}
            </span>
        
          </el-col>
          <el-col :span="2">
            <span>
              {{ Para.Gender.getName(item.Gender) }}

            </span>
          </el-col>
          <el-col :span="6">
            <span v-if="item.Birthday || item.Year_Of_Birth">
              <i class="fa fa-birthday-cake" aria-hidden="true"></i>
              {{
                item.Birthday
                  ? ConvertStr.ToDateStr(item.Birthday)
                  : item.Year_Of_Birth
              }}</span
            >
            &nbsp;
            <span v-if="item.Date_of_death">
              <i class="fa fa-chain-broken" aria-hidden="true"></i>
              {{ ConvertStr.ToDateStr(item.Date_of_death) }}</span
            >
          </el-col>

          <el-col :span="4"> </el-col>
          <el-col :span="3">
            <el-button
              @click="
                form.ShowForm(`Sửa thông tin của ${item.Name}`, item, false)
              "
              type="warning"
              style="padding: 4px"
            >
              <i class="fa fa-edit" aria-hidden="true"></i>
            </el-button>
            <el-button
              type="danger"
              @click="form.Delete(item)"
              style="padding: 4px; margin-left: 2px"
            >
              <i class="fa fa-times"></i>
            </el-button>
          </el-col>
        </el-row>
      </div>
    </div>
    <DefaultForm :model="form" @actionOK="form.Save.call(this)">
      <div slot="content">
        <FormInfo ref="form" :model="form.obj.form2()" />
      </div>
    </DefaultForm>
  </div>
</template>
<script>
import Giapha from "~/assets/scripts/objects/Giapha";
import API from "~/assets/scripts/API";
import DefaultForm from "~/assets/scripts/base/DefaultForm";
import GetDataAPI from "~/assets/scripts/GetDataAPI";
import APIHelper from "~/assets/scripts/API/APIHelper";
import Para from "~/assets/scripts/Para";
import { MessageType, ShowConfirm, ShowMessage } from "~/assets/scripts/Functions";
import { EventBus } from "~/assets/scripts/EventBus.js";
export default {
  props: {
    obj: {},
  },
  data() {
    return {
      form: new DefaultForm({
        obj: new Giapha(),

        title: "",
        visible: false,
        // type: 'dialog',
        width: "450px",
        // noSave: true,
        // fullscreen: true,
        ShowForm: (title, obj, isAdd) => {
          this.isAdd = isAdd;
          var _app = this;
          this.form.title = title;

          if (!isAdd) {
            APIHelper.Giapha.getInfor(obj.Id).then((re) => {
              //   console.log(re);
              this.form.obj = new Giapha({ ...re, type: 4 });

              this.form.visible = true;
            });
          } else {
            this.form.visible = true;
            this.form.obj = new Giapha({
              type: 4,
            });
          }
        },
        Save: () => {
          // thêm/sửa bố mẹ
          // if (this.type == 1) {
          this.form.obj.Dongho_id = this.user.Dongho_id;
          console.log(this);
          //   return;
          this.$refs.form.getValidate().then((re) => {
            if (!re) {
              ShowMessage("Vui lòng nhập đầy đủ thông tin", MessageType.error);
              return;
            } else {
              this.$refs.form
                .getEntry("avatarUrl")
                .submitUpload()
                .then((res) => {
                  console.log(res);
                  this.form.obj.Avatar = res.join(",");
                  GetDataAPI({
                    method: "post",
                    url: API.Add_ACE,
                    params: {
                      Curent_Info: this.obj.Curent,
                      ACE_Info: this.form.obj.toJSON(),
                    },
                    action: (re) => {
                      //   this.LoadData();
                      //   if (this.isAdd) {
                      //     this.form.obj.Project_id = re;
                      //   }
                      //   this.$refs.form.getEntry("formProjectSave").LoadData();
                      ShowMessage("Đã lưu thành công", "success");
                      this.form.visible = false;
                      EventBus.$emit("reloadFormFam", this.obj.Curent.Id);
                    },
                  });
                });
            }
          });
          // vEventBus.$emit("reloadFormFam", this.obj.Curent.Id);

          // }
        },
        Delete: (obj) => {
          ShowConfirm({
            message: "Xóa thông tin của <b>" + obj.Name + "</b>",
            title: "Cảnh báo",
            type: MessageType.warning,
          })
            .then(() => {
              GetDataAPI({
                method: "delete",
                url: API.Giapha + "/" + obj.Id,
                // params: Ơ,
                action: function (re) {
                  ShowMessage("Xóa thành công");
                  this.form.visible = false;
                  EventBus.$emit("reloadFormFam", this.obj.Curent.Id);
                },
              });
            })
            .catch((err) => {
              // An error occurred
            });
        },
      }),
    };
  },
  mounted() {
    // console.log("mountedSiblings",this.obj)
  },
};
</script>
<style lang="scss" scoped>
.sibling {
  display: flex;
  flex-direction: column;
  height: 100%;
  .title {
    background: #6bb4df;
    padding: 5px;
    border: 1px solid #6bb4df;
    border-radius: 5px;
    display: flex;
    align-items: center;
    justify-content: space-between;
    span {
      font-weight: bold;
      color: #ffffff;
    }
  }
  .list_sib {
    padding: 0 5px;
    overflow: auto;
    div {
      .sib_infor {
        display: flex;
        justify-content: unset !important;
        align-items: center;
        padding: 10px 0;
        flex-wrap: wrap;
        border-radius: 0;
        border-bottom: 1px solid gray;
      }
    }
  }
}
</style>
