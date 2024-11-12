import { GetTime } from "../Functions";

var DefaultParams = function () {
    return {
        shortcuts: [{
            text: 'Hôm nay',
            onClick(picker) {
                picker.$emit('pick', GetTime());
            }
        }],
        disabledDate: function (time = new Date()) {
            return false;
        },
        cellClassName: function (time = new Date()) {
            return '';
        },
        firstDayOfWeek: 1,
        onPick: function (maxDate = new Date(), minDate = new Date()) {
            return '';
        },
    };
}
export default class DatePickerOption {

    shortcuts;
    disabledDate;
    cellClassName;
    firstDayOfWeek;
    onPick;
    constructor(obj = DefaultParams()) {
        Object.assign(this, DefaultParams(), obj)
    }


}
