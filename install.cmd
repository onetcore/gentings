﻿@ECHO OFF 
SET RARLINE="%ProgramFiles%\WinRAR\Winrar.exe"
%RARLINE% a -m1 -r -o+ -s -x%0 ./install.rar ./publisher/app_data
%RARLINE% a -m1 -r -o+ -s -x%0 ./install.rar ./publisher/runtimes
%RARLINE% a -m1 -r -o+ -s -x%0 ./install.rar ./publisher/Areas
%RARLINE% a -m1 -r -o+ -s -x%0 ./install.rar ./publisher/wwwroot/favicon.ico
%RARLINE% a -m1 -r -o+ -s -x%0 ./install.rar ./publisher/wwwroot/*.styles.css
%RARLINE% a -m1 -r -o+ -s -x%0 ./install.rar ./publisher/wwwroot/js/*.min.js
%RARLINE% a -m1 -r -o+ -s -x%0 ./install.rar ./publisher/wwwroot/css/*.min.css
%RARLINE% a -m1 -r -o+ -s -x%0 ./install.rar ./publisher/wwwroot/images
%RARLINE% a -m1 -r -o+ -s -x%0 ./install.rar ./publisher/wwwroot/_content
%RARLINE% a -m1 -r -o+ -s -x%0 ./install.rar ./publisher/wwwroot/lib/*.min.js
%RARLINE% a -m1 -r -o+ -s -x%0 ./install.rar ./publisher/wwwroot/lib/*.min.css
%RARLINE% a -m1 -r -o+ -s -x%0 ./install.rar ./publisher/wwwroot/lib/*.woff
%RARLINE% a -m1 -r -o+ -s -x%0 ./install.rar ./publisher/wwwroot/lib/*.woff2
%RARLINE% a -m1 -r -o+ -s -x%0 ./install.rar ./publisher/wwwroot/lib/*.svg
%RARLINE% a -m1 -r -o+ -s -x%0 ./install.rar ./publisher/*.dll
%RARLINE% a -m1 -r -o+ -s -x%0 ./install.rar ./publisher/*.deps.json
%RARLINE% a -m1 -r -o+ -s -x%0 ./install.rar ./publisher/appsettings.json
%RARLINE% a -m1 -r -o+ -s -x%0 ./install.rar ./publisher/*.runtimeconfig.json
%RARLINE% a -m1 -r -o+ -s -x%0 ./install.rar ./publisher/*.xml
%RARLINE% a -m1 -r -o+ -s -x%0 ./install.rar ./publisher/*.exe
%RARLINE% a -m1 -r -o+ -s -x%0 ./install.rar ./publisher/web.config