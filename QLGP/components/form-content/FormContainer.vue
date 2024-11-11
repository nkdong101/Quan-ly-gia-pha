<template>
  <div :class="model.class" :id="model.id" :style="model.style">
    <template v-for="item in model.children">
      <component
        v-bind:key="item.id"
        v-if="item.constructor.name == 'BaseWidget'"
        v-bind:is="getType(item)"
        v-bind="item.attr"
        v-on="item.events"
      >
        <span v-if="item.attr.content" v-html="item.attr.content()"></span>
      </component>
      <component
        v-else
        v-bind:key="item.id"
        v-bind:is="getType(item)"
        :model="item"
        v-bind="item.attr"
        v-on="item.events"
      />
    </template>
  </div>
</template>
<script>
import FormContainer from "~/assets/scripts/base/FormContainer";
export default {
  props: {
    model: {
      type: FormContainer,
      required: true,
    },
  },
  methods: {
    getType(item) {
      if (item.constructor.name == "BaseWidget") {
        return item.tag;
      } else {
        return item.constructor.name;
      }
      // switch (typeof item) {
      //     case "object":
      //         return item.constructor.name;
      //     case "string":
      //         return item;
      //     default:
      //         break;
      // }
    },
  },
  mounted() {
    // alert(this.getType());
    // this.model.children;
    //console.log("re------render");
  },
};
</script>
