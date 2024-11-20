<template lang="">
    <div style="height: 100%;max-width:fit-content;">
      <TablePaging ref="tp" :model="tp">
        <template slot="column-header-button">
          <el-button
            @click="form.ShowForm('Thêm tài khoản', {})"
            type="primary"
            v-if="pagePermission.add"
          >
            <i class="fa fa-plus"></i>
          </el-button>
        </template>
        <template slot="column-content-button" slot-scope="{ row }">
          <div style="display: flex; align-items: center">
            <el-button
              @click="form.ShowForm('Sửa tài khoản', row)"
              type="warning"
              v-if="pagePermission.edit"
            >
              <i class="fa fa-edit"></i>
            </el-button>
            <el-button @click="form.Delete(row)" v-if="pagePermission.delete" type="danger">
              <i class="fa fa-times"></i>
            </el-button>
          </div>
        </template>
      </TablePaging>
  
      <!-- <DefaultForm :model="form" @actionOK="form.Save.call(this)">
        <div slot="content">
          <FormInfo :model="form.obj.form()" />
        </div>
      </DefaultForm> -->
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
  import Ho_VN from "~/assets/scripts/objects/Ho_VN";
  import {Para} from "~/assets/scripts/Para";
  export default {
    data() {
      return {
        form: {
          obj: new Ho_VN(),
          visible: false,
          width: "400px",
          title: "",
          ShowForm: (title, obj) => {
            this.form.title = title;
  
            this.form.obj = new Ho_VN(obj);
  
            this.form.visible = true;
          },
          Save: () => {
            // this.$refs.form.
            if (this.form.obj.Id) {
              GetDataAPI({
                url: API.HoVietNam + `/${this.form.obj.Id}`,
                //    API.DonviHanhchinh_Add_Tinh,
                method: "PUT",
                params: this.form.obj.toJSON(),
                action: (re) => {
                  ShowMessage("Sửa thành công", "success");
                  this.LoadTable();
                  this.form.visible = false;
                },
              });
            } else {
              GetDataAPI({
                url:
                  API.HoVietNam,
                method: "POST",
                params: this.form.obj.toJSON(),
  
                action: (re) => {
                  ShowMessage("Thêm thành công", "success");
                  this.LoadTable();
                  this.form.visible = false;
                },
              });
            }
          },
          Delete: (row) => {
            ShowConfirm({
              message: "Xóa tỉnh [" + row.Name + "]",
              title: "Cảnh báo",
              type: MessageType.warning,
            })
              .then((re) => {
                // console.log(re);
                GetDataAPI({
                  method: "Delete",
                  url: API.HoVietNam + "/" + row.Id,
                  // params: this.selectedRow,
                  action: function (re) {
                    this.LoadTable();
  
                    ShowMessage("Xóa thành công");
                  },
                });
              })
              .catch((err) => {
                // An error occurred
              });
          },
        },
        tp: {
          title: "Danh sách đơn vị hành chính",
          data: [],
          cols: [
          new TablePagingCol({
              title: "STT",
              data: "SttTP",
              width: 60,
            }),
           
            new TablePagingCol({
              title: "Tên đăng nhập",
              data: "UserName",
            //   width: 350,
            }),
            new TablePagingCol({
              title: "Tên đầy đủ",
              data: "FullName",
            }),
            new TablePagingCol({
              title: "Địa chỉ",
              data: "Address",
              width: 250,
            }),
             new TablePagingCol({
               title: "Dòng họ",
               data: "Dongho_Name",
               width: 250,
              //  formatter: (value) => Para.dong_ho.getName(value),
             }),
            // new TablePagingCol({
            //   title: "Số CMND/CCCD",
            //   data: "Dongho_id",
            // }),
            new TablePagingCol({
              title: "Ngày sinh",
              data: "BirthDay",
              formatter: "date",
            }),
            new TablePagingCol({
              title: "Điện thoại",
              data: "Phone",
            }),
            new TablePagingCol({
              title: "Giới Tính",
              data: "Gender",
              formatter: (value) => Para.Gender.getName(value)
            }),
            new TablePagingCol({
              title: "Email",
              data: "Email",
            }),
            // new TablePagingCol({
            //   title: "Danh sách tỉnh",
            //   data: "DS_Tinh",
            // }),
            new TablePagingCol({
              title: "",
              data: "button",
              fix: 'right',
              sortable: false,
            }),
          ],
        },
      };
    },
    methods: {
      loadData() {
        this.$refs.tp.LoadData(true);
      },
      LoadTable() {
        GetDataAPI({
          url: API.Account,
        //   method: "get",
          action: (re) => {
            this.tp.data = re;
            this.loadData();
          },
        });
      },
    },
    mounted() {
      console.log(this)
      this.LoadTable();
    },
  };
  </script>
  <style lang=""></style>
  