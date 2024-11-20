<template>
    <div style="height: 100%">
      <TablePaging :ref="tp.tpRef" :tpRef="tp.tpRef" :model="tp.model">
        <template slot="column-header-button">
          <el-button
            class="icon-btn icon-btn"
            v-if="pagePermission.add"
            type="primary"
            @click="Add()"
          >
            <i class="el-icon-plus"></i
          ></el-button>
        </template>
        <template slot="column-content-button" slot-scope="{ row }">
          <div style="display:flex">
            <el-button
            v-if="pagePermission.Edit"
            type="primary"
            icon="fa fa-edit"
            class="icon-btn"
            @click="Edit(row)"
          ></el-button>
          <el-button
            v-if="pagePermission.delete"
            type="danger"
            icon="fa fa-times"
            class="icon-btn"
            @click="form.Delete(row)"
          ></el-button>
          </div>
         
        </template>
      </TablePaging>
  
      <DefaultForm :model="form" @actionOK="form.Save.call(this)">
        <div slot="content">
          <FormInfo :model="form.obj.form()" />
        </div>
      </DefaultForm>
    </div>
  </template>
  
  <script>
  import API from "~/assets/scripts/API";
  import TablePagingCol from "~/assets/scripts/base/TablePagingCol";
  import { EventBus } from "~/assets/scripts/EventBus";
  import {
    ShowMessage,
    ShowConfirm,
    MessageType,
  } from "~/assets/scripts/Functions";
  import GetDataAPI from "~/assets/scripts/GetDataAPI";
  import GroupPermission from "~/assets/scripts/objects/menu/GroupPermission";
  
  export default {
    data() {
      return {
        tp: {
          tpRef: "GroupPermissionTable",
          model: {
            // title: "menu",
            data: API.GroupPermission,
            params: {},
  
            cols: [
              { Index: 1, data: "SttTP", title: "STT", min_width: 60 },
              {
                Index: 2,
                data: "Name",
                title: "Loại tài khoản",
                min_width: 200,
              },
  
              {
                Index: 3,
                data: "Description",
                title: "Mô tả",
                min_width: 200,
                width: "auto",
              },
              // new TablePagingCol({
              //   title: "User Update",
              //   data: "UserUpdate",
              //   min_width: 160,
              // }),
              new TablePagingCol({
                title: "Ngày cập nhật",
                data: "DateUpdate",
                min_width: 150,
                formatter: "date",
              }),
              new TablePagingCol({
                data: "button",
                min_width: 100,
                sortable: false,
              }),
            ],
          },
        },
        form: {
          obj: GroupPermission(),
          visible: false,
          width: "800px",
          title: "",
          ShowForm: (title, obj) => {
            this.form.title = title;
            if (obj) {
              this.form.obj = GroupPermission(obj);
            } else {
              this.form.obj = GroupPermission(null);
            }
            this.form.visible = true;
          },
          Save: () => {
            var _app = this;
            GetDataAPI({
              method: this.form.obj.Id ? "put" : "post",
              url:
                API.GroupPermission +
                (this.form.obj.Id ? "/" + this.form.obj.Id : ""),
              params: this.form.obj,
              action: function (re) {
                if (re == "OK" || Number.isInteger(+re)) {
                  _app.LoadData();
                  _app.form.visible = false;
                  ShowMessage("Đã lưu thành công");
                } else {
                  ShowMessage(re);
                }
              },
            });
          },
          Delete: (row) => {
            var _app = this;
            if (this.selectedRow == null) {
              ShowMessage("Chọn 1 nhóm quyền để xóa!");
              return;
            }
            ShowConfirm({
              message: "Xóa loại tài khoản[" + Delete.Name + "]",
              title: "Cảnh báo",
              type: MessageType.warning,
            })
              .then(() => {
                GetDataAPI({
                  method: "delete",
                  url: API.GroupPermission + "/" + Delete.Id,
                  params: Delete,
                  action: function (re) {
                    if (re == "OK") {
                      _app.LoadData();
                      ShowMessage("Xóa thành công");
                    } else {
                      ShowMessage(re);
                    }
                  },
                });
              })
              .catch((err) => {
                // An error occurred
              });
          },
        },
      };
    },
    computed: {
      selectedRow: function () {
        return this.$refs[this.tp.tpRef].selectedRow;
      },
    },
    methods: {
      LoadData() {
        this.$refs[this.tp.tpRef].LoadData(true);
      },
      Add() {
        this.form.ShowForm("Thêm quyền cho tài khoản");
      },
      Edit(row) {
        this.form.ShowForm("Sửa quyền cho tài khoản", row);
      },
    },
    beforeMount() {
      EventBus.$on("Add", this.Add);
    },
  
    beforeDestroy() {
      EventBus.$off("Add", this.Add);
    },
  };
  </script>
  
  <style lang="scss" scoped></style>
  