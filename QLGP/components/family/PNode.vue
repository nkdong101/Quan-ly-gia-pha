<template lang="">
  <div style="width: 100%">
    <el-card
      v-if="obj"
      class="person"
      :style="{ backgroundColor: person.Gender == 1 ? '#6bb4df' : '#cb4aaf' }"
    >
      <div class="top">
        <span>{{ nameTitle }}</span>

        <!-- <i
          @click="
            form.ShowForm(`Chỉnh sửa thông tin Cha/Mẹ`, obj, person, hus, false)
          "
          class="fa fa-edit"
        ></i> -->
        <!-- <i class="fa fa-check-circle"></i> -->
      </div>
      <div class="bottom">
        <div class="img">
          <el-avatar fit="fill" v-if="!person.Avatar" icon="el-icon-user-solid"></el-avatar>
          <el-avatar v-else :src="'/Images/avatar/' + person.Avatar.split('|')[0]" fit="fill"></el-avatar>


        </div>
        <div class="infor">
          <el-row type="flex" align="middle" :gutter="10">
            <el-col :span="18"
              ><i class="fa fa-user-circle" aria-hidden="true"></i>
              <span style="    word-break: break-word;">
                {{ person.Name }}
                {{ person.Other_Name ? `(${person.Other_Name})` : "" }}
              </span>
             
            </el-col>
            <el-col :span="6"
              >

              <el-tooltip class="item"  :content="'Thêm ' + (person.Gender == 1 ? 'vợ' : 'chồng')" placement="top-start">


                <el-button
          type="primary"
          v-if="nameTitle === ''"
          @click="AddVo()"
         style="color: white;padding: 7px;  "
        >
          <i style="color: white; margin: 0;   font-size: 12px;" :class="person.Gender == 2? 'fa fa-male' : 'fa fa-female'"></i>
        </el-button>
    </el-tooltip>



            </el-col>
            <!-- <el-col :span="8">Số điện thoại: {{ person.Phone }}</el-col> -->
          </el-row>
          <el-row :gutter="10">
            <el-col :span="24"
              >
              <span v-if="person.Birthday && !person.Date_of_death">
              <i class="fa fa-birthday-cake" aria-hidden="true"></i>
              {{
                person.Birthday
                  ? ConvertStr.ToDateStr(person.Birthday)
                  : person.Year_Of_Birth
              }}</span>
              <span v-if="person.Date_of_death">
                <i class="fa fa-chain-broken" aria-hidden="true"></i>
              {{
               ConvertStr.ToDateStr(person.Date_of_death)
              }}</span>

              </span>
              </el-col
            >
          </el-row>
          <el-row>
            <el-col>
              <i class="fa fa-map-marker" aria-hidden="true"></i>
              {{
                person.Date_of_death ? person.burial_ground : person.Address
              }}</el-col
            >
          </el-row>
          <el-row :gutter="10">
            <!-- <el-col :span="8">Con thứ: {{ person.Number_Childs }}</el-col>
            <el-col :span="8">
              {{ person.Gender == 1 ? "Vợ:" : "Chồng:" }}
              {{ person.Number_Couple }}</el-col
            >
            <el-col :span="8">Anh/Chị em: {{ person.Number_Siblings }}</el-col> -->
            <!-- <el-col :span="8">Name</el-col> -->
            <el-col :span="24">



              <p style='word-break: break-word;'>
                {{person.Description}}
              </p>


            </el-col>
            <!-- <el-col  :span="8">
              <span></span>
            </el-col>
            <el-col  :span="8">

            </el-col> -->
          </el-row>
          <!-- <el-row>
            <el-col :span="8">CCCD/CMND</el-col>
            <el-col :span="8">Email</el-col>



        </el-row> -->
        </div>
      </div>
    </el-card>

    <el-card
      shadow="never"
      v-else
      class="person"
      style="background-color: #e8e8e8"
    >
      <el-tooltip
        class="item"
        effect="light"
        :content="hus == true ? 'Thêm Cha' : 'Thêm Mẹ'"
        placement="top"
      >
        <div
          class="block"
          @click="showForm"
          style="text-align: center; cursor: pointer"
        >
          <el-avatar icon="el-icon-user-solid"></el-avatar>
        </div>
      </el-tooltip>
    </el-card>
  </div>
</template>
<script>
import API from "~/assets/scripts/API";
import DefaultForm from "~/assets/scripts/base/DefaultForm";
import GetDataAPI from "~/assets/scripts/GetDataAPI";

export default {
  props: {
    type: {},
    hus: {},
    obj: {},
    Pid: {},
    nameTitle: {},
  },
  data() {
    return {
      person: {},
    };
  },
  methods: {
    showForm() {
      // this.form.ShowForm(`Thêm  Cha/Mẹ`, {}, 1, '', true)
      // console.log('node click')
      this.$emit("showForm", `Thêm  Cha/Mẹ`, {}, 1, "", true);
    },
    AddVo(title){
      // console.log("AddVo")
      this.$emit("AddVo",this.person.Gender == 2 ? "Thêm chồng thứ" : "Thêm vợ thứ")
    }
  },
  mounted() {
    this.person = this.obj;
    // switch (this.type) {
    //   case 1: // bố/mẹ
    //     // if (this.Pid) {
    //     //   GetDataAPI({
    //     //     url: API.Giapha + "/" + this.Pid,
    //     //     action: (re) => {
    //     //       this.person = re;
    //     //       // this.form.visible = true;
    //     //     },
    //     //   });
    //     // }
    //     this.person = this.obj;
    //     break;
    //   case 2: //chủ thể
    //     this.person = this.obj;
    //     break;
    //   case 3: //vợ/Chồng
    //     // this.person = this.obj;
    //     // GetDataAPI({
    //     //   url: API.Giapha + "/" + this.Pid,
    //     //   action: (re) => {
    //     //     this.person = re;
    //     //     // this.form.visible = true;
    //     //   },
    //     // });

    //     break;
    // }
    // console.log("mounted", this.Pid);
    // if (this.Pid && this.type !== 2) {
    // }
    console.log(this.obj);
  },
};
</script>
<style lang="scss" scoped>
.person {
  padding: 5px;
  border-radius: 10px;
  //   background-color: ;
  ::v-deep .el-card__body {
    padding: 0 !important;
    display: flex;
    flex-direction: column;
    height: 100%;
    .top {
      height: 20px;
      text-align: center;
      color: #ffffff;
      position: relative;
      i {
        position: absolute;
        right: 5px;
      }
    }
    .bottom {
      padding: 5px 0;
      background-color: #ffffff;
      height: 100%;
      border-radius: 0 0 5px 5px;
      display: flex;
      align-items: center;
      //   justify-content: space-around;
      font-size: 12px;
      .img {
        width: fit-content;
        // width: 50px;
        // height: 50px;
        // border-radius: 50%;
        // border: 1px solid black;
        margin: 0 10px 0 10px;
      }
      .infor {
        width: 100%;
        .el-row {
          padding: 2px 0;
          .el-col {
            display: flex;
            align-items: center;
            i {
              font-size: 16px;
              color: rgb(25, 172, 221);
              margin-right: 10px;
            }
          }
        }
      }
    }
  }
}

.formCM {
  div {
    &:first-child {
      padding-right: 10px;
    }
  }
}

@media only screen and (max-width: 500px) {
  span,p{
  font-size:11px;
  }
  
}

</style>
