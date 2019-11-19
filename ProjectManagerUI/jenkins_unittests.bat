cd "%WORKSPACE%\ProjectManagerUI\
npm install

cd "%WORKSPACE%\ProjectManagerUI\node_modules\.bin\"
ng build --prod

ng test --watch=false

ng test --code-coverage