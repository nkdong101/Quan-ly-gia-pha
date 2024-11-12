// @ts-check
import BaseWidget, {
  BaseWidgetDefaultParams
} from "./BaseWidget";
export default class extends BaseWidget {
  /** @type {Array} - children */
  children;
  constructor(obj = {
    ...BaseWidgetDefaultParams(),
    children: null,
  }) {
    super(obj);
    this.children = obj.children;
  }
}
