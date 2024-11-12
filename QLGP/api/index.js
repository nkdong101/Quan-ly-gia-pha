import express from "express";
import sharp from "sharp";
const https = require("https");
const axios = require("axios");
// Create express router
const router = express.Router();
sharp.cache(false);

var formidable = require("formidable");
var fs = require("fs");
var path = require("path");
var uploadDir = "/images/upload/";
var shell = require("shelljs");


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



function isImg(item) {
  var type = [".JPG", ".JPE", ".BMP", ".GIF", ".PNG", '.WEBP'];
  var splt = item.split(".");
  var ext = splt[splt.length - 1].toUpperCase();
  return type.some(p => p == "." + ext);
}


router.post("/SaveFileToServer", (req, res) => {
  try {
    console.log("SaveFileToServer");
    console.log("Request handler 'upload' was called.");
    console.log(req.body);
    var form = new formidable.IncomingForm();
    form.uploadDir = path.join(__dirname, "../static") + uploadDir;
    form.type = true;
    form.maxFieldsSize = 20 * 1024 * 1024;
    form.keepExtensions = true;
    form.parse(req, function (error, fields, files) {
      if (error) {
        var err = new Error(error);
        err.status = 500;
        next(err);
      }

      var pathBase = req.headers.path || "";
      var folder = req.headers.folder || "";
      var str = [];
      for (var file in files) {
        var fileName = new Date().getTime() + "_" + files[file].name;
        if (
          !fs.existsSync(path.join(__dirname, "../static" + pathBase + folder))
        ) {
          shell.mkdir(
            "-p",
            path.join(__dirname, "../static" + pathBase + folder)
          );
        }

        console.log(fields);


        if (isImg(files[file].path)) {
          sharp(files[file].path).resize({
            height: 680,
            width: 680,
            fit: 'inside',
          }).toFile(path.join(
            __dirname,
            "../static" + pathBase + folder + "/" + fileName
          ), function (error) {
            if (error) {
              var err = new Error(error);
              err.status = 500;
              next(err);
            }
            fs.unlinkSync(files[file].path)
          })
        } else {
          fs.rename(
            files[file].path,
            path.join(
              __dirname,
              "../static" + pathBase + folder + "/" + fileName
            ),
            function (error) {
              if (error) {
                var err = new Error(error);
                err.status = 500;
                next(err);
              }
            }
          );
        }
        let result = (fileName + '|' + files[file].name).replace(/,/g, '?');
        console.log(result)
        str.push(result);
      }

      res.json({
        Status: "OK",
        Data: str,
        other: "test"
      });
    });
  } catch (e) {
    console.log(e);
    res.json({
      Status: "FALSE",
      Data: e,
      other: "test"
    });
  }
});

router.get("/GenPDF", (req, res) => {
  console.log("GenPDF");
  const pdf = require('html-pdf');
  const html = fs.readFileSync(path.join(__dirname, "/template/test.html")).toString()

  pdf.create(html, {
    format: 'A4'
  }).toStream(function (err, stream) {
    if (err) {
      console.log(err);
      res.status(500).send("Some kind of error...");
      return;
    }
    stream.pipe(res)
  })


});


router.get("/GenPDF2", (req, res) => {
  console.log("GenPDF2");
  const pdf = require('phantom-html-to-pdf')();
  const html = fs.readFileSync(path.join(__dirname, "/template/test.html")).toString()
  pdf({
    html: html,
    format: 'A4'
  }, function (err, pdf) {
    if (err) {
      console.log(err);
      res.status(500).send("Some kind of error...");
      return;
    }
    var output = fs.createWriteStream('sample.pdf');
    console.log(pdf.logs);
    // respond an http request.
    pdf.stream.pipe(output);
  })


});
// Export the server middleware
export default {
  path: "/api",
  handler: router
};
