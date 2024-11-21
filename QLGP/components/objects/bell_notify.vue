<
<template lang="">
  <div class="bell">
    <div class="number-mess"><p>2</p></div>
    <el-dropdown trigger="click" @command="handleCommand" :hide-on-click="false">
      <span class="el-dropdown-link">
        <i style="color: white" class="el-icon-message-solid" />
      </span>
      <el-dropdown-menu class="dd-mess" slot="dropdown">
        <el-dropdown-item command="a" :icon="isCheck ? 'el-icon-remove' : 'el-icon-success'"
          ><div class="title">
            <p>Ngày 21/11/2024 tới có ngày giỗ của abc (39012 ÂL)</p>
          </div></el-dropdown-item
        >
      </el-dropdown-menu>
    </el-dropdown>
  </div>
</template>
<script>
export default {
    data() {
        return {
            isCheck:true
        }
    },
  methods: {
    handleCommand(command) {
        this.isCheck = !this.isCheck
    //   this.$message("click on item " + command);
    },
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
      right:0px;
      font-size: 10px;
      font-weight: bold;
      color: white;
    }
  }
}
</style>
