import BaseWidget, {
  BaseWidgetDefaultParams
} from "./BaseWidget";
import TablePagingCol from "./TablePagingCol";
import FormContainer from "./FormContainer";

var defaultParams = function () {
  return {
    title: '',
    emptyText: '',
    keyId: '',
    disableControl: false,
    noFilter: false,
    showHeader: true,
    disableFixedHead: false,
    disablePaging: false,
    disableSelectRow: false,
    handleSumary: null,
    spanMethod: null,
    clickRow: null,
    dbClickRow: null,
    LoadDataSuccess: null,
    Loaded: null,
    Validate: null,
    cols: [new TablePagingCol()].splice(0, 0),
    data: null,
    params: '',
    control: null,
    treeprops: {},
    pagingShowOpt: {
      summary: true,
      recordOfPage: true
    },
    ...BaseWidgetDefaultParams()
  };
}
export default class TablePaging extends BaseWidget {
  /** @type {String} - tiêu đề bảng */
  title;
  /** @type {String} - xxx*/
  emptyText;
  /** @type {String} - key của row */
  keyId;
  /** @type {Boolean} - hidden all control */
  disableControl;
  /** @type {Boolean} - hidden filter */
  noFilter;
  /** @type {{
   *  summary: boolean,
   *  recordOfPage: boolean,
   * }} - hidden filter */
  pagingShowOpt;
  /** @type {Boolean} - disable fixheader */
  disableFixedHead;
  /** @type {Boolean} - disable paging */
  disablePaging;
  /** @type {Boolean} - Show header */
  showHeader;
  /** @type {Boolean} - Show header */
  expandAll;
  /** @type {Boolean} - Show header */
  isSearchEqual;
  /** @type {Boolean} - disable event select row */
  disableSelectRow;
  /** @type {Function} - Function custom summary method	Function({ columns, data }) */
  handleSumary;
  /** @type {({ row, column, rowIndex, columnIndex })=> [rows: number,columns: number]} - Function call khi muốn gộp cột= Function({ row, column, rowIndex, columnIndex }){ return [rs,cs] || {rowspan=xx,colspan=xx}} */
  spanMethod;
  /** @type {(row, column, event) => void)} - Function call khi click row when disableSelectRow != true= function(rowData){} */
  clickRow;

  /** @type {Function} - Function call khi click row when disableSelectRow != true= function(rowData){} */
  dbClickRow;
  /** @type {Function} - Function call trước khi xử lý data (dùng để custom lại data)= function(data){} */
  LoadDataSuccess;
  /** @type {Function} - Function call sau khi table rendered = function(data){} */
  Loaded;
  /** @type {Function} - Function validate params trước khi call API= function(params){  return (True= Success || False= Fail) }    */
  Validate;
  /** @type {Array<TablePagingCol>} - Mảng định nghĩa các cột của Table */
  cols;
  /** @type {any} - kiểu dữ liệu [String= link API] || [Array= Mảng object] || [Object= Object phân trang { CurentPage= 0, Data= [], RecordOfPage= 50, TotalPage= 10 }] */
  data;
  /** @type {Object} - Tham số */
  params;
  /** @type {Function} - control table function(model){return new FormContainer(...} */
  control;
  /** @type {(rows: [])=>Array<{label:string,value: string}>} - desc */
  sumary;
  /** @type {(row: *, rowIndex:  number)=> string - desc */
  tableRowClassName;

  /**
   * @param {TablePaging} obj - description
  */
  constructor(obj) {
    super(obj);
    Object.assign(this, defaultParams(), obj);
  }
}
