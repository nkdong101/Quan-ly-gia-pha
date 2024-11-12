// @ts-check
import BaseWidget, {
  BaseWidgetDefaultParams
} from "./BaseWidget";
/**
    *@return { TablePagingCol  } Test
    */
var defaultParams = function () {
  // @ts-ignore
  return {
    // type: '',
    width: '',
    Index: 0,
    min_width: 150,
    children: [],
    sortable: true,
    data: '',
    title: '',
    fix: undefined,
    align: 'left',
    formatter: null,
    exporter: null,
    ...BaseWidgetDefaultParams()
  };
}
export default class TablePagingCol extends BaseWidget {
  /** @type {String} - selection/index/expand*/
  type;
  /** @type {any} - Độ rộng cột default: auto*/
  width;
  /** @type {Number} - index cột (không trùng) */
  Index;
  /** @type {Number} - Độ rộng tối thiểu */
  min_width;
  /** @type {Array<TablePagingCol>} - Cột child */
  children;
  /** @type {Boolean} - Cho phép cột có thể sort default: true */
  sortable;
  /** @type {String} - trường lấy dữ liệu */
  data;
  /** @type {String} - Nhãn cột */
  title;
  /** @type {String} - Nhãn cột */
  headerClass;
  /** @type {undefined|'left'|'right'} - fix cột cố định (left || right)*/
  fix;
  /** @type {'right'|'left'|'center'} - align */
  align;
  /**
   * @type {((value: *,row: Object,column)=>*)|'number'|'date'|'datetime'|'time'|'datetimes'|'times'} - kiểu dữ liệu của cột
   *
   * (value,row)=> custom_data: function(value: giá trị cột,row: bản ghi hiện tại){ return 'Dữ liệu cần'}
   *
   * number: Kiểu số
   *
   * date: Datetime trả về DD/MM/YYYY
   *
   * datetime:  Datetime trả về DD/MM/YYYY HH:mm
   *
   * time:  Datetime trả về HH:mm
   *
   * datetimes: (thêm second) Datetime trả về DD/MM/YYYY HH:mm:ss
   *
   * times: (thêm second) Datetime trả về HH:mm:ss
   */
  formatter;
  /**
   * @type {(value: *,row: Object,column)=>*} type - description
  */
  exporter;
  constructor(obj = defaultParams()) {
    // @ts-ignore
    super(obj);
    Object.assign(this, defaultParams(), obj);
    if (!this.Index)
      this.Index = +this.id;
  }

  GetAllChild() {
    var a = [];
    if (this.data)
      a.push(this.data);
    if (this.children && this.children.length) {
      this.children.forEach(p => {
        a = a.concat(p.GetAllChild());
      });
    }
    // console.log(a);
    return a;
  }
}
