import express from "express";
import UserAuthManage from "./obj/UserAuthManage";
const http = require('http');
const https = require('https');
const url = require('url');

UserAuthManage.init();

// Create express router
const router = express.Router();


// 'http://localhost:3000/Account/Callback'
// oauth2Client.red

// console.log(API);
// Transform req & res to have the same API as express
// So we can use res.status() & res.json()
const app = express();
router.use((req, res, next) => {
  Object.setPrototypeOf(req, app.request);
  Object.setPrototypeOf(res, app.response);
  req.res = res;
  res.req = req;
  next();
});

router.get('/GoogleLogin', (req, res) => {
  // Access scopes for read-only Drive activity.
  res.writeHead(302, { "Location": UserAuthManage.generateAuthUrl() });
  res.end();
});

router.post('/Logout', (req, res) => {
  // Access scopes for read-only Drive activity.
  let q = req.body;
  let access_token = q.access_token;
  UserAuthManage.remove(access_token);
  res.json({
    Code: 1,
    Data: 'OK'
  });
});
router.post('/GetUserAuth', (req, res) => {
  let q = req.body;
  let access_token = q.access_token;
  try {
    const userinfo = UserAuthManage.GetUserAuth(access_token);
    res.json({
      Code: 1,
      Data: userinfo
    });
  } catch (error) {
    res.json({
      Code: 3,
      ErrMessage: error.message
    });
  }
});
router.post('/GetUserInfo', async (req, res) => {
  let q = req.body;
  let access_token = q.access_token;
  try {
    const userinfo = await UserAuthManage.getUserInfo(access_token);
    res.json({
      Code: 1,
      Data: userinfo
    });
  } catch (error) {
    res.json({
      Code: 3,
      ErrMessage: error.message
    });
  }
});

router.post('/Callback', async (req, res) => {
  let q = req.body;
  try {
    let token = await UserAuthManage.getToken(q.code);
    res.json({
      Code: 1,
      Data: token
    });
  } catch (error) {
    res.json({
      Code: 3,
      ErrMessage: error.message
    });
  }
})

// Export the server middleware
export default {
  path: "/auth",
  handler: router
};
