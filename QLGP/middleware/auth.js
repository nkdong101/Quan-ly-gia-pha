import { $auth } from "~/plugins/auth";

export default function (context) {
  // Add the userAgent property to the context
  context.userAgent = process.server
    ? context.req.headers['user-agent']
    : navigator.userAgent

  // console.log('auth mdw')
  if (!localStorage.access_token && context.route.path != '/Account/Home') {
    context.redirect('/Account/Home');
  } 
  else {
    return $auth.init(localStorage.user || JSON.stringify({}));
  }
}
