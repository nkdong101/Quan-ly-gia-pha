import moment from "moment";
import ConvertStr from "./ConvertStr";


import createNumberMask from 'text-mask-addons/dist/createNumberMask'

// First, you need to create the `numberMask` with your desired configurations
/**
 *  prefix (string): what to display before the amount. Defaults to '$'.
    suffix (string): what to display after the amount. Defaults to empty string.
    includeThousandsSeparator (boolean): whether or not to separate thousands. Defaults to to true.
    thousandsSeparatorSymbol (string): character with which to separate thousands. Default to ','.
    allowDecimal (boolean): whether or not to allow the user to enter a fraction with the amount. Default to false.
    decimalSymbol (string): character that will act as a decimal point. Defaults to '.'
    decimalLimit (number): how many digits to allow after the decimal. Defaults to 2
    integerLimit (number): limit the length of the integer number. Defaults to null for unlimited
    requireDecimal (boolean): whether or not to always include a decimal point and placeholder for decimal digits after the integer. Defaults to false.
    allowNegative (boolean): whether or not to allow negative numbers. Defaults to false
    allowLeadingZeroes (boolean): whether or not to allow leading zeroes. Defaults to false
 */
const numberMask = createNumberMask({
  prefix: '',
  suffix: '', // This will put the dollar sign at the end, with a space.

})
import createAutoCorrectedDatePipe from "./base/TextMaskDate";
var momentFormat = ['DD', 'MM', 'YYYY', 'HH', 'mm', 'ss'];
var textMaskFormat = ['dd', 'mm', 'yyyy', 'HH', 'MM', 'SS'];

export default {
  timeMask: {
    errorText: "Sai định dạng thời gian",
    placeholder: "hh:mm:ss",
    mask: function (value) {
      const chars = value.split("");
      const hours = [/[0-2]/, chars[0] == "2" ? /[0-3]/ : /[0-9]/];

      const minutes = [/[0-5]/, /[0-9]/];
      const seconds = [/[0-5]/, /[0-9]/];

      return hours
        .concat(":")
        .concat(minutes)
        .concat(":")
        .concat(seconds);
    }
  },

  dateMaskOld: {
    errorText: "Sai định dạng thời gian",
    placeholder: "DD/MM/YYYY",
    mask: function (value) {
      const chars = value.split("");
      var DD = chars.slice(0, 2);
      var MM = chars.slice(3, 5);
      var YYYY = chars.slice(6, 10);

      const days = [
        /[0-3]/,
        chars[0] == "0" ? /[1-9]/ : chars[0] == "3" ? /[0-1]/ : /[0-9]/
      ];

      const months = validateMonth(DD, MM);
      const year = validateYear(DD, MM, YYYY);

      return days
        .concat("/")
        .concat(months)
        .concat("/")
        .concat(year);
    }
  },

  yearMask: {
    errorText: "Sai định dạng thời gian",
    placeholder: "YYYY",
    checkFormat(value) {
      var d = moment(value, this.placeholder);
      return d.isValid();
    },
    mask: function (value) {
      return [/[0-9]/, /[0-9]/, /[0-9]/, /[0-9]/]
    },
    pipe: createAutoCorrectedDatePipe("yyyy", { minYear: 1900, maxYear: 2099 }),
    keepCharPositions: true
  },

  dateMask: {
    errorText: "Sai định dạng thời gian",
    placeholder: "DD/MM/YYYY",
    checkFormat(value) {
      var d = moment(value, this.placeholder);
      return d.isValid();
    },
    mask: function (value) {
      return [/[0-9]/, /[0-9]/, "/", /[0-9]/, /[0-9]/, "/", /[0-9]/, /[0-9]/, /[0-9]/, /[0-9]/]
    },
    handleEnter(value, callback) {
      if (value.indexOf("_") > -1) {
        let v = value.replace(/_/g, '0');
        if (v.substr(3) == "00/0000") {
          v = v.replace('0000', new Date().getFullYear())
          v = v.replace('00', ConvertStr.ToDateStr(new Date(), 'MM'))
        } else if (v.substr(6) == "0000") {
          v = v.replace('0000', new Date().getFullYear())
        }
        callback(v);
      }
    },
    pipe: createAutoCorrectedDatePipe("dd/mm/yyyy"),
    keepCharPositions: true
  },
  dateTimeMask: {
    errorText: "Sai định dạng thời gian",
    placeholder: "DD/MM/YYYY HH:mm",
    checkFormat(value) {
      var d = moment(value, this.placeholder);
      return d.isValid();
    },
    handleEnter(value, callback) {
      if (value.indexOf("_") > -1) {
        let v = value.replace(/_/g, '0');
        if (v.substr(11, 5) == "00:00") {
          var d = moment(v, this.placeholder);
          d.hour(new Date().getHours());
          d.minute(new Date().getMinutes());
          v = ConvertStr.ToDateStr(d, this.placeholder);
        }
        callback(v);
      }
    },
    mask: function (value) {
      return [/[0-9]/, /[0-9]/, "/", /[0-9]/, /[0-9]/, "/", /[0-9]/, /[0-9]/, /[0-9]/, /[0-9]/, ' ', /[0-9]/, /[0-9]/, ":", /[0-9]/, /[0-9]/]
      // return ['##/##/####','##/##/#### ##:##']
    },
    pipe: createAutoCorrectedDatePipe("dd/mm/yyyy HH:MM"),
    keepCharPositions: true
  },
  numberMask: {
    errorText: "Sai định dạng số",
    placeholder: "--",
    mask: numberMask
  },





};

function validateMonth(dd, mm) {
  var vld = [];
  var ddValue = +dd.join("");
  //thang 31 ngay : 01,03,05,07,08,10,12
  //thang 30 ngay : 04,06,09,11
  //thang 28-28 ngay : 02
  vld[0] = /[0-1]/;
  if (ddValue == 31) {
    vld[1] = mm[0] == "0" ? /[13578]/ : /[02]/;
  } else if (ddValue == 30) {
    vld[1] = mm[0] == "0" ? /[13456789]/ : /[0-2]/;
  } else {
    vld[1] = mm[0] == "0" ? /[1-9]/ : /[0-2]/;
  }

  return vld;
}

function validateYear(dd, mm, yyyy) {
  var vld = [/[1-2]/, yyyy[0] == "2" ? /[0-9]/ : /[8-9]/, /[0-9]/];
  // if (["0", "1", "2"].indexOf(yyyy[0]) > -1 && yyyy[1] == '_') {
  //   vld = [2, 0, yyyy[0], /0-9/];
  // }

  var ddValue = +dd.join("");
  var mmVal = +mm.join("");

  //nhung so chia het cho 4
  /*
  12,16,
  32,36,
  52,56,
  72,76,
  92,96
  00,04,08,
  20,24,28,
  40,44,48,
  60,64,68,
  80,84,88,
  */
  if (ddValue == 29 && mmVal == 2) {
    vld[3] =
      ["0", "2", "4", "6", "8"].indexOf(yyyy[2]) > -1 ? /[048]/ : /[26]/;
  } else vld[3] = /[0-9]/;

  return vld;
}
