<template>
  <div class="main-content">
    <!-- <div class="main-header" v-if="model.title">
      <span>{{ model.title }}</span>
      <span>{{
        model.title == "menu"
          ? currentMenu.LiClass || currentMenu.Title
          : model.title
      }}</span>

    </div> -->
    <!-- {{ currentMenu }} -->
    <!-- <div v-if="!model.disableControl" class="main-control">
      <div class="main-input-control">
        <slot name="btn" v-bind:tempRows="tempRows">
          <FormContainer
            v-if="model.control"
            :model="formControl"
          ></FormContainer>
        </slot>
      </div>
      <div class="main-control-search" v-if="!model.noFilter">
        <el-input
          v-model="iSearchInfo"
          placeholder="Tìm kiếm..."
          :clearable="true"
          @change="SearchData()"
        ></el-input>
      </div>
    </div> -->
    <div class="table-controls-c">
      <slot name="filter" v-bind:tempRows="tempRows"> </slot>
    </div>
    <div v-if="model.disablePaging != true" class="paging-control">
      <div class="paging-size">
        <div class="main-control-search" v-if="!model.noFilter">
          <el-input
            v-model="iSearchInfo"
            placeholder="Search..."
            :clearable="true"
            @change="SearchData()"
          ></el-input>
        </div>
        <slot name="btn" v-bind:tempRows="tempRows"> </slot>
        <span
          v-if="pagingShowOpt.summary"
          v-html="GetPagingDetail()"
          style="padding-left: 5px"
        ></span>
      </div>
      <div class="paging-page">
        <span v-if="pagingShowOpt.recordOfPage">Shows</span>
        <el-pagination
          v-if="pagingShowOpt.recordOfPage"
          @size-change="handleSizeChange"
          @current-change="handleCurrentChange"
          :current-page.sync="curentPage"
          :page-sizes="[10, 50, 100, 200, 400]"
          :page-size="params.iRecordOfPage"
          layout="sizes"
          :total="totalRecord"
        ></el-pagination>
        <el-pagination
          :hide-on-single-page="true"
          @size-change="handleSizeChange"
          @current-change="handleCurrentChange"
          :current-page.sync="curentPage"
          :page-sizes="[10, 50, 100, 200, 400]"
          :page-size="params.iRecordOfPage"
          layout="prev, pager, next"
          :total="totalRecord"
        ></el-pagination>
        <el-link
          class="btn-export"
          :underline="false"
          icon="fa fa-download"
          style="padding: 10px"
          @click.native="ExportExcel()"
        >
        </el-link>
      </div>
    </div>
    <div class="main-table" style="display:flex;align-items: flex-start;">

      <el-table
        v-if="isVisible"
        :empty-text="
          isLoading ? 'loading...' : model.emptyText ?? 'Không có dữ liệu.'
        "
        ref="tpRef"
        :data="tableData"
        :row-key="model.keyId"
        style="width: 100%; height: 100%"
        :height="model.disableFixedHead ? undefined : '100%'"
        header-row-class-name="table-header"
        highlight-current-row
        :stripe="model.rowStripe"
        border
        :show-summary="model.handleSumary != undefined"
        :summary-method="model.handleSumary"
        :show-header="model.showHeader"
        @current-change="handleCurrentRowChange"
        @row-click="handleRowClick"
        @row-dblclick="handleRowDBClick"
        @sort-change="handleSortChange"
        :default-expand-all="model.expandAll"
        :span-method="model.spanMethod"
        :row-class-name="model.tableRowClassName"
        :tree-props="model.treeprops"
      >
        <template v-for="col in model.cols.filter((p) => p)">
          <TablePagingCol
            v-bind:key="col.Index"
            :col="col"
            v-if="allChild(col)"
          >
            <template
              v-for="item in allChild(col)"
              :slot="'column-header-' + item"
              slot-scope="scope"
            >
              <slot
                :name="'column-header-' + item"
                v-bind:column="scope.column"
                v-bind:col="scope.col"
              >
              </slot>
            </template>

            <template
              v-for="item in allChild(col)"
              :slot="'column-content-' + item"
              slot-scope="scope"
            >
              <slot
                :name="'column-content-' + item"
                v-bind:row="scope.row"
                v-bind:column="scope.column"
                v-bind:index="scope.index"
              ></slot>
            </template>
          </TablePagingCol>
          <TablePagingCol v-bind:key="col.Index" :col="col" v-else>
            <slot
              :slot="'column-header-' + col.data"
              slot-scope="scope"
              :name="'column-header-' + col.data"
              v-bind:column="scope.column"
            ></slot>
            <slot
              :slot="'column-content-' + col.data"
              slot-scope="scope"
              :name="'column-content-' + col.data"
              v-bind:row="scope.row"
              v-bind:column="scope.column"
              v-bind:index="scope.index"
            ></slot>
          </TablePagingCol>
        </template>
      </el-table>
    </div>
    <div v-if="model.sumary" class="sumary-c">
      <div
        v-for="item in model.sumary(rawData)"
        :key="item.label"
        class="sumary-c-item"
      >
        <div class="sumary-c-item-label">{{ item.label }}</div>
        <div class="sumary-c-item-value">{{ item.value }}</div>
      </div>
    </div>
  </div>
</template>

<script>
import GetDataAPI from "@/assets/scripts/GetDataAPI";
import FormContainer from "~/assets/scripts/base/FormContainer";
import {
  Compare,
  concatDeep,
  MessageType,
  objContainStr,
  resolve,
  s2ab,
  sheet_from_array_of_arrays,
  ShowMessage,
  Uni2None,
  Workbook,
} from "@/assets/scripts/Functions";
export default {
  name: "TablePaging",
  props: {
    model: {
      type: Object,
      required: true,
    },
  },
  data() {
    return {
      iSearchInfo: "",
      tmpData: ["Name", "xxx", "yyy"],
      isVisible: true,
      tempRows: [],
      isLoading: false,
      totalPage: 0,
      curentPage: 1,
      pagingShowOpt: {
        ...{
          summary: true,
          recordOfPage: true,
        },
        ...this.model.pagingShowOpt,
      },
      params: {
        iRecordOfPage: 50,
        iPage: 0,
        sortOrder: "",
        sortProp: "",
        ...this.model.params,
        iSearchInfo: "",
      },
      // sortOrder: "",
      // sortProp: "",
      isPagingServer: false,
      selectedRow: null,
      formControl: new FormContainer(),
      totalRecord: 0,
    };
  },
  watch: {
    iSearchInfo() {
      if (this.model.params) this.model.params.iSearchInfo = this.iSearchInfo;
    },
    "model.params": {
      deep: true,
      handler() {
        Object.keys(this.model.params).forEach((p) => {
          this.params[p] = this.model.params[p];
        });
        // this.params = { ...this.params, ...this.model.params };
      },
    },
    "model.cols": function () {
      let _app = this;
      _app.isVisible = false;
      this.$nextTick(function () {
        _app.isVisible = true;
      });
    },
    // "model.data": {
    //   deep: true,
    //   handler() {
    //     let _app = this;
    //     _app.isVisible = false;
    //     this.$nextTick(function() {
    //       _app.isVisible = true;
    //     });
    //   }
    // }
  },
  computed: {
    /**
     * @return {Array} description
     */
    rawData() {
      let rows = [];
      if (this.isPagingServer) {
        rows = this.tempRows;
      } else {
        rows = this.tempRows.filter(
          (re) =>
            !this.params.iSearchInfo
              ? true
              : this.GetValueDisplay(re).some((p) =>
                  objContainStr(
                    p,
                    Uni2None(this.params.iSearchInfo || ""),
                    this.model.isSearchEqual
                  )
                )
          //    JSON.stringify(re)
          //   .toUpperCase()
          //   .indexOf(this.params.iSearchInfo.toUpperCase()) > -1
        );
        this.totalRecord = rows.length;
        rows.sort(this.sortHandle);
        rows.forEach((item, index) => {
          item.SttTP = index + 1;
        });
      }

      return rows;
    },
    /**
     * @return {Array} description
     */
    tableData() {
      let data = this.rawData;
      if (this.isPagingServer) {
        return data;
      } else {
        if (this.model.disablePaging) return data;
        return data.filter(
          (re) =>
            data.indexOf(re) >=
              (this.curentPage - 1) * this.params.iRecordOfPage &&
            data.indexOf(re) < this.curentPage * this.params.iRecordOfPage
        );
      }
    },

    // totalRecord: function() {
    //   return this.totalPage * this.params.iRecordOfPage;
    // }
  },
  methods: {
    GetValueDisplay(obj) {
      let str = this.model.cols.map((p) => {
        // if(p.data == 'Manager'){
        //   console.log(p.formatter(obj[p.data], obj));
        // }
        if (typeof p.formatter == "function") {
          return Uni2None(
            JSON.stringify(p.formatter(obj[p.data], obj) || "").replace(
              /(<([^>]+)>)/gi,
              ""
            )
          );
        }

        return Uni2None(
          JSON.stringify(obj[p.data] || "").replace(/(<([^>]+)>)/gi, "")
        );
      });
      // console.log(str);
      return str;
    },
    GetPagingDetail() {
      let tu = (this.curentPage - 1) * this.params.iRecordOfPage + 1;
      let den =
        (this.curentPage - 1) * this.params.iRecordOfPage +
        this.params.iRecordOfPage;
      return `Viewing from <b>${tu}</b> to <b>${
        den > this.totalRecord ? this.totalRecord : den
      }</b> out of <b>${this.totalRecord}</b> records`;
    },
    allChild(col) {
      if (col.GetAllChild) return col.GetAllChild();
      return null;
    },
    tmp(e) {
      //   //console.log(e);
    },

    ExportExcel: function (opt) {
      var _app = this;
      opt = opt || {};
      if (this.model.ExportConfig) this.model.ExportConfig(opt);
      new Promise(function (rs, rj) {
        if (opt.data) rs(opt.data);
        else {
          if (_app.isPagingServer) {
            GetDataAPI({
              url: _app.model.data,
              params: {
                ..._app.params,
                iRecordOfPage: 50000,
              },
              action: (re) => {
                if (_app.model.LoadDataSuccess) {
                  _app.model.LoadDataSuccess(re);
                }
                re.Data.forEach((item, index) => {
                  item.SttBase =
                    (_app.curentPage - 1) * _app.params.iRecordOfPage +
                    index +
                    1;
                  item.SttTP =
                    (_app.curentPage - 1) * _app.params.iRecordOfPage +
                    index +
                    1;
                });
                rs(re.Data);
              },
            });
          } else rs(_app.rawData);
        }
      }).then(function (data) {
        if (!opt.data) opt.data = data;

        if (opt.ReloadData) {
          opt.data = opt.ReloadData(opt.data);
        }

        if (!opt.header) opt.header = _app.model.cols;
        let cols = [];

        function ReRenderHeader(data, rowIndex, baseIndex) {
          let totalCols = 0;
          cols[rowIndex] = cols[rowIndex] || [];
          let baseX = baseIndex;
          data.forEach(function (obj) {
            let i = baseIndex;

            let p = {
              title: obj.title,
              data: obj.data,
              min_width: obj.min_width,
              row_span: 0,

              startIndex: i,
              index: 0, //obj.children.length ? 0 : baseIndex,
              formatter: obj.formatter,
              exporter: obj.exporter,
            };

            if ((obj.children || []).length) {
              let totalColReturn = ReRenderHeader(
                obj.children,
                rowIndex + 1,
                i
              );

              p.col_span = totalColReturn;
              p.row_span = 1;
              totalCols += totalColReturn;
              baseIndex += totalColReturn;
            } else {
              totalCols += 1;
              baseIndex += 1;
              p.index = baseIndex;
            }
            cols[rowIndex].push(p);
          });

          return totalCols;
        }
        // function ReRenderHeader(data, rowIndex, parent) {
        //   let i = parent.startIndex;

        //   if (!cols[rowIndex]) cols[rowIndex] = [];
        //   let nextRowIndex = rowIndex + 1;
        //   data.forEach(function (p) {
        //     let index = 0;
        //     if (rowIndex === 0) {
        //       startIndex += p.children.length ? p.children.length : 1;
        //       index = startIndex;
        //     } else {
        //     }
        //     let obj = {
        //       data: p.data,
        //       title: p.title,
        //       min_width: p.min_width,
        //       row_span: parent ? 1 : 0,

        //       index: p.children.length ? 0 : index,
        //       startIndex: i,
        //       formatter: p.formatter,
        //     };

        //     if (p.children && p.children.length) {
        //       obj.col_span = p.children.length;
        //       obj.row_span = 1;
        //       ReRenderHeader(p.children, nextRowIndex, p);
        //     }
        //     cols[rowIndex].push(obj);
        //   });
        // }

        ReRenderHeader(opt.header, 0, 0);
        cols.forEach((p, index) => {
          p.forEach((item) => {
            if (!item.row_span) item.row_span = cols.length - index;
          });
        });
        // console.log(cols);
        var title = _app.model.title || opt.title || "export";
        var tD = new Date();
        var ws_name = "sheet";

        var wb = new Workbook(),
          ws = sheet_from_array_of_arrays(opt.data, cols, title);
        //.map(p => +p.style.split(';')[0].replace(/[^0-9]/g, ''))

        /* add worksheet to workbook */
        wb.SheetNames.push(ws_name);
        wb.Sheets[ws_name] = ws;
        var wbout = XLSX.write(wb, {
          bookType: "xlsx",
          bookSST: true,
          type: "binary",
        });
        // alert("OK");
        var blob = new Blob([s2ab(wbout)], {
          type: "application/octet-stream",
        });
        var dlink = document.createElement("a");
        dlink.download = "file.xlsx";
        dlink.href = window.URL.createObjectURL(blob);
        dlink.onclick = function (e) {
          // revokeObjectURL needs a delay to work properly
          var that = this;
          setTimeout(function () {
            window.URL.revokeObjectURL(that.href);
          }, 1500);
        };

        dlink.click();
        dlink.remove();
        // alert(objectURL);
        // saveAs(
        //   new Blob([s2ab(wbout)], { type: "application/octet-stream" }),
        //   title.toLowerCase().trim().replace(/ +/g, " ").replace(/ /g, "-") +
        //     "-" +
        //     tD.getDate() +
        //     "-" +
        //     (tD.getMonth() + 1) +
        //     "-" +
        //     tD.getFullYear() +
        //     ".xlsx"
        // );
      });
    },
    clearSelected: function () {
      // this.$refs[this.key].setCurrentRow(undefined);
    },
    handleCurrentRowChange: function (val) {
      // this.selectedRow = val;
    },
    handleSortChange({ column, prop, order }) {
      this.params.sortOrder = order;
      this.params.sortProp = prop;
      if (this.isPagingServer) {
        this.LoadData(true);
      }
      //   //console.log(column, prop, order);
    },
    handleRowDBClick: function (row, column, event) {
      if (this.model.dbClickRow) this.model.dbClickRow(row);
    },
    handleRowClick: function (row, column, event) {
      if (!this.model.disableSelectRow) {
        if (this.selectedRow == row) {
          this.$refs.tpRef.setCurrentRow(undefined);
          this.selectedRow = null;
        } else {
          this.$refs.tpRef.setCurrentRow(row);
          this.selectedRow = row;
        }
      } else {
        this.$refs.tpRef.setCurrentRow(undefined);
        this.selectedRow = null;
      }
      if (this.model.clickRow) this.model.clickRow(row, column, event);
    },
    handleSizeChange(val) {
      this.params.iRecordOfPage = val;

      //   this.totalPage
      if (this.isPagingServer) this.LoadData(true);
      else
        this.totalPage = Math.ceil(
          this.rawData.length / this.params.iRecordOfPage
        );
    },
    handleCurrentChange(val) {
      this.params.iPage = val - 1;
      if (this.isPagingServer) this.LoadData();
    },
    SearchData: function () {
      this.params.iSearchInfo = this.iSearchInfo;
      if (this.isPagingServer) this.LoadData(true);
    },

    handleData: function (re) {
      if (this.model.LoadDataSuccess) {
        this.model.LoadDataSuccess(re);
      }

      this.$nextTick(function () {
        if (Array.isArray(re)) {
          this.isPagingServer = false;
          this.tempRows = re;

          this.curentPage = 1;
          this.totalPage = Math.ceil(
            this.rawData.length / this.params.iRecordOfPage
          );
          this.totalRecord = this.rawData.length;
        } else {
          this.isPagingServer = true;
          this.tempRows = re.Data || [];
          this.totalPage = re.TotalPage;
          this.curentPage = (re.CurentPage || 0) + 1;
          this.totalRecord = re.TotalRecord;
        }

        this.tempRows.forEach((item, index) => {
          item.SttBase =
            (this.curentPage - 1) * this.params.iRecordOfPage + index + 1;
          item.SttTP =
            (this.curentPage - 1) * this.params.iRecordOfPage + index + 1;
        });

        this.isLoading = false;
        if (this.model.Loaded)
          this.$nextTick(function () {
            this.model.Loaded();
          });
      });
    },
    LoadData: function (isReset) {
      if (this.model.Validate != undefined) {
        if (!this.model.Validate(this.params)) return;
      }

      this.isLoading = true;
      if (this.$refs.tpRef) this.$refs.tpRef.setCurrentRow(undefined);
      this.selectedRow = null;
      if (isReset) this.params.iPage = 0;

      if (typeof this.model.data == "string") {
        GetDataAPI({
          url: this.model.data,
          params: this.params,
          action: (re) => {
            this.handleData(re || []);
          },
          error: (err) => {
            ShowMessage(err, MessageType.error);
            this.handleData([]);
          },
        });
      } else if (Array.isArray(this.model.data)) {
        this.handleData(JSON.parse(JSON.stringify(this.model.data)));
      } else if (typeof this.model.data == "function") {
        this.model
          .data(this.params.toJSON ? this.params.toJSON() : this.params)
          .then((re) => {
            this.handleData(re || []);
          })
          .catch(() => {
            this.handleData([]);
          });
      } else {
        this.handleData([]);
      }
    },

    sortHandle(a, b) {
      switch (this.params.sortOrder) {
        case "ascending":
          return Compare(
            resolve(a, this.params.sortProp),
            resolve(b, this.params.sortProp)
          );
          break;
        case "descending":
          return -Compare(
            resolve(a, this.params.sortProp),
            resolve(b, this.params.sortProp)
          );
          break;
        default:
          return Compare(a.SttBase, b.SttBase);
          break;
      }
    },
  },
  mounted() {
    this.LoadData();
    // //console.log(this.$refs[this.tpRef]);
    if (this.model.control) this.formControl = this.model.control(this.model);

    this.itv = setInterval(() => {
      this.$nextTick(() => {
        var listFixBody = document.getElementsByClassName(
          "el-table__fixed-body-wrapper"
        );
        Array.from(listFixBody).forEach((p) => {
          p.style.height = `calc(100% - ${p.style.top})`;
        });
      });
    }, 500);
  },
  beforeDestroy() {
    clearInterval(this.itv);
  },
};
</script>

<style lang="scss" scoped>
@import "~@/assets/css/variable.scss";

.main-content {
  display: flex;
  flex-direction: column;
  height: 100%;
  ::v-deep  .el-table__header-wrapper table {
    border-top: 1px solid #efefef !important;
  }
  .main-table {
    padding: 0 5px;
    flex: 1;
    overflow: hidden;
  }
  .main-header {
    flex: 0 0 37px;
    margin: 0px;
    position: relative;
    // color: #5183e8;
    background: #ffffff;
    text-transform: uppercase;
    // border-radius: 10px 0px 0px 0px;
    span {
      // font-family: Bold;
      position: absolute;
      font-weight: bold;
      font-size: 15px;
      top: 50%;
      transform: translateY(-50%);
      display: block;
      width: 100%;
      padding-right: 10px !important;
      padding-left: 5px;
    }

    + .main-control {
      .main-control-search {
        position: absolute;
        top: -33.5px;
        right: 5px;

        margin-top: 0;
      }
    }
  }
  ::v-deep  .table-header {
    th {
      padding: 10px 0px;
      color: #2a4b69;
      background: #efefef;
      div.cell {
        .caret-wrapper {
          position: absolute;
          right: 0;
          top: 50%;
          transform: translateY(-50%);
        }
      }
    }
  }
  ::v-deep  .el-table__row {
    td {
      padding: 10px 0px;
      cursor: pointer;
    }
  }
  ::v-deep  .el-table .cell.el-tooltip {
    width: 100% !important;
    min-width: 0;
    line-height: 16px;
  }

  ::v-deep  .el-table {
    th {
      &.is-sortable {
        .cell {
          padding-right: 24px;
        }
      }
      & > .cell {
        // padding-right: 24px;
        // white-space: nowrap;
      }
    }
    .el-table__footer-wrapper {
      tbody {
        td {
          background-color: #efefef;
          border-right: 1px solid #ddd;
          border-top: 1px solid #ddd;
        }
      }
      .cell {
        line-height: 16px;
      }
    }
  }
  ::v-deep  .el-table--striped .el-table__body tr.el-table__row--striped td {
    background: #f4f5f6;
  }
  ::v-deep  .el-table__body tr.hover-row.current-row > td,
  ::v-deep  .el-table__body tr.hover-row.el-table__row--striped.current-row > td,
  ::v-deep  .el-table__body tr.hover-row.el-table__row--striped > td,
  ::v-deep  .el-table__body tr.hover-row > td {
    background-color: #e2ecf8;
  }
  ::v-deep
    .el-table--striped
    .el-table__body
    tr.el-table__row--striped.current-row
    td,
  ::v-deep  .el-table__body tr.current-row > td {
    background-color: $swatch_1;
    color: white;
  }
  ::v-deep  .el-table .el-table__fixed th.is-leaf {
    border-bottom: 0px;
  }
  ::v-deep  .el-table td,
  ::v-deep  .el-table th.is-leaf {
    border-bottom: 1px solid #ddd;
  }
  ::v-deep  .el-table--border td,
  ::v-deep  .el-table--border th,
  ::v-deep
    .el-table__body-wrapper
    .el-table--border.is-scrolling-left
    ~ .el-table__fixed {
    // border-right: 1px solid #ddd;
    overflow: hidden;
    border-bottom: 1px solid #ddd;
    .full-cell {
      position: absolute;
      top: 0;
      left: 0;
      width: 100%;
      height: 100%;
      display: flex;
      align-items: center;
      padding-left: 10px;
    }
  }
  ::v-deep  {
    .el-table--border .el-table__cell,
    .el-table__body-wrapper
      .el-table--border.is-scrolling-left
      ~ .el-table__fixed {
      border-right: 0;
    }
  }

  ::v-deep  .el-table tr {
    &.danger-row {
      color: #f44336;
    }
  }
  ::v-deep
    .el-table--striped
    .el-table__body
    tr.danger-row.el-table__row--striped.current-row
    td,
  ::v-deep  .el-table__body tr.danger-row.current-row > td {
    color: #f44336;
  }
  .main-control {
    // padding: 5px;
    display: flex;
    position: relative;
    // &:empty {
    //   padding: 0px;
    // }
    // .main-control-btn:empty {
    //   display: none;
    // }
    // &:not(.main-input-control:empty) {
    //     padding: 0;
    //     padding-top: 5px;
    // }
    .main-control-search {
      width: 200px;
      margin-left: auto;
      padding-left: 5px;

      margin-bottom: 5px;
      display: flex;
      align-items: flex-end;
    }
    ::v-deep  .main-input-control {
      flex: 1;
      padding: 5px;
      &:empty {
        padding-top: 0px;
        display: none;
      }
      > div {
        @include control-container;
      }
    }
    ::v-deep  .para-control {
      @include control-container;
    }
    .main-control-btn {
      flex: 1;
      overflow: hidden;
      .el-button + .el-button {
        margin-left: 0px;
      }

      .el-button--mini,
      .el-button--mini.is-round {
        padding: 7px 15px;
      }
      > div {
        @include control-container;
      }
    }
  }
  .paging-control {
    display: flex;
    justify-content: space-between;
    font-size: 13px;
    flex-wrap: wrap;

    // padding-top: 3px;
    .paging-size,
    .paging-page {
      display: flex;
      align-items: center;
      padding-left: 5px;
    }
  }
  ::v-deep  .el-table.el-table--border {
    border-top: 1px solid #ddd;
  }
  ::v-deep  .el-table__fixed,
  ::v-deep  .el-table__fixed-right {
    height: calc(100%) !important;
    border-right: 1px solid #ddd;

    .el-table__fixed-body-wrapper {
      //   height: calc(100% - 35px) !important;
    }
  }
  ::v-deep  .el-table--scrollable-x {
    .el-table__fixed,
    .el-table__fixed-right {
      height: calc(100% - 15px) !important;
    }
  }
  ::v-deep  .el-table__body-wrapper.is-scrolling-left ~ .el-table__fixed,
  ::v-deep  .el-table__body-wrapper.is-scrolling-none ~ .el-table__fixed,
  ::v-deep  .el-table__body-wrapper.is-scrolling-none ~ .el-table__fixed-right,
  ::v-deep  .el-table__body-wrapper.is-scrolling-right ~ .el-table__fixed-right {
    border-right: 0px;
  }
  ::v-deep  .el-table {
    .cell {
      line-height: 20px;
      white-space: normal;
      word-break: break-word;
    }
  }
}

// .main-control {
//   padding: 5px;
//   display: flex;
//   // &:empty {
//   //   padding: 0px;
//   // }
//   // .main-control-btn:empty {
//   //   display: none;
//   // }
//   .main-control-search {
//     width: 200px;
//     margin-left: auto;
//     padding-left: 5px;
//   }

//   .main-control-btn {
//     flex: 1;
//     .el-button + .el-button {
//       margin-left: 5px;
//     }
//     .el-button--mini,
//     .el-button--mini.is-round {
//       padding: 7px 15px;
//     }
//   }
// }
.sumary-c {
  display: flex;
  padding: 0 5px;
  padding-top: 5px;
  .sumary-c-item {
    display: flex;
    align-items: center;
    font-size: 13px;
    background: #d3d3d3b3;
    border: 1px solid #ddd;
    padding: 0 5px;
    border-radius: 4px;
    .sumary-c-item-label {
      font-weight: bold;
      padding-right: 5px;
    }
    .sumary-c-item-value {
    }
  }
}

.table-controls-c {
  width: 100%;
  > div {
    width: 100%;
    padding: 0 5px;
    min-height: 33px;
    display: flex;
    align-items: center;
  }
}
@media screen and (max-width: 992px) {
}

/* On screens that are 600px wide or less, the background color is olive */
@media screen and (max-width: 600px) {
}
</style>
