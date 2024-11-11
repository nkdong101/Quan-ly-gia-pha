import moment from "moment";
import config from "./config";
import UserAuth, { UserAuthToken } from "./UserAuth";

const { google } = require('googleapis');
const oauth2 = google.oauth2("v2");

const fs = require('fs');



//Set up Google Auth OAuth2
const oauth2Client = new google.auth.OAuth2(
  config.client_id,
  config.client_secret,
  config.url_callback
);
export default {
  /**
   * @type {Array<UserAuth>} type - description
  */
  users: [],

  init() {
    this.readConfig();
    console.log('OK');
  },
  generateAuthUrl() {
    const scopes = [
      'openid', 'profile', 'email'
    ];
    const authorizationUrl = oauth2Client.generateAuthUrl({
      // 'online' (default) or 'offline' (gets refresh_token)
      access_type: 'offline',
      /** Pass in the scopes array defined above.
        * Alternatively, if only one scope is needed, you can pass a scope URL as a string */
      scope: scopes,
      // redirect_uri: req.protocol + '://' + req.get('host') + '/Auth/Callback'
      // Enable incremental authorization. Recommended as a best practice.
      // include_granted_scopes: true
    });
    console.log(authorizationUrl);
    return authorizationUrl;
  },
  async getToken(code) {
    let find = this.users.filter(p => p.tokens.some(p1 => p1.code == code))[0];
    if (!find) {
      let { tokens } = await oauth2Client.getToken(code);
      // console.log(tokens)
      // let token_info = await oauth2Client.getTokenInfo(tokens.access_token);
      let userInfo = await this.getUserInfo(tokens.access_token);
      this.add(userInfo, tokens, code);
      return tokens.access_token;
    } else {
      return find.tokens.filter(p1 => p1.code == code)[0].access_token
    }
  },
  GetUserAuth(access_token) {
    let find = this.users.filter(p => p.tokens.some(p1 => p1.access_token == access_token))[0];
    if (find) {
      return find.userInfo;
    }
    return null;
  },
  readConfig() {
    try {
      let rawdata = fs.readFileSync('./api/obj/users.json');
      this.users = JSON.parse(rawdata);
    } catch (error) {
      this.users = [];
    }
  },
  saveConfig() {
    let data = JSON.stringify(this.users, null, 4);
    fs.writeFileSync('./api/obj/users.json', data);
  },
  /**
   * @param {} email - description
   * @param {import("google-auth-library").Credentials} tokens - description
   * @param {string} code - description
  */
  add(userInfo, tokens, code) {
    let find = this.users.filter(p => p.email == userInfo.email)[0];
    if (!find) {
      this.users.push(new UserAuth({
        email: userInfo.email,
        userInfo: userInfo,
        refresh_token: tokens.refresh_token,
        tokens: [
          new UserAuthToken({
            access_token: tokens.access_token,
            expiry_date: tokens.expiry_date,
            code: code
          })
        ]
      }));
    } else {
      find.refresh_token = tokens.refresh_token;
      find.userInfo = userInfo;
      find.tokens.push(new UserAuthToken({
        access_token: tokens.access_token,
        expiry_date: tokens.expiry_date,
        code: code
      }));

      if (find.tokens.length > 10) {
        find.tokens = find.tokens.filter(p => {
          return moment(p.expiry_date).add(7, 'day').isAfter(new Date());
        })
      }
    }
    this.saveConfig();
  },
  /**
   * @return {} description
   */
  async getUserInfo(access_token, has_retry = false) {
    try {
      const userinfo = await oauth2.userinfo.get({ oauth_token: access_token });
      if (!has_retry)
        return userinfo.data;
      else
        return {
          ...userinfo.data,
          access_token
        }
    } catch (error) {
      if (!has_retry) {
        access_token = await this.getRefreshToken(access_token);
        return this.getUserInfo(access_token, true)
      } else {
        throw error;
      }
    }
  },
  async getRefreshToken(access_token) {
    let find = this.users.filter(p => p.tokens.some(p1 => p1.access_token == access_token))[0];
    if (find) {
      let find_at = find.tokens.findIndex(p => p.access_token == access_token);
      if (find_at > -1) {
        find.tokens.splice(find_at, 1);//Remove old AccessToken
        this.saveConfig();
      }
      oauth2Client.setCredentials({
        refresh_token: find.refresh_token
      });
      let rft = await oauth2Client.refreshAccessToken();
      this.add(find.userInfo, rft.credentials);
      return rft.credentials.access_token;
    }

    throw "Can't getRefreshToken";
  },
  remove(access_token) {
    let find = this.users.filter(p => p.tokens.some(p1 => p1.access_token == access_token))[0];
    if (find) {
      let find_at = find.tokens.findIndex(p => p.access_token == access_token);
      if (find_at > -1) {
        find.tokens.splice(find_at, 1);//Remove old AccessToken
        this.saveConfig();
      }
    }
  }

}
