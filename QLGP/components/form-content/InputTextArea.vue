<template>
  <textarea
    v-model.lazy="data"
    @change="handleChange"
    @input="handleInput"
    style="height: 25px"
  >
  </textarea>
</template>

<script>
import DelayCall from "~/assets/scripts/base/DelayCall";
export default {
  props: {
    value: {},
    minLine: {},
  },
  data() {
    return {
      itv: null,
      data: this.value,
      /**
       * @type {DelayCall} type - description
       */
      dc: null,
    };
  },
  watch: {
    value(newValue, oldValue) {
      this.data = newValue;
      this.$nextTick(() => {
        this.OnInput();
      });
    },
  },
  methods: {
    clearData() {
      this.data = "";
      this.handleChange();
    },
    handleChange() {
      this.$emit("input", this.data);
    },
    handleInput() {
      this.dc.run();
    },
    OnInput() {
      this.$el.style.height = 0;
      this.$el.style.height = this.$el.scrollHeight + "px";
    },
  },
  destroyed() {
    if (this.itv) clearInterval(this.itv);
  },
  mounted() {
    this.dc = new DelayCall(() => {
      this.$emit("input", this.$el.value);
    });
    this.$el.setAttribute(
      "style",
      `height: ${this.$el.scrollHeight}px;` +
        `overflow-y:hidden;` +
        `min-height: ${(this.minLine || 2) * 25}px;`
    );
    this.itv = setInterval(() => {
      // console.log("itv");
      this.OnInput();
    }, 500);

    this.$el.addEventListener("input", this.OnInput, false);
  },
};
</script>

<style lang="scss" scoped>
</style>
