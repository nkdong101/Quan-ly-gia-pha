<template>
  <tbody>
    <tr :key="item.IdTmp" style="font-weight: bold; cursor: pointer">
      <td></td>
      <td colspan="4">{{ index + 1 }}. {{ item.Name }}</td>
    </tr>
    <tr :key="item.IdTmp">
      <td></td>
      <td>Contract Date:</td>
      <td>
        <el-date-picker
          style="width: 200px"
          ref="entry"
          v-model.lazy="item.ContractDate"
          type="date"
          format="dd MMM, yyyy"
          placeholder="dd MMM, yyyy"
          prefix-icon="none"
        ></el-date-picker>
      </td>
      <td>Contract Completion Date:</td>
      <td>
        <el-date-picker
          style="width: 200px"
          ref="entry"
          v-model.lazy="item.ContractCompletionDate"
          type="date"
          format="dd MMM, yyyy"
          placeholder="dd MMM, yyyy"
          prefix-icon="none"
        ></el-date-picker>
      </td>
    </tr>
    <tr :key="item.IdTmp">
      <td></td>
      <td>Contract No.:</td>
      <td>
        <el-input v-model="item.ContractNo" />
      </td>
      <td>Contract Value:</td>
      <td>
        <InputNumber v-model="item.ContractValue" style="width: 200px" />

        <InputSelect
          style="width: 90px"
          :model="{ ...Para.Para_Currency, placeholder: 'Currency' }"
          v-model="item.ContractValueCurrency"
        />
      </td>
    </tr>
    <tr :key="item.IdTmp">
      <td></td>
      <td>Attached Signed Contract (Award, Contract, Bridgercheck…):</td>
      <td colspan="3">
        <InputFile
          ref="ContractSignedAttachment"
          :model="Para.PR_Procurment_Attachment"
          v-model="item.ContractSignedAttachment"
          label="Attachment(s)"
        />
      </td>
    </tr>
    <tr :key="item.IdTmp">
      <td></td>
      <td>Contract Status:</td>
      <td colspan="3">
        <InputSelect
          :model="Para.ContractStatus"
          v-model="item.ContractStatus"
        />
        <el-button
          v-if="item.ContractStatus == 1"
          type="warning"
          @click="showFormEvaluate()"
          >Evaluate</el-button
        >
        <DefaultForm :model="formEvaluation" @actionOK="saveEvaluation()">
          <div slot="content">
            <div><b>Scores:</b></div>
            <div class="marks">
              <el-slider
                v-model="objEvaluation.PerformanceRating"
                :max="10"
                :marks="marks"
              >
              </el-slider>
            </div>
            <div><b>Evaluation Details:</b></div>
            <div>
              <InputTextArea
                v-model="objEvaluation.EvaluationDetails"
                spellcheck="false"
              />
            </div>
          </div>
        </DefaultForm>
      </td>
    </tr>
    <tr v-if="item.ContractStatus == 1">
      <td></td>
      <td>Contractor Performance rating:</td>
      <td>
        <el-progress
          :percentage="percentage"
          :color="customColorMethod"
          :format="customFormatMethod"
        ></el-progress>
      </td>
      <td>Evaluation Details:</td>
      <td>
        <InputTextArea
          v-model="item.EvaluationDetails"
          spellcheck="false"
          :minLine="1"
          :disabled="true"
        />
      </td>
    </tr>
    <tr v-if="item.ContractStatus == 1">
      <td></td>
      <td>EvaluatedBy:</td>
      <td colspan="3">
        <img
          v-if="item.EvaluatedBySignature"
          height="80px"
          width="auto"
          :src="Para.SignImage.baseUrl + getFilePath(item.EvaluatedBySignature)"
        />
      </td>
    </tr>
  </tbody>
</template>

<script>
import API from "~/assets/scripts/API";
import DefaultForm from "~/assets/scripts/base/DefaultForm";
import GetDataAPI from "~/assets/scripts/GetDataAPI";
export default {
  props: {
    index: {},
    item: {},
  },
  computed: {
    percentage() {
      return (this.item.PerformanceRating / 10) * 100;
    },
  },
  data() {
    return {
      formEvaluation: new DefaultForm({
        title: "Contractor Performance rating",
        width: "400px",
      }),
      marks: {
        0: "0",
        1: "1",
        2: "2",
        3: "3",
        4: "4",
        5: "5",
        6: "6",
        7: "7",
        8: "8",
        9: "9",
        10: "10",
      },
      objEvaluation: {
        PerformanceRating: 0,
        EvaluationDetails: "",
      },
    };
  },
  methods: {
    customFormatMethod(percentage) {
      return percentage / 10;
    },
    customColorMethod(percentage) {
      if (percentage < 30) {
        return "#909399";
      } else if (percentage < 70) {
        return "#e6a23c";
      } else {
        return "#67c23a";
      }
    },
    saveEvaluation() {
      if (this.item.Id) {
        GetDataAPI({
          url: API.Procurement_SaveEvaluation,
          method: "post",
          params: {
            ...this.item,
            PerformanceRating: this.objEvaluation.PerformanceRating,
            EvaluationDetails: this.objEvaluation.EvaluationDetails,
          },
          action: (re) => {
            this.item.PerformanceRating = this.objEvaluation.PerformanceRating;
            this.item.EvaluationDetails = this.objEvaluation.EvaluationDetails;
            this.item.EvaluatedBy = this.user.UserSerial;
            this.item.EvaluatedBySignature = this.user.Image;
            this.formEvaluation.visible = false;
          },
        });
      } else {
        this.item.PerformanceRating = this.objEvaluation.PerformanceRating;
        this.item.EvaluationDetails = this.objEvaluation.EvaluationDetails;
        this.item.EvaluatedBy = this.user.UserSerial;
        this.item.EvaluatedBySignature = this.user.Image;
        this.$emit("saveEvaluation");
        this.formEvaluation.visible = false;
      }
    },
    showFormEvaluate() {
      this.objEvaluation.PerformanceRating = this.item.PerformanceRating;
      this.objEvaluation.EvaluationDetails = this.item.EvaluationDetails;
      this.formEvaluation.visible = true;
    },
    upload() {
      return new Promise((rs) => {
        this.$refs.ContractSignedAttachment.submitUpload().then((results) => {
          this.item.ContractSignedAttachment = results.join(",");
          rs();
        });
      });
    },
  },
};
</script>

<style lang="scss" scoped>
.marks {
  padding: 0 8px;
  position: relative;
  display: flex;
  flex-direction: column;
  margin-bottom: 15px;
  ::v-deep  .el-slider__marks-text {
    white-space: nowrap;
  }
}
</style>
