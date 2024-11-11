<template lang="">
  <div class="Comment">
    <div class="Comment-left">
      <h4>Góp ý cùng phát triển</h4>
      <div style="width: 100%; text-align: center; padding: 0 0 10px 0">
        <i>Chúng tôi trân trọng góp ý của bạn như cách bạn tôn trọng văn hóa việt</i>
      </div>
      <div class="fCm">
        <el-form
:rules="rules"
:model="cmOb"
ref="CmForm"
label-width="120px"
class="cm-form"
label-position="top"
>
<el-form-item label="Người góp ý" prop="Name" required>
  <el-input   size="medium"  v-model="cmOb.Name"></el-input>
</el-form-item>
<el-form-item label="Số liên hệ" prop="Phone" required>
  <el-input   size="medium" type="tel" v-model.number="cmOb.Phone"></el-input>
</el-form-item>
<el-form-item label="Email liên hệ" prop="Email" >
  <el-input   size="medium" type="email" v-model="cmOb.Email"></el-input>
</el-form-item>
<el-form-item label="Nội dung góp ý" prop="Content">
  <el-input :autosize="{
    minRows:3,maxRows: 9,
  }"   size="medium" type="textarea" v-model="cmOb.Content"></el-input>
</el-form-item>

</el-form-item>


</el-form>
      </div>



      <div style="text-align:center;padding:20px 0 0 0; width: 100%" class="footer">
      <el-button @click="submitSend" type="primary" style="padding: 12px 30%;font-size:16px; border-radius: 20px;"
        >Gửi</el-button
      >
    </div>
    </div>
    <div class="Comment-right"></div>
  </div>
</template>
<script>
import API, { ServerAPI } from "~/assets/scripts/API";
import DefaultForm from "~/assets/scripts/base/DefaultForm";
import { ShowMessage, validateEmail } from "~/assets/scripts/Functions";
import GetDataAPI from "~/assets/scripts/GetDataAPI";
export default {
  data() {
    return {
      rules:{
        Name: [
            { required: true, message: 'Vui lòng nhập trường này', trigger: 'blur' },
          ],
          Email: [
            { required: true, message: 'Vui lòng nhập trường này', trigger: 'blur' },
            { type: 'email', message: 'Vui lòng nhập đúng địa chỉ email', trigger: ['blur'] }
          ],
          Phone:[
          { required: true, message: 'Vui lòng nhập trường này'},
          { type: 'number', message: 'Vui lòng nhập số'}
          ]
      },
      cmOb: {
        Name: "",
        Phone: "",
        Email: "",
        Content: "",
      },
    };
  },
  methods:{
    submitSend(){
      this.$refs.CmForm.validate((valid)=>{
        if(valid){
          console.log(this.cmOb,this.user);
          GetDataAPI({
            url: API.Comments_Send,
            method: "POST",
            params:this.cmOb,
            action:(re)=>{
              console.log()
              ShowMessage("Cảm ơn quý khách đã góp ý",'success');
              // this.cmOb={}
              this.$refs.CmForm.resetFields();
            }
          })
        }else return;
      })
    }
  }
};
</script>
<style lang="scss" scoped>
.Comment {
    background: #fff;

  display: flex;
//   padding-bottom: 30px;
  .Comment-left {
    width: 50%;
    // text-align: center;
    padding-top: 50px;
    display: flex;
    flex-direction: column;
    align-items: center;
    padding-bottom: 20px;
    h4 {
      font-family: PlayfairDisplay;
      padding: 10px 0 10px 0;
      text-transform: uppercase;
      font-size: 24pt;
      color: #9e1c1e;
    }
    .fCm{
        border-radius: 10px;
        border: 1px solid  rgba(0, 0, 0, 0.24);;
        padding: 30px 40px;
        // text-align: left;
        width: 70%;
        .cm-form{
            ::v-deep  .el-form-item{
                .el-form-item__label{
                    font-size: 12px;
                    padding: 0 !important;
                    line-height: 25px;
                }
            }
            .el-form-item__content{
                .el-input{
                    .el-input__inner{

                    }
                }
            }
        }
    }
  }
  .Comment-right {
    width: 50%;
    background: url("/images/HERO-WritingLetters2.0-1200x675.png");
    background-position: center;
    background-size: cover;
  }
}
</style>
