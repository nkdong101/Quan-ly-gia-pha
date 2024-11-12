import {
  GetGlobalId,
  Global
} from "../Functions";


export var BaseWidgetDefaultParams = function () {
  return {
    id: new Date().getTime() + GetGlobalId() + "",
    tag: '',
    class: '',
    style: '',
    attr: {},
    events: {}
  };
}
export default class BaseWidget {

  id;
  class;
  style;
  tag;
  attr;
  events;
  constructor(obj = BaseWidgetDefaultParams()) {
    Object.assign(this, BaseWidgetDefaultParams(), obj)
    this.id = obj.id || ('el_' + new Date().getTime() + "_" + Global.counter++);
    this.attr = this.attr || {};
    this.events = this.events || {};
  }

  genContructor() {
    let data = Object.keys(this).filter(p => Object.keys(BaseWidgetDefaultParams()).indexOf(p) == -1);
    let prop = data.map(p => `/** @type {String} - ${p} */            \n${p};`).join('\n');
    let cons = data.map(p => `this.${p} = obj.${p};`).join('\n');
    let params = data.map(p => `${p} : '',`).join('\n');
    let str = `
    // @ts-check
    import BaseWidget, { BaseWidgetDefaultParams } from "./BaseWidget";
    var defaultParams = function(){
        return {
          ${params}
          ...BaseWidgetDefaultParams()
        };

    }
    export default class extends BaseWidget {
    ${prop}
    constructor(obj = defaultParams()) {
        super(obj);
        Object.assign(this, defaultParams(), obj);
    }
    }
    `
    // //console.log(str);
  }

}


export const BaseWidgetType = {
  DefaultForm: 'DefaultForm',
}
