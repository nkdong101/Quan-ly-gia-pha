import API from "../API";
import GetDataAPI from "../GetDataAPI";

export default {
  getInfor(id) {
    return new Promise((rs, rj) => {
      if (id) {
        GetDataAPI({
          url: API.Giapha + '/' + id,
          action: re => {
            rs(re);
          },
          // error: (re) => {
          //   rj({})
          // }
        })
      }

      else{
        rj({})
      }
    })
  },
  GetMarried(fid, mid) {
    return new Promise((rs) => {
      Promise.all([this.getInfor(fid), this.getInfor(mid)]).then((result) => {
        rs(result);
      });

    });
  }

}
