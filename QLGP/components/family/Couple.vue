<template lang="">
  <div style="height: fit-content">
    <!-- <div v-for="item in obj.Couple">
      <PNode
        :nameTitle="item.Gender == 1 ? 'Chồng' : 'Vợ'"
        :type="3"
        :Pid="''"
        :obj="item"
        :hus="item.Gender == 1 ? false : true"
      />
      <div class="childs">
        <p>Danh sách con chung</p>
        <div v-for="itemC in obj.Childs" :key="itemC.Id">
          <childe :id="itemC.Id"></childe>
        </div>
      </div>
    </div> -->
    <el-tree
      default-expand-all
      v-if="obj.Couple && obj.Couple.length > 0"
      :data="obj.Couple"
      :default-expanded-keys="[1, 2]"
      :props="defaultProps"
      node-key="Id"
    >
      <div class="custom-tree-node" slot-scope="{ node, data }">
        <!-- <p v-if="node.level == 1">{{data.Gender == 1? "Chồng": "Vợ"}}</p> -->
        <el-row class="">
          <el-col :span="3">
            <el-avatar
              size="small"
              v-if="!data.Avatar"
              icon="el-icon-user-solid"
            ></el-avatar>
            <el-avatar
              v-else
              size="small"
              :src="'/Images/avatar/' + data.Avatar.split('|')[0]"
              fit="cover"
            ></el-avatar>
          </el-col>
          <el-col :span="6">
            <span>
              {{ data.Name }}
              {{ data.Other_Name ? `(${data.Other_Name})` : "" }}</span
            >
          </el-col>
          <el-col
            style="text-align: center"
            :sm="4"
            :style="{
              paddingLeft: node.level == 1 ? '8px' : '0',
            }"
            :span="4"
          >
            <span>
              {{ Para.Gender.getName(data.Gender) }}
            </span>
          </el-col>
          <el-col
            :sm="6"
            :style="{
              paddingLeft: node.level == 1 ? '8px' : '0',
            }"
            :span="6"
          >
            <span
              v-if="
                (data.Birthday || data.Year_Of_Birth) && !data.Date_of_death
              "
            >
              <i class="fa fa-birthday-cake" aria-hidden="true"></i>
              {{
                data.Birthday
                  ? ConvertStr.ToDateStr(data.Birthday)
                  : data.Year_Of_Birth
              }}</span
            >

            <span v-if="data.Date_of_death">
              <i class="fa fa-chain-broken" aria-hidden="true"></i>
              {{ ConvertStr.ToDateStr(data.Date_of_death) }}</span
            >
          </el-col>

          <!-- <el-col :sm="4" :span="5"> </el-col>
          <el-col :sm="4" :span="2"> </el-col> -->
        </el-row>
        <div>
          <el-button
            @click="EditVo(data)"
            type="primary"
            v-if="node.level === 1"
            style="padding: 4px"
          >
            <i class="fa fa-edit"></i>
          </el-button>
          <el-button
            @click="AddChild(data)"
            type="primary"
            v-if="node.level === 1 && user.userLevel == 1"
            style="padding: 4px; margin-left: 5px; margin-right: 10px"
          >
            <i class="fa fa-plus"></i>
          </el-button>

          <el-button
            type="warning"
            v-if="node.level === 2"
            @click="
              form.ShowForm(`Sửa thông tin ${data.Name}`, data, false, false)
            "
            style="padding: 4px; margin-right: 7px"
          >
            <i class="fa fa-edit"></i>
          </el-button>
          <el-button
            type="danger"
            v-if="node.level === 2 && user.userLevel == 1"
            @click="form.Delete(data)"
            style="padding: 4px; margin-right: 12px; margin-left: 0"
          >
            <i class="fa fa-times"></i>
          </el-button>
        </div>
      </div>
    </el-tree>

    <div
      v-else
      style="
        width: 100%;
        padding: 20px 0 0 0;
        font-weight: normal;
        text-align: center;
      "
    >
      <i style="font-size: 14px"
        >Không có thông tin {{ obj.Curent.Gender == 1 ? "vợ" : "chồng" }}</i
      >
    </div>
    <DefaultForm :model="form" @actionOK="form.Save.call(this)">
      <div slot="content">
        <div v-if="isAdd">
          <el-radio v-model="conrieng" :label="true">Con riêng</el-radio>
          <el-radio v-model="conrieng" :label="false">Con chung</el-radio>
        </div>

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
import {
  MessageType,
  ShowConfirm,
  ShowMessage,
} from "~/assets/scripts/Functions";
import { EventBus } from "~/assets/scripts/EventBus.js";
export default {
  props: {
    obj: {},
  },
  data() {
    return {
      isAdd: false,
      isEditWife: false,
      mother: {},
      conrieng: false,
      defaultProps: {
        children: "Childs",
        label: "Name",
        // gender: 'gender',
      },
      form: new DefaultForm({
        obj: new Giapha(),
        // btns: [
        //   {
        //     id: 1,
        //     action: "addConrieng",
        //     text: "Thêm con riêng",
        //     type:"info"
        //   },
        // ],
        // OKtext: "Thêm con chung",
        title: "",
        visible: false,
        // type: 'dialog',
        width: "450px",
        // noSave: true,
        // fullscreen: true,
        ShowForm: (title, obj, isAdd, isEditWife) => {
          if (this.user.userLevel !== 1) {
            this.form.type = "dialog";
          } else {
            this.form.type = "";
          }
          this.isAdd = isAdd;
          var _app = this;
          this.form.title = title;
          // console.log(obj);
          this.isEditWife = isEditWife;
          if (!isAdd) {
            APIHelper.Giapha.getInfor(obj.Id).then((re) => {
              //   console.log(re);
              this.form.obj = new Giapha({ ...re, type: 4 });

              this.form.visible = true;
            });
          } else {
            this.mother = obj;
            this.form.visible = true;
            this.form.obj = new Giapha({
              type: 4,
            });
          }
        },
        Save: () => {
          this.form.obj.Dongho_id = this.user.Dongho_id;

          this.$refs.form.getValidate().then((re) => {
            if (!re) {
              ShowMessage("Vui lòng nhập đầy đủ thông tin", MessageType.error);
              return;
            } else {
              this.$refs.form
                .getEntry("avatarUrl")
                .submitUpload()
                .then((res) => {
                  // console.log(res);
                  this.form.obj.Avatar = res.join(",");
                  if (this.isEditWife) {
                    GetDataAPI({
                      method: "post",
                      url: API.Add_Vo,
                      params: {
                        Chong_Info: this.obj.Curent,
                        Vo_Info: this.form.obj.toJSON(),
                        Index: this.obj.Couple.findIndex(
                          (p) => p.Id === this.form.obj.Id
                        ),
                      },
                      action: (re) => {
                        //   this.LoadData();
                        //   if (this.isAdd) {
                        //     this.form.obj.Project_id = re;
                        //   }
                        //   this.$refs.form.getEntry("formProjectSave").LoadData();
                        ShowMessage("Cập nhật thông tin thành công", "success");
                        this.form.visible = false;
                        EventBus.$emit("reloadFormFam", this.obj.Curent.Id);
                      },
                    });
                  }

                  console.log(this);
                  // return;
                  GetDataAPI({
                    method: "post",
                    url: API.Add_Con,
                    params: {
                      Con_Info: this.form.obj.toJSON(),
                      Me_Info:
                        this.obj.Curent.Gender == 1
                          ? this.mother
                          : new Giapha(this.obj.Curent).toJSON(),
                      Father_id:
                        this.obj.Curent.Gender == 1
                          ? this.obj.Curent.Id
                          : this.mother.Id,
                      Conrieng: this.conrieng,
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
          console.log(obj);
          return;
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
  methods: {
    EditVo(obj) {
      this.form.ShowForm(
        "Sửa thông tin của " + obj.Name + "",
        obj,
        false,
        true
      );
    },
    SaveCR() {
      this.conrieng = true;
      this.form.Save();
    },
    AddChild(mother) {
      this.form.ShowForm("Thêm con", mother, true);
      // this;
    },
    clickRow(node, data) {
      console.log(node, data);
    },
  },
  mounted() {
    // console.log("mountedCouple",this.obj)
  },
};
</script>
<style lang="scss" scoped>
.el-tree-node.is-expanded
  > .el-tree-node__children
  > .el-tree-node
  > .el-tree-node__content:first-child
  .custom-tree-node {
  /* Empty to ensure children are not affected */
}

.el-tree {
  height: 100%;
  overflow: auto;
  ::v-deep .el-tree-node.is-expanded {
    // height: 100%;
    // display: flex;
    // flex-direction: column;
    // overflow: hidden;
    // .el-tree-node__children {
    //   height: 100%;
    //   overflow: auto;
    // }
  }
}
::v-deep
  .el-tree-node
  > .el-tree-node__children
  > .el-tree-node
  > .el-tree-node__content {
  //   padding: 2.5px;
  height: 40px;
  /* Empty to ensure children are not affected */
}

::v-deep .el-tree-node__content {
  height: unset;
}

.el-tree-node__content .custom-tree-node {
  width: 100%;
  //   background-color: rgb(222, 224, 225);
  padding: 2px;
  padding-left: 5px;
  //   margin-bottom: 5px;
  display: flex;
  align-items: center;
  justify-content: space-between;
  .el-row {
    display: flex;
    align-items: center;
    width: 100%;
    margin-bottom: 0 !important;
  }
  border: 1px solid #dcdcdc;
}

/* Additional styles */
.el-tree-node__children
  > .el-tree-node
  > .el-tree-node__content
  .custom-tree-node {
  background-color: unset;
  padding: unset;
  border: unset;
}
</style>
