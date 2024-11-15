import bodyParser from "body-parser";

export default {
  // Disable server-side rendering: https://go.nuxtjs.dev/ssr-mode
  ssr: false,

  // Global page headers: https://go.nuxtjs.dev/config-head
  head: {
    title: 'Quản lý gia phả',// 'Procurement Tool',
    htmlAttrs: {
      lang: 'en'
    },
    meta: [
      { charset: 'utf-8' },
      // { name: 'viewport', content: 'width=device-width, initial-scale=1' },
      { hid: 'description', name: 'description', content: '' },
      { name: 'format-detection', content: 'telephone=no' },
      { name: "viewport", content: "width=device-width, initial-scale=1.0" }
    ],
    link: [
      { rel: 'icon', type: 'image/x-icon', href: '/images.ico' },
      { rel: "stylesheet", href: "/font-awesome-4.7.0/css/font-awesome.min.css" },
      { rel: "stylesheet", href: 'https://fonts.googleapis.com/css?family=Open+Sans:300italic,400italic,600italic,700italic,400,600,700,300' },
      // { rel: "stylesheet", href: "/scripts/quill-editor/katex.min.css" },
      // { rel: "stylesheet", href: "/scripts/quill-editor/monokai-sublime.min.css" },
      // { rel: "stylesheet", href: "/scripts/quill-editor/quill.snow.css" }
    ],
    script: [
      { src: "/scripts/socket.io.min.js" },
      // { src: "/scripts/ExportExcel/FileSaver.js" },
      // { src: "/scripts/ExportExcel/xlsx.core.min.js" },
      // { src: "/scripts/quill-editor/katex.min.js" },
      // { src: "/scripts/quill-editor/highlight.min.js" },
      // { src: "/scripts/quill-editor/quill.min.js" },
      // { src: "/scripts/quill-editor/image-resize.min.js" }
    ]
  },

  // Global CSS: https://go.nuxtjs.dev/config-css
  css: [

    'element-ui/lib/theme-chalk/index.css',
    '@/assets/css/site.scss'
  ],

  // Plugins to run before rendering page: https://go.nuxtjs.dev/config-plugins
  plugins: [
    '@/plugins/element-ui',
    "@/plugins/moment",
    "@/plugins/something",
    "@/plugins/auth",
    // { src: '~/plugins/chart.js', mode: 'client' },
  ],

  // Auto import components: https://go.nuxtjs.dev/config-components
  components: true,

  // Modules for dev and build (recommended): https://go.nuxtjs.dev/config-modules
  buildModules: [
  ],

  // Modules: https://go.nuxtjs.dev/config-modules
  modules: [
    // https://go.nuxtjs.dev/axios
    '@nuxtjs/axios',
  ],

  // Axios module configuration: https://go.nuxtjs.dev/config-axios
  axios: {
    // Workaround to avoid enforcing hard-coded localhost:3000: https://github.com/nuxt-community/axios-module/issues/308
    baseURL: '/',
  },
  server: {
    host: '0.0.0.0',
    port: 4000 // default: 3000
  },
  router: {
    // middleware: ["auth"],
  },

  // Build Configuration: https://go.nuxtjs.dev/config-build
  build: {
    transpile: [/^element-ui/],
    extend(config, ctx) {

    }
  },
  serverMiddleware: [
    bodyParser.json(),
    "~/api",
    "~/api/auth",
  ]
}
