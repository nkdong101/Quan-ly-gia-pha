<template>
  <div class="pf_main" id="pfd_main">
    <div style="overflow: hidden">
      <div class="pf-h el-row" style="margin-left: -15px; margin-right: -15px">
        <div
          class="pf-h-logo el-col el-col-4"
          style="padding-left: 15px; padding-right: 15px"
        >
          <img src="/images/icon/wwf-logo.jpg" alt="" />
        </div>
        <div
          class="pf-h-title el-col el-col-16"
          style="padding-left: 15px; padding-right: 15px"
        >
          <h1 style="margin-top: 20px">Select Supplier</h1>
        </div>
        <div
          class="el-col el-col-4"
          style="padding-left: 15px; padding-right: 15px; position: relative"
        >
          <div class="pf-h-serial">
            <div class="text-bold">P/R No.</div>
            <div class="pr_no">{{ model.Info.Serial }}</div>
          </div>
        </div>
      </div>
    </div>
    <div
      style="
        padding: 15px 15px;
        font-weight: bold;
        font-size: 13px;
        display: flex;
        justify-content: space-between;
        align-items: center;
      "
    >
      <span>
        To be filled by Procurement Department and/or Project Staff for Direct
        Purchase/Direct Appointment Package
      </span>
      <el-button type="success" size="small" @click="Save()">
        <i class="fa fa-edit" style="margin-right: 5px"></i>
        Save
      </el-button>
    </div>
    <!-- {{ obj ? obj.Info : null }} -->
    <div style="padding: 15px">
      <table class="pf_form" v-if="obj != null" cellspacing="0" cellpadding="0">
        <tr>
          <td style="width: 50px">1</td>
          <td style="width: 220px">Procurement Methods:</td>
          <td style="width: 200px">
            <ValidatePR
              :baseValue="obj.Info.Method_id"
              ref="Method_id"
              :showAlert="true"
            >
              <InputSelect
                style="width: 200px"
                v-model="obj.Info.Method_id"
                :model="Para.Get_Procurement_Methods"
              />
            </ValidatePR>
          </td>
          <td>Notes:</td>
          <td style="width: auto; min-width: 500px">
            <InputTextArea
              v-model.lazy="obj.Info.Method_id_Note"
              :minLine="1"
            />
          </td>
        </tr>

        <tr>
          <td>2</td>
          <td>Package Advertisement:</td>
          <td>
            <InputSelect
              style="width: 100%"
              :model="Para.YesNo"
              v-model="obj.Info.hasAdv"
            />
          </td>
          <td></td>
          <td style="width: auto; min-width: 500px"></td>
        </tr>
        <tr v-if="obj.Info.hasAdv == 1">
          <td></td>
          <td>Package Announcement Date:</td>
          <td>
            <el-date-picker
              style="width: 200px"
              ref="entry"
              v-model.lazy="obj.Info.Bid_Announcement_Date"
              type="date"
              format="dd MMM, yyyy"
              placeholder="dd MMM, yyyy"
              prefix-icon="none"
            ></el-date-picker>
          </td>
          <td>Notes:</td>
          <td style="width: auto; min-width: 500px">
            <InputTextArea
              v-model.lazy="obj.Info.Bid_Announcement_Date_Note"
              :minLine="1"
            />
          </td>
        </tr>
        <tr v-if="obj.Info.hasAdv == 1">
          <td></td>
          <td>Submission Deadline:</td>
          <td>
            <el-date-picker
              style="width: 200px"
              ref="entry"
              v-model.lazy="obj.Info.Submission_Deadline"
              type="date"
              format="dd MMM, yyyy"
              placeholder="dd MMM, yyyy"
              prefix-icon="none"
            ></el-date-picker>
          </td>
          <td>Notes:</td>
          <td style="width: auto; min-width: 500px">
            <InputTextArea
              v-model.lazy="obj.Info.Submission_Deadline_Note"
              :minLine="1"
            />
          </td>
        </tr>
        <tr v-if="obj.Info.hasAdv == 1">
          <td></td>
          <td>Advertisements Info:</td>

          <td colspan="3">
            <InputFile
              ref="AdvAttachment"
              :model="Para.PR_Procurment_Attachment"
              v-model="obj.Info.AdvAttachment"
              label="Attachment(s)"
            />
            <div style="padding: 5px 0">
              <b style="padding-left: 10px">Notes:</b>
            </div>
            <InputTextArea
              v-model.lazy="obj.Info.Advertisements"
              :minLine="1"
            />
          </td>
        </tr>
        <tr v-if="obj.Info.hasAdv == 1">
          <td></td>
          <td>Total advertisement days:</td>
          <td colspan="3">
            <InputNumber
              style="width: 200px"
              align="left"
              v-model="obj.Info.TotalAdvertisementDates"
            />
          </td>
        </tr>

        <tr>
          <td>3</td>
          <td colspan="4"><b>Shortlisted Consultants/Bidders:</b></td>
        </tr>

        <tr>
          <td></td>
          <td colspan="4">
            <table class="tbl-vendor" cellspacing="0" cellpadding="0">
              <tr data-v-6d63258d="">
                <th width="50">STT</th>
                <th style="width: 450px">Name in full</th>
                <th style="width: 200px">Registration No./ ID No.</th>
                <th style="width: 150px">Contact person</th>
                <th style="width: 150px">Contact Email</th>
                <th style="width: 155px">Contact phone No.</th>
                <th width="100">Attachment</th>
                <th width="100">Selection</th>
                <th width="50"></th>
              </tr>
              <ProcurementVendorForm
                v-for="(item, index) in vendor_tmp_picked"
                :key="item.IdTmp"
                :index="index"
                :vendor_state_tmp="vendor_state_tmp"
                :item="item"
                ref="vendor_form"
              />

              <tr>
                <td>
                  <el-button
                    class="icon-btn"
                    type="primary"
                    @click="handleVendorPicked(true)"
                    ><i class="fa fa-plus"></i
                  ></el-button>
                </td>
                <td colspan="10">
                  <InputSelect
                    v-if="isvendor"
                    :model="vendor_tmp"
                    v-model="vendor_tmp_selected"
                    style="width: 100%"
                    ref="vendor_tmp_selected"
                    @change="handleVendorPicked"
                  />
                </td>
              </tr>
            </table>
          </td>
        </tr>
        <!-- <tr>
          <td>6</td>
          <td>Selected Consultant/Bidder:</td>
          <td colspan="3">
            {{ selectedVendor ? selectedVendor.Name : "" }}
          </td>
        </tr> -->
        <tr>
          <td>4</td>
          <td>Selection Completion Date:</td>
          <td>
            <el-date-picker
              style="width: 200px"
              ref="entry"
              v-model.lazy="obj.Info.CompetionDate"
              type="date"
              format="dd MMM, yyyy"
              placeholder="dd MMM, yyyy"
              prefix-icon="none"
            ></el-date-picker>
          </td>
          <td colspan="2"></td>
        </tr>
        <tr>
          <td>5</td>
          <td>Attached Evaluation/Selection Document/Memos:</td>
          <td colspan="3">
            <InputFile
              ref="PR_Attachment"
              :model="Para.PR_Procurment_Attachment"
              v-model="obj.Info.Attachment"
              label="Attachment(s)"
            />
          </td>
        </tr>

        <tr v-if="selectedVendor.length">
          <td>6</td>
          <td colspan="4">
            <!-- <el-checkbox v-model="obj.Info.ContractSigned"
              >Contract signed</el-checkbox
            > -->
            <b> Contract signed </b>
          </td>
        </tr>

        <ProcurementContractForm
          v-for="(item, index) in selectedVendor"
          ref="ProcurementContractForm"
          :index="index"
          :item="item"
          :key="item.IdTmp"
          @saveEvaluation="Save()"
        />
      </table>
    </div>
  </div>
</template>

<script>
import moment from "moment";
import API from "~/assets/scripts/API";
import { SelectOption } from "~/assets/scripts/base/SelectOption";
import {
  GetGlobalId,
  Global,
  IsNullDate,
  ShowMessage,
} from "~/assets/scripts/Functions";
import GetDataAPI from "~/assets/scripts/GetDataAPI";
import Procurement_vendors from "~/assets/scripts/objects/Procurement/Procurement_vendors";
import Save_Procurement_Input from "~/assets/scripts/objects/Procurement/Save_Procurement_Input";
export default {
  props: {
    model: {},
  },
  data() {
    return {
      ParaKeywords: new SelectOption({
        multiple: true,

        data: [],
      }),
      ProcurementMethod: null,
      vendor_tmp_selected: "",
      vendor_tmp_picked: [],
      isvendor: false,
      vendor_state_tmp: new SelectOption({
        data: [
          { Id: 1, Name: "YES" },
          { Id: 2, Name: "NO" },
        ],

        // IsItemDisabled: (data) => {
        //   return this.vendor_tmp_picked.some(
        //     (p) => p.State == data.Id && p.State == 1
        //   );
        // },
      }),
      vendor_tmp: new SelectOption({
        allowCreate: true,
        IsItemDisabled: (data) => {
          // console.log(data, this.vendor_tmp_picked);
          return this.vendor_tmp_picked.some((p) => p.vendor_id == data.Id);
        },
        placeholder: "Select a vendor...",
        key: "IdTmp",
        data: [],
      }),
      /**
       * @type {Save_Procurement_Input} type - description
       */
      obj: null,
      filePicked: null,
      hasPick: false,
      obj_vendor: new Procurement_vendors(),
    };
  },
  computed: {
    /**
     * @return {*} description
     */
    selectedVendor() {
      let result = (this.vendor_tmp_picked || []).filter((p) => p.State == 1);
      result.forEach((p) => {
        if (!p.ContractValueCurrency)
          p.ContractValueCurrency = this.model.Info.Money_code;
      });
      return result;
    },
  },
  watch: {
    // ProcurementMethod(newValue, oldValue) {
    //   this.setDefaultMethod();
    // },
    "obj.Info.Bid_Announcement_Date"() {
      this.obj.Info.TotalAdvertisementDates = this.getTotalAdvertisementDates();
    },
    "obj.Info.Submission_Deadline"() {
      this.obj.Info.TotalAdvertisementDates = this.getTotalAdvertisementDates();
    },
    // "obj.Info.Category_id"(nv, ov) {
    //   this.$nextTick(() => {
    //     console.log(nv, ov, this.obj.Info.KeywordsSelected);
    //     if (ov !== undefined) this.obj.Info.KeywordsSelected = [];
    //     let selectedData = this.$refs.Category_id.selectedData;
    //     // console.log(this.$refs.Category_id.selectedData);
    //     if (selectedData) {
    //       this.ParaKeywords.data = (selectedData.Keywords || "")
    //         .trim()
    //         .split("\n")
    //         .filter((p) => p)
    //         .map((p) => {
    //           return {
    //             Id: p.trim(),
    //             Name: p.trim(),
    //           };
    //         });
    //     } else {
    //       this.ParaKeywords.data = [];
    //     }
    //   });
    // },
  },
  methods: {
    getWeekDayList(startDate, endDate, ignoreWeekend) {
      let days = [];
      let end = new Date(endDate);
      for (
        let start = new Date(startDate);
        start <= end;
        start.setDate(start.getDate() + 1)
      ) {
        let day = start.getDay();

        if (!ignoreWeekend || (day != 6 && day != 0)) {
          days.push(new Date(start));
        }
      }
      return days;
    },
    getTotalAdvertisementDates() {
      if (
        !IsNullDate(this.obj.Info.Bid_Announcement_Date) &&
        !IsNullDate(this.obj.Info.Submission_Deadline)
      ) {
        return this.getWeekDayList(
          this.obj.Info.Bid_Announcement_Date,
          this.obj.Info.Submission_Deadline,
          true
        ).length;
      }
      return "";
    },
    getTotalEvaluationDates() {
      if (
        !IsNullDate(this.obj.Info.Submission_Deadline) &&
        !IsNullDate(this.obj.Info.CompetionDate)
      ) {
        return this.getWeekDayList(
          this.obj.Info.Submission_Deadline,
          this.obj.Info.CompetionDate
        ).length;
      }
      return "";
    },
    // setDefaultMethod() {
    //   if (this.ProcurementMethod && this.ProcurementMethod.Info) {
    //     if (!this.obj.Info.Method_id)
    //       this.obj.Info.Method_id = this.ProcurementMethod.Info.Procurement_id;
    //     this.obj.Info.Method_Auto_id =
    //       this.ProcurementMethod.Info.Procurement_id;
    //   } else {
    //     this.obj.Info.Method_Auto_id = 0;
    //   }
    // },
    getValidate() {
      return new Promise((rs) => {
        if (this.$refs.vendor_form) {
          return Promise.all([
            this.$refs.vendor_form.every((p) => p.getValidate()),
          ]).then((re) => {
            rs(re.every((p) => p));
          });
        } else rs(true);
      });
    },
    Save() {
      this.getValidate().then((re) => {
        if (re) {
          let file_stuff = [];
          let file_stuff1 = new Promise((rs) => {
            this.$refs.PR_Attachment.submitUpload().then((results) => {
              this.obj.Info.Attachment = results.join(",");
              rs();
            });
          });
          file_stuff.push(file_stuff1);
          let file_stuff2 = new Promise((rs) => {
            if (this.obj.Info.hasAdv == 1) {
              this.$refs.AdvAttachment.submitUpload().then((results) => {
                this.obj.Info.AdvAttachment = results.join(",");
                rs();
              });
            } else rs();
          });
          file_stuff.push(file_stuff2);

          if (this.$refs.vendor_form) {
            file_stuff.push(
              ...this.$refs.vendor_form.map((p) => p.submitUpload())
            );
          }
          if (this.$refs.ProcurementContractForm) {
            file_stuff.push(
              ...this.$refs.ProcurementContractForm.map((p) => p.upload())
            );
          }
          Promise.all(file_stuff).then((results) => {
            this.obj.Info.Purchase_id = this.$route.params.id;
            this.obj.Details = this.vendor_tmp_picked;
            GetDataAPI({
              method: "post",
              url: API.Procurement_Save,
              params: this.obj,
              action: (re) => {
                ShowMessage(`Save successed`);
              },
            });
          });
        } else {
          ShowMessage("Please complete all information", "error");
        }
      });
      // console.log(this.vendor_tmp_picked);
      // console.log(this.obj);
    },
    vendorRemove(item) {
      this.vendor_tmp_picked.splice(this.vendor_tmp_picked.indexOf(item), 1);
    },
    AddVendor() {
      this.vendor_tmp_picked.push({
        Id: 0,
        IdTmp: GetGlobalId(),
        Name: "",
        Serial: "",
        ContactName: "",
        State: "",
      });
    },
    handleVendorPicked(isAdd) {
      if (this.$refs.vendor_tmp_selected.selectedData) {
        this.vendor_tmp_picked.push(
          new Procurement_vendors({
            Procurement_id: this.obj.Info.Id,
            ContactEmail:
              this.$refs.vendor_tmp_selected.selectedData.ContactEmail,
            ContactName:
              this.$refs.vendor_tmp_selected.selectedData.ContactName,
            ContactPhone:
              this.$refs.vendor_tmp_selected.selectedData.ContactPhone,
            Serial: this.$refs.vendor_tmp_selected.selectedData.Serial,
            TechnicalEvaluation:
              this.$refs.vendor_tmp_selected.selectedData.TechnicalEvaluation,
            vendor_id: this.$refs.vendor_tmp_selected.selectedData.Id,
            Name: this.$refs.vendor_tmp_selected.selectedData.Name,
            OverallScore:
              this.$refs.vendor_tmp_selected.selectedData.OverallScore,
          })
        );
      } else {
        if (this.vendor_tmp_selected || isAdd) {
          let obj = {
            Id: 0,
            IdTmp: GetGlobalId(),
            Name: this.vendor_tmp_selected,
            Serial: "",
            ContactName: "",
            State: "",
          };
          this.vendor_tmp_picked.push(obj);
          setTimeout(() => {
            document.getElementById(obj.IdTmp).focus();
          }, 0);
        }
      }
      this.vendor_tmp_selected = "";
    },
    pickAttachment() {
      document.getElementById("txtPF_Contract_Url").click();
    },
    TakeFile() {
      this.filePicked = document.getElementById("txtPF_Contract_Url").files[0];
      this.hasPick = true;
      this.obj.Info.Contract_Url = "";
      // console.log(this.filePicked);
    },
    GetdataVendors() {
      this.isvendor = false;
      GetDataAPI({
        url: API.Get_List_Vendors,
        action: (re) => {
          let data = re;
          data.forEach((p) => {
            p.IdTmp = GetGlobalId();
          });
          this.vendor_tmp.data = data;
          this.isvendor = true;
        },
      });
    },
    GetProcurement() {
      return new Promise((rs) => {
        GetDataAPI({
          url: API.Get_Info_Procurement,
          params: {
            iPurchase_id: this.$route.params.id,
          },
          action: (re) => {
            if (re) {
              let obj = new Save_Procurement_Input(re);
              this.vendor_tmp_picked = re.Details || [];
              // console.log(obj.Info.Category_id);
              if (!obj.Info.Category_id)
                obj.Info.Category_id = this.model.Info.Category_id;

              this.obj = obj;
            }
            rs();
          },
          error: () => {
            rs();
          },
        });
      });
    },
    // GetProcurementMethodDefault() {
    //   GetDataAPI({
    //     isWait: false,
    //     url: API.GetProcurementInfo,
    //     params: {
    //       iCategory_id: this.model.Info.Category_id,
    //       Budget: this.model.Info.Money_CHF,
    //     },
    //     action: (re) => {
    //       if (re[0]) {
    //         this.ProcurementMethod = re[0];
    //       } else {
    //         this.ProcurementMethod = null;
    //       }
    //     },
    //   });
    // },
  },
  mounted() {
    // this.obj = new Save_Procurement_Input({
    //   Purchase_id: this.$route.params.id,
    // });
    console.log(this);
    this.GetProcurement().then(() => {
      this.GetdataVendors();
      // this.GetProcurementMethodDefault();
    });
    // if (this.model.Info.Use != 2)
    //   setTimeout(() => {
    //     this.$nextTick(() => {
    //       let a = document.getElementById("pf_c");
    //       a.scrollTo({
    //         top: this.$el.getBoundingClientRect().top + a.scrollTop - 80,
    //         behavior: "smooth",
    //       });
    //     });
    //   }, 500);
  },
};
</script>

<style lang="scss" scoped>
.pf_main {
  // padding: 0 15px;
  margin-bottom: 20px;
  background: white;
  border: 1px solid #ddd;
  border-radius: 10px;
  overflow: hidden;
  .pf-h {
    text-align: center;
    padding: 0 15px;
    .pf-h-logo {
      img {
        width: 110px;
        height: 110px;
      }
    }
    .pf-h-title {
      display: flex;
      flex-direction: column;
      align-items: center;
      h1 {
        font-weight: 700 !important;
        font-size: 32px;
        margin-bottom: 10px;
      }
      h2 {
        font-weight: 300 !important;
        font-size: 36px;
        margin-top: 20px;
        margin-bottom: 10px;
      }
    }
    .pf-h-serial {
      border: 1px solid #cccccc;
      margin-top: 20px;
      font-size: 18px;
      position: absolute;
      right: 15px;
      .text-bold {
        border-bottom: 1px solid #cccccc;
      }
      .pr_no {
        background-color: #e2efd9;
        white-space: nowrap;
        padding: 0 5px;
      }
    }
  }
  .pf_form {
    width: 100%;
    border-bottom: 1px solid #ddd;
    border-right: 1px solid #ddd;
    ::v-deep  {
      td,
      th {
        padding: 10px;
        border-top: 1px solid #ddd;
        border-left: 1px solid #ddd;
        vertical-align: top;
      }

      tr {
        td {
          &:first-child {
            text-align: center;
          }
        }
      }
    }
  }
  .tbl-vendor {
    width: 100%;
    border-bottom: 1px solid #ddd;
    border-right: 1px solid #ddd;

    tr {
      &[role="1"] {
        color: blue;
        ::v-deep  input {
          color: blue;
        }
      }
    }
    td,
    th {
      padding: 4px 8px;
      border-top: 1px solid #ddd;
      border-left: 1px solid #ddd;
      text-align: left;
    }
  }
}
</style>
