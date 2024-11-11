<template>
  <div style="height: 100%; overflow: hidden">

    <div id="tree" ref="tree"></div>
  </div>
</template>

<script>
import FamilyTree from "@balkangraph/familytree.js";

import editForm from "~/assets/scripts/objects/FamilyTreeEdit";

import API from "~/assets/scripts/API";
import {
  ShowMessage,
  ShowConfirm,
  MessageType,
} from "~/assets/scripts/Functions";
import GetDataAPI from "~/assets/scripts/GetDataAPI";
export default {
  name: "tree",
  props: {
    nodes: {},
    model: Object,
  },
  data() {
    return {

      vFamilyTree: FamilyTree,

    };
  },
  watch: {
    nodes(newNodes) {
      console.log(newNodes);
      this.loadTree(this.$refs.tree, newNodes);
    },
  },
  methods: {
    generateTemplate() {
      // console.log(FamilyTree)
      this.vFamilyTree.templates.myTemplate_male = Object.assign(
        {},
        this.vFamilyTree.templates.base,
        {}
      );

      this.vFamilyTree.templates.myTemplate_female = Object.assign(
        {},
        this.vFamilyTree.templates.base,
        {
          node:
            '<rect x="0" y="0" height="{h}" width="{w}" fill="#ffffff" stroke-width="3" stroke="#ccc" rx="5" ry="5"></rect>' +
            '<rect x="0" y="0" height="20" width="{w}" fill="#b1b9be" stroke-width="1" stroke="#b1b9be" rx="5" ry="5"></rect>' +
            '<line x1="0" y1="20" x2="250" y2="20" stroke-width="5" stroke="#b1b9be"></line>',
        }
      );

      this.vFamilyTree.templates.base.defs = this.base_defs;

      if (!this.vFamilyTree.templates[this.nameTemplate]) {
        this.vFamilyTree.templates[this.nameTemplate] = Object.assign(
          {},
          this.vFamilyTree.templates.base
        );

        // Assign the custom config to the template
        Object.assign(
          this.vFamilyTree.templates[this.nameTemplate],
          this.familyTreeConfig
        );
      }
      if (!this.vFamilyTree.templates["male"]) {
        this.vFamilyTree.templates["male"] = Object.assign(
          {},
          this.vFamilyTree.templates[this.nameTemplate]
        );
        Object.assign(this.vFamilyTree.templates["male"], this.tm_Male);
      }

      this.vFamilyTree.templates["male_child"] = Object.assign(
        {},
        this.vFamilyTree.templates["male"],
        this.tm_Male_child
      );

      this.vFamilyTree.templates["female"] = Object.assign(
        {},
        this.vFamilyTree.templates.male,
        this.tm_Female
      );
      this.vFamilyTree.templates["female_child"] = Object.assign(
        {},
        this.vFamilyTree.templates["female"],
        this.tm_Female_child
      );

      this.vFamilyTree.templates.single = Object.assign(
        {},
        FamilyTree.templates.tommy,
        this.tm_Single
      );
      this.vFamilyTree.templates.single_male = Object.assign(
        {},
        FamilyTree.templates.single,
        this.tm_single_male
      );
      this.vFamilyTree.templates.single_female = Object.assign(
        {},
        FamilyTree.templates.single_male,
        this.tm_single_female
      );
      this.vFamilyTree.templates.family_single_male = Object.assign(
        {},
        FamilyTree.templates.single_male,
        this.tm_family_single_male
      );
      this.vFamilyTree.templates.family_single_female = Object.assign(
        {},
        FamilyTree.templates.single_female,
        this.tm_family_single_female
      );
      this.vFamilyTree.templates.isDead = Object.assign(
        {},
        this.vFamilyTree.templates.myTemplate
      );
    },

    loadTree(domEl, nodes) {
      this.generateTemplate();
      var _app = this;
      console.log("generating template", nodes);
      const familyTreeConfig = {
        nodes: Array.isArray(nodes) ? nodes : [],
        template: this.nameTemplate,
        enableSearch: this.model.enableSearch,
        mouseScrool: FamilyTree.action.scroll,
        nodeMouseClick: FamilyTree.action.zoom,
        // scaleInitial: FamilyTree.match.height,
  //       keyNavigation: true,
  //  keyNavigation: { focusId:this.user },
        // siblingSeparation : 50,
        nodeMenu: {
          //   details: { text: "Details" },
          edit: {
            text: "",
            icon: "<i class='fa fa-edit'></i>", // Path to your icon file
            onClick: function (obj) {
              console.log(this);
              const nodeData = this.get(obj);
              // _app.form.ShowForm("Edit", false, nodeData);
              _app.$emit("nodeEventClicked",'Sửa',nodeData,false);
              // console.log('_app',_app)
            },
          },
          add: {
            text: "",
            icon: "<i class='fa fa-plus'></i>", // Path to your icon file
            onClick: function (obj) {
              const nodeData = this.get(obj);
              // _app.form.ShowForm("Add node", true, nodeData);
              _app.$emit("nodeEventClicked",'Thêm',nodeData,true);
            },
          },
        },

        nodeBinding: {
          field_0: "gioitinh",
          field_1: "name",
          field_2: "SL_Anhem",
          field_3: "id",
          img_0: "img",
        },
        editUI: {
          init: (obj) => {},
          show: (nodeId) => {
            this.nodeId = nodeId;
            const nodeData = this.family.get(nodeId) || { name: "", title: "" };

            this.form.ShowForm("Edit", false, nodeData);
            // console.log(nodeData);
          },
          hide: () => {},
        },
        toolbar: {
          zoom: true,
          expandAll: true,
        },
        // mouseScrool: FamilyTree.action.ctrlZoom,
        orderBy: "orderId",
        tags: {
          isDead: {
            template: "isDead",
          },
          single_male: {
            template: "single_male",
          },
          male: {
            template: "male",
          },
          female: {
            template: "female",
          },
          single_female: {
            template: "single_female",
          },
          female_child: {
            template: "female_child",
          },
          male_child: {
            template: "male_child",
          },
          family_single_female: {
            template: "family_single_female",
          },
          family_single_male: {
            template: "family_single_male",
          },
          myTemplate_male: {
            template: "myTemplate_male",
          },
          myTemplate_female: {
            template: "myTemplate_female",
          },
        },
        //  subtreeSeparation : 100
      };
      // console.log(familyTreeConfig);
      this.family = new FamilyTree(domEl, familyTreeConfig);
      this.family.on("render-link", function (sender, args) {
        if (args.cnode.ppid != undefined)
          args.html +=
            '<use data-ctrl-ec-id="' +
            args.node.id +
            '" xlink:href="#heart" x="' +
            args.p.xa +
            '" y="' +
            args.p.ya +
            '"/>';
        if (args.cnode.isPartner && args.node.partnerSeparation == 30)
          args.html +=
            '<use data-ctrl-ec-id="' +
            args.node.id +
            '" xlink:href="#heart" x="' +
            args.p.xb +
            '" y="' +
            args.p.yb +
            '"/>';
      });
    },
  },
  mounted() {
    GetDataAPI({
      url: API.GetTree,
      params: {
        iDongho_id: this.user.Dongho_id,
      },
      action: (re) => {
        this.loadTree(this.$refs.tree, re);
      },
    });

    // console.log(this);
    // console.log("this.vFamilyTree", this.vFamilyTree);
  },
  computed: {
    nameTemplate() {
      return this.model.template || "base"; // default to "base" if no template provided
    },
    base_defs() {
      return `<g transform="matrix(0.05,0,0,0.05,-13 ,-12)" id="heart">
    <path d="M448,256c0-106-86-192-192-192S64,150,64,256s86,192,192,192S448,362,448,256Z" style="fill:#fff;stroke:red;stroke-miterlimit:10;stroke-width:24px" fill="red"></path><path d="M256,360a16,16,0,0,1-9-2.78c-39.3-26.68-56.32-45-65.7-56.41-20-24.37-29.58-49.4-29.3-76.5.31-31.06,25.22-56.33,55.53-56.33,20.4,0,35,10.63,44.1,20.41a6,6,0,0,0,8.72,0c9.11-9.78,23.7-20.41,44.1-20.41,30.31,0,55.22,25.27,55.53,56.33.28,27.1-9.31,52.13-29.3,76.5-9.38,11.44-26.4,29.73-65.7,56.41A16,16,0,0,1,256,360Z" fill="red"></path>
  </g>
     <g transform="matrix(1,0,0,1,0,0)" id="heart"></g>
      <g id="base_node_menu" style="cursor:pointer;">
          <rect x="0" y="0" fill="transparent" width="22" height="22"></rect>
          <circle cx="4" cy="11" r="2" fill="#b1b9be"></circle>
          <circle cx="11" cy="11" r="2" fill="#b1b9be"></circle>
          <circle cx="18" cy="11" r="2" fill="#b1b9be"></circle>
      </g>
      <g style="cursor: pointer;" id="base_tree_menu">
          <rect x="0" y="0" width="25" height="25" fill="transparent"></rect>
          ${FamilyTree.icon.addUser(25, 25, "#fff", 0, 0)}
      </g>
      <g style="cursor: pointer;" id="base_tree_menu_close">
          <circle cx="12.5" cy="12.5" r="12" fill="#F57C00"></circle>
          ${FamilyTree.icon.close(25, 25, "#fff", 0, 0)}
      </g>
      <g id="base_up">
          <circle cx="115" cy="30" r="15" fill="#fff" stroke="#b1b9be" stroke-width="1"></circle>
          ${FamilyTree.icon.ft(20, 80, "#6bb4df", 105, -10)}
      </g>
      <g id="female_base_up">
          <circle cx="115" cy="30" r="15" fill="#fff" stroke="#b1b9be" stroke-width="1"></circle>
          ${FamilyTree.icon.ft(20, 80, "#cb4aaf", 105, -10)}
      </g>
      <clipPath id="base_img_0">
        <circle id="base_img_0_stroke" cx="45" cy="62" r="35"/>
      </clipPath>
      <clipPath id="base_img_1">
        <circle id="base_img_1_stroke" cx="100" cy="62" r="35"/>
      </clipPath>
      `;
    },
    familyTreeConfig() {
      return {
        // size: [200, 200],
        node:
          '<rect x="0" y="0" height="{h}" width="{w}" fill="#ffffff" stroke-width="3" stroke="#ccc" rx="5" ry="5"></rect>' +
          '<rect x="0" y="0" height="20" width="{w}" fill="#b1b9be" stroke-width="1" stroke="#b1b9be" rx="5" ry="5"></rect>' +
          '<line x1="0" y1="20" x2="250" y2="20" stroke-width="5" stroke="#b1b9be"></line>',
        // nodeBinding: {
        //   field_0: "name",
        //   img_0: "img",
        //   field_1: "gender",
        // },
        field_0:
          "<text " +
          FamilyTree.attr.width +
          ' ="250" style="font-size: 14px;" font-variant="all-small-caps" fill="white" x="125" y="16" text-anchor="middle">{val}</text>',
        field_1:
          "<text " +
          FamilyTree.attr.width +
          ' ="160" data-text-overflow="multiline" style="font-size: 14px;" fill="black" x="100" y="66" text-anchor="start">{val}</text>',
        field_2:
          "<text " +
          FamilyTree.attr.width +
          ' ="160" style="font-size: 10px;" fill="#b1b9be" x="100" y="95" text-anchor="start">{val}</text>',
        field_3:
          "<text " +
          FamilyTree.attr.width +
          ' ="60" style="font-size: 12px;" fill="black" x="47" y="112" text-anchor="middle">{val}</text>',

        img_0: `<use xlink:href="#base_img_0_stroke" />
       <circle id="base_img_0_stroke" fill="#b1b9be" cx="45" cy="62" r="37"/>
      <image preserveAspectRatio="xMidYMid slice" clip-path="url(#base_img_0)" xlink:href="{val}" x="10" y="26" width="72" height="72"></image>`,

        nodeMenuButton: `<use ${this.vFamilyTree.attr.control_node_menu_id}="{id}" x="220" y="100" xlink:href="#base_node_menu" />`,
        defs: `
        <style>
                                        .{randId} .bft-edit-form-header, .{randId} .bft-img-button{
                                            background-color: #aeaeae;
                                        }
                                        .{randId}.male .bft-edit-form-header, .{randId}.male .bft-img-button{
                                            background-color: #6bb4df;
                                        }
                                        .{randId}.male div.bft-img-button:hover{
                                            background-color: #cb4aaf;
                                        }
                                        .{randId}.female .bft-edit-form-header, .{randId}.female .bft-img-button{
                                            background-color: #cb4aaf;
                                        }
                                        .{randId}.female div.bft-img-button:hover{
                                            background-color: #6bb4df;
                                        }
    </style>

        `,
        plus:
          '<circle cx="106" cy="-13" r="10" fill="#b1b9be" stroke="#fff" stroke-width="1"><title>Expand</title></circle>' +
          '<line x1="99" y1="-13" x2="113" y2="-13" stroke-width="1" stroke="#fff"></line>' +
          '<line x1="106" y1="-20" x2="106" y2="-6" stroke-width="1" stroke="#fff"></line>',
      };
    },
    tm_Male() {
      return {
        node:
          '<rect x="0" y="0" height="{h}" width="{w}" fill="#ffffff" stroke-width="3" stroke="#6bb4df" rx="5" ry="5"></rect>' +
          '<rect x="0" y="0" height="20" width="{w}" fill="#6bb4df" stroke-width="1" stroke="#6bb4df" rx="5" ry="5"></rect>' +
          '<line x1="0" y1="20" x2="250" y2="20" stroke-width="5" stroke="#6bb4df"></line>',
        img_0: `<use xlink:href="#base_img_0_stroke" />
       <circle id="base_img_0_stroke" fill="#6bb4df" cx="45" cy="62" r="37"/>
      <image preserveAspectRatio="xMidYMid slice" clip-path="url(#base_img_0)" xlink:href="{val}" x="10" y="26" width="72" height="72"></image>`,
        plus:
          '<circle cx="106" cy="-13" r="10" fill="#039BE5" stroke="#fff" stroke-width="1"><title>Expand</title></circle>' +
          '<line x1="99" y1="-13" x2="113" y2="-13" stroke-width="1" stroke="#fff"></line>' +
          '<line x1="106" y1="-20" x2="106" y2="-6" stroke-width="1" stroke="#fff"></line>',
        // up:  '<use x="210" y="10" xlink:href="#male_base_up"></use>',
      };
    },
    tm_Male_child() {
      return {
        link: '<path stroke-linejoin="round" stroke="#aeaeae" stroke-width="2px" fill="none" d="{rounded}" />',
      };
    },

    tm_Female() {
      return {
        node:
          '<rect x="0" y="0" height="{h}" width="{w}" fill="#ffffff" stroke-width="3" stroke="#cb4aaf" rx="5" ry="5"></rect>' +
          '<rect x="0" y="0" height="20" width="{w}" fill="#cb4aaf" stroke-width="1" stroke="#cb4aaf" rx="5" ry="5"></rect>' +
          '<line x1="0" y1="20" x2="250" y2="20" stroke-width="5" stroke="#cb4aaf"></line>',
        img_0: `<use xlink:href="#base_img_0_stroke" />
       <circle id="base_img_0_stroke" fill="#cb4aaf" cx="45" cy="62" r="37"/>
      <image preserveAspectRatio="xMidYMid slice" clip-path="url(#base_img_0)" xlink:href="{val}" x="10" y="26" width="72" height="72"></image>`,
        plus:
          '<circle cx="106" cy="-13" r="10" fill="#FF46A3" stroke="#fff" stroke-width="1"></circle>' +
          '<line x1="99" y1="-13" x2="113" y2="-13" stroke-width="1" stroke="#fff"></line>' +
          '<line x1="106" y1="-20" x2="106" y2="-6" stroke-width="1" stroke="#fff"></line>',
        // up:  '<use x="110" y="-10"  xlink:href="#female_base_up"></use>',
      };
    },
    tm_Female_child() {
      return {
        link: '<path stroke-linejoin="round" stroke="#aeaeae" stroke-width="2px" fill="none" d="{rounded}" />',
      };
    },
    tm_Single() {
      return {
        size: [200, 200],
        // nodeMenuButton: `<use ${FamilyTree.attr.control_node_menu_id}="{id}" x="89" y="5" xlink:href="#base_node_menu" />`,
        defs: `<style>
                                        .{randId} .bft-edit-form-header, .{randId} .bft-img-button{
                                            background-color: #aeaeae;
                                        }
                                        .{randId}.male .bft-edit-form-header, .{randId}.male .bft-img-button{
                                            background-color: #6bb4df;
                                        }
                                        .{randId}.male div.bft-img-button:hover{
                                            background-color: #cb4aaf;
                                        }
                                        .{randId}.female .bft-edit-form-header, .{randId}.female .bft-img-button{
                                            background-color: #cb4aaf;
                                        }
                                        .{randId}.female div.bft-img-button:hover{
                                            background-color: #6bb4df;
                                        }
    </style>`,
        node: '<circle cx="100" cy="100" r="100" fill="white" stroke-width="1" stroke="#aeaeae"></circle>',
        field_0:
          "<text " +
          FamilyTree.attr.width +
          ' ="160" style="font-size: 14px;" font-variant="all-small-caps"  font-weight="bold" fill="black" x="100" y="115" text-anchor="middle">{val}</text>',
        field_1:
          "<text " +
          FamilyTree.attr.width +
          ' ="190" data-text-overflow="multiline" style="font-size: 16px;" fill="black" x="100" y="135" text-anchor="middle">{val}</text>',

        field_3:
          "<text " +
          FamilyTree.attr.width +
          ' ="60" style="font-size: 12px;" fill="black" x="100" y="180" text-anchor="middle">{val}</text>',

        nodeMenuButton: `<use ${FamilyTree.attr.control_node_menu_id}="{id}" x="89" y="5" xlink:href="#base_node_menu" />`,
      };
    },
    tm_single_male() {
      return {
        node: '<circle cx="100" cy="100" r="100" fill="white" stroke-width="3" stroke="#6bb4df" ></circle>',
        img_0: `<use xlink:href="#base_img_1_stroke"/>
       <circle id="base_img_1_stroke" fill="#6bb4df" cx="100" cy="62" r="37"/>
      <image preserveAspectRatio="xMidYMid slice" clip-path="url(#base_img_1)" xlink:href="{val}" x="65" y="26" width="72" height="72"></image>`,
      };
    },
    tm_single_female() {
      return {
        node: '<circle cx="100" cy="100" r="100" fill="white" stroke-width="3" stroke="#cb4aaf" ></circle>',
        img_0: `<use xlink:href="#base_img_1_stroke"/>
       <circle id="base_img_1_stroke" fill="#cb4aaf" cx="100" cy="62" r="37"/>
      <image preserveAspectRatio="xMidYMid slice" clip-path="url(#base_img_1)" xlink:href="{val}" x="65" y="26" width="72" height="72"></image>`,
      };
    },
    tm_family_single_male() {
      return {
        link: '<path stroke-linejoin="round" stroke="#aeaeae" stroke-width="2px" fill="none" d="{rounded}" />',
      };
    },
    tm_family_single_female() {
      return {
        link: '<path stroke-linejoin="round" stroke="#aeaeae" stroke-width="2px" fill="none" d="{rounded}" />',
      };
    },
  },
};
</script>

<style lang="scss" scoped>
#tree {
  width: 100%;
  height: 100%;
  overflow: hidden;
  position: relative;
  ::v-deep  .bft-family-menu {
    min-width: fit-content;
    display: flex;
    // left: 320px !important;
    border-radius: 5px;
    // padding: 5px;
    overflow: hidden;
    div {
      border: none;
      font-size: 18px;
      border-radius: 0;
      padding: 5px 12px;
      span {
        display: none;
      }
      &:first-child {
        border-right: 1px solid #ccc;
      }
    }
  }
}
</style>
