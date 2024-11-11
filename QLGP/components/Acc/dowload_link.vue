<template lang="">
    <div class="dowload">
        <div style="padding-right: 10px;
    text-align: right;" class="ggPlay">
          <img src="/images/ggPlay.png" />
          <a href=""> <img src="/images/link_qrcode.png" /></a>
        </div>
        <div class="appStore">
          <img src="/images/appStore.png" />
          <a href=""> <img src="/images/link_qrcode.png" /></a>
        </div>
      </div>
    </div>
</template>
<script>
export default {
    
}
</script>
<style lang="scss" scoped>
     .dowload {
      display: flex;
      align-items: center;
      justify-content: center;
      div {
        img {
          width: 80%;
        }
      }
    }
</style>