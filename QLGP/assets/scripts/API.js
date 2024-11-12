export var linkAPI = 'http://localhost:8301/';
// linkAPI = 'http://asset.quanlynoibo.com/fa_api/';
linkAPI = 'http://demo.quanlynoibo.com:8115/';
// linkAPI = 'http://localhost:63008/';


// export var linkPurchase = 'http://purchase.quanlynoibo.com:8060';
export default {
  Login: linkAPI + 'Account/Login',
  Logout: linkAPI + 'Account/Logout',
  CheckLogin: linkAPI + 'Account/CheckLogin',
  Register: linkAPI + 'Account/Register',
  LoginGoogle: linkAPI + 'Account/LoginGoogle',
  ChangePassword: linkAPI + 'Account/ChangePassword',

  GetUserInfo: linkAPI + 'Account/GetUserInfo',
  // UpdateProfile: linkAPI + 'Account/UpdateProfile',
  Account: linkAPI + 'Account',

  GroupPermission: linkAPI + "GroupPermission",
  Menu: linkAPI + "Menu",
  MenuButton: linkAPI + "MenuButton",



  //Ho_VietNam
  HoVietNam : linkAPI + "HoVietNam",

  // /Giapha
  GetTree : linkAPI + "Giapha/GetTree",
  GetFamily : linkAPI + "Giapha/GetFamily",
  Save_Bome : linkAPI + "Giapha/Save_Bome",
  Add_ACE : linkAPI + "Giapha/Add_ACE",
  Add_Con : linkAPI + "Giapha/Add_Con",
  Add_Vo : linkAPI + "Giapha/Add_Vo",
  // GetTree : linkAPI + "Giapha/GetTree",
  Giapha : linkAPI + "Giapha",
  Dongho : linkAPI + "Dongho",
  Reports_Home : linkAPI + "Reports/Home",
  Comments_Send : linkAPI + "Comments/Send",


}


export var ServerAPI = {
  APISaveFileToServer: '/API/SaveFileToServer',


}
