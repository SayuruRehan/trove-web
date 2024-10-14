## E-Commerce web-client - EAD

1.  Run "npm install" backend, frontend, and leading(main) directory separately.
2.  Create .env file inside the frontend directory and initialize the following values

        < For WIN users >
        REACT_APP_WEB_API = http://localhost:2030/api

        < For MAC users >
        REACT_APP_WEB_API = http://localhost:5004/api

3.  Create .env file inside the backened directory and initialize the following values

        NODE_ENV = development
        PORT = 5000
        MONGO_URI = mongo db url here without "" or ''
