<template lang="">
    <div style="height: 100%;
    line-height: 25px;">
        ÂL: {{ConvertStr.toLunarStr(obj.Date_of_death)}}
    </div>
</template>
<script>
export default {
    props: {
        obj: {}
    },
    mounted(){
        console.log(this.obj)
    }
}
</script>
<style lang="">
    
</style>