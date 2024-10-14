const express = require("express");
require("dotenv").config();
const bodyParser = require("body-parser");
const port = process.env.PORT || 5000;

const app = express();

const cors = require("cors");
app.use(cors());

//give access to different domains
app.use((req, res, next) => {
  res.setHeader("Access-Control-Allow-Origin", "*");
  res.setHeader(
    "Access-Control-Allow-Headers",
    "Origin, X-Requested-With, Content-Type, Accept, Authorization"
  );
  res.setHeader("Access-Control-Allow-Methods", "GET, POST, PATCH, DELETE");

  next();
});

//routes
// app.use("/app", functionHere);

//common
app.use((req, res, next) => {
  const error = new HttpError("Could not find this route.", 404);
  throw error;
});

app.use((error, req, res, next) => {
  if (res.headerSent) {
    return next(error);
  }
  res.status(error.code || 500);
  res.json({message: error.message || "An unknown error occurred!"});
});

app.listen(port, () => console.log(`Server started on port ${port}`));
