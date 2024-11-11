<template>
  <div v-if="disableState"
       style="display: flex; align-items: baseline; margin-bottom: 10px">
    <div style="flex: 0 0 30px">{{ index + 1 }}.</div>
    <div style="margin-top: 0px">
      <b>{{ Item.Emp_FullName }}</b>
      <p>{{ Item.Emp_Title }}</p>
    </div>
  </div>
  <div v-else-if="Item.Type_id != 3 && Item.Type_id != 4 && Item.Type_id != 6">
    <div v-if="!btns_rejectvoid.length"
         style="display: flex; align-items: flex-end">
      <div style="flex: 0 0 30px">{{ index + 1 }}.</div>
      <div style="margin-top: 0px"
           v-if="!disabled">
        <el-button type="danger"
                   @click="NoneCheckbox()">
          <i class="el-icon-delete"></i>
        </el-button>
      </div>
      <el-button-group v-if="btns.length && disabled">
        <el-button v-for="item in btns"
                   class="btn-action"
                   :key="item.index"
                   :type="item.path"
                   size="small"
                   @click="handleClick(item)"
                   :has-action="item.action">
          <i v-if="item.icon"
             :class="item.icon"
             style="margin-right: 5px"></i>
          {{ item.title }}
        </el-button>
      </el-button-group>
      <img v-if="Item.Emp_SignatureImage"
           height="80px"
           width="auto"
           style="margin-left: 10px"
           :src="getPRFileAttach(
             Para.SignImage.baseUrl + getFilePath(Item.Emp_SignatureImage),
             isPRLink
           )
             " />
    </div>
    <div v-else
         style="display: flex; align-items: flex-end">
      <div style="flex: 0 0 30px">{{ index + 1 }}.</div>
      <el-button-group v-if="btns_rejectvoid.length && disabled">
        <el-button v-for="item in btns_rejectvoid"
                   class="btn-action"
                   :key="item.index"
                   :type="item.path"
                   size="small"
                   @click="handleClick(item)"
                   :has-action="item.action">
          <i v-if="item.icon"
             :class="item.icon"
             style="margin-right: 5px"></i>
          {{ item.title }}
        </el-button>
      </el-button-group>
      <img v-if="Item.Emp_SignatureImage && (Item.State == 4 || Item.State == 5)"
           height="80px"
           width="auto"
           style="margin-left: 10px"
           :src="getPRFileAttach(
             Para.SignImage.baseUrl + getFilePath(Item.Emp_SignatureImage),
             isPRLink
           )
             " />
    </div>
    <div style="padding-left: 30px; position: relative">
      <div style="margin-top: 5px"
           class="row">
        <div class="detail-c">
          <div class="detail-c-l">
            <b>{{ Item.Emp_FullName }}</b>
            <p>{{ Item.Emp_Title }}</p>
          </div>
          <div class="detail-c-r">
            <p>{{ Item.Comment ? "Comments: " + Item.Comment : "" }}</p>
          </div>
        </div>
      </div>
      <div v-if="Item.State == 5">
        Date: {{ ConvertStr.ToDateStr(Item.DateUpdate, "DD MMM, YYYY") }}
      </div>
      <div v-else-if="(Item.Type_id != 3 && Item.Type_id != 4 && Item.DateActive) || Item.State == 4">
        Date: {{ ConvertStr.ToDateStr(Item.DateActive, "DD MMM, YYYY") }}
      </div>
      <div v-if="Item.State == 4"
           class="void-c">
        <div class="void-title">VOID</div>
        <div class="void-date">
          {{ ConvertStr.ToDateStr(Item.DateUpdate, "DD MMM, YYYY") }}
        </div>
      </div>
      <br />
    </div>

    <DefaultForm :model="form_confirm">
      <div slot="content"
           v-focusFirstOnLoad>
        <InputTextArea v-model.lazy="form_comment.comment" />
        <div style="display: flex; justify-content: end; margin-top: 10px">
          <el-button :type="form_comment.btnType"
                     @click="form_comment.action()">
            <i class="fa fa-thumbs-down"
               style="margin-right: 5px"></i>
            {{ form_comment.btnText }}
          </el-button>
          <el-button @click="form_confirm.visible = false">
            <i class="fa fa-times-circle"
               style="margin-right: 5px"></i>
            Cancel
          </el-button>
        </div>
      </div>
    </DefaultForm>
  </div>
  <div v-else>
    <div v-if="!btns_rejectvoid.length"
         style="display: flex; align-items: baseline">
      <div style="flex: 0 0 30px">{{ index + 1 }}.</div>
      <div style="margin-top: 0px">
        <b>{{ Item.Emp_FullName }}</b>
        <p>{{ Item.Emp_Title }}</p>
        {{ Item.Emp_Email }}
      </div>
    </div>
  </div>
</template>

<script>
import API from "~/assets/scripts/API";
import GetDataAPI from "~/assets/scripts/GetDataAPI";
import Purchase_Approval from "~/assets/scripts/objects/Form/Purchase_Approval";
import Purchase_info from "~/assets/scripts/objects/Form/Purchase_info";
import MenuItem from "~/assets/scripts/objects/MenuItem";
import {
  MessageType,
  ShowConfirm,
  ShowMessage,
} from "~/assets/scripts/Functions";
import DefaultForm from "~/assets/scripts/base/DefaultForm";
export default {
  props: {
    Item: Purchase_Approval,
    Purchase: Purchase_info,
    Details: {},
    disabled: {},
    noNeedEndorsed: {},
    index: {},
    disableState: {},
  },
  data() {
    return {
      all_btn: [
        new MenuItem({
          index: "approve",
          title: "Approve",
          path: "success",
          visible: () => {
            if (this.Purchase.Status == 7 || this.Purchase.Status == 8)
              return false;
            if (this.Purchase.Status == 2) {
              //Submited

              if (this.noNeedEndorsed) {
                //Nếu không cần endorsed
                if (this.Item.State == 0 && this.Item.Type_id == 2) {
                  if (this.Item.Emp_id == this.user.UserSerial) {
                    return true;
                  }
                }
              } else {
                //Nếu vẫn cần endorsed
                if (this.Item.State == 0 && this.Item.Type_id == 1) {
                  if (this.Item.Emp_id == this.user.UserSerial) {
                    return true;
                  }
                }
              }
            } else if (this.Purchase.Status == 4) {
              //Endored
              if (this.Item.State == 0 && this.Item.Type_id == 2) {
                if (this.Item.Emp_id == this.user.UserSerial) {
                  return true;
                }
              }
            }
          },
          action: () => {
            this.Approve(1);
          },
        }),
        new MenuItem({
          index: "reject",
          title: "Reject",
          path: "danger",
          visible: () => {
            if (this.Purchase.Status == 2) {
              //Submited

              if (this.noNeedEndorsed) {
                //Nếu không cần endorsed
                if (this.Item.State == 0 && this.Item.Type_id == 2) {
                  if (this.Item.Emp_id == this.user.UserSerial) {
                    return true;
                  }
                }
              } else {
                //Nếu vẫn cần endorsed
                if (this.Item.State == 0 && this.Item.Type_id == 1) {
                  if (this.Item.Emp_id == this.user.UserSerial) {
                    return true;
                  }
                }
              }
            } else if (this.Purchase.Status == 4) {
              //Endored
              if (this.Item.State == 0 && this.Item.Type_id == 2) {
                if (this.Item.Emp_id == this.user.UserSerial) {
                  return true;
                }
              }
            }
          },
          action: () => {
            this.Reject();
          },
        }),
        new MenuItem({
          index: "wait",
          icon: "fa fa-refresh",
          title: "Waiting",
          path: "default",
          visible: () => {
            if (this.Item.State == 0 && this.Item.Type_id != 3 && this.Item.Type_id != 4) {
              if (
                this.Item.Emp_id != this.user.UserSerial ||
                (this.Item.Type_id == 2 && // Loại approved là Approve
                  this.Purchase.Status != 4 && //Trường hợp hồ sơ chưa Endorsed
                  !(this.noNeedEndorsed && this.Purchase.Status == 2)) // Trạng thái ko phải đã submited và ko cần Endorsed
              ) {
                return true;
              }
            }
          },
        }),
        new MenuItem({
          index: "approved",
          icon: "fa fa-check-circle",
          title: "Approved",
          path: "success",
          visible: () => {
            // if (this.Purchase.Status == 7 || this.Purchase.Status == 8)
            //   return false;
            if (this.Item.State == 1) return true;
            if (this.Item.State == 4) return true;
            if (this.Item.State == 5) return true;
          },
        }),

        new MenuItem({
          index: "rejected",
          icon: "fa fa-times",
          title: "Rejected",
          path: "danger",
          visible: () => {
            if (this.Item.State == 2) return true;
          },
        }),
      ],
      btn_reject_void: [
        new MenuItem({
          index: "wait",
          icon: "fa fa-refresh",
          title: "Waiting",
          path: "default",
          visible: () => {
            if (this.Item.State == 1) {
              if (
                this.Item.Emp_id != this.user.UserSerial &&
                this.Item.Type_id == 2 && // Loại approved là Approve
                (this.Purchase.Status == 7 || this.Purchase.Status == 8) //Trường hợp hồ sơ đang request void hoặc pending void
              ) {
                return true;
              }
            }
          },
        }),
        new MenuItem({
          index: "sendback",
          icon: "fa fa-check-circle",
          title: "Rejected Void",
          path: "primary",
          visible: () => {
            if (this.Item.State == 5) return true;
          },
        }),
        new MenuItem({
          index: "reject_void_pr",
          title: "Reject Void PR",
          path: "primary",
          visible: () => {
            if (
              this.Item.Type_id == 2 &&
              this.Item.Emp_id == this.user.UserSerial
            ) {
              if (
                this.Item.State == 1 && //Đã approved
                (this.Purchase.Status == 7 || this.Purchase.Status == 8) //Trạng thái PR là Request_Void hoặc Pending_Void
              )
                return true;
            }
          },
          action: () => {
            this.RejectVoidPR();
          },
        }),
        new MenuItem({
          // index: "voided",
          // icon: "fa fa-times",
          // title: "Voided",
          // path: "danger",
          index: "approved",
          icon: "fa fa-check-circle",
          title: "Approved",
          path: "success",
          visible: () => {
            if (this.Item.State == 4) return true;
          },
        }),
        new MenuItem({
          index: "void_pr",
          title: "Void PR",
          path: "danger",
          visible: () => {
            if (
              this.Item.Type_id == 2 &&
              this.Item.Emp_id == this.user.UserSerial
            ) {
              if (
                this.Item.State == 1 && // Đã Approved
                (this.Purchase.Status == 7 || //PR request void
                  this.Purchase.Status == 8) //PR pending void
              )
                return true;
              // if (this.Item.State == 5) return true;
            }
          },
          action: () => {
            this.VoidPR();
          },
        }),
      ],
      form_comment: {
        comment: "",
        btnText: "",
        action: null,
      },
      form_confirm: new DefaultForm({
        title: "Comment for reject",
        width: "400px",
        // OKtext: "Reject",
        // Canceltext: "Cancel",
        type: "dialog",
      }),
    };
  },
  computed: {
    /**
     * @return {boolean} description
     */
    isPRLink() {
      if (this.Purchase.Procurement_id) return true;
      return false;
    },
    /**
     * @return {Array} description
     */
    btns() {
      return this.all_btn.filter(
        (p) => p.visible() && (this.Purchase.Use != 2 || !p.action)
      );
    },
    /**
     * @return {Array} description
     */
    btns_rejectvoid() {
      return this.btn_reject_void.filter(
        (p) => p.visible() && (this.Purchase.Use != 2 || !p.action)
      );
    },
  },
  methods: {
    showForm({ title, btnText, btnType = "danger", action }) {
      this.form_comment.comment = "";
      this.form_comment.btnText = btnText;
      this.form_comment.btnType = btnType;
      this.form_comment.action = action;

      this.form_confirm.title = title;
      this.form_confirm.visible = true;
    },
    RejectVoidPR() {
      this.showForm({
        title: "Comment for Reject Void PR",
        btnText: "Reject Void",
        btnType: "primary",
        action: () => {
          GetDataAPI({
            method: "post",
            url: API.Purchase_SendBack,
            params: {
              ...this.Item,
              comment: this.form_comment.comment,
            },
            action: (re) => {
              ShowMessage("Reject Void PR successfully");
              this.$emit("Approve");
            },
          });
        },
      });
    },
    VoidPR() {
      this.showForm({
        title: "Comment for Void PR",
        btnText: "Void",
        action: () => {
          GetDataAPI({
            method: "post",
            url: API.Purchase_Voided,
            params: {
              ...this.Item,
              comment: this.form_comment.comment,
            },
            action: (re) => {
              ShowMessage("Void PR successfully");
              this.$emit("Approve");
            },
          });
        },
      });
    },
    Reject() {
      this.showForm({
        title: "Comment for Reject PR",
        btnText: "Reject",
        action: () => {
          GetDataAPI({
            method: "post",
            url: API.Purchase_Approved,
            params: {
              Info:{
                ...this.Item,
                comment: this.form_comment.comment,
                State: 2,
              },
              Details: this.Details
            },
            action: (re) => {
              ShowMessage("Reject successfully");

              this.$emit("Approve");
            },
          });
        },
      });
    },
    Approve(State) {
      ShowConfirm({
        message: "Are you sure ?",
        title: "Approve",
        type: MessageType.success,
      }).then(() => {


        GetDataAPI({
          method: "post",
          url: API.Purchase_Approved,
          params: {
            Info: {
              ...this.Item,
              State: State,
            },
            Details: this.Details
          },
          action: (re) => {
            switch (State) {
              case 1:
                ShowMessage("Approval successfully");
                break;
              case 2:
                ShowMessage("Reject successfully");
                break;
              default:
                break;
            }
            this.$emit("Approve");
            // location.reload();
          },
        });
      });
    },
    /**
     * @param {MenuItem} item - description
     */
    handleClick(item) {
      if (item.action) item.action();
    },
    NoneCheckbox() {
      this.$emit("NoneCheckbox", this.Item);
    },
  },
  mounted() {
    // if (this.Item.State == 0)
    //   if (this.Item.Emp_id == this.user.UserSerial) {
    //     switch (this.Purchase.Status) {
    //       case 2:
    //         if (this.Item.Type_id == 1) {
    //           this.btns = [this.all_btn.btn_approve, this.all_btn.btn_reject];
    //         } else if (this.Item.Type_id == 2) {
    //           this.btns = [this.all_btn.btn_wait];
    //         }
    //         break;
    //     }
    //   } else {
    //     this.btns = [this.all_btn.btn_wait];
    //   }
  },
};
</script>

<style lang="scss" scoped>
textarea {
  font-family: "Open Sans", "Segoe UI";
  font-size: 13px;
  color: #444444;
  -webkit-box-shadow: none;
  -moz-box-shadow: none;
  box-shadow: none;
  padding: 0;
  border: none;
  background: url("/images/icon/border-line.png") scroll left bottom #edf3e9;
  line-height: 25px !important;
  width: 100%;
  min-height: 50px;
  resize: none;
  outline: none;

  &[disabled] {
    color: black; //#858585;
    cursor: not-allowed;
    background-color: #fbfbfb;
    border-color: #e5e5e5;
  }
}

.detail-c {
  display: flex;

  .detail-c-l {
    flex: 0 0 calc(100% * 8 / 24);
  }

  .detail-c-r {
    flex: 1;
    color: blue;
  }
}

.btn-action {

  // min-width: 130px;
  &:not([has-action]) {
    // pointer-events: none;
    cursor: not-allowed;
  }
}

.void-c {
  position: absolute;
  top: -90px;
  border: 8px solid red;
  width: 300px;
  display: flex;
  flex-direction: column;
  align-items: center;
  border-radius: 17px;
  left: -5px;
  opacity: 1;
  transform: scale(0.5);
  background: #ffffffe0;

  .void-title {
    color: red;
    font-size: 61px;
    font-weight: bold;
  }

  .void-date {
    color: red;
    align-self: end;
    margin-right: 10px;
    position: absolute;
    bottom: 3px;
    font-weight: bold;
  }
}
</style>
