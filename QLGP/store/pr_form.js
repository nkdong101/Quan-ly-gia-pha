
export const state = () => ({
  currency_code: null,
  btns: []

});


export const mutations = {
  Setcurrency_code(state, currency_code) {
    state.currency_code = currency_code;
  },
  SetMenuButton(state, btns) {
    state.btns = btns;
  },


}
