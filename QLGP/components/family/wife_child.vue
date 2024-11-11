<template lang="">
  <div style="display: flex;flex-direction:column">
  
      <PNode  :nameTitle="gender == 1 ? 'Chồng' : 'Vợ'"  :type="3" :Pid="id" :obj="{}" :hus="obj.Gender == 1? false : true"/>
      <!-- Danh sách con chung -->
    <div class="childs">
        <p>Danh sách con chung</p>
        <div v-for="item in obj.Childs">
            <childe  :id="item.Id"></childe>
        </div>
    </div>
  </div>
</template>
<script>
import GetDataAPI from "~/assets/scripts/GetDataAPI";
import API from "~/assets/scripts/API";
import DefaultForm from "~/assets/scripts/base/DefaultForm";
import Giapha from "~/assets/scripts/objects/Giapha";
import { MessageType, ShowMessage } from "~/assets/scripts/Functions";
export default {
  props: {
    id: {},
    gender:{},
    obj: {},
  },
//   computed:{
//     obj2(){
//         return this.obj
//     }
//   },
  data() {
    return {
      obj: {},
    };
  },
  mounted() {
    // GetDataAPI({
    //   url: API.Giapha + "/" + this.id,
    //   action: (re) => {
    //     this.obj = re;
    //   console.log(this.obj)
    //   },
    // });

  },
};
</script>
<style lang=""></style>
