<template lang="">
  <div style="">
    <PNode
      @showForm="showForm"
      :nameTitle="'Cha'"
      :type="1"
      :obj="obj.Farther"
      :hus="true"
    />
    <el-button
      :disabled="!obj.Farther || !obj.Mother"
      type="warning"
      @click="form.ShowForm(`Chỉnh sửa thông tin Cha/Mẹ`, obj, '', '', false)"
      style="margin: 0 10px"
    >
      <i class="fa fa-edit"></i
    ></el-button>
    <PNode
      @showForm="showForm"
      :nameTitle="'Mẹ'"
      :type="1"
      :obj="obj.Mother"
      :hus="false"
    />

    <DefaultForm :model="form" @actionOK="form.Save.call(this)">
      <div class="formCM" style="display: flex" slot="content">
        <formGiapha ref="formC" :hus="true" :obj="form.objC" />
        <formGiapha ref="formM" :hus="false" :obj="form.objM" />
      </div>
    </DefaultForm>
  </div>
</template>
<script>
import Giapha from "~/assets/scripts/objects/Giapha";
import API from "~/assets/scripts/API";
import DefaultForm from "~/assets/scripts/base/DefaultForm";
import GetDataAPI from "~/assets/scripts/GetDataAPI";
// import APIHelper
import APIHelper from "~/assets/scripts/API/APIHelper";
import { MessageType, ShowMessage } from "~/assets/scripts/Functions";
import { EventBus } from "~/assets/scripts/EventBus.js";
import ConvertStr from "~/assets/scripts/ConvertStr";
export default {
  props: {
    // type: {},
    obj: {},
  },
  data() {
    return {
      isAdd: null,
      form: new DefaultForm({
        objC: new Giapha(),
        objM: new Giapha(),
        title: "",
        visible: false,
        // type: 'dialog',
        width: "900px",
        // noSave: true,
        // fullscreen: true,
        ShowForm: (title, obj, person, male, isAdd) => {
          this.isAdd = isAdd;
          var _app = this;
          this.form.title = title;

          if (!isAdd) {
            APIHelper.Giapha.GetMarried(obj.Farther.Id, obj.Mother.Id).then(
              (re) => {
                console.log(re);
                this.form.objC = new Giapha({ ...re[0] });
                this.form.objM = new Giapha({ ...re[1] });
                // console.log(this.form.objC, this.form.objM);
                this.form.visible = true;
              }
            );
          } else {
            this.form.visible = true;
          }
        },
        Save: () => {
          // thêm/sửa bố mẹ
          // if (this.type == 1) {

          this.obj.Curent.Dongho_id = this.user.Dongho_id;

          if (this.isAdd) {
            this.form.objC.Gender = 1;
            this.form.objM.Gender = 2;
            this.form.objC.Dongho_id = this.user.Dongho_id;
            this.form.objM.Dongho_id = this.user.Dongho_id;
          }

          console.log(this);
          // return;
          this.$refs.formC.$refs.formFamC.getValidate().then((re) => {
            if (!re) {
              ShowMessage("Vui lòng nhập đầy đủ thông tin", MessageType.error);
              return;
            } else {
              this.$refs.formM.$refs.formFamM.getValidate().then((re) => {
                if (!re) {
                  ShowMessage(
                    "Vui lòng nhập đầy đủ thông tin",
                    MessageType.error
                  );
                  return;
                } else {
                  this.$refs.formC.$refs.formFamC
                    .getEntry("avatarUrl")
                    .submitUpload()
                    .then((res) => {
                      this.form.objC.Avatar = res.join(",");
                      this.$refs.formM.$refs.formFamM
                        .getEntry("avatarUrl")
                        .submitUpload()
                        .then((re) => {
                          this.form.objM.Avatar = re.join(",");

                          GetDataAPI({
                            url: API.Save_Bome,
                            params: {
                              Con_Info: new Giapha(this.obj.Curent).toJSON(),
                              Bo_Info: this.form.objC.toJSON(),
                              Me_Info: this.form.objM.toJSON(),
                            },
                            method: "post",
                            action: (re) => {
                              ShowMessage(
                                this.isAdd
                                  ? "Thêm thành công"
                                  : "Sửa thành công",
                                "success"
                              );
                              this.form.visible = false;
                              // this.reLoad();
                              EventBus.$emit(
                                "reloadFormFam",
                                this.obj.Curent.Id
                              );
                            },
                          });
                        });
                    });
                }
              });

              // this.$refs.formC
              //   .getEntry("avatarUrl")
              //   .submitUpload()
              //   .then((res) => {
              //     console.log(res);

              //     this.form.obj.Avatar = res.join(",");
              //     GetDataAPI({
              //       method: "post",
              //       url: API.Save_Bome,
              //       params: {
              //         Con_Info: new Giapha(this.obj.Curent).toJSON(),
              //         Bo_Info: this.form.objC.toJSON(),
              //         Me_Info: this.form.objM.toJSON(),
              //       },
              //       action: (re) => {
              //         ShowMessage(
              //           this.isAdd ? "Thêm thành công" : "Sửa thành công",
              //           "success"
              //         );
              //         this.form.visible = false;
              //         // this.reLoad();
              //         EventBus.$emit("reloadFormFam", this.obj.Curent.Id);
              //       },
              //     });
              //   });
            }
          });
        },
      }),
    };
  },
  methods: {
    showForm(title, obj, person, male, isAdd) {
      console.log(title, obj, person, male, isAdd);
      this.form.ShowForm(title, obj, person, male, isAdd);
    },
  },
  mounted() {
    // console.log("mounted",this.obj)
    // this.$watch( this.obj.Farther, function (newVal, oldVal) {
    //   // do something+
    //   this.$nextTick(() => {
    //     this.$emit("onChange", this.re_khac, newVal, oldVal);
    //   });
    // });
    // this.$watch("data." + this.Mother, function (newVal, oldVal) {
    //   // do something+
    //   this.$nextTick(() => {
    //     this.$emit("onChange", this.Mother, newVal, oldVal);
    //   });
    // });
  },
};
</script>
<style lang="scss" scoped>
.formCM {
  justify-content: space-between;
  .fm_form:first-child {
    padding-right: 10px;
  }
}

#father_form{
  display: flex; justify-content: center; align-items: center;
  
}

@media only screen and (max-width: 767px) {
  #father_form{
  display: flex; justify-content: center; align-items: center;
  flex-direction: column;
  
}

}
@media only screen and (max-width: 490px) {
  .formCM{
  flex-direction: column;
}
}


</style>
