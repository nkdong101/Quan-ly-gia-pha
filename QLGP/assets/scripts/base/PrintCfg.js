import TablePaging from "./TablePaging";




export default class PrintCfg {
    /** @type {TablePaging} - bảng */
    tp;
    /**@type {string} - Tiêu đề */
    title;
    /** @type {'landscape'|'portrait'} - description */
    layout = 'landscape';


    /**
     *  
     * @param {PrintCfg} obj 
     */
    constructor(obj) {
        Object.assign(this, obj);
    }



}


