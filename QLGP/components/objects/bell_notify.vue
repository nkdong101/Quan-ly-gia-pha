<
<template lang="">
  <div class="bell">
    <div v-if="data_GP.length > 0 || data_CM.length > 0" class="number-mess">
      <p>{{ data_GP.length > 0 ? data_GP.length + data_CM.length : "" }}</p>
    </div>
    <el-dropdown trigger="click" @command="" :hide-on-click="false">
      <span class="el-dropdown-link">
        <i style="color: white" class="el-icon-message-solid" />
      </span>
      <el-dropdown-menu class="dd-mess" slot="dropdown">
        <el-dropdown-item v-if="data_GP.length"
          ><div style="background: #6b83ff; flex: 1; font-size:15px; padding: 2px 5px; border-radius: 3px;color: white" class="title">
            <p>Ngày giỗ 7 ngày tới</p>
          </div></el-dropdown-item
        >
        <el-dropdown-item icon="el-icon-minus" v-for="item in data_GP" :key="item"
          ><div class="title">
            <span v-html="item"></span></div
        ></el-dropdown-item>

        <el-dropdown-item v-if="data_CM.length"
          ><div style="background: #6b83ff; flex: 1; font-size:15px; padding: 2px 5px; border-radius: 3px;color: white" class="title">
            <p>Góp ý</p>
          </div></el-dropdown-item
        >
        <el-dropdown-item icon="el-icon-minus" v-for="item in data_CM" :key="item.Id"
          ><div style="flex: 1" class="title">
            <span>{{ item.Content }}</span>
            <div style="text-align: right; font-size: 10px; line-height: 8px">
              <span>{{ item.Email }}</span>
              <span>{{ ConvertStr.ToDateStr(item.DateCreate) }}</span>
            </div>
          </div></el-dropdown-item
        >
      </el-dropdown-menu>
    </el-dropdown>
  </div>
</template>
<script>
import API from "~/assets/scripts/API";
import GetDataAPI from "~/assets/scripts/GetDataAPI";
export default {
  data() {
    return {
      isCheck: true,
      data_GP: [],
      data_CM: [],
    };
  },
  methods: {
    handleCommand(command) {
      this.isCheck = !this.isCheck;
      //   this.$message("click on item " + command);
    },
  },
  mounted() {
    GetDataAPI({
      url: API.Giapha_GetNotify,
      action: (re) => {
        this.data_GP = re;
      },
    });
    GetDataAPI({
      url: API.Comments_GetNoti,
      action: (re) => {
        this.data_CM = re;
        console.log(this);
      },
    });
  },
};
</script>
<style>
.bell {
  position: relative;
  i {
    font-size: 28px !important;
  }
  .number-mess {
    z-index: 1;
    position: absolute;
    background-color: red;
    width: 15px;
    height: 15px;
    top: 5px;
    right: 0;
    border-radius: 50%;
    p {
      position: absolute;
      width: 15px;
      height: 15px;
      top: -17px;
      right: 0px;
      font-size: 10px;
      font-weight: bold;
      color: white;
    }
  }
}
</style>
