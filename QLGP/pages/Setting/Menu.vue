<template>
  <div class="main-content">
    <div class="main-header">
      <span>Manage Menu</span>
      <el-button type="primary" @click="Add()" class="btn-add"
        ><i class="el-icon-plus"></i
      ></el-button>
    </div>
    <div class="main">
      <TreeMenu ref="menu" />
    </div>
  </div>
</template>
<script>
export default {
  data() {
    return {};
  },
  methods: {
    Add() {
      this.$refs.menu.Add();
    },
  },
};
</script>

<style lang="scss" scoped>
@import "@/assets/css/variable.scss";
.main-content {
  display: flex;
  flex-direction: column;
  height: 100%;
  .main-header {
    flex: 0 0 37px;
    margin: 0px;
    position: relative;
    // color: #5183e8;
    // color: $swatch_7;
    background: #efefef;
    border-bottom: 1px solid #d8dce5;
    text-transform: uppercase;
    span {
      position: absolute;
      font-weight: bold;
      //   font-size: 15px;
      top: 50%;
      transform: translateY(-50%);
      display: block;
      width: 100%;
      padding-right: 10px !important;
      padding-left: 5px;
    }
    .btn-add {
      position: absolute;
      font-weight: bold;
      //   font-size: 15px;
      top: 50%;
      transform: translateY(-50%);
      right: 5px;
      padding: 5px 10px;
    }
  }
  .main {
    flex: 1;
    overflow: auto;
  }
}
</style>
