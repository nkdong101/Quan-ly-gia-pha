<template>
  <div>
    <div class="main-header" v-if="Title">
      <b>{{ Title }}</b>
    </div>
    <div style="overflow: auto; height: 150px">
      <TablePaging ref="tp" :model="tp">
        <template slot="column-content-checkbox" slot-scope="{ row }">
          <el-checkbox
            v-model.lazy="row.act"
            @change="checkbox(row)"
          ></el-checkbox>
        </template>
      </TablePaging>
    </div>
  </div>
</template>

<script>
import TablePaging from "~/assets/scripts/base/TablePaging";
import TablePagingCol from "~/assets/scripts/base/TablePagingCol";
export default {
  props: {
    valuecheck: {
      type: Object,
      // default: [],
    },
    Title: {
      type: String,
    },
    data: {
      type: Array,
    },
  },
  data() {
    return {
      tp: new TablePaging({
        title: this.Title,
        params: {
          iSearchInfo: "",
        },
        LoadDataSuccess: (re) => {
          let _app = this;
          (re || []).forEach((element) => {
            var find = (_app.valuecheck.Category_id || []).filter(
              (p) => p == element.Id
            )[0];
            if (find) {
              element.act = true;
            } else element.act = false;
          });
        },
        disablePaging: true,
        noFilter: true,
        disableSelectRow: true,
        data: [],
        cols: [
          new TablePagingCol({
            title: "",
            data: "checkbox",
            min_width: 50,
          }),
          new TablePagingCol({
            title: "",
            data: "Name",
            min_width: 200,
            width: "auto",
            sortable: false,
          }),
        ],
      }),
      datatp: [],
    };
  },
  methods: {
    checkbox(row) {
      if (row.act) {
        this.valuecheck.Category_id.push(row.Id);
        // this.valuecheck.push([])
      } else {
        // this.valuecheck.splice(this.valuecheck.indexOf(row.Id), 1);
        this.valuecheck.Category_id = this.valuecheck.Category_id.filter(
          (p) => {
            if (p == row.Id) {
              return false;
            } else return true;
          }
        );
      }
    },
    ResetCheck() {
      this.valuecheck.length = 0;
      this.$refs.tp.LoadData(true);
    },
  },
  watch: {},
  mounted() {
    // this.datatp = JSON.parse(JSON.stringify(this.data));
    this.tp.data = this.data;
    this.$refs.tp.LoadData(true);
  },
};
</script>

<style lang="scss" scoped>
::v-deep .main-table {
  .el-table__header-wrapper {
    display: none;
  }
  .el-table__body-wrapper {
    height: 100%;
  }
}
.main-header {
  padding: 6px 10px;
  color: #2a4b69;
  line-height: 13px;
}
.buttonFilter {
  position: absolute;
  z-index: 1;
  right: 10px;
  top: 9px;
  width: 15px;
  height: 25px;
  display: flex;
  align-items: center;
  justify-content: center;
}
</style>
