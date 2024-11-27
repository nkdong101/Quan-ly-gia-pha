<template>
  <div>
    <div
      :id="app_id"
      role="main-content"
      :style="{ position: 'relative', height: height }"
    ></div>
  </div>
  <!-- :content="content"
    :options="editorOption"
    @change="onEditorChange($event)" -->
  <!-- @change="onEditorChange($event)" -->
</template>

<script>
import { GetGlobalId } from "~/assets/scripts/Functions";
export default {
  props: {
    value: {
      type: String,
    },
    height: {
      type: String,
    },
    placeholder: {
      type: String,
    },
  },
  data() {
    return {
      app_id: "q-editor-" + GetGlobalId(),
      quill: null,
      content: this.value,
      // height: this.height || "",
    };
  },
  watch: {
    value: {
      handler(newValue, oldValue) {
        if (oldValue == "") {
          this.quill.root.innerHTML = this.value || "";
        }
      },
      deep: true,
    },
  },
  methods: {},
  computed: {},
  mounted() {
    this.quill = new Quill("#" + this.app_id, {
      theme: "snow",
      placeholder: this.placeholder || "Nhập nội dung...",
      modules: {
        syntax: true,
        toolbar: [
          [{ font: [] }, { size: [] }],
          ["bold", "italic", "underline", "strike"],
          [{ color: [] }, { background: [] }],
          [{ script: "super" }, { script: "sub" }],
          [{ header: "1" }, { header: "2" }, "blockquote", "code-block"],
          [
            { list: "ordered" },
            { list: "bullet" },
            { indent: "-1" },
            { indent: "+1" },
          ],
          ["direction", { align: [] }],
          ["link", "image", "video", "formula"],
          ["clean"],
        ],
        imageResize: {
          displaySize: true,
        },
      },
    });
    this.quill.on("text-change", (delta, oldDelta, source) => {
      // console.log(source);

      if (source == "api") {
        console.log("An API call triggered this change.");
      } else if (source == "user") {
        // console.log("A user action triggered this change.");
        this.$emit("input", this.quill.root.innerHTML);
      }
    });
    this.quill.clipboard.dangerouslyPasteHTML(this.value || "");
    console.log(this);
    // let data = JSON.parse(
    //   '{"ops":[{"attributes":{"width":"145"},"insert":{"image":"data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAKgAAACkCAIAAACM6/KYAAAACXBIWXMAAA7EAAAOxAGVKw4bAAAEEElEQVR4nO3dP27iQBhAcVjtISgiFKEUqXKGUJEmB4GzpOYiNFAFyTdIRYFQFKXILdhipS1iNozH32DgvV+5YhzjJ1vj9R/6+/2+J55fXa+AumF4KMNDGR7K8FCGhzI8lOGhDA9leCjDQxkeyvBQhof63ejTb29vHx8fhVYl3f39/d3d3bd/7Pf7KWMTL0PXl1YfuN1uN5tNytKKGg6HDw8PjYftm1gsFgXWvLGqqurrljg28ZumDKyqKvRrZXp5eWkU8S8P9VCGhzI8lOGhms3qD3p8fGy/kB+s1+uiy49Vemv0gjZIQPjX19f2C/lB7Hlaouylld4a4/E4ZDke6qEMD2V4KMNDGR4qYFZf9/T0lDdwNBrN5/O8sSmXVQ5+LFH2PH82m+12u7yxy+Uyb+BRRcL3er3VapUxajqdhq/JOcjbGpPJJHxN/vFQD2V4KMNDGR7K8FClZvXn4OCZW/2sLPsE76K5x0MZHsrwUIaHMjzUNc/qDyp9LedSuMdDGR7K8FCGhzI8VJFZ/Wg0utZ7afKc4dYoEj77vrk2su+JK33m1snWOMpDPZThoQwPZXgow0MFzOrP5HpG9tWX2Is0Z7I1jnKPhzI8lOGhDA9leKh+o//i3m63X19f5dYm0e3t7c3NzdGPxU6w6xvq8/Pz/f098E/kGQwG9Vc6H9Us/GUpHf6ieaiHMjyU4aEMD2V4qGaz+k7myaWvvrS5SJPyFdpstMSn+TPOONzjoQwPZXgow0MZHqrZrVex8+TECWr2zPYE0+m6S3n+3j0eyvBQhocyPJThoQwP1ex07kxOkGJ1cuUp9gJVBvd4KMNDGR7K8FCGhwp4oCL2993LzWP/t/w2Sj9l4axewQwPZXgow0MZHsrwUEWepCl9l1n2mWGbpWX/iTY3KpbjHg9leCjDQxkeyvBQ1/PWq9jn41MGHhT7Cyk+H69ghocyPJThoQwP1eyBist6ibF+0Cz8ZrN5fn4utCrpqqqqh499MUKgqBOwlgO/8VAPZXgow0MZHsrwUAG/NHn6p0k6cSZXs7xIo1YMD2V4KMNDGR4qYFZfN5vNssfO5/PANcl2+sc/Yj92VJHwu91utVplDJxOp+Ero4M81EMZHsrwUIaHMjxUkVl9J2IfkTn9D+HE3qF1lHs8lOGhDA9leCjDQ13PrD5R9qw7+2n7E7x8K4N7PJThoQwPZXgow0OVmtVPJpNCS1aIIuGXy2WJxYZIPN06/UuGT3zW56EeyvBQhocyPJThoQJm9ePxuP1CSmhzM1Pg24Nj7wCLEhB+vV63X4hOzEM9lOGhDA9leKhmk7vhcLhYLAqtSrrBYND1Kly86/lNGjXioR7K8FCGhzI8lOGhDA9leCjDQxkeyvBQhocyPNQflj/x14R2dVAAAAAASUVORK5CYII="}},{"insert":"\\nsdfasdf\\nádf\\nádf\\nádf\\nádf\\nádf\\nádfasdfsda\\nfasdfasdfadsfasđfsadf\\n"}]}'
    // );
    // this.quill.setContents(data);
    // console.log(this.quill);
  },
};
</script>

<style lang="scss" scoped>
[role="main-content"] {
  // height: calc(100vh - 427px);
  word-break: break-word;
  text-align: justify;
}
</style>
