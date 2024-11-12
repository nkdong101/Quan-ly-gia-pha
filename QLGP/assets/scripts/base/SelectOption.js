


export class SelectOption {
  data = null;
  labelFormat = null;
  selectFirst = false;
  label = null;
  value = "Id";
  key = "";
  placeholder = "";
  params = {};
  multiple = false;
  allowCreate = false;
  remote = false;
  remoteValue = [];
  dataExt = [];
  /**
   * @type {(any) => bool} type - description
  */
  IsItemDisabled;
  /**
   * @param {SelectOption} obj - description
  */
  constructor(obj) {
    Object.assign(this, obj)

  }

  // getName(id) {
  //   if (Array.isArray(this.data)) {
  //     var find = (this.data.filter(p => p[this.value] == id)[0]);
  //     if (find)
  //       return this.labelFormat ? this.labelFormat(find) : find[this.label || 'Name'];
  //     return '';
  //   }
  //   return '';

  // }
  getValue(item) {
    return this.labelFormat ? this.labelFormat(item) : item[this.label || 'Name']
  }
  getName(id) {
    let data = [].concat(this.data, this.dataExt)
    if (!this.multiple) {
      if (Array.isArray(data)) {
        var find = (data.filter(p => p[this.value] == id)[0]);
        if (find)
          return this.getValue(find);
        return '';
      }
      return '';
    } else {
      if (typeof id == 'string') {
        let splt = id.split(',');
        return data.filter(p => splt.some(p1 => p1 == p[this.value])).map(p => this.getValue(p));
      } else if (Array.isArray(id)) {
        return data.filter(p => id.some(p1 => p1 == p[this.value])).map(p => this.getValue(p));
      }
      return [];
    }

  }

  /**
   * @param {(obj:SelectOption)=>void} cb - description
  */
  set(cb) {
    let newSet = new SelectOption(this);
    cb(newSet);

    return newSet;

  }

}
