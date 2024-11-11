<template>
  <div>
    <el-tree
      v-if="isLoaded"
      ref="tree"
      :data="data"
      :default-expand-all="false"
      :render-after-expand="false"
      :expand-on-click-node="false"
      :show-checkbox="isConfig"
      node-key="key"
      :props="defaultProps"
      :empty-text="emptyText"
      :default-checked-keys="checked"
      @check-change="handleCheckChange"
      :filter-node-method="filterNode"
    >
      <span
        v-if="data.Id > 0"
        class="custom-tree-node"
        @click="expand(node)"
        :isButton="data.Type_id == 0"
        slot-scope="{ node, data }"
      >
        <span v-if="data.Type_id == 0">
          {{ data.Stt }}.
          <el-button
            :type="data.LiClass || 'primary'"
            @click="ShowAction(data)"
            class="btn-permission"
            :disabled="(data.LiClass || '').indexOf('hidden') > -1"
          >
            <i
              v-if="data.Icon"
              :class="data.Icon"
              style="margin-right: 2px"
            ></i>

            <span v-html="GetTitle(data)"></span>
          </el-button>
        </span>
        <span v-else class="menu-parent">
          <img
            v-if="data.Image"
            :src="Para.MenuImage.getImage(data.Image)"
            alt=""
          />
          <i v-else :class="data.Icon || 'fa fa-server'"></i>
          {{ data.Stt }}. {{ data.Title }}
          <span v-if="data.isViewAll" style="color: #66b1ff">All</span>
        </span>

        <div v-if="!isConfig" class="btn-action" @click="isBtnClick = true">
          <el-button v-if="data.Type_id != 0" type="primary" @click="Add(data)">
            <i class="el-icon-plus"></i>
          </el-button>
          <el-button type="info" @click="Edit(data)">
            <i class="el-icon-edit"></i>
          </el-button>
          <el-button type="danger" @click="Delete(data)">
            <i class="el-icon-delete"></i>
          </el-button>
          <el-button
            v-if="data.Type_id != 0"
            type="primary"
            @click="Add(data, true)"
            >Thêm nút lệnh</el-button
          >
        </div>

        <!-- <span>
        <el-button type="text" size="mini" @click="() => append(data)">
          Append
        </el-button>
        <el-button type="text" size="mini" @click="() => remove(node, data)">
          Delete
        </el-button>
                </span>-->
      </span>
      <div v-else></div>
    </el-tree>
    <DefaultForm :model="form" @actionOK="form.Save.call(this)">
      <div slot="content" v-focusFirstOnLoad>
        <FormInfo ref="form" :model="form.obj.form()" />
      </div>
    </DefaultForm>
  </div>
</template>

<script>
import GetDataAPI from "~/assets/scripts/GetDataAPI";
import API from "~/assets/scripts/API";
import {
  ShowAlert,
  ShowConfirm,
  ShowMessage,
  MessageType,
} from "~/assets/scripts/Functions";
import Menu from "~/assets/scripts/objects/menu/Menu";
import DefaultForm from "~/components/form-content/DefaultForm";

export default {
  components:{DefaultForm},
  props: {
    isConfig: {
      type: Boolean,
      default: false,
    },
    value: {
      type: Array,
      default: [],
    },
  },
  data() {
    return {
      isBtnClick: false,
      timer: 0,
      isLoaded: false,
      checked: [],
      data: [],
      lastObject: null,
      defaultProps: {
        children: "Child",
        label: "Title",
      },
      form: {
        obj: Menu(),
        SelectedItem: null,
        visible: false,
        size: "md",
        title: "",
        ShowForm: (title, obj) => {
          var _app = this;

          this.form.title = title;
          this.lastObject = JSON.parse(JSON.stringify(obj));
          this.form.obj = Menu(obj);
          this.form.visible = true;
        },
        Save: () => {
          var _app = this;
          let obj = {};
          let api = "";
          let method = this.form.obj.Id ? "put" : "post";

          if (this.form.obj.Type_id == 0) {
            obj = {
              Id: this.form.obj.Id,
              Menu_id: this.form.obj.ParentId,
              Name: this.form.obj.Title,
              Action: this.form.obj.Action,
              Display: true,
              Element_Class: this.form.obj.LiClass,
              Type_id: this.form.obj.Type_id,
              Stt: this.form.obj.Stt,
              ICon: this.form.obj.Icon,
            };
            api =
              API.MenuButton + (this.form.obj.Id ? "/" + this.form.obj.Id : "");
          } else {
            obj = {
              id: this.form.obj.Id,
              Parent_id: this.form.obj.ParentId,
              Controller: this.form.obj.Href,
              Name: this.form.obj.Title,
              FullName: this.form.obj.Title,
              ICon: this.form.obj.Icon,
              ViewAll: this.form.obj.isViewAll == true,
              Stt: this.form.obj.Stt,
              Tag: this.form.obj.Tag,
              Type_id: this.form.obj.Type_id,
              Action: this.form.obj.Action,
              Element_Class: this.form.obj.LiClass,
              Image: this.form.obj.Image,
              Note: this.form.obj.Note,
            };
            api = API.Menu + (this.form.obj.Id ? "/" + this.form.obj.Id : "");
          }

          new Promise((rs) => {
            if (this.form.obj.Type_id == 0) rs();
            else {
              this.$refs.form
                .getEntry("txtImage")
                .submitUpload()
                .then((results) => {
                  obj.Image = results[0];
                  rs();
                });
            }
          }).then(() => {
            GetDataAPI({
              method: method,
              url: api,
              params: obj,
              action: function (re) {
                let isAdd = false;
                if (re == "OK" || Number.isInteger(+re)) {
                  if (!_app.form.obj.Id) {
                    isAdd = true;
                    _app.form.obj.Id = re;
                    if (obj.Type_id == 0) {
                      _app.form.obj.key = re;
                      _app.form.SelectedItem.Child.push(_app.form.obj);
                    } else {
                      if (_app.form.obj.ParentId != 0)
                        _app.form.SelectedItem.Child.push(_app.form.obj);
                      else _app.data.push(_app.form.obj);
                    }
                  } else {
                    for (var key in _app.form.obj) {
                      if (key in _app.form.SelectedItem) {
                        _app.form.SelectedItem[key] = _app.form.obj[key];
                      }
                    }
                  }
                  _app.form.visible = false;
                  if (isAdd) {
                    _app.$nextTick(() => {
                      _app.form.ShowForm(_app.form.title, _app.lastObject);
                    });
                  }
                } else {
                  ShowMessage(re);
                }
                // _app.LoadData();data
              },
            });
          });
        },
        Delete: (data) => {
          var _app = this;
          let api = "";
          if (data.Type_id == 0) {
            api = API.MenuButton + "/" + data.Id;
            data.Menu_id = data.ParentId;
          } else {
            api = API.Menu + "/" + data.Id;
          }
          ShowConfirm({
            message:
              "Delete " +
              (data.Type_id == 0 ? "perrmission" : "menu") +
              " [" +
              data.Title +
              "]",
            title: "Alert",
            type: MessageType.warning,
          })
            .then(() => {
              GetDataAPI({
                method: "delete",
                url: api,
                action: function (re) {
                  if (re == "OK") {
                    data.Id = -1;
                    // _app.LoadData();
                    _app.$refs.tree.filter("");
                  } else {
                    ShowMessage(re);
                  }
                },
              });
            })
            .catch((err) => {
              // An error occurred
            });
        },
      },
    };
  },
  computed: {
    /**
     * @return {string} description
     */
    emptyText() {
      if (this.isLoaded) return "Không có dữ liệu";
      return "Đang tải...";
    },
  },
  watch: {
    value(newValue, oldValue) {
      if (this.$refs.tree) this.$refs.tree.setCheckedKeys([]);
      this.checked = JSON.parse(JSON.stringify(this.value.map((p) => p + "")));
      if (this.$refs.tree) this.$refs.tree.setCheckedKeys(this.checked);
    },
  },
  methods: {
    filterNode(value, data) {
      return data.Id > 0;
    },
    expand(node) {
      if (!this.isBtnClick) node.expanded = !node.expanded;
      else this.isBtnClick = false;
      // //console.log(node, this.$refs.tree);
    },
    handleCheckChange(data, checked, indeterminate) {
      let _app = this;
      _app.timer += 1;
      let timer = _app.timer;
      setTimeout(function () {
        if (timer == _app.timer) {
          let values = _app.$refs.tree
            .getCheckedKeys()
            .filter((p) => p.indexOf("Menu") < 0);
          _app.$emit("input", values);
        }
      }, 0);
    },
    Add(data, isBtn) {
      let obj = Menu();
      let title = "";
      if (!data) {
        //THêm menu ngoài
        obj.Type_id = 1;
        title = "Thêm menu";
      } else {
        if (isBtn) {
          //Thêm nút
          obj.Type_id = 0;
          obj.ParentId = data.Id;
          title = "Thêm nút lệnh";
        } else {
          //Thêm menu con
          obj.Type_id = 1;
          obj.ParentId = data.Id;
          title = "Thêm menu";
        }
      }
      this.form.SelectedItem = data || {};
      if (!this.form.SelectedItem.Child) this.form.SelectedItem.Child = [];
      this.form.ShowForm(title, obj);
    },
    Edit(data) {
      this.form.SelectedItem = data;
      if (!this.form.SelectedItem.Child) this.form.SelectedItem.Child = [];

      this.form.ShowForm(
        data.Type_id == 0 ? "Sửa thông tin nút lệnh" : "Sửa thông tin menu",
        data
      );
    },
    Delete(data) {
      this.form.Delete(data);
    },
    GetTitle(data) {
      let str = data.Title;
      //   if (data.LiClass.indexOf("hidden") > -1) str += " <b>[Ẩn]</b>";
      return str;
    },
    ShowAction(data) {
      // //console.log(data);
      ShowAlert({ message: data.Action });
    },
    ProcessData(rawData) {
      let data = JSON.parse(JSON.stringify(rawData));
      return (data || []).reduce((arr, p) => {
        if (p.Type_id != 0) p.key = "Menu_" + p.Id;
        let check = false;
        if ((p.Child || []).length == 0) {
        } else {
          p.Child = this.ProcessData(p.Child);
          check = true;
        }
        if ((p.ListButton || []).length) {
          p.ListButton.forEach((p) => {
            p.key = p.Id + "";
          });
          p.Child.push(...p.ListButton);
          // if(this.isConfig){
          //     p.Child = p.Child.filter(p1=>  (p1.LiClass||'').indexOf('hidden') == -1)
          // }
          check = true;
        }
        arr.push(p);

        return arr;
      }, []);
    },
    LoadData() {
      let _app = this;
      GetDataAPI({
        url: API.Menu,
        action: function (re) {
          _app.data = _app.ProcessData(re);
          _app.isLoaded = true;
          _app.$nextTick(function () {
            let arr = document.querySelectorAll("[isButton]").forEach((p) => {
              p.parentElement.parentElement.parentElement.setAttribute(
                "btn-container",
                ""
              );
            });
          });
        },
      });
    },
  },
  mounted: function () {
    this.LoadData();
    if (this.value) {
      this.checked = JSON.parse(JSON.stringify(this.value.map((p) => p + "")));
    }
  },
};
</script>

<style lang="scss" scoped>
::v-deep  .el-tree-node__content {
  height: auto;
}
.custom-tree-node {
  flex: 1;
  border-bottom: 1px solid #ddd;
  padding: 5px 0px;
  font-size: 13px;
  line-height: 13px;
  position: relative;

  .btn-action {
    display: none;
    position: absolute;
    top: 50%;
    right: 5px;
    transform: translateY(-50%);
    // margin-bottom: 5px;
  }
  &:hover {
    .btn-action {
      display: flex;
    }
  }
}
.el-tree {
  ::v-deep  button {
    padding: 5px 10px;
    font-size: 13px;
    line-height: 13px;
  }

  ::v-deep  img {
    width: 13px;
    height: 13px;
  }
  ::v-deep  .el-tree-node__content:hover {
    background: #f5f7fa !important;
  }

  ::v-deep  .el-tree-node:focus > .el-tree-node__content {
    background: transparent;
  }

  ::v-deep  .el-tree-node {
    &.is-expanded {
      & > .el-tree-node__content {
        // font-weight: bold;
      }
    }
  }

  .menu-parent {
    i {
      margin-right: 5px;
    }
  }
  ::v-deep  [btn-container] {
    display: flex;
    flex-wrap: wrap;
    margin-bottom: -5px;
    padding-left: 56px;
    .el-tree-node {
      flex: 0 0 25%;
      margin-bottom: 5px;
      .el-tree-node__content {
        padding-left: 0px !important;
      }
      //   background: red;
    }
  }
}
</style>
