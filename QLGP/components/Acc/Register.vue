<template lang="">



  <div class="regis">

    
    <div style="display: flex; align-items: baseline">
      <div class="left-side">
        <!-- <p class="title" style="margin: 20px 0">Thông tin đăng nhập</p>

        -->

        <el-form
          :model="objUSer"
          :rules="rules"
          ref="objUSer"
          label-width="120px"
          class="demo-objUSer"
        >
          <el-form-item label="Tên đăng nhập" prop="UserName" required>
            <el-input v-model="objUSer.UserName"></el-input>
          </el-form-item>
          <el-form-item label="Mật khẩu" prop="Password" required>
            <el-input type="password" v-model="objUSer.Password"></el-input>
          </el-form-item>
          <el-form-item label="Tên liên hệ" prop="FullName" required>
            <el-input v-model="objUSer.FullName"></el-input>
          </el-form-item>
          <!-- <el-form-item label="Địa chỉ" prop="Address">
            <el-input v-model="objUSer.Address"></el-input>
          </el-form-item> -->
          <!-- <el-form-item label="Email" prop="Email">
            <el-input v-model="objUSer.Email"></el-input>
          </el-form-item>
          <el-form-item label="Số định danh" prop="CMND">
            <el-input v-model="objUSer.CMND"></el-input>
          </el-form-item> -->
          <el-form-item label="Ngày sinh" prop="BirthDay">
            <el-date-picker
              format="dd/MM/yyyy"
              type="date"
              placeholder="Ngày sinh"
              v-model="objUSer.BirthDay"
            ></el-date-picker>
          </el-form-item>
          <el-form-item label="Số liên hệ" prop="Phone">
            <el-input v-model="objUSer.Phone"></el-input>
          </el-form-item>

          <el-form-item class="gender" label="Giới tính" required prop="Gender">
            <el-radio-group v-model="objUSer.Gender">
              <el-radio :label="1">Nam</el-radio>
              <el-radio :label="2">Nữ</el-radio>
            </el-radio-group>
          </el-form-item>

          <!-- <el-form-item>
            <el-button type="primary" @click="submitForm('objUSer')"
              >Create</el-button
            >
            <el-button @click="resetForm('objUSer')">Reset</el-button>
          </el-form-item> -->
        </el-form>
        <div
          @click="$emit('regiter', false)"
          style="cursor: pointer; width: fit-content"
        >
          <el-button style="font-size: 12px; "
            >Đã có tài khoản?</el-button
          >
        </div>
      </div>
      <div class="right-side">
        <!-- <p class="title" style="margin: 20px 0">Thông tin dòng họ</p> -->
        <el-form
          :model="Dongho_Info"
          :rules="rules_Dongho_Info"
          ref="Dongho_Info"
          label-width="120px"
        >
          <el-form-item label="Tên dòng họ" prop="Name" required>
            <el-input v-model="Dongho_Info.Name"></el-input>
          </el-form-item>
          <el-form-item label="Họ">
            <InputSelect
              placeholder=""
              v-model="Dongho_Info.Ho_Vietnam_id"
              :model="list_Dongho"
            />
          </el-form-item>
          <el-form-item required label="Tỉnh">
            <InputSelect
              ref="tinh"
              placeholder=""
              @input="selectTinh"
              v-model="Dongho_Info.Tinh_id"
              :model="list_Tinh"
            />
          </el-form-item>
          <el-form-item required label="Huyện">
            <InputSelect
              placeholder=""
              v-model="Dongho_Info.Huyen_id"
              :model="list_huyen"
            />
          </el-form-item>
          <el-form-item label="Mô tả">
            <el-input
              type="textarea"
              v-model="Dongho_Info.Description"
            ></el-input>
          </el-form-item>
          <!-- <el-form-item>


          </el-form-item> -->
        </el-form>
      </div>
    </div>
    <div style="text-align: center; padding: 20px 0 0 0">
      <el-button
        type="primary"
        style="padding: 8px 30%; font-size: 16px"
        @click="submitForm('objUSer', 'Dongho_Info')"
        >Khởi tạo</el-button
      >
    </div>
  </div>
</template>
<script>
import API, { ServerAPI } from "~/assets/scripts/API";
import DefaultForm from "~/assets/scripts/base/DefaultForm";
import { SelectOption } from "~/assets/scripts/base/SelectOption";
import { ShowMessage, validateEmail } from "~/assets/scripts/Functions";
import GetDataAPI from "~/assets/scripts/GetDataAPI";
import StoreManager from "~/assets/scripts/StoreManager";
import { $auth } from "~/plugins/auth";
export default {
  layout: "blank",
  data() {
    return {
      list_huyen: new SelectOption({
        data: [],
      }),
      list_Tinh: new SelectOption({
        data: API.DonviHanhchinh_Get_Tinh,
      }),
      list_Dongho: new SelectOption({
        data: API.HoVietNam,
      }),
      Dongho_Info: {
        Name: "",
        Ho_Vietnam_id: "",
        Tinh_id: "",
        Huyen_id: "",
        Description: "",
      },
      objUSer: {
        UserName: "",
        Password: "",
        FullName: "",
        Images: "",
        Address: "",
        CMND: "",
        BirthDay: "",
        Phone: "",
        Gender: "",
        Email: "",
      },
      rules: {
        UserName: [
          {
            required: true,
            message: "Vui lòng nhập trường này",
            trigger: "blur",
          },
         
        ],
        Password: [
          {
            required: true,
            message: "Vui lòng nhập trường này",
            trigger: "blur",
          },
         
        ],
        FullName: [
          {
            required: true,
            message: "Vui lòng nhập trường này",
            trigger: "blur",
          },
         
        ],
      },
      rules_Dongho_Info: {
        Name: [
          {
            required: true,
            message: "Vui lòng nhập thông tin",
            trigger: "blur",
          },
        ],
      },
      tinhSeleceted: {},
    };
  },
  computed: {
    // listHuyen() {
    //   // var _app = this;
    //   return new SelectOption({
    //     data: this.$refs.tinh && this.$refs.tinh.selectedData ? this.$refs.tinh.selectedData.Childs : [],
    //   })
    // },
  },
  methods: {
    selectTinh(value) {
      console.log(this);
      if (value) {
        this.$nextTick(() => {
          console.log(
            "this.$refs.tinh.selectedData",
            this.$refs.tinh.selectedData
          );
          // this.tinhSeleceted = this.$refs.tinh.selectedData

          this.list_huyen.data = this.$refs.tinh.selectedData.Childs;
        this.Dongho_Info.Huyen_id = "";

        });
      } else {
        this.list_huyen.data = [];
        this.Dongho_Info.Huyen_id = "";
      }

      // this.list_huyen.data
    },
    submitForm(formName1, formName2) {
      this.$refs[formName1].validate((valid) => {
        if (valid) {
          this.$refs[formName2].validate((valid) => {
            if (valid) {
              alert("submit!");
              GetDataAPI({
                method: "POST",
                url: API.Register,
                params: {
                  User_Info: this.objUSer,
                  Dongho_Info: this.Dongho_Info,
                },
                action: (re) => {
                  ShowMessage("Đăng ký thành công", "success");
                  this.$emit("regiter", false);
                },
              });
            } else {
              return false;
            }
          });
        } else {
          console.log("error submit!!");
          return false;
        }
      });
    },
    resetForm(formName) {
      this.$refs[formName].resetFields();
    },
  },
  mounted() {
    console.log(this);
  },
};
</script>
<style lang="scss" scoped>
.regis {
  // background: #fff;
  // border-radius: 20px;

  // box-shadow: 0px 10px 34px -15px rgba(0, 0, 0, 0.24);
  padding: 40px 30px;
  border-radius: 20px;
  border: 1px solid rgba(0, 0, 0, 0.24);
  display: flex;
  flex-direction: column;

  .title {
    text-align: center;
  }
  .left-side {
    padding-right: 15px;
  }
  .right-side {
    padding-left: 15px;
  }
}

::v-deep .el-form {
  div:not(.gender) {
    margin-bottom: 25px;
    position: relative;
    .el-form-item__label {
      position: absolute;
      top: 0;
      left: 0;
      //   background: antiquewhite;
      transform: translateY(-50%);
      z-index: 2;
      width: fit-content !important;
      background-color: #fff !important;
      border: 1px solid #dcdfe6;
      // border-radius: 5px;
      font-size: 12px;
      margin-left: 10px;
      padding: 0 5px;
      line-height: 12px;
    }
    div {
      position: relative;
      font-size: 12px;
      margin: 0 !important;

      .el-date-editor {
        width: 241px;
      }
      .el-input--mini {
        .el-input__inner {
          line-height: 15px;
          height: 30px;
          // border:
        }
      }

      .el-form-item__error {
        position: absolute;
        font-size: 9px;
        // background: #aaa;
      }
    }
  }
  .gender {
    margin-bottom: 5px;
    label {
      font-size: 12px;
      width: fit-content !important;
    }
    div {
      .el-radio-group {
        // display: flex;
        // align-items: center;
        // justify-content: space-around;
        // label{

        // }
        .el-radio__label {
          font-size: 12px;
        }
      }
    }
  }
}
</style>
