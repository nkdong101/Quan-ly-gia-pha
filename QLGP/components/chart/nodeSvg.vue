<template lang="">
  <g  :filter="data.User_id == user.AccountSerial ? 'url(#dropShadow)'  : ''"
       ref="node"
    style="pointer-events: bounding-box"
    :transform="transform"
    @click="handleClick"
      :style="{userSelect: 'none', filter: isSearch? 'drop-shadow(0 10px 10px rgba(151, 153, 12)) ' : user.AccountSerial === data.User_id ? 'drop-shadow(0 0 20px rgba(70, 72, 240, 0.8))' : 'none', }"
  >
    <rect
      :x="0"
      :y="0"
      :height="config.Height"
      :width="config.Width"
      fill="#ffffff"
      stroke-width="3"
      :stroke="data.Nammat? '#c9c5c5' : getFillColor(data.gioitinh)"
      rx="5"
      ry="5"
    ></rect>
    <rect
      x="0"
      y="0"
      height="20"
      :width="config.Width"
      :fill="data.Nammat? '#c9c5c5' : getFillColor(data.gioitinh)"
      stroke-width="0"
      stroke="#b1b9be"
      rx="5"
      ry="5"
    ></rect>
    <line
      x1="0"
      y1="20"
      :x2="config.Width"
      y2="20"
      stroke-width="5"
      :stroke="data.Nammat? '#c9c5c5' : getFillColor(data.gioitinh)"
    ></line>
    <text
      data-text-overflow="multiline"
      :style="{userSelect: 'none' }"
      fill="white"
      x="110"
      y="15"
   font-size="16"
   font-weight="600"
      >
        Đời {{ ConvertStr.convertToRoman(data.Level+1) }}
      </text
    >


    <foreignObject
      cursor="pointer"
      @click="$emit('nameClick', data)"
      :style="{
        fontSize: '14px',
      }"
      x="100"
      y="40"
      width="140"
      height="30"
      ><div class="text-container" xmlns="http://www.w3.org/1999/xhtml">
        {{ data.name }} 
       
      </div>
    </foreignObject>

    <text
      data-width="160"
      data-text-overflow="multiline"

      :style="{fontSize: '12px',userSelect: 'none' }"
      fill="black"
      x="100"
      y="83"
      text-anchor="start"
      ><tspan x="100" y="83" text-anchor="start"
        >{{ data.Namsinh == 0 || !data.Namsinh ? '?': data.Namsinh }}
        {{ data.Nammat ? " - "  + data.Nammat : "" }}
      </tspan></text
    >
    <use xlink:href="#base_img_0_stroke" />
    <circle id="base_img_0_stroke" fill="#b1b9be" cx="45" cy="62" r="37" />
    <image
      preserveAspectRatio="xMidYMid slice"
      clip-path="url(#base_img_0)"
      :xlink:href="
        data.Avatar
          ? '/Images/avatar/' + data.Avatar.split('|')[0]
          : '/Images/avatar/user.png'
      "
      :x="10"
      :y="data.Avatar ? '26' : '26'"
      width="72"
      height="78"
    ></image>

    <!-- <el-popover
      :ref="'pop' + data.id"
      placement="top-start"
      title="Title"
      width="200"
      trigger="click"
      content="This is content, this is content, this is content"
    >
     
    </el-popover> -->
    <!-- <use cursor="pointer" class="colored-use"  @click="abc" v-if="data.Hongoai_id > 0" x="200" y="10" xlink:href="#base_up"></use> -->

    <foreignObject
    
      cursor="pointer"
      :style="{
        fontSize: '14px',
      }"
     x="210"
      y="95"
      width="20"
      height="20"
      ><i  @click="handClickdot" class="fa fa-pencil-square-o" aria-hidden="true"></i>
    </foreignObject>

    <foreignObject
    v-if="!data.User_id && user.userLevel == 1"
      cursor="pointer"
      :style="{
        fontSize: '14px',
      }"
      x="185"
      y="95"
      width="20"
      height="20"
      >
      <!-- <el-tooltip class="item" effect="dark" content="Cấp tài khoản" placement="top-end"> -->
        <i @click="clickCreateAcc" class="fa fa-user-plus" aria-hidden="true"></i>
      <!-- </el-tooltip> -->
    </foreignObject>
    <!-- <use
      @click="handClickdot"
      :data-ctrl-n-menu-id="data.id"
      x="210"
      y="95"
      xlink:href="#base_node_menu"
    > -->
  
  </use>
  </g>
</template>
<script>
// import Functions from '~/assets/scripts/Functions'
export default {
  props: {
    isSearch: {},
    data: {},
    config: {},
    transform: {
      type: String,
      default: "",
    },
  },
  methods: {
    clickCreateAcc(){
      // console.log(this)
      this.$emit('CreateAcc',this.data);
    },
    // abc(){
    //   this.$emit('findHoNgoai',this.data);
    // },
    handClickdot() {
      // console.log("handClickdotdotdotdot");
      // console.log(this.$refs['pop'+this.data.id])
      // this.$refs["pop" + this.data.id].doToggle();
      this.$emit('dotClick',this.data);
    },
    getFillColor(gioitinh) {
      return gioitinh === 1 ? "#6bb4df" : "#cb4aaf";
    },
    handleClick() {
      // console.log(this.data);
    },
  },

  mounted() {
    // if(this.data.id == 10 ){
    // console.log(this.user.AccountSerial)
    // console.log(this.data.id )
    // }
  },
};
</script>
<style lang="scss">
.text-container {
  display: inline-block;
  width: 100%;
  height: 100%;
  // vertical-align: sub;
  word-break: break-word;
  user-select:none;
}
.colored-use {
  fill: red;
  stroke: black;
}
</style>
