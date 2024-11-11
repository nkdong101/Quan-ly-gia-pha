<template lang="">
  <div class="fm_form">
    <div>{{hus? 'Cha' : 'Mẹ'}}</div>
    <FormInfo :ref="'formFam' + (hus ? 'C' : 'M')" :model="obj.form2()" />

  </div>
</template>
<script>

export default {
  props: {
    hus: {},
    obj: {},
  },
 
};
</script>
<style lang="scss">


</style>
