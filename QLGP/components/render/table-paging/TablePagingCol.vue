<template>
  <el-table-column
    :type="col.type"
    :width="width"
    :min-width="col.min_width"
    :show-overflow-tooltip="false"
    :sortable="sortable"
    :sort-by="col.sortBy || col.data"
    :class-name="getClassName()"
    :label-class-name="col.headerClass"
    :prop="col.data"
    :label="col.title"
    :fixed="col.fix"
    :filters="col.filters"
    :filter-method="col.filterMethod"
    filter-placement="bottom-end"
    :align="align"
    :header-align="align"
  >
    <template v-if="col.children && col.children.length">
      <TablePagingCol
        v-for="item in col.children.filter((p) => p.GetAllChild)"
        v-bind:key="item.Index"
        :col="item"
      >
        <template
          v-for="item1 in item.GetAllChild()"
          :slot="'column-content-' + item1"
          slot-scope="scope"
        >
          <slot
            :name="'column-content-' + item1"
            v-bind:row="scope.row"
            v-bind:column="scope.column"
            v-bind:index="scope.$index"
          ></slot>
        </template>

        <template
          v-for="item1 in item.GetAllChild()"
          :slot="'column-header-' + item1"
          slot-scope="scope"
        >
          <slot
            :name="'column-header-' + item1"
            v-bind:column="scope.column"
            v-bind:col="scope.col"
          ></slot>
        </template>
      </TablePagingCol>
      <TablePagingCol
        v-for="item in col.children.filter((p) => !p.GetAllChild)"
        v-bind:key="item.Index"
        :col="item"
      >
        <slot
          :slot="'column-content-' + item.data"
          slot-scope="scope"
          :name="'column-content-' + item.data"
          v-bind:row="scope.row"
          v-bind:column="scope.column"
          v-bind:index="scope.$index"
        ></slot>

        <slot
          :slot="'column-header-' + item.data"
          slot-scope="scope"
          :name="'column-header-' + item.data"
          v-bind:row="scope.row"
          v-bind:column="scope.column"
          v-bind:index="scope.$index"
        ></slot>
      </TablePagingCol>
    </template>
    <slot
      :name="'column-header-' + col.data"
      slot-scope="scope"
      v-bind:column="scope.column"
      v-bind:col="col"
      slot="header"
    >
      <span v-html="col.title" class="tbpg-th" style="display: block"> </span>
    </slot>
    <slot
      :name="'column-content-' + col.data"
      slot-scope="scope"
      v-bind:row="scope.row"
      v-bind:column="scope.column"
      v-bind:index="scope.$index"
    >
      <span v-html="rowValue(scope)"></span
    ></slot>
  </el-table-column>
</template>

<script>
import TablePagingCol from "~/assets/scripts/base/TablePagingCol";
import ConvertStr from "~/assets/scripts/ConvertStr";
import { resolve } from "~/assets/scripts/Functions";

export default {
  props: {
    /** @type {TablePagingCol|Object} - description */
    col: {
      type: TablePagingCol | Object,
      required: true,
    },
  },
  computed: {
    /**
     *@return {boolean|string} description
     */
    sortable() {
      return this.col.children && this.col.children.length
        ? false
        : this.col.sortable == false
        ? false
        : "custom";
    },
    /**
     *@return {number|string} description
     */
    width() {
      return this.col.width || this.col.min_width || "auto";
    },
    /**
     *@return {'left'|'center'|'right'} description
     */
    align() {
      if (this.col.align) return this.col.align;
      else if (this.col.formatter == "number") return "right";
      return "left";
    },
  },
  methods: {
    getClassName() {
      return "tablepaging-cell";
    },
    rowValue({ row, column }) {
      if (!column) return "";
      let value = resolve(row, column.property);
      if (this.col.formatter) {
        if (typeof this.col.formatter == "function") {
          return this.col.formatter(value, row, column);
        } else {
          switch (this.col.formatter) {
            case "number":
              return ConvertStr.ToMoneyStr(value);
            case "date":
              return ConvertStr.ToDateStr(value, "DD/MM/YYYY");
            case "datetime":
              return ConvertStr.ToDateStr(value, "DD/MM/YYYY HH:mm");
            case "time":
              return ConvertStr.ToDateStr(value, "HH:mm");
            case "datetimes":
              return ConvertStr.ToDateStr(value, "DD/MM/YYYY HH:mm:ss");
            case "times":
              return ConvertStr.ToDateStr(value, "HH:mm:ss");
            default:
              return value;
          }
        }
      } else {
        return value;
      }
      // col.formatter
      //       ? col.formatter(
      //           rowValue(scope.row, scope.column.property),
      //           scope.row,
      //           scope.column
      //         )
      //       :
      // return resolve(row, path);
    },
  },
};
</script>
