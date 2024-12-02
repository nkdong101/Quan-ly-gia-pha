<template>
  <div
    class="giapha"
    style="height: 100%; overflow: hidden"
    @wheel="onWheel"
    :style="{
      cursor: isPanning ? 'move' : 'auto',
    }"
  >
    <div
      ref="svgContainer"
      class="svgContainer"
      style="
        height: 100%;
        width: 100%;
        overflow: hidden;
        display: block;
        position: relative;
      "
    >
      <div style="position: absolute; right: 0">
        <InputSelect
          @input="searchGiapha"
          :model="Para.Giapha"
          ref="entry"
          v-model.lazy="filterId"
        />
      </div>
      <!-- -->
      <svg
        @mousedown="startPan"
        @mousemove="pan"
        @mouseup="endPan"
        @mouseleave="endPan"
        @touchstart="startPan"
        @touchmove="pan"
        @touchend="endPan"
        @touchcancel="endPan"
        ref="svgElement"
        :viewBox="viewBox"
        style="height: 100%; width: 100%; overflow-x: auto; overflow-y: auto"
      >
        <defs>
          <filter id="shadow2">
            <feDropShadow dx="0" dy="0" stdDeviation="0.5" flood-color="cyan" />
          </filter>

          <filter id="dropShadow" x="0" y="0" width="200%" height="200%">
            <!-- <feOffset result="offOut" in="SourceGraphic" dx="5" dy="5" /> -->
            <!-- <feGaussianBlur result="blurOut" in="offOut" stdDeviation="5" /> -->
            <!-- <feBlend in="SourceGraphic" in2="blurOut" mode="normal" /> -->
            <feDropShadow dx="0" dy="0" stdDeviation="0.5" flood-color="cyan" />
          </filter>

          <g id="base_node_menu" style="cursor: pointer">
            <rect x="0" y="0" fill="transparent" width="22" height="22"></rect>
            <circle cx="4" cy="11" r="2" fill="#aeaeae"></circle>
            <circle cx="11" cy="11" r="2" fill="#aeaeae"></circle>
            <circle cx="18" cy="11" r="2" fill="#aeaeae"></circle>
          </g>
          <g id="base_up">
            <circle
              cx="15"
              cy="15"
              r="15"
              fill="#fff"
              stroke="#aeaeae"
              stroke-width="1"
            ></circle>
            <svg width="20" height="20" x="5" y="5" viewBox="0 0 512 512">
              <path
                fill="#aeaeae"
                d="m336.061 377.731c-5.086-6.54-14.511-7.717-21.049-2.631l-44.012 34.231v-200.331c0-8.284-6.716-15-15-15s-15 6.716-15 15v200.331l-44.011-34.231c-6.538-5.086-15.962-3.908-21.049 2.631-5.086 6.539-3.908 15.963 2.631 21.049l62.429 48.556v49.664c0 8.284 6.716 15 15 15s15-6.716 15-15v-49.664l62.429-48.556c6.54-5.086 7.717-14.51 2.632-21.049z"
              ></path>
              <path
                fill="#aeaeae"
                d="m271 497v-49.664l62.429-48.556c6.54-5.086 7.717-14.51 2.631-21.049-5.086-6.54-14.511-7.717-21.049-2.631l-44.011 34.231v-200.331c0-8.284-6.716-15-15-15v318c8.284 0 15-6.716 15-15z"
              ></path>
              <path
                fill="#aeaeae"
                d="m320 512h-128c-8.284 0-15-6.716-15-15s6.716-15 15-15h128c8.284 0 15 6.716 15 15s-6.716 15-15 15z"
              ></path>
              <path
                fill="#aeaeae"
                d="m320 482h-64v30h64c8.284 0 15-6.716 15-15s-6.716-15-15-15z"
              ></path>
              <path
                fill="#aeaeae"
                d="m400 439c-61.206 0-111-49.794-111-111s49.794-111 111-111 111 49.794 111 111-49.794 111-111 111z"
              ></path>
              <path
                fill="#aeaeae"
                d="m112 439c-61.206 0-111-49.794-111-111s49.794-111 111-111 111 49.794 111 111-49.794 111-111 111z"
              ></path>
              <path
                fill="#aeaeae"
                d="m256 222c-61.206 0-111-49.794-111-111s49.794-111 111-111 111 49.794 111 111-49.794 111-111 111z"
              ></path>
              <path
                fill="#aeaeae"
                d="m367 111c0-61.206-49.794-111-111-111v222c61.206 0 111-49.794 111-111z"
              ></path>
            </svg>
          </g>
          <!-- <symbol id="base_node_menu" viewBox="0 0 24 24">
            <rect x="0" y="0" fill="transparent" width="22" height="22"></rect>
            <circle cx="4" cy="11" r="2" fill="#aeaeae"></circle>
            <circle cx="11" cy="11" r="2" fill="#aeaeae"></circle>
            <circle cx="18" cy="11" r="2" fill="#aeaeae"></circle>
  </symbol> -->
          <clipPath id="base_img_0">
            <circle id="base_img_0_stroke" cx="45" cy="62" r="35" />
          </clipPath>
        </defs>
        <g
          :data-l-id="`[${item.Id1}][${item.Id2}]`"
          v-for="item in nodes.Lines"
          :key="item.Points"
        >
          <path
            stroke-linejoin="round"
            stroke="#aeaeae"
            stroke-width="1px"
            fill="none"
            :d="item.Points"
          ></path>
        </g>
        <nodeSvg
          v-for="(item, index) in nodes.Data"
          :key="item.id"
          :data="item"
          :config="nodes.Config"
          :isSearch="index === nodes.Data.findIndex((p) => p.id == filterId)"
          :transform="`translate(${item.Box.X}, ${item.Box.Y})`"
          @dotClick="dotClick"
          @nameClick="nameClick"
          @CreateAcc="CreateAcc"
          @findHoNgoai="findHoNgoai"
          ref="node"
        />
      </svg>
      <!-- @hook:mounted="movetoNode0(item, index)" -->
      <!-- <el-popover
        :style="nodePoperStyles"
        class="btn-p"
        placement="right"
        popper-class="btn-poper"
        ref="pop"
        width="100"
        trigger="manual"
      >
        <div>
          <el-button>aaa</el-button>
        </div>
        <el-button style="opacity: 1; padding: 0" slot="reference"></el-button>
      </el-popover> -->
    </div>
    <!-- <div v-if="node.Data && node.Data.length ==0">
      <el-button type="text">Thêm thông tin đầu tiền</el-button>
    </div> -->
    <DefaultForm :model="form" @actionOK="form.Save.call(this)">
      <div class="fm" :style="{ display: !isAdd ? 'flex' : '' }" slot="content">
        <FormInfo
          :style="{ width: !isAdd ? '30%' : '100%' }"
          ref="form"
          :model="isAdd ? form.obj.form() : form.obj.form3()"
        />
        <div class="tieusu" v-if="!isAdd">
          <p class="title">Tiểu sử</p>
          <div class="ts-content">
            <div v-for="(item, i) in tsArr" :key="i">
              <p>- {{ item }}</p>
            </div>
          </div>
          <div ref="ts_ip">
            <!-- <el-input
              type="textarea"
              :autosize="{ minRows:2}"
              placeholder="Thêm tiểu sử"
              v-model="tieusu"
              @input="showSaveTS"
            >
            </el-input>
            <el-button  @click="addTS" type="primary" v-if="isAddTs"
              >Save</el-button
            > -->
            <QEditor
              ref="entry"
              v-model.lazy="form.obj.tieusu"
              class="quill-container"
            />
          </div>
        </div>
      </div>
    </DefaultForm>

    <DefaultForm :model="formCrAcc" @actionOK="formCrAcc.Save.call(this)">
      <div slot="content">
        <el-form
          :model="formCrAcc.objUser"
          status-icon
          :rules="rules"
          ref="ruleForm"
          label-width="120px"
          class="demo-ruleForm"
        >
          <el-form-item label="Tên tài khoản" prop="UserName">
            <el-input
              v-model="formCrAcc.objUser.UserName"
              autocomplete="off"
            ></el-input>
          </el-form-item>
          <el-form-item label="Mật khẩu" prop="Password">
            <el-input
              v-model="formCrAcc.objUser.Password"
              autocomplete="off"
            ></el-input>
          </el-form-item>
        </el-form>
      </div>
    </DefaultForm>
  </div>
</template>

<script>
import Giapha from "~/assets/scripts/objects/Giapha";
import DefaultForm from "~/assets/scripts/base/DefaultForm";
import API from "~/assets/scripts/API";

import {
  ShowMessage,
  ShowConfirm,
  MessageType,
  Uni2None,
} from "~/assets/scripts/Functions";
import GetDataAPI from "~/assets/scripts/GetDataAPI";
import { EventBus } from "~/assets/scripts/EventBus.js";
import APIHelper from "~/assets/scripts/API/APIHelper";
import User from "~/assets/scripts/objects/User";
import ConvertStr from "~/assets/scripts/ConvertStr";
import { Para } from "~/assets/scripts/Para";
export default {
  // layout: this.user.ChucVu === "Administrator" ? 'default' : "UserLayout",
  computed: {
    svgStyle() {
      return {
        height: this.nodes.Config.MaxHeight + "px",
        // width: this.nodes.Config.MaxWidth + "px",
        width: this.nodes.Config.MaxWidth + "px",
      };
    },
  },
  data() {
    return {
      tieusu: "",
      filterId: "",
      isAddTs: false,
      isAdd: null,
      tsArr: [],
      nodeClicked: null,
      isPanning: null,
      viewBox: "561 0 648 729",

      rules: {
        UserName: [
          {
            required: true,
            message: "Trường này bắt buộc nhập",
            trigger: "blur",
          },
        ],
        Password: [
          {
            required: true,
            message: "Trường này bắt buộc nhập",
            trigger: "blur",
          },
        ],
      },
      dongho_id: null,
      model: {
        enableSearch: false,
        template: "myTemplate",
      },
      nodePoperStyles: {
        top: `0px`,
        left: `0px`,
      },
      nodes: {
        Config: {},
      },
      form: new DefaultForm({
        obj: new Giapha(),
        title: "",
        visible: false,
        // type: 'dialog',
        // width: "500px",
        noSave: true,
        fullscreen: true,
        ShowForm: (title, obj, isAdd) => {
          var _app = this;

          // if()
          if (_app.user.AccountSerial === obj.User_id) {
            _app.form.type = "";
          } else {
            if (_app.user.userLevel == 1) {
              _app.form.type = "";
            } else {
              _app.form.type = "dialog";
            }
          }
          _app.isAdd = isAdd;
          _app.form.title = title;
          // if (!isAdd) {
          //   obj = obj;
          //   if (!obj) {
          //     ShowMessage("You need choose 1 selection!");
          //     return;
          //   }
          // }
          // console.log(quill)
          if (isAdd) {
            GetDataAPI({
              url: API.GetFamily,
              params: {
                iPerson_id: obj.id,
              },
              action: (re) => {
                this.form.obj = new Giapha({
                  ...re,
                  Hongoai_id: obj.Hongoai_id,
                });
                this.form.visible = true;
              },
            });
          } else {
            APIHelper.Giapha.getInfor(obj.id).then((re) => {
              console.log(obj);
              this.form.obj = new Giapha({ ...re, Hongoai_id: obj.Hongoai_id });
              this.form.noSave = false;
              this.form.visible = true;
            });
          }
        },
        Save: () => {
          console.log("save", this.tieusu);
          this.$refs.form.getValidate().then((re) => {
            if (!re) {
              ShowMessage("Vui lòng nhập đầy đủ thông tin", MessageType.error);
              return;
            } else {
              this.$refs.form
                .getEntry("URL")
                .submitUpload()
                .then((result) => {
                  this.form.obj.URL = result;
                  // this.form.obj.tieusu = this.tieusu
                  // console.log(this);
                  GetDataAPI({
                    url: API.AddTieuSu,
                    params: this.form.obj.toJSON(),
                    method: "post",
                    action: (re) => {
                      ShowMessage("Thêm thành công", "success");
                      this.GetNodeData();
                      this.form.visible = false;
                    },
                  });
                });
            }
          });
        },
      }),
      formCrAcc: new DefaultForm({
        obj: new Giapha(),
        objUser: new User(),
        title: "",
        visible: false,
        // type: 'dialog',
        width: "400px",
        // noSave: true,
        // fullscreen: true,
        ShowForm: (title, obj, isAdd) => {
          // this.isAdd = isAdd;
          var _app = this;
          this.formCrAcc.title = title;

          GetDataAPI({
            url: API.Giapha + `/${obj.id}`,
            // params: {
            //   iPerson_id: ,
            // },
            action: (re) => {
              this.formCrAcc.objUser = new User({
                ...re,
              });
              const objN = this.formCrAcc.objUser;

              this.formCrAcc.objUser.UserName =
                Uni2None(objN.Name.replaceAll(" ", "")).toLocaleLowerCase() +
                ConvertStr.ToDateStr(objN.Birthday, "ddMMyyyy");
              this.formCrAcc.objUser.Password =
                Uni2None(objN.Name.replaceAll(" ", "")).toLocaleLowerCase() +
                ConvertStr.ToDateStr(objN.Birthday, "ddMMyyyy");
              console.log(this.formCrAcc.objUser);
              this.formCrAcc.visible = true;
            },
          });
        },
        Save: () => {
          console.log("save", this.tieusu);
          this.formCrAcc.objUser.userLevel = 1;
          this.$refs.ruleForm.validate((valid) => {
            if (valid) {
              // alert("submit!");
              GetDataAPI({
                url: API.Register,
                method: "POST",
                // params: this.formCrAcc.objUser.toJSON(),
                params: {
                  User_Info: {
                    UserName: this.formCrAcc.objUser.UserName,
                    Password: this.formCrAcc.objUser.Password,
                    Images: this.formCrAcc.objUser.Avatar,
                    Address: this.formCrAcc.objUser.Address,
                    CMND: this.formCrAcc.objUser.CCCD,
                    BirthDay: this.formCrAcc.objUser.Birthday,
                    Gender: this.formCrAcc.objUser.Gender,
                    userLevel: 2,
                    Email: this.formCrAcc.objUser.Email,
                    FullName: this.formCrAcc.objUser.Name,
                    Giapha_id: this.formCrAcc.objUser.Id,
                  },
                },
                action: (re) => {
                  ShowMessage(
                    `Đã cấp tài khoản cho <b>${this.formCrAcc.objUser.Name}</b>`
                  );
                  this.formCrAcc.visible = false;
                },
              });
            } else {
              // console.log("error submit!!");
              return false;
            }
          });
        },
      }),
    };
  },
  methods: {
    searchGiapha() {
      let index = this.nodes.Data.findIndex((p) => p.id == this.filterId);
      // console.log(index)
      if (index >= 0) {
        this.moveScreenTo(index);
      }
      // console.log(this.$refs.node[index])
      // if( this.$refs.node[index]){
      //   this.$refs.node[index].$el.style.filter = 'drop-shadow(0 0 20px rgba(70, 72, 240, 0.8))'

      // }
    },
    CreateAcc(data) {
      console.log(data);
      this.formCrAcc.ShowForm(
        `Cấp tài khoản cho <b>${data.name}</b> `,
        data,
        false
      );
    },
    movetoNode0(item, index) {
      if (index === 0) {
        const node0 = this.$refs.node[0].$el.getBoundingClientRect();
        const svgElement = this.$refs.svgElement;
        const svgRect = svgElement.getBoundingClientRect();

        const svgCenterX = svgRect.width;

        const [_, y, viewBoxWidth, viewBoxHeight] = this.viewBox
          .split(" ")
          .map(Number);

        this.viewBox = `${svgCenterX} ${y} ${viewBoxWidth} ${viewBoxHeight}`;
      }
    },

    findHoNgoai(data) {
      console.log(data, localStorage.Dongho_watching);
      //return;
      GetDataAPI({
        url: API.GetTree,
        params: {
          iDongho_id:
            data.Hongoai_id == localStorage.Dongho_watching
              ? data.Dongho_id
              : data.Hongoai_id,
        },
        action: (re) => {
          this.nodes = re;
          localStorage.Dongho_watching =
            data.Hongoai_id == localStorage.Dongho_watching
              ? data.Dongho_id
              : data.Hongoai_id;
          // console.log(this.nodes);
        },
      });
    },
    addTS() {
      this.tsArr.push(this.tieusu);
      this.tieusu = "";
      this.isAddTs = false;
    },
    showSaveTS() {
      if (this.tieusu) {
        this.isAddTs = true;
        console.log(this);
        this.$nextTick(() => {
          this.$refs.ts_ip.scrollIntoView({ behavior: "smooth", block: "end" });
        });
      }
      if (!this.tieusu) {
        this.isAddTs = false;
      }
    },
    nameClick(obj) {
      this.form.ShowForm("Thông tin", obj, true);
    },

    startPan(event) {
      if (event.type === "mousedown" && event.button !== 0) return;

      this.isPanning = true;

      const clientX =
        event.type === "mousedown" ? event.clientX : event.touches[0].clientX;
      const clientY =
        event.type === "mousedown" ? event.clientY : event.touches[0].clientY;

      this.startPoint = { x: clientX, y: clientY };

      const viewBoxValues = this.viewBox.split(" ").map(Number);
      
      this.startViewBox = {
        x: viewBoxValues[0],
        y: viewBoxValues[1],
        width: viewBoxValues[2],
        height: viewBoxValues[3],
      };

      // Lấy kích thước và giới hạn của nội dung SVG
      const svgBounds = this.$refs.svgElement.getBBox(); // Lấy giới hạn nội dung
      
      this.panLimits = {
        minX: svgBounds.x,
        minY: svgBounds.y - 100,
        maxX: svgBounds.x + svgBounds.width - this.startViewBox.width,
        maxY: svgBounds.y + svgBounds.height - this.startViewBox.height + 100,
      };
    },
    pan(event) {
      if (!this.isPanning) return;

      const clientX =
        event.type === "mousemove" ? event.clientX : event.touches[0].clientX;
      const clientY =
        event.type === "mousemove" ? event.clientY : event.touches[0].clientY;

      const dx =
        (this.startPoint.x - clientX) *
        ((this.startViewBox.width + 1) / this.$refs.svgElement.clientWidth);
      const dy =
        (this.startPoint.y - clientY) *
        ((this.startViewBox.height + 1) / this.$refs.svgElement.clientHeight);

      // Tính toán giá trị mới của x và y
      let newX = this.startViewBox.x + dx;
      let newY = this.startViewBox.y + dy;

      // Áp dụng giới hạn pan
      newX = Math.max(this.panLimits.minX, Math.min(this.panLimits.maxX, newX));
      newY = Math.max(this.panLimits.minY, Math.min(this.panLimits.maxY, newY));

      // Cập nhật viewBox
      this.viewBox = `${newX} ${newY} ${this.startViewBox.width} ${this.startViewBox.height}`;
    },
    endPan() {
      this.isPanning = false;
    },

    nodePosition(item, index) {
      // console.log(item);
      if (item.fid && item.mid) {
        // console.log(item);
        // console.log(`translate(${index * (this.nodes.Config.Width + 30)},  ${item.Level * (this.nodes.Config.Height + 50)})`)
        return `translate(${index * (this.nodes.Config.Width + 30)},  ${
          item.Level * (this.nodes.Config.Height + 50)
        })`;
      }

      return `translate(${index * (this.nodes.Config.Width + 50)}, 0)`;
    },

    LoadData(id) {
      // console.log('id',id);
      this.form.visible = false;
      // return;
      GetDataAPI({
        url: API.GetFamily,
        params: {
          iPerson_id: id,
        },
        action: (re) => {
          this.$nextTick(() => {
            this.form.obj = new Giapha({
              ...re,
            });
          });
          this.GetNodeData();
          this.form.visible = true;
        },
      });
    },
    handleClickNode(title, obj, isAdd) {
      console.log(title, obj, isAdd);
      this.form.ShowForm(title, obj, isAdd);
    },
    ChangeInpt(val) {
      this.dongho_id = val;
      this.GetNodeData();
    },
    GetNodeData() {
      GetDataAPI({
        url: API.GetTree,
        params: {
          iDongho_id: this.dongho_id || this.user.Dongho_id,
        },
        action: (re) => {
          this.nodes = re;
          console.log(this.nodes);
          this.$nextTick(() => {
            this.moveScreenTo(0);
          });
        },
      });
    },

    moveScreenTo(index) {
      const node = this.$refs.node[index].$el.getBoundingClientRect();
      const svg = this.$refs.svgElement.getBoundingClientRect();

      // Dimensions of the SVG
      const svgWidth = svg.width;
      const svgHeight = svg.height;

      // Calculate the position of the node relative to the SVG
      const nodeX = node.left + node.width / 2 - svg.left;
      const nodeY = node.top + node.height / 2 - svg.top;

      // Calculate the new center for the viewBox
      const centerX = nodeX - svgWidth / 2;
      const centerY = nodeY - svgHeight / 2;

      // Update the viewBox to center on the node
      const currentViewBox = this.viewBox.split(" ").map(Number);
      const [x, y, w, h] = currentViewBox;

      // Recalculate viewBox to reflect new center
      const newViewBoxX = x + centerX * (w / svgWidth);
      const newViewBoxY = y + centerY * (h / svgHeight);

      this.viewBox = `${newViewBoxX} ${newViewBoxY} ${w} ${h}`;
    },
    onWheel(e) {
      if (e.ctrlKey) {
        e.preventDefault();
        const svgElement = this.$refs.svgElement;

        const { left, top, width, height } = svgElement.getBoundingClientRect();
        const mouseX = e.clientX - left;
        const mouseY = e.clientY - top;
        const zoomFactor = e.deltaY > 0 ? 1.1 : 0.9;

        const viewBoxValues = this.viewBox.split(" ").map(Number);
        const [x, y, viewBoxWidth, viewBoxHeight] = viewBoxValues;

        // Giới hạn zoom
        const minZoom = 200; // Kích thước nhỏ nhất cho viewBoxWidth
        const maxZoom = 1700; // Kích thước lớn nhất cho viewBoxWidth

        const newWidth = viewBoxWidth * zoomFactor;
        const newHeight = viewBoxHeight * zoomFactor;

        // Kiểm tra giới hạn zoom
        if (newWidth > maxZoom || newWidth < minZoom) return;

        const dx = (mouseX / width) * (viewBoxWidth - newWidth);
        const dy = (mouseY / height) * (viewBoxHeight - newHeight);

        // Cập nhật viewBox
        this.viewBox = `${x + dx} ${y + dy} ${newWidth} ${newHeight}`;
      }
    },

    // updatePopoverPosition() {
    //   const { x, y } = this.lastClickPosition;
    //   // Convert coordinates to viewBox-relative coordinates
    //   const [viewBoxX, viewBoxY, viewBoxWidth, viewBoxHeight] = this.viewBox
    //     .split(" ")
    //     .map(Number);
    //   const svgWidth = this.$refs.svgElement.getBoundingClientRect().width;
    //   const svgHeight = this.$refs.svgElement.getBoundingClientRect().height;

    //   const scaleX = svgWidth / viewBoxWidth;
    //   const scaleY = svgHeight / viewBoxHeight;

    //   const relativeX = (x - viewBoxX) * scaleX;
    //   const relativeY = (y - viewBoxY) * scaleY;

    //   this.nodePoperStyles = {
    //     position: "absolute",
    //     top: `${relativeY}px`,
    //     left: `${relativeX}px`,
    //     transform: "none", // Ensure no additional transform interferes
    //   };
    // },

    dotClick(item) {
      // console.log("click", item);
      this.form.ShowForm(`Thông tin của ${item.name}`, item, false);
    },
    nodeClick() {
      // console.log("click");
    },
    moveTofirst() {
      const firstElement = this.nodes.Data[0];
      if (firstElement) {
        const padding = 20; // Add some padding
        const x = firstElement.Box.X - padding;
        const y = firstElement.Box.Y - padding;
        const width = firstElement.Box.Width + padding * 2;
        const height = firstElement.Box.Height + padding * 2;

        // Set the viewBox to focus on the first element
        this.viewBox = `${x} ${y} ${width} ${height}`;
      }
    },
  },
  mounted() {
    this.$refs.svgContainer.addEventListener("wheel", this.onWheel, {
      passive: false,
    });
    // console.log(this)
    // this.$refs.svgContainer.querySelector('g').addEventListener("click", this.nodeClick);
    this.dongho_id = this.user.Dongho_id;
    GetDataAPI({
      url: API.GetTree,
      params: {
        iDongho_id: this.dongho_id || this.user.Dongho_id,
      },
      action: (re) => {
        this.nodes = re;
        Para.Giapha.data = re.Data;
        this.$nextTick(() => {
          this.moveScreenTo(
            this.nodes.Data.findIndex(
              (p) => p.User_id == this.user.AccountSerial
            )
          );
        });
      },
    });
    console.log(this);

    // <
  },
  beforeMount() {
    EventBus.$on("reloadFormFam", this.LoadData);
  },

  beforeDestroy() {
    this.$refs.svgContainer.removeEventListener("wheel", this.onWheel);
    // this.$refs.svgContainer.querySelector('g').removeEventListener("click", this.nodeClick);
    EventBus.$off("reloadFormFam", this.LoadData);
  },
};
</script>

<style lang="scss" scoped>
.giapha {
  background-color: #fff;
  border-radius: 10px;
}

.fm {
  height: 100%;
  // overflow: hidden;

  .form-info {
    height: 100%;
    ::v-deep .form-info-c {
      height: 100%;
      display: flex;
      // overflow-y: auto;
      overflow-x: hidden;
      flex-direction: column;
      > div:nth-child(3) {
        // height:100%;
        // background:red;
        // div:nth-child(2){
        // height: 100%;
        // overflow-y: auto;
        // }
        #div_siblings_form {
          // height: 100%;
          // overflow-y: auto;
        }
      }
    }
  }
  .tieusu {
    border: 1px solid rgb(203, 203, 203);
    border-radius: 10px;
    width: 100%;
    height: 100%;
    padding: 10px;
    margin-left: 10px;
    position: relative;
    overflow: auto;
    .title {
      //  position: absolute;
      //  top: 0;
      //  left: 0;
      font-weight: bolder;
      //  transform: translate(50%, 50%);
      //  z-index: 1000;
      height: auto;
      //  width:100%;
      padding: 5px 10px;
      // background-color: white;
    }
    .ts-content {
      padding: 10px 0;
      div {
        padding-bottom: 10px;
      }
    }
  }
}
.fm ::v-deep .form-info .form-info-c > .el-row {
  justify-content: center;
}

.svgContainer {
  .btn-p {
    position: absolute;
    top: 0;
    left: 0;
  }

  ::v-deep .el-select {
    .el-input--mini .el-input__inner {
      height: 40px;
    }
  }
}

@media only screen and (max-width: 800px) {
  ::v-deep#div_Couple_form {
    width: 100%;
    margin-bottom: 5px;
    overflow-x: scroll;
    border-right: 1px solid gray;
    #Couple_form {
      min-width: 560px;
    }
  }

  ::v-deep#div_siblings_form {
    width: 100%;
    margin-bottom: 5px;
    overflow-x: scroll;
    #siblings_form {
      min-width: 560px;
    }
  }
}
</style>
