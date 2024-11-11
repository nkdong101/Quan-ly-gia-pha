<template>
  <div class="app-main-children">
    <div class="app-main-children-label">
      <b>{{ title }}</b>
    </div>
    <div class="app-main">
      <div
        class="app-main-item"
        v-for="item in items"
        :key="item.index"
        @click="handleClick(item)"
      >
        <div class="app-main-item-title">
          <div class="app-main-item-title-icon">
            <img v-if="item.image" :src="Para.MenuImage.getImage(item.image)" />
            <i v-else :class="item.icon || 'fa fa-server'"></i>
          </div>
          <div class="app-main-item-title-text">
            {{ item.title }}
          </div>
        </div>
        <div class="app-main-item-notes">{{ item.note }}</div>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  props: {
    title: {},
    items: {},
  },
  methods: {
    handleClick(item) {
      if (item.path) {
        this.$router.push(item.path);
      }
    },
  },
};
</script>

<style lang="scss" scoped>
.app-main {
  margin-top: 10px;
  grid-template-columns: repeat(auto-fill, minmax(284.333px, 1fr));
  display: grid;
  column-gap: 20px;
  row-gap: 20px;
  .app-main-item {
    height: 116px;
    flex: 0 0 33%;
    // border: 1px solid #ddd;
    border-radius: 10px;
    padding: 15px;
    display: flex;
    flex-direction: column;
    justify-content: space-between;
    box-shadow: 0 0 2px rgba(0, 0, 0, 0.12), 0 2px 4px rgba(0, 0, 0, 0.14);
    cursor: pointer;
    &:hover {
      background: #f5f5f5;
      box-shadow: 0 0 2px rgba(0, 0, 0, 0.12), 0 4px 8px rgba(0, 0, 0, 0.14);
    }
    .app-main-item-title {
      display: flex;
      align-items: center;
      .app-main-item-title-icon {
        width: 40px;
        height: 40px;
        display: flex;
        align-items: center;
        justify-content: center;
        border: 1px solid #ddd;
        border-radius: 2px;
        padding: 4px;
        overflow: hidden;
        img {
          height: 100%;
        }
      }
      .app-main-item-title-text {
        padding-left: 15px;
        font-weight: bold;
        font-size: 14px;
      }
    }
    .app-main-item-notes {
      min-height: 32px;
      font-size: 12px;
    }
  }
}
.app-main-children {
  margin-top: 15px;
  .app-main-children-label {
    font-size: 20px;
    padding-bottom: 32px;
  }
}
</style>
