<template>
  <div class="pf_main" v-if="isLoaded">
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
          <h1 style="margin-top: 20px">Techinical Form</h1>
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
      <span> Enter consulting information about the assets </span>
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
          <td style="width: 220px"><b>Technical Manager:</b></td>
          <td style="width: 200px" colspan="3">
            <InputSelect
              style="width: 200px"
              v-model="obj.Technical_id"
              :model="ParaTechnical"
              :disabled="obj.Technical_id == user.UserSerial"
            />
            <!-- {{ obj.Technical_id }} - {{ user.UserSerial }} -->
          </td>
        </tr>

        <tr>
          <td>2</td>
          <td><b>Office International:</b></td>
          <td colspan="3">
            <InputSelect
              style="width: 200px"
              v-model="obj.OfficeIntl"
              :model="ParaOfficeIntl"
              :disabled="obj.Technical_id == user.UserSerial"
            />
          </td>
        </tr>

        <tr>
          <td>3</td>
          <td colspan="4"><b>Assets information</b></td>
        </tr>

        <tr>
          <td></td>
          <td colspan="4">
            <table class="tbl-vendor" cellspacing="0" cellpadding="0">
              <tr>
                <th width="50">STT</th>
                <th style="width: auto">Detailed Descriptions</th>
                <th style="width: 50px">Quantity</th>
                <th style="width: 250px">Type</th>
                <th style="width: 150px">Producer</th>
                <th style="width: 150px">Model</th>
                <th style="width: 250px">Specifications</th>
              </tr>
              <tr v-for="(item, index) in model.Details" :key="item.Id">
                <td>{{ index + 1 }}</td>
                <td>{{ item.Description }}</td>
                <td>{{ item.Quantity }}</td>
                <td>
                  <InputSelect
                    style="width: 100%"
                    :model="Para.fixed_asset_type_Get_List"
                    v-model="item.Type_id"
                  />
                </td>
                <td>
                  <InputSelect
                    style="width: 100%"
                    :model="Para.producer_Get_List"
                    v-model="item.Producer_id"
                  />
                </td>
                <td>
                  <InputTextArea v-model="item.Model" :minLine="1" />
                </td>
                <td>
                  <InputTextArea v-model="item.specifications" :minLine="1" />
                </td>
              </tr>
            </table>
          </td>
        </tr>
      </table>
    </div>
  </div>
</template>

<script>
import moment from "moment";
import API from "~/assets/scripts/API";
import APIHelper from "~/assets/scripts/API/APIHelper";
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
import { Para } from "~/assets/scripts/Para";
export default {
  props: {
    model: {},
  },
  data() {
    return {
      isLoaded: false,
      obj: {
        Technical_id: "",
        OfficeIntl: "",
      },
      Technical: this.model.Approval.filter((p) => p.Type_id == 5)[0],
      ParaTechnical: new SelectOption({
        data: [],
        label: "FullName",
      }),
    };
  },
  computed: {
    ParaOfficeIntl() {
      return Para.Para_Office.set(
        (p) => (p.data = p.data.filter((p1) => p1.OfficeType == 2))
      );
    },
  },
  watch: {},
  methods: {
    getValidate() {
      return new Promise((rs) => {
        rs(true);
      });
    },
    Save() {
      this.getValidate().then((re) => {
        if (re) {
          Promise.all([]).then((results) => {
            this.obj.Purchase_id = this.model.Info.Id;
            this.obj.Details = this.model.Details;
            APIHelper.purchase.SaveTechinicalConsulting(this.obj).then((re) => {
              ShowMessage("Save Techinical Form successfully");
            });
          });
        } else {
          ShowMessage("Please complete all information", "error");
        }
      });
      // console.log(this.vendor_tmp_picked);
      // console.log(this.obj);
    },
    GetTechinicalForm() {
      if (this.Technical) {
        this.obj.Technical_id = this.Technical.Emp_id;
      }
      this.obj.OfficeIntl = this.model.Info.Office_intl_id || "";
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
    console.log(this);
    let account = new Promise((rs) => {
      GetDataAPI({
        url: API.Account,
        action: (re) => {
          this.ParaTechnical.data = re.filter(
            (p) =>
              p.UserLevel_id == "202303031015160093" ||
              (this.Technical &&
                this.Technical.Emp_id == this.user.UserSerial &&
                this.user.UserSerial == p.Id)
          );
          rs();
        },
      });
    });

    Promise.all([account]).then((re) => {
      this.GetTechinicalForm();
      this.isLoaded = true;
    });
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
      padding: 8px 8px;
      border-top: 1px solid #ddd;
      border-left: 1px solid #ddd;
      text-align: left;
    }
  }
}
</style>
