import ElementUI from "element-ui";
import moment from "moment";
import ConvertStr from "./ConvertStr";

export var Global = {
  counter: 0,
};
export function validateEmail(email) {
  return String(email)
    .toLowerCase()
    .match(
      /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/
    );
};


export function GetGlobalId() {
  Global.counter++;
  return Global.counter;
}

if (location.host ==
  'procurement.wwf.org.vn') {
  console.log = () => { };
  console.error = () => { };
  console.warn = () => { };
}

export function SearchTree(obj, query, tag) {
  for (var key in obj) {
    var value = obj[key];

    if (typeof value === "object") {
      var rs = SearchTree(value, query, tag);
      if (rs != null) return rs;
    } else {
      if (key == tag)
        if (obj[key] === query) {
          return obj;
        }
    }
  }
  return null;
}
export function SearchAllTree(array, query, child) {
  let result = [];
  array.forEach(p => {
    if (query(p))
      result.push(p);
    else if (p[child] && p[child].length) {
      let find = SearchAllTree(p[child], query, child);
      if (find.length) {
        result.push(p);
        result.push(...find);
      }
    }
  });
  // for (var key in obj) {
  //   var value = obj[key];

  //   if (typeof value === "object") {
  //     var rs = SearchAllTree(value, query, tag);
  //     if (rs != null && rs.length) {
  //       result.push(value);
  //       result.push(...rs);
  //     }
  //   } else {
  //     if (key == tag)
  //       if (obj[key] === query) {
  //         result.push(obj);
  //       }
  //   }
  // }

  return result;
}
Number.prototype.formatMoney = function (c, d, t) {
  var n = this,
    c = isNaN((c = Math.abs(c))) ? 0 : c,
    d = d == undefined ? "." : d,
    t = t == undefined ? "," : t,
    s = n < 0 ? "-" : "",
    i = String(parseInt((n = Math.abs(Number(n) || 0).toFixed(c)))),
    j = (j = i.length) > 3 ? j % 3 : 0;
  var rs =
    s +
    (j ? i.substr(0, j) + t : "") +
    i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + t) +
    (c ?
      d +
      Math.abs(n - i)
        .toFixed(c)
        .slice(2) :
      "");
  return rs == 0 ? "" : rs;
};
Array.prototype.sortBy = function () {
  function _sortByAttr(attr) {
    var sortOrder = 1;
    if (attr[0] == "-") {
      sortOrder = -1;
      attr = attr.substr(1);
    }
    return function (a, b) {
      var result = a[attr] < b[attr] ? -1 : a[attr] > b[attr] ? 1 : 0;
      return result * sortOrder;
    };
  }

  function _getSortFunc() {
    if (arguments.length == 0) {
    }
    var args = arguments;
    return function (a, b) {
      for (var result = 0, i = 0; result == 0 && i < args.length; i++) {
        result = _sortByAttr(args[i])(a, b);
      }
      return result;
    };
  }
  return this.sort(_getSortFunc.apply(null, arguments));
};
export function DateFormated(format, date) {
  format = format || "DD/MM/YYYY";
  if (!date) return "";
  date = date || new Date();
  if (moment(date).year() < 1900) return "";
  return moment(date).format(format);
}




  


export function ShowAlert({
  message = "Content",
  title = "Alert",
  type = MessageType.info,
  opt = {
    dangerouslyUseHTMLString,
    confirmButtonText
  }
}) {
  return ElementUI.MessageBox.alert(message, title, {
    ...opt,
    type
  });
}

export const MessageType = {
  info: "info",
  error: "error",
  success: "success",
  warning: "warning"
};
let dangerouslyUseHTMLString = true;
let confirmButtonText = "OK";
let cancelButtonText = "Cancel";

/**
 * @param {string} message - description
 * @param {info|error|success|warning} type - description
*/
export function ShowMessage(
  message = "alert",
  type = MessageType.success,
  opt = {
    dangerouslyUseHTMLString
  }
) {
  console.error(message)

  ElementUI.Message({
    showClose: true,
    duration: 3000,
    offset: 45,
    message,
    type: type,
    dangerouslyUseHTMLString: true,
    ...opt
  });
}
/**
 * @param {Object} param - description
 * @param {string} param.message - description
 * @param {string} param.title - description
 * @param {'info'|'error'|'success'|'warning'}  param.type - description
 * @param {Object}  param.opt - description
 * @param {string}  param.opt.confirmButtonText - description
 * @param {string}  param.opt.cancelButtonText - description
 * @param {boolean}  param.opt.dangerouslyUseHTMLString - description
 * @return {Promise} description
*/
export function ShowConfirm({
  message = "Are your sure?",
  title = "Confirm",
  type = MessageType.info,
  opt = {
    confirmButtonText,
    cancelButtonText,
    dangerouslyUseHTMLString
  }
}) {
  return ElementUI.MessageBox.confirm(message, title, {
    ...opt,
    type
  });
}





//#region ExportExcel


//Export excel
export function Workbook() {
  if (!(this instanceof Workbook)) return new Workbook();
  this.SheetNames = [];
  this.Sheets = {};
}

export function s2ab(s) {
  var buf = new ArrayBuffer(s.length);
  var view = new Uint8Array(buf);
  for (var i = 0; i != s.length; ++i) view[i] = s.charCodeAt(i) & 0xFF;
  return buf;
}


export function getAvailableCell(cell, listChecker) {
  if (!cell)
    return { r: 0, c: 0 };
  listChecker = listChecker || [];
  var checker = listChecker;

  var maxC = (checker.filter(function (p) { return p.s.r == 0 }).sort(function (a, b) { return b.e.c - a.e.c })[0] || { e: { r: 0, c: 0 } }).e.c;
  var findR = checker.filter(function (p) {
    return (p.s.r <= cell.r && cell.r <= p.e.r) && (p.s.c <= cell.c && cell.c <= p.e.c);
  }).length;
  if (findR) {
    cell.c += 1;
    if (cell.r == 0 || cell.c <= maxC) {
      return getAvailableCell(cell, checker);
    } else {
      cell.r += 1;
      cell.c = 0;
      return getAvailableCell(cell, checker);
    }
  } else {
    return cell;
  }
}

export function concatDeep(arr, d = 1) {
  return d > 0 ? arr.reduce((acc, val) => acc.concat(Array.isArray(val) ? concatDeep(val, d - 1) : val), [])
    : arr.slice();

}



export function datenum(v, date1904) {
  if (date1904) v += 1462;
  var epoch = Date.parse(v);
  return (epoch - new Date(Date.UTC(1899, 11, 30))) / (24 * 60 * 60 * 1000);
}



export function getHexColor(colorStr) {
  var a = document.createElement('div');
  a.style.color = colorStr;
  var colors = window.getComputedStyle(document.body.appendChild(a)).color.match(/\d+/g).map(function (a) { return parseInt(a, 10); });
  document.body.removeChild(a);
  return (colors.length >= 3) ? '#' + (((1 << 24) + (colors[0] << 16) + (colors[1] << 8) + colors[2]).toString(16).substr(1)) : false;
}


export function Uni2None(str = "") {
  str = (str || '') + ''
  str = str.replace(/(à|á|ạ|ả|ã|â|ầ|ấ|ậ|ẩ|ẫ|ă|ằ|ắ|ặ|ẳ|ẵ)/g, "a");
  str = str.replace(/(è|é|ẹ|ẻ|ẽ|ê|ề|ế|ệ|ể|ễ)/g, "e");
  str = str.replace(/(ì|í|ị|ỉ|ĩ)/g, "i");
  str = str.replace(/(ò|ó|ọ|ỏ|õ|ô|ồ|ố|ộ|ổ|ỗ|ơ|ờ|ớ|ợ|ở|ỡ)/g, "o");
  str = str.replace(/(ù|ú|ụ|ủ|ũ|ư|ừ|ứ|ự|ử|ữ)/g, "u");
  str = str.replace(/(ỳ|ý|ỵ|ỷ|ỹ)/g, "y");
  str = str.replace(/(đ)/g, "d");
  str = str.replace(/(À|Á|Ạ|Ả|Ã|Â|Ầ|Ấ|Ậ|Ẩ|Ẫ|Ă|Ằ|Ắ|Ặ|Ẳ|Ẵ)/g, "A");
  str = str.replace(/(È|É|Ẹ|Ẻ|Ẽ|Ê|Ề|Ế|Ệ|Ể|Ễ)/g, "E");
  str = str.replace(/(Ì|Í|Ị|Ỉ|Ĩ)/g, "I");
  str = str.replace(/(Ò|Ó|Ọ|Ỏ|Õ|Ô|Ồ|Ố|Ộ|Ổ|Ỗ|Ơ|Ờ|Ớ|Ợ|Ở|Ỡ)/g, "O");
  str = str.replace(/(Ù|Ú|Ụ|Ủ|Ũ|Ư|Ừ|Ứ|Ự|Ử|Ữ)/g, "U");
  str = str.replace(/(Ỳ|Ý|Ỵ|Ỷ|Ỹ)/g, "Y");
  str = str.replace(/(Đ)/g, "D");
  str = str.replace(/[^A-Za-z0-9\-\s]/g, "");
  str = str.replace(/\s+/g, " ");
  // str = str.replace(/\s/g, "-");
  return str;
}
export function getRange(sC, sR, eC, eR) {
  return { s: { c: sC, r: sR }, e: { c: eC, r: eR } };
}

export function writeCell(ws, c, r, value) {
  let cell = value;
  let cellRef = XLSX.utils.encode_cell({ c: c, r: r });
  ws[cellRef] = cell;
}

export function sheet_from_array_of_arrays(data, cols, title) {
  var ws = {};
  let tmpStyle = {
    alignment: {
      wrapText: true
    },
    border: {
      top: { style: "thin", color: { auto: 1 } },
      bottom: { style: "thin", color: { auto: 1 } },
      left: { style: "thin", color: { auto: 1 } },
      right: { style: "thin", color: { auto: 1 } }
    },
    fill: {
      fgColor: { rgb: "ffdddddd" }
    },
    font: {
      bold: true
    }
  };
  let rowStyle = {
    alignment: {
      wrapText: true
    },
    border: {
      top: { style: "thin", color: { auto: 1 } },
      bottom: { style: "thin", color: { auto: 1 } },
      left: { style: "thin", color: { auto: 1 } },
      right: { style: "thin", color: { auto: 1 } }
    },
  };
  let wBorder = {
    border: {
      top: { style: "thin", color: { auto: 1 } },
      bottom: { style: "thin", color: { auto: 1 } },
      left: { style: "thin", color: { auto: 1 } },
      right: { style: "thin", color: { auto: 1 } }
    }
  };
  ws["!merges"] = [];
  let header = concatDeep(cols, Infinity)
    .filter((p) => p.data)
    .sortBy("index");
  ws["!cols"] = header.map(function (p) {
    return { wpx: p.min_width + 5 };
  });

  //Header
  var startC = 0;
  var startR = 1;

  let maxC = header.length;
  let maxR = startR;

  ws["!merges"].push(getRange(0, 0, maxC - 1, 0));
  writeCell(ws, 0, 0, {
    v: `${title || ''}`, t: "s", s: {
      alignment: { horizontal: "center" },
      border: {
        top: { style: "thin", color: { auto: 1 } },
        bottom: { style: "thin", color: { auto: 1 } },
        left: { style: "thin", color: { auto: 1 } },
        right: { style: "thin", color: { auto: 1 } }
      },
      fill: {
        fgColor: { rgb: "ffdddddd" }
      },
      font: {
        bold: true,
        color: 'ff3c6f9c'
      }
    }
  });

  cols.forEach(function (pR, C) {
    startC = 0;
    pR.forEach(function (p, index) {
      let curentCell = getAvailableCell({ c: startC, r: startR }, ws["!merges"]);
      if (p.title || p.data || p.col_span > 1) {

        if (p.row_span > 1 || p.col_span > 1) {
          ws["!merges"].push(getRange(curentCell.c, curentCell.r, curentCell.c + (p.col_span || 1) - 1, curentCell.r + (p.row_span || 1) - 1));
        }
        writeCell(ws, curentCell.c, curentCell.r, { v: `${p.title || ''}`, t: "s", s: tmpStyle });
        startC = curentCell.c + 1;
      }
    });
    startR += 1;
    maxR = startR;
  });


  data.forEach(function (pData, index) {
    startC = 0;
    header.forEach(function (p, C) {
      resolve(pData, p.data);
      let value = resolve(pData, p.data);
      if (!Array.isArray(value) && value !== null)
        value = value === 0 ? '0' : (value || '') + '';



      if (p.exporter) {
        value = p.exporter(value, pData);
      } else if (p.formatter && typeof p.formatter == 'function') {
        value = p.formatter(value, pData);
      }

      let type = 's';
      if (p.formatter && typeof p.formatter == 'string') {
        switch (p.formatter) {
          case 'number':
            type = 'n';
            break;
          case 'date':
            value = ConvertStr.ToDateStr(value);
            break;
          case 'datetime':
            value = ConvertStr.ToDateStr(value, 'DD/MM/YYYY HH:mm');
            break;
          case 'datetimes':
            value = ConvertStr.ToDateStr(value, 'DD/MM/YYYY HH:mm:ss');
            break;
          default:
            break;
        }
      }

      writeCell(ws, startC, startR, {
        v: `${value || ''}`, t: type, s: {
          ...rowStyle,
          fill: {
            fgColor: { rgb: index % 2 ? "ffe2ecf8" : 'ffffffff' }
          },
        }
      });
      startC += 1;
    });
    startR += 1;
  });


  maxR = startR;
  ws["!merges"].forEach(function (p) {
    for (let i = p.s.c; i <= p.e.c; i++) {
      for (let j = p.s.r; j <= p.e.r; j++) {
        if (!(i == p.s.c && j == p.s.r))
          writeCell(ws, i, j, {
            v: "",
            t: "s",
            s: wBorder
          });
      }
    }
  });
  ws["!ref"] = XLSX.utils.encode_range({ s: { c: 0, r: 0 }, e: { c: maxC, r: maxR } });
  // console.log(ws);
  return ws;
}

//export Excel
//#endregion



/**
 * path: string cách nhau dấu "."
 * object: đối tượng cần lấy value
 * vd:
 *      var obj = { a : { b : 1 } };
 *      resolve(obj,"a") return { b : 1 }
 *      resolve(obj,"a.b") return  1
 *
 */
export function resolve(obj, path = '') {
  let value = path.split('.').reduce(function (prev, curr) {
    return prev ? prev[curr] : null
  }, obj || self)
  return value
}
/**
 * obj: đối tượng cần set
 * path: string cách nhau dấu ".""
 * value: giá trị cần set
 */
export function resolveSet(obj, path, value) {
  path = path.split('.');
  if (path.length === 1) {
    obj[path] = value
    return
  }
  return resolveSet(obj[path[0]], path.slice(1).join('.'), value)
}

export function Compare(valueA = "", valueB = "") {
  let result = 0;
  //   if(!valueA || !valueB){
  //       //console.log(valueA,valueB);
  //   }
  if (!isNaN(+valueA) && typeof (+valueA) == 'number' && !isNaN(+valueB) && typeof (+valueB) == 'number') {
    //number
    result = +valueA - +valueB;
  } else if (
    IsValidDate(valueA) && IsValidDate(valueB)
  ) {
    //date
    result = moment(valueA).isAfter(moment(valueB));
  }
  //   if (typeof valueA == "string" && typeof valueB == "string")
  else {
    //string
    result = (valueA + "")
      .toLowerCase()
      .localeCompare((valueB + "").toLowerCase());
  }

  //   else {
  //     if (valueA > valueA) return 1;
  //     else if (valueA == valueA) return 0;
  //     else return -1;
  //   }
  //   if ([1, -1, 0].indexOf(result) < 0) //console.log(result, valueA, valueB);

  return result;
}


export function GenrerateArray(length = 0, sampleData = 0) {
  var a = [];
  for (let i = 0; i < length; i++) {
    if (sampleData) {
      var obj = JSON.parse(JSON.stringify(sampleData));
      obj.id = i;
      a.push(obj);
    }
    else
      a.push(i);
  }
  return a;
}


export function GetTimeNow() {
  return moment()._d.toJSON();
}
export function GetStartMonth(date) {
  date = date || new Date();
  var firstDay = new Date(date.getFullYear(), date.getMonth(), 1);
  return firstDay.toJSON();
}
export function GetEndMonth(date) {
  date = date || new Date();
  let startMonth = new Date(GetStartMonth(date));
  var nextMonth = new Date(startMonth.getFullYear(), startMonth.getMonth() + 1, 1);
  nextMonth.setSeconds(-1);
  return nextMonth.toJSON();
}

export function GetTime(iday, isEndDay) {
  isEndDay = isEndDay || false;
  iday = iday || 0;
  var d = moment().subtract(-iday, "days");
  if (isEndDay)
    d = d
      .hours(23)
      .minute(59)
      .second(59);
  else
    d = d
      .hours(0)
      .minute(0)
      .second(0);
  var dd = d._d;
  return dd.toJSON();
}

export function GetValidDate(str) {
  if (!str) return GetTimeNow();
  if (moment(str).year() > 1900) return str;
  return GetTimeNow();
}

export function GetBoolean(str) {
  if (str === true) return true;
  if (str === false) return false;
  return null;
}

/**
 * @return {'after'|'equal'|'before'} description
 */
export function CompareDate(source, target) {
  if (source == null && target == null)
    return 'equal';
  if (source == null)
    return 'before';
  if (target == null)
    return 'after';

  var source_n = +ConvertStr.ToDateStr(source, 'YYYYMMDD');
  var target_n = +ConvertStr.ToDateStr(target, 'YYYYMMDD');
  if (source_n == target_n)
    return 'equal';
  if (source_n > target_n)
    return 'after'
  return 'before';
}
export function IsAfterDate(source, target) {
  return CompareDate(source, target) == 'after';
}
export function IsEqualDate(source, target) {
  return CompareDate(source, target) == 'equal';
}
export function IsBeforeDate(source, target) {
  return CompareDate(source, target) == 'before';
}
export function IsNullDate(source) {
  return !IsAfterDate(source, new Date(1900, 1, 1));
}
export function IsValidDate(source) {

  source = source || '';
  let check = (source + '').substring(0, 10);
  return moment(check, 'YYYY-MM-DD', true).isValid();
}



const axios = require("axios");

export function uploadFile(formData, opts = { folder: '' }) {
  return new Promise(rs => {
    axios.post('/API/SaveFileToServer', formData, {
      headers: {
        'Content-Type': 'multipart/form-data',
        folder: opts.folder
      }
    }).then(re => {
      try {
        rs(re.data.Data);
      } catch (error) {
        ShowMessage('Đã có lỗi sảy ra khi upload');
      }
    })
  });
}




export function objContainStr(str, text, isEqual) {
  text = text.toLowerCase().split(' ');
  // console.log(str, text)
  // console.log(isEqual)
  return text.every(el => {
    let check = false;
    if (isEqual)
      check = str.replaceAll('n',' ').toLowerCase().split(' ').some(p => p == el);
    else
      check = str.toLowerCase().includes(el);
    return check;
  });
}

export function getFileName(str) {
  str = (str || '') + '';
  let splt = str.split("|");
  return splt[0].replace(/\?/g, ",");
}
export function getNameDownload(str) {
  str = (str || '') + '';
  let splt = str.split("|");
  return splt[1].replace(/\?/g, ",");
}
/**
 * @return {{
 *  file: string,
 *  fileDownload: string
 * }} description
 */
export function getFileObj(str) {
  str = ((str || '') + '').replace(/\?/g, ",");
  let splt = str.split("|");
  function GetFileName(str) {
    var splt = str.split("/");
    return splt[splt.length - 1];
  }
  return {
    file: splt[0],
    fileDownload: GetFileName(splt[1] || splt[0])
  }
}


export function readFileInput(file) {
  return new Promise(rs => {
    const reader = new FileReader();
    reader.addEventListener('load', (event) => {
      rs(event.target.result)
    });
    reader.readAsDataURL(file);
  });
}


export function dynamicColors() {
  var r = Math.floor(Math.random() * 255);
  var g = Math.floor(Math.random() * 255);
  var b = Math.floor(Math.random() * 255);
  return "rgba(" + r + "," + g + "," + b + ", 0.5)";
}
