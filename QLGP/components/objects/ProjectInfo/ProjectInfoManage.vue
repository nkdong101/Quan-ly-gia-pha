<!-- <template>
  <div class="pim-c">
    <div class="pim-title">
      {{ title }}
      <el-input v-model="iSearchInfo" placeholder="type to search..." />
    </div>
    <div style="flex: 1">
      <TablePaging ref="tp" :model="tp" style="">
        <template slot="column-header-button">
          <el-button
            class="icon-btn icon-btn"
            v-if="pagePermission.add"
            type="primary"
            @click="Add()"
          >
            <i class="el-icon-plus"></i
          ></el-button>
        </template>

        <template slot="column-content-button" slot-scope="{ row }">
          <div style="display: flex">
            <el-button
              v-if="pagePermission.edit"
              class="icon-btn"
              type="primary"
              @click="Edit(row)"
            >
              <i class="el-icon-edit"></i
            ></el-button>
            <el-button
              v-if="pagePermission.delete"
              class="icon-btn"
              type="danger"
              @click="Delete(row)"
            >
              <i class="el-icon-delete"></i>
            </el-button>
          </div>
        </template>

        <template slot="column-content-Use" slot-scope="{ row }">
          <div style="display: flex; justify-content: center">
            <i
              style="font-size: 17px; width: 17px; height: 17px"
              :class="
                'code-state fa ' +
                (row.Use != 2 ? 'fa-check-square-o' : 'fa-square-o')
              "
              :Use="row.Use != 2"
              @click="ChangeState(row)"
            ></i>
          </div>
        </template>
      </TablePaging>
    </div>
    <DefaultForm :model="form" @actionOK="form.Save.call(this)">
      <div slot="content">
        <FormInfo ref="form" :model="form.obj.form()" />
      </div>
    </DefaultForm>
  </div>
</template>

<script>
import API from "~/assets/scripts/API";
import DefaultForm from "~/assets/scripts/base/DefaultForm";
import DelayCall from "~/assets/scripts/base/DelayCall";
import TablePaging from "~/assets/scripts/base/TablePaging";
import TablePagingCol from "~/assets/scripts/base/TablePagingCol";
import { MessageType, ShowConfirm } from "~/assets/scripts/Functions";
import GetDataAPI from "~/assets/scripts/GetDataAPI";
import Project_Info from "~/assets/scripts/objects/Project_Info";
export default {
  props: {
    type: {},
    project_code: {},
  },
  data() {
    return {
      iSearchInfo: "",
      tp: new TablePaging({
        noFilter: true,
        // disablePaging: this.type == 0 ? false : true,
        // disablePaging: true,
        pagingShowOpt:
          this.type == 0
            ? {}
            : {
                summary: false,
                recordOfPage: false,
              },
        emptyText:
          this.type != 0 && !this.project_code ? "Select Project first" : "",
        clickRow: (row) => {
          this.handleClickRow(row);
        },
        cols:
          this.type == 0
            ? [
                new TablePagingCol({
                  title: "STT",
                  data: "SttTP",
                  min_width: 50,
                  sortable: false,
                }),

                new TablePagingCol({
                  title: "Project Code",
                  data: "Project_Code",
                  min_width: 120,
                }),
                new TablePagingCol({
                  title: "Project Name",
                  data: "Project_Name",
                  min_width: 240,
                  width: "auto",
                }),
                new TablePagingCol({
                  title: "Total Award",
                  data: "Total_Award",
                  min_width: 115,
                  formatter: "number",
                  align: "right",
                }),
                new TablePagingCol({
                  title: "Total Task",
                  data: "Total_Task",
                  min_width: 105,
                  formatter: "number",
                  align: "right",
                }),
                new TablePagingCol({
                  title: "Use",
                  data: "Use",
                  min_width: 50,
                  sortable: false,
                  align: "center",
                  fix: "right",
                }),
                new TablePagingCol({
                  title: "",
                  data: "button",
                  min_width: 80,
                  fix: "right",
                  align: "center",
                  sortable: false,
                }),
              ]
            : [
                new TablePagingCol({
                  title: "STT",
                  data: "SttTP",
                  min_width: 50,
                  sortable: false,
                }),

                new TablePagingCol({
                  title: this.type == 1 ? "Award Code" : "Task Code",
                  data: "Name",
                  min_width: 120,
                  width: "auto",
                }),
                new TablePagingCol({
                  title: "Use",
                  data: "Use",
                  min_width: 50,
                  sortable: false,
                  align: "center",
                  fix: "right",
                }),
              ],
      }),
      data: [],
      dc: null,
      form: new DefaultForm({
        obj: new Project_Info(),
        title: "",
        visible: false,
        width: "800px",
        ShowForm: (title, isAdd, obj) => {
          this.isAdd = isAdd;
          var _app = this;
          if (obj) {
            this.form.obj = new Project_Info({
              Project_Code: obj.Project_Code,
              Project_id: obj.Id,
              Project_Name: obj.Project_Name,
            });
          } else this.form.obj = new Project_Info(obj);
          this.form.title = title;

          this.form.visible = true;
        },
        Save: () => {
          this.Save();
        },
      }),
    };
  },
  watch: {
    project_code(newValue, oldValue) {
      this.LoadData();
      this.tp.emptyText =
        this.type != 0 && !this.project_code ? "Select Project first" : "";
    },
    iSearchInfo() {
      this.dc.run();
    },
  },
  computed: {
    title() {
      switch (this.type) {
        case 0:
          return "Project Info";
        case 1:
          return "Award Info";
        case 2:
          return "Task Info";
        default:
          break;
      }
    },
  },
  methods: {
    ChangeState(row) {
      console.log(row, this.type);
      GetDataAPI({
        url: API.Project_Info_ChangeState,
        params: {
          iType_id: this.type,
          iCode: this.type == 0 ? row.Project_Code : row.Id,
        },
        action: (re) => {
          if (row.Use != 2) row.Use = 2;
          else row.Use = 1;
        },
      });
    },
    Delete(row) {
      ShowConfirm({
        message: "Delete [" + row.Project_Name + "]",
        title: "Waring!",
        type: MessageType.warning,
      })
        .then(() => {
          GetDataAPI({
            method: "POST",
            url: API.Delete_Project_Info,
            params: row,
            action: (re) => {
              if (re == "OK") {
                this.LoadData();
                ShowMessage("Delete successfully");
              } else {
                // ShowMessage(re);
              }
            },
          });
        })
        .catch((err) => {
          // An error occurred
        });
    },
    Add() {
      this.form.ShowForm("Add Type of Competition", true, null);
    },
    Edit(row) {
      this.form.ShowForm("Edit Type of Competition", false, row);
    },
    Save() {
      var _app = this;
      this.$refs.form.getValidate().then((re) => {
        if (!re) {
          ShowMessage(
            "Please, You need enter full information!",
            MessageType.error
          );
          return;
        } else {
          GetDataAPI({
            method: "post",
            url: API.Save_Project_Info,
            params: this.form.obj.toJSON(),
            action: (re) => {
              this.LoadData();
              if (this.isAdd) {
                this.form.obj.Project_id = re;
              }
              this.$refs.form.getEntry("formProjectSave").LoadData();
              ShowMessage("Save successfully");
              // this.form.visible = false;
            },
          });
        }
      });
    },
    handleClickRow(row) {
      if (this.type == 0) {
        this.$emit("onPickProject", this.$refs.tp.selectedRow);
      }
    },
    GetData() {
      this.$refs.tp.params.iSearchInfo = this.iSearchInfo;
      // this.tp.data = this.data.filter((p) => {
      //   return (
      //     JSON.stringify(p)
      //       .toLowerCase()
      //       .indexOf(this.iSearchInfo.toLowerCase()) > -1
      //   );
      // });
      // this.$refs.tp.LoadData(true);
    },
    LoadData() {
      if (this.type == 0) {
        GetDataAPI({
          url: API.Get_Project_Info,
          action: (re) => {
            this.data = re;
            this.tp.data = re;
            this.$refs.tp.LoadData(true);
            this.GetData();
          },
        });
      } else {
        if (this.project_code) {
          GetDataAPI({
            url: API.Project_Info_Get_ValueCode,
            params: {
              Project_Code: this.project_code,
              Type_id: this.type,
            },
            action: (re) => {
              this.tp.data = re;
              this.$refs.tp.LoadData(true);
              this.GetData();
            },
          });
        } else {
          this.tp.data = [];
          this.$refs.tp.LoadData(true);
          this.GetData();
        }
      }
    },
  },
  mounted() {
    if (this.type == 0) {
      this.LoadData();
    }
    console.log(this);
    this.dc = new DelayCall(() => {
      this.GetData();
    });
  },
};
</script>

<style lang="scss" scoped>
.pim-c {
  height: 100%;
  display: flex;
  flex-direction: column;
  overflow: hidden;
  border: 1px solid #ddd;
  border-radius: 4px;
  .pim-title {
    padding: 5px 5px;
    font-size: 20px;
    background: white;
  }
}
.code-state {
  &[Use] {
    color: #409eff;
  }
  // color: red;
  color: #409eff;
}
</style> -->
