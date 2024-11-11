<template lang="">
  <div>
    <PNode
      @AddVo="AddVo"
      style=""
      :nameTitle="''"
      :type="2"
      :Pid="obj.Id"
      :obj="obj.Curent"
      :hus="obj.Curent.Gender == 1 ? true : false"
    />
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
      objF: {},
      wifeIndex: null,
      form: new DefaultForm({
        obj: new Giapha(),

        title: "",
        visible: false,
        // type: 'dialog',
        width: "450px",
        // noSave: true,
        // fullscreen: true,
        ShowForm: (title) => {
        //   this.isAdd = isAdd;
          var _app = this;
          this.form.title = title;


            this.form.visible = true;
            this.form.obj = new Giapha({
            //   type: 4,
            });
        
        },
        Save: () => {
          // thêm/sửa bố mẹ
          // if (this.type == 1) {
          this.form.obj.Dongho_id = this.user.Dongho_id;
          if(this.obj.Curent.Gender == 1){
            this.form.obj.Gender = 2;
          }else{
            this.form.obj.Gender = 1
          }
        
          console.log(this);
            // return;
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
                    url: API.Add_Vo,
                    params: {
                        Chong_Info:  this.obj.Curent ,
                        Vo_Info: this.form.obj.toJSON(),
                        Index: this.voTHu
                    },
                    action: (re) => {
                      //   this.LoadData();
                      //   if (this.isAdd) {
                      //     this.form.obj.Project_id = re;
                      //   }
                      //   this.$refs.form.getEntry("formProjectSave").LoadData();
                      ShowMessage("Thêm vợ thành công", "success");
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


      }),
    }
  },
  computed:{
    voTHu(){
        if(this.obj.Couple && this.obj.Couple.length > 0) return this.obj.Couple.length + 1;
        return 1;
    }
  },
  methods: {
    AddVo(title) {
        this.form.ShowForm(title + this.voTHu)
    },
  },
  mounted() {
    // console.log("mounted me",this.obj)
  },
};
</script>
<style lang=""></style>
